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
            string templatePath = Directory.GetFiles(searchPath, fileName, SearchOption.AllDirectories)[0];
            int index = templatePath.IndexOf("Assets", StringComparison.Ordinal);
            return templatePath.Substring(index, templatePath.Length - index);
        }
    }
}