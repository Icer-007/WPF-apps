using Explorlight.Extensions;
using Explorlight.Models;
using Icer.Commons.Extensions;
using System.Collections.ObjectModel;
using System.IO;

namespace Explorlight.ViewModels.Business
{
    /// <summary>
    /// Directory view model, exposes sub-directories and APIs to list files recursively
    /// </summary>
    public class DirectoryViewModel : FileSystemViewModel
    {
        private readonly DirectoryInfo? directoryInfo;

        private readonly Semaphore startScanningSemaphore = new(initialCount: 1, maximumCount: 1);

        private CancellationTokenSource? currentCancellationToken;

        /// <summary>
        /// Build a <see cref="DirectoryViewModel"/>
        /// </summary>
        /// <param name="fullPath">Target full path</param>
        public DirectoryViewModel(string? fullPath)
        {
            this.directoryInfo = string.IsNullOrWhiteSpace(fullPath)
                                 ? null
                                 : new DirectoryInfo(fullPath);

            this.SubDirectories = this.CountTopSubDirectories() > 0
                                  ? [new(null)]
                                  : [];
        }

        /// <summary>
        /// Number of directories that contains at least one file that matches the search
        /// </summary>
        public long? DirectoriesMatchingCount
        {
            get => this.GetProp<long?>();
            private set => this.SetProp(value);
        }

        /// <summary>
        /// Number of scanned directories
        /// </summary>
        public long? DirectoriesTotalCount
        {
            get => this.GetProp<long?>();
            private set => this.SetProp(value);
        }

        /// <inheritdoc/>
        public override bool Exists => Directory.Exists(this.FullPath);

        /// <summary>
        /// Files that match the search
        /// </summary>
        public ObservableCollection<FileViewModel>? Files
        {
            get => this.GetProp<ObservableCollection<FileViewModel>>();
            private set => this.SetProp(value);
        }

        /// <summary>
        /// Number of files that match the search
        /// </summary>
        public long? FilesMatchingCount
        {
            get => this.GetProp<long?>();
            private set => this.SetProp(value);
        }

        /// <inheritdoc/>
        public override string FullPath => this.directoryInfo?.FullName ?? string.Empty;

        /// <summary>
        /// True if subdirectories are visible, false otherwise
        /// </summary>
        public bool IsExpanded
        {
            get => this.GetProp<bool>();
            set
            {
                this.SetProp(value);
                if (this.IsExpanded)
                    this.LoadTopSubDirectories();
            }
        }

        /// <inheritdoc/>
        public override string Name => this.directoryInfo?.Name ?? string.Empty;

        /// <summary>
        /// Scanning status of the directory
        /// </summary>
        public ReadingStatus Status
            => this.currentCancellationToken?.IsCancellationRequested switch
            {
                null => ReadingStatus.Ready,
                true => ReadingStatus.Cancelling,
                _ => ReadingStatus.Running
            };

        /// <summary>
        /// Subdirectories
        /// </summary>
        public ObservableCollection<DirectoryViewModel>? SubDirectories
        {
            get => this.GetProp<ObservableCollection<DirectoryViewModel>>();
            private set => this.SetProp(value);
        }

        /// <summary>
        /// Abort current search if any
        /// </summary>
        public void AbortScan()
        {
            try
            {
                this.currentCancellationToken?.Cancel();
            }
            catch (Exception) { }

            this.RaiseStatusChanged();
        }

        /// <summary>
        /// Clear search results
        /// </summary>
        public void ClearScanResults()
        {
            this.DirectoriesTotalCount = null;
            this.DirectoriesMatchingCount = null;
            this.Files = null;
            this.FilesMatchingCount = null;
        }

        /// <summary>
        /// Get group of files that have the same name
        /// </summary>
        /// <returns>A list of group of files that have the same name</returns>
        public IEnumerable<IGrouping<string?, FileViewModel>> GetDuplicates()
        {
            var grp =
                this.Files?
                    .Where(f => f is not SeparatorViewModel)
                    .GroupBy(f => f.Name)
                    .Where(g => g.LongCount() > 1);

            return grp ?? [];
        }

