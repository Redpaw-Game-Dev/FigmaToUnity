using System;
using System.IO;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    public static class PathHelper
    {
        public static string FindFilePath(string fileName)
        {
            string searchPath = Application.dataPath;
            searchPath = searchPath.Replace("/Assets", string.Empty);
            string templatePath = Directory.GetFiles(searchPath, fileName, SearchOption.AllDirectories)[0];
            int index = 0;
            if(templatePath.Contains("Packages")) index = templatePath.IndexOf("Packages", StringComparison.Ordinal);
            else if(templatePath.Contains("Assets")) index = templatePath.IndexOf("Assets", StringComparison.Ordinal);
            return templatePath.Substring(index, templatePath.Length - index);
        }
    }
}