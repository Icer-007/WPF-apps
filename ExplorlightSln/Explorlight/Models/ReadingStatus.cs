namespace Explorlight.Models
{
    /// <summary>
    /// Scanning statuses of a directory
    /// </summary>
    public enum ReadingStatus

    {
        /// <summary>
        /// Scanning done
        /// </summary>
        Ready,

        /// <summary>
        /// Scanning will be aborted
        /// </summary>
        Cancelling,

        /// <summary>
        /// Scanning is in progress
        /// </summary>
        Running,
    }
}
