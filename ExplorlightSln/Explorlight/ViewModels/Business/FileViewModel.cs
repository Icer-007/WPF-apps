using System.IO;

namespace Explorlight.ViewModels.Business
{
    /// <summary>
    /// File view model, exposes path, name and length
    /// </summary>
    public class FileViewModel : FileSystemViewModel
    {
        private const string SEP_FILEDESC = "; ";

        private static readonly string[] units = ["Go", "m", "k", ""];

        /// <summary>
        /// Build a <see cref="FileViewModel"/> from full path and size
        /// </summary>
        /// <param name="fullPath">Full path of the target file.</param>
        /// <param name="bytesLength">Size, in bytes, of the target file</param>
        public FileViewModel(string? fullPath, long bytesLength)
        {
            this.FullPath = fullPath ?? string.Empty;
            this.Name = Path.GetFileName(this.FullPath);
            this.LengthBytes = bytesLength;

            var size = units.Select((u, i) => new { unit = u, length = bytesLength / Math.Pow(1024, units.Length - i - 1) })
                            .FirstOrDefault(su => su.length >= 1);
            this.Display = $"{this.FullPath}{FileViewModel.SEP_FILEDESC}{size?.length ?? 0:N1} {size?.unit ?? string.Empty}";
        }

        /// <inheritdoc/>
        public override string Display { get; }

        /// <inheritdoc/>
        public override bool Exists => File.Exists(this.FullPath);

        /// <inheritdoc/>
        public override string FullPath { get; }

        /// <summary>
        /// Size of the target file in bytes
        /// </summary>
        public long LengthBytes { get; }

        /// <inheritdoc/>
        public override string Name { get; }
    }
}
