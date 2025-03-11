using Explorlight.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace Explorlight.ViewModels.Business
{
    /// <summary>
    /// Containers for drives (c:, d:, etc...)
    /// </summary>
    public class RootViewModel : FileSystemViewModel
    {
        /// <summary>
        /// Build a <see cref="RootViewModel"/>
        /// </summary>
        public RootViewModel()
        {
            this.Name = "/";
            this.IsExpanded = true;
        }

        /// <summary>
        /// <see cref="RootViewModel"/> is always ready since its loading is fast
        /// </summary>
        public static ReadingStatus Status => ReadingStatus.Ready;

        /// <summary>
        /// <see cref="RootViewModel"/> always exists. (it exists "physically" even if its <see
        /// cref="FullPath"/> is not reachable)
        /// </summary>
        public override bool Exists => true;

        /// <summary>
        /// <see cref="FullPath"/> is same as the <see cref="Name"/>
        /// </summary>
        public override string FullPath => this.Name;

        /// <summary>
        /// True if drives are visible, false otherwise
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
        public override string Name { get; }

        /// <summary>
        /// Drives
        /// </summary>
        public ObservableCollection<DirectoryViewModel>? SubDirectories
        {
            get => this.GetProp<ObservableCollection<DirectoryViewModel>>();
            private set => this.SetProp(value);
        }

        private void LoadTopSubDirectories()
            => this.SubDirectories = [.. Directory.GetLogicalDrives().Select(d => new DirectoryViewModel(d))];
    }
}
