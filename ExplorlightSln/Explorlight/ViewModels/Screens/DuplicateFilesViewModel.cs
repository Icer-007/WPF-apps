using Explorlight.Models;
using Explorlight.ViewModels.Business;
using Explorlight.Views;
using Icer.Commons;
using Icer.Commons.Extensions;
using Microsoft.VisualBasic.FileIO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Explorlight.ViewModels.Screens
{
    /// <summary>
    /// Duplicate file names main view model
    /// </summary>
    public class DuplicateFilesViewModel : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Build a <see cref="DuplicateFilesViewModel"/>
        /// </summary>
        public DuplicateFilesViewModel()
        {
            this.Duplicates = [];

            this.UpdateDeletionRights();

            this.CommandCopyToClipboard = new RelayCommand(() => Clipboard.SetText(this.GetDisplay()));
            this.CommandCleanUp = new RelayCommand(this.CleanUp);
            this.CommandDeleteSelected = new RelayCommand(this.DeleteSelected, () => this.IsDeletionAllowed);
        }

        /// <summary>
        /// Command to remove file names that no longer have several instances from <see cref="Duplicates"/>
        /// </summary>
        public ICommand CommandCleanUp { get; }

        /// <summary>
        /// Command to copy duplicate file names and their instances full names to clipboard
        /// </summary>
        public ICommand CommandCopyToClipboard { get; }

        /// <summary>
        /// Command to delete selected instances
        /// </summary>
        public ICommandRaisable CommandDeleteSelected { get; }

        /// <summary>
        /// List of duplicated file names
        /// </summary>
        public ObservableCollection<DuplicateFileNameViewModel>? Duplicates
        {
            get => this.GetProp<ObservableCollection<DuplicateFileNameViewModel>>();
            private set => this.SetProp(value);
        }

        /// <summary>
        /// True if allowed to delete files, false otherwise
        /// </summary>
        public bool IsDeletionAllowed
        {
            get => this.GetProp<bool>();
            private set
            {
                this.SetProp(value);
                this.CommandDeleteSelected?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Retrieves duplicated file names from provided directory and displays them in the popup
        /// </summary>
        /// <param name="rootDirectory"></param>
        public void ShowDuplicates(DirectoryViewModel? rootDirectory)
        {
            // Work only on already scanned directory
            if (rootDirectory?.Status != ReadingStatus.Ready)
                return;

            this.UpdateDeletionRights();

            var dups = rootDirectory.GetDuplicates()
                                    .Where(dp => !string.IsNullOrWhiteSpace(dp.Key))
                                    .ToArray();

            if (dups.IsEmptyOrNull())
            {
                MessageBox.Show("No duplicated file name", "Duplicate file names", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            this.Duplicates = [.. dups!.OrderBy(dp => dp.Key).Select(dp => new DuplicateFileNameViewModel(dp.Key!, [.. dp]))];

            new DuplicateFilesPopup { DataContext = this }.ShowDialog();
        }

        private void CleanUp()
        {
            // Clean each duplicate from instances that no longer exist and keep only duplicates
            // that still have several instances
            this.Duplicates?.ToList().ForEach(d => d.CleanUp());
            this.Duplicates = [.. this.Duplicates?.Where(d => d.Instances?.Count > 1) ?? []];
        }

        private void DeleteSelected()
        {
            // Check there are selected instances
            var selected = this.Duplicates?.SelectMany(d => d.Instances?.Where(i => i.IsSelected).Select(i => i.Value) ?? []);
            if (selected.IsEmptyOrNull())
            {
                MessageBox.Show("No selection", "Deleting files", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Check for file names that have all instances selected
            var entireDeletion = this.Duplicates?.Where(d => d.Instances?.All(i => i.IsSelected) is true).ToArray();
            if (entireDeletion?.Length > 0
                &&
                MessageBox.Show(
                            string.Join(Environment.NewLine, entireDeletion.Select(f => $" - {f.FileName}")
                                                                           .Prepend("All instances of the following name(s) will be deleted :")
                                                                           .Append("Continue ?")),
                            "Be careful when deleting files",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Hand) != MessageBoxResult.OK)
            {
                return;
            }

            // Ask for confirmation before deleting files (!!)
            if (MessageBox.Show(
                        $"Delete {selected!.LongCount()} file(s) ?",
                        "Deleting files",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    selected!.Where(f => f.Exists)
                             .ToList()
                             .ForEach(tr => FileSystem.DeleteFile(tr.FullPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Failed to delete", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                this.CleanUp();
            }
            else
            {
                MessageBox.Show("canceled", "Deleting files", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private string GetDisplay()
        {
            var lines = this.Duplicates?
                            .SelectMany(
                                d =>
                                d.Instances?
                                 .Select(i => i.Value.Display)
                                 .Prepend($"****{d.Instances?.Count ?? 0}**** {d.FileName}")
                                ?? []);

            return string.Join(Environment.NewLine, lines ?? []);
        }

        private void UpdateDeletionRights()
            => this.IsDeletionAllowed = AppConfig.Instance.IsSafeModeOff;
    }
}
