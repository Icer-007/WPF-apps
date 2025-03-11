using System.IO;

namespace Explorlight.Extensions
{
    /// <summary>
    /// Extensions class for <see cref="DirectoryInfo"/>
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Enumerate files (using <see cref="DirectoryInfo.EnumerateFiles"/>) without throwing any exception
        /// </summary>
        /// <param name="directoryInfo">Target directory</param>
        /// <param name="searchPattern">
        /// The search string to match against the names of files. This parameter can contain a
        /// combination of valid literal path and wildcard (* and ?) characters, but it doesn't
        /// support regular expressions
        /// </param>
        /// <returns>
        /// An enumerable collection of <see cref="FileInfo"/> that matches <paramref name="searchPattern"/>
        /// </returns>
        public static IEnumerable<FileInfo> SafeEnumerateFiles(this DirectoryInfo directoryInfo, string searchPattern)
        {
            // faster to first try without EnumerationOptions
            try
            {
                return directoryInfo.EnumerateFiles(searchPattern);
            }
            catch (Exception) { }

            try
            {
                return directoryInfo.EnumerateFiles(searchPattern, new EnumerationOptions { IgnoreInaccessible = true });
            }
            catch (Exception) { }

            return [];
        }
    }
}
