using Explorlight.ViewModels.Business;
using Explorlight.ViewModels.Screens;
using Icer.Commons;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Explorlight.ViewModels
{
    /// <summary>
    /// Application main view model
    /// </summary>
    public class MainViewModel : NotifyPropertyChangedBase
    {
        private const string FILE_SAVE_ENCODING = "windows-1252";

        private const string FILE_SAVE_FILTERS = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";

        private const string FILTER_SEP = ":";

        private readonly DuplicateFilesViewModel duplicateFilesPopupVM;

        /// <summary>
        /// Build a <see cref="MainViewModel"/>
        /// </summary>
        public MainViewModel()
        {
            this.duplicateFilesPopupVM = new DuplicateFilesViewModel();

            this.IsDirSepNeeded = true;
            this.Roots = [new RootViewModel()];

            this.CommandCopyToClipboard = new RelayCommand(() => Clipboard.SetText(this.GetSelectedDirectoryDisplay()));
            this.CommandReloadDirectory = new RelayCommand(async () => await this.ScanSelectedDirectoryAsync());
            this.CommandFindDuplicates = new RelayCommand(() => this.duplicateFilesPopupVM.ShowDuplicates(this.SelectedDirectory));

            this.CommandSaveToFile = new RelayCommand(this.SaveToFile);

            this.PropertyChanged +=
              async (s, e) =>
              {
                  if (e.PropertyName == nameof(MainViewModel.SelectedDirectory))
                  {
                      await this.ScanSelectedDirectoryAsync();
                  }
              };
        }

        /// <summary>
        /// Command to copy scan result to clipboard
        /// </summary>
        public ICommand CommandCopyToClipboard { get; }

        /// <summary>
        /// Command to search for duplicate file names in last scan result
        /// </summary>
        public ICommand CommandFindDuplicates { get; }

        /// <summary>
        /// Command to search re-scan current directory
        /// </summary>
        public ICommand CommandReloadDirectory { get; }

        /// <summary>
        /// Command to save scan result into a file
        /// </summary>
        public ICommand CommandSaveToFile { get; }

        /// <summary>
        /// File filter for scanning (can be multiple, separated by <see cref="FILTER_SEP"/>)
        /// </summary>
        public string? Filter
        {
            get => this.GetProp<string>();
            set => this.SetProp(value);
        }

        /// <summary>
        /// True to display <see cref="SeparatorViewModel"/> between each directory file list, false otherwise
        /// </summary>
        public bool IsDirSepNeeded
        {
            get => this.GetProp<bool>();
            set => this.SetProp(value);
        }

        /// <summary>
        /// True if the current value of <see cref="Filter"/> should be applied to the next scan,
        /// false otherwise
        /// </summary>
        public bool IsFilterNeeded
        {
            get => this.GetProp<bool>();
            set => this.SetProp(value);
        }

        /// <summary>
        /// List of <see cref="RootViewModel"/>, each one embedds its list of drives
        /// </summary>
        public IEnumerable<RootViewModel> Roots { get; }

        /// <summary>
        /// Target directory of scan
        /// </summary>
        public DirectoryViewModel? SelectedDirectory
        {
            get => this.GetProp<DirectoryViewModel>();
            private set
            {
                this.SelectedDirectory?.AbortScan();
                this.SetProp(value);
            }
        }

        /// <summary>
        /// Selected target in UI
        /// </summary>
        public FileSystemViewModel? SelectedFileSystem
        {
            get => this.GetProp<FileSystemViewModel>();
            set
            {
                this.SetProp(value);
                this.SelectedDirectory = value as DirectoryViewModel;
            }
        }

        private string GetSelectedDirectoryDisplay()
            => string.Join(Environment.NewLine, this.SelectedDirectory?.Files?.Select(f => f.Display) ?? []);

        private void SaveToFile()
        {
            var saver = new SaveFileDialog() { Filter = MainViewModel.FILE_SAVE_FILTERS };

            if (saver.ShowDialog() is true)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                File.WriteAllText(saver.FileName, this.GetSelectedDirectoryDisplay(), Encoding.GetEncoding(FILE_SAVE_ENCODING));
            }
            else
            {
                MessageBox.Show("Aborted", "Save to file", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task ScanSelectedDirectoryAsync()
        {
            string[]? filters = (this.IsFilterNeeded ? this.Filter : null)?
                                .Split([MainViewModel.FILTER_SEP], StringSplitOptions.RemoveEmptyEntries);

            if (this.SelectedDirectory != null)
            {
                await this.SelectedDirectory.ScanAsync(filters, this.IsDirSepNeeded);
            }
        }
    }
}
