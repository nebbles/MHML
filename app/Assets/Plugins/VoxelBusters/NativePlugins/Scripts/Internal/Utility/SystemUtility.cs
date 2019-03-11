using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    public static class SystemUtility
    {
        #region String methods

        public static string EscapeString(string value)
        {
            return WWW.EscapeURL(value).Replace("+", "%20");
        }

        #endregion

        #region List methods

        public static object[] ConvertListToArray(IList list)
        {
            int         count   = list.Count;
            object[]    array   = new object[count];
            for (int iter = 0; iter < count; iter++)
            {
                array[iter]     = list[iter];
            }

            return array;
        }

        #endregion

        #region File operations

        public static string GetDirectoryFullName(string path)
        {
            return new FileInfo(path).Directory.FullName;
        }

        #endregion
    }
}