namespace Explorlight.ViewModels.Business
{
    /// <summary>
    /// Separator view model, represents directory separator in file lists
    /// </summary>
    public class SeparatorViewModel : FileViewModel
    {
        /// <summary>
        /// Build a <see cref="SeparatorViewModel"/>
        /// </summary>
        public SeparatorViewModel() : base(string.Empty, 0) { }

        /// <inheritdoc/>
        public override string Display => this.FullPath;

        /// <inheritdoc/>
        public override bool Exists => false;

        /// <inheritdoc/>
        public override string FullPath => "************************************************************";

        /// <inheritdoc/>
        public override string Name => this.FullPath;
    }
}
