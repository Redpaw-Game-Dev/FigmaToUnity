using System;
using System.IO;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    public static class PathHelper
    {
        public static string FindFilePath(string fileName)
        {
            string templatePath = Directory.GetFiles(Application.dataPath, fileName, SearchOption.AllDirectories)[0];
            int index = templatePath.IndexOf("Assets", StringComparison.Ordinal);
            return templatePath.Substring(index, templatePath.Length - index);
        }
    }
}