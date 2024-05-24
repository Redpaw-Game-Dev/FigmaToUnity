using System.IO;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    public static class Texture2DExtensions
    {
        public static void SaveToFile(this Texture2D tex, string fileFullName, string savePath)
        {
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
            SaveToFile(tex, savePath + fileFullName, false);
        }

        public static void SaveToFile(this Texture2D tex, string fullPath, bool checkDirectoryExistence)
        {
            if (checkDirectoryExistence)
            {
                string dirName = $"{Path.GetDirectoryName(fullPath)}/";
                if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
            }
            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(fullPath, bytes);
        }
    }
}