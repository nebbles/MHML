using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    internal class FileUtility
    {
        public static void Delete(string path)
        {
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Exists)
            {
                if ((fileInfo.Attributes & FileAttributes.Directory) != 0)
                {
                    Directory.Delete(path, true);
                }
                else
                {
                    File.Delete(path);
                }
            }
        }
    }
}
