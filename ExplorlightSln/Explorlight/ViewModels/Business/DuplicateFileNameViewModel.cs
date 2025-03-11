using Icer.Commons;
using System.Collections.ObjectModel;

namespace Explorlight.ViewModels.Business
{
    /// <summary>
    /// List of files that have the same name
    /// </summary>
    public class DuplicateFileNameViewModel : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Build a <see cref="DuplicateFileNameViewModel"/> with duplicated name and list of file instances
        /// </summary>
        /// <param name="fileName">Duplicated file name</param>
        /// <param name="instances">List of files named as <paramref name="fileName"/> (no local check on file names)</param>
        public DuplicateFileNameViewModel(string fileName, IEnumerable<FileViewModel> instances)
        {
            this.FileName = fileName;
            this.Instances = [.. instances.Select(i => new SelectableWrapper<FileViewModel>(i))];
        }

        /// <summary>
        /// Duplicated file name
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// List of instances named <see cref="FileName"/>
        /// </summary>
        public ObservableCollection<SelectableWrapper<FileViewModel>>? Instances
        {
            get => this.GetProp<ObservableCollection<SelectableWrapper<FileViewModel>>>();
            private set => this.SetProp(value);
        }

        /// <summary>
        /// Remove files that no longer exist from <see cref="Instances"/>
        /// </summary>
        public void CleanUp()
        {
            var dups = this.Instances?
                           .Where(i => i.Value.Exists)
                           .ToArray()
                       ?? [];

            this.Instances = [.. dups];
        }
    }
}