        /// <summary>
        /// Start recursive search for files that match the provided filters
        /// </summary>
        /// <param name="filters">
        /// List of inclusive filters (found files will have to match all filters)
        /// </param>
        /// <param name="isDirSepNeeded">
        /// True to display a separator between each directory in result list
        /// </param>
        public async Task ScanAsync(string[]? filters, bool isDirSepNeeded)
        {
            var guid = Guid.NewGuid();
            await Task.Run(() => this.startScanningSemaphore.WaitOne());

            if (this.Status != ReadingStatus.Ready)
            {
                this.AbortScan();
                await Task.Run(() => { while (this.Status != ReadingStatus.Ready) ; });
            }

            this.ClearScanResults();

            if (this.directoryInfo?.Exists is true)
            {
                try
                {
                    using (var cal = new CancellationTokenSource())
                    {
                        this.currentCancellationToken = cal;
                        this.RaiseStatusChanged();

                        await Task.Run(() => this.Scan(filters ?? [], isDirSepNeeded, cal.Token), cal.Token);
                    }
                }
                catch (Exception) { }
                finally
                {
                    this.currentCancellationToken = null;

                    this.startScanningSemaphore.Release();

                    this.RaiseStatusChanged();
                }
            }
            else
            {
                this.startScanningSemaphore.Release();
            }
        }

        private long CountTopSubDirectories()
        {
            try
            {
                return this.directoryInfo?.Exists is true
                       ? Directory.EnumerateDirectories(this.FullPath).LongCount()
                       : 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void LoadTopSubDirectories()
        {
            if (this.directoryInfo?.Exists is true)
            {
                try
                {
                    this.SubDirectories = [.. Directory.GetDirectories(this.FullPath).OrderBy(d => d).Select(d => new DirectoryViewModel(d))];
                }
                catch (Exception)
                {
                    this.SubDirectories?.Clear();
                }
            }
            else
            {
                this.SubDirectories?.Clear();
            }
        }

        private void RaiseStatusChanged()
            => this.OnPropertyChanged(nameof(DirectoryViewModel.Status));

        private void Scan(string[] filters, bool isDirSepNeeded, CancellationToken cancellationToken)
        {
            if (this.directoryInfo?.Exists is true)
            {
                string searchPattern = filters.Length > 0
                                       ? $"*{filters.First()}*"
                                       : "*";

                var resDirs =
                    this
                    .directoryInfo
                    .EnumerateDirectories("*", new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true })
                    .WithCancellation(cancellationToken)
                    .OrderBy(d => d.FullName)
                    .Prepend(this.directoryInfo);

                var resFiles =
                    resDirs
                    .Select(
                        d =>
                        d.Exists
                        ? d.SafeEnumerateFiles(searchPattern)
                           .WithCancellation(cancellationToken)
                           .Where(file => filters.Skip(1).All(f => file.FullName.IndexOf(f, StringComparison.InvariantCultureIgnoreCase) > -1))
                           .OrderBy(f => f.FullName)
                           .Select(file => new FileViewModel(file.FullName, file.Length))
                           .PrependIf(new SeparatorViewModel(), isDirSepNeeded)
                           .ToArray()
                        : [])
                    .ToLookup(files => files.Length > (isDirSepNeeded ? 1 : 0));

                this.DirectoriesTotalCount = resFiles[true].LongCount() + resFiles[false].LongCount();
                this.DirectoriesMatchingCount = resFiles[true].LongCount();

                this.Files = [.. resFiles[true].SelectMany(l => l)];

                this.FilesMatchingCount = this.Files.Count - (isDirSepNeeded ? this.DirectoriesMatchingCount : 0);
            }
        }
    }
}
