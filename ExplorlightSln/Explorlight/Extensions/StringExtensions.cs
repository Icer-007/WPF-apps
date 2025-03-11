using System.IO;

namespace Explorlight.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Open the provided full path or its containing folder with windows explorer
        /// </summary>
        /// <param name="fullPath">Target full path</param>
        /// <param name="openContainingFolderInstead">
        /// False to open <paramref name="fullPath"/>, true to open its contatning folder
        /// </param>
        public static void OpenWithExplorer(this string? fullPath, bool openContainingFolderInstead)
        {
            if (File.Exists(fullPath)
                || Directory.Exists(fullPath))
            {
                string explorerParam = openContainingFolderInstead
                                       ? $@"/select,""{fullPath}"""
                                       : fullPath;

                System.Diagnostics.Process.Start("explorer", explorerParam);
            }
        }
    }
}
