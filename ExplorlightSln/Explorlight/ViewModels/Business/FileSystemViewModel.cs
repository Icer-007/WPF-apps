using Explorlight.Extensions;
using Icer.Commons;
using System.Windows;
using System.Windows.Input;

namespace Explorlight.ViewModels.Business
{
    /// <summary>
    /// Base class for files and folders view models
    /// </summary>
    public abstract class FileSystemViewModel : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Build a <see cref="FileSystemViewModel"/>
        /// </summary>
        public FileSystemViewModel()
        {
            this.CommandFind = new RelayCommand(() => this.OpenWithExplorer(true));
            this.CommandOpen = new RelayCommand(() => this.OpenWithExplorer(false));
        }

        /// <summary>
        /// Whether this <see cref="FileSystemViewModel"/> is a file or a directory, open its
        /// containing folder
        /// </summary>
        public ICommand CommandFind { get; }

        /// <summary>
        /// Whether this <see cref="FileSystemViewModel"/> is a file or a directory, open it with
        /// windows explorer
        /// </summary>
        public ICommand CommandOpen { get; }

        /// <summary>
        /// String display of this <see cref="FileSystemViewModel"/>
        /// </summary>
        public virtual string Display => this.Name;

        /// <summary>
        /// True if targeted full path exists, false otherwise
        /// </summary>
        public abstract bool Exists { get; }

        /// <summary>
        /// Target full path
        /// </summary>
        public abstract string FullPath { get; }

        /// <summary>
        /// Target Name
        /// </summary>
        public abstract string Name { get; }

        private void OpenWithExplorer(bool openContainingFolderInstead)
        {
            if (this.Exists)
                this.FullPath.OpenWithExplorer(openContainingFolderInstead);
            else
                MessageBox.Show($"'{this.FullPath}' not found", "Launch", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
