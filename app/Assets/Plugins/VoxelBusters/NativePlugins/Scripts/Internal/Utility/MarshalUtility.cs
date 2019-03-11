using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    internal static class MarshalUtility
    {
        #region Marshalling methods

        public static string[] UnmanagedArrayToStringArray(IntPtr arrayPtr, int elementCount)
        {
            if (IntPtr.Zero == arrayPtr)
            {
                return null;
            }

            // create array
            IntPtr[]    elementPtr  = new IntPtr[elementCount];
            Marshal.Copy(arrayPtr, elementPtr, 0, elementCount);

            string[]    stringArray = new string[elementCount];
            for (int iter = 0; iter < elementCount; iter++)
            {
                stringArray[iter]   = Marshal.PtrToStringAuto(elementPtr[iter]);
            }
            return stringArray;
        }

        public static IntPtr GetIntPtr(object value)
        {
            if (null == value)
            {
                return IntPtr.Zero;
            }
            return GCHandle.ToIntPtr(value: GCHandle.Alloc(value));
        }

        public static IntPtr CreateUnmanagedStringArray(string[] array, int startIndex, int count)
        {
            int         endIndex    = startIndex + count;
            
            // copy string array to unmanaged space
            IntPtr[]    dataArray   = new IntPtr[count];
            for (int iter = 0; iter < endIndex; iter ++)
            {
                dataArray[iter]     = Marshal.StringToHGlobalAuto(array[iter]);
            }

            // get pointer to array
            IntPtr unmanagedArrayPtr    = GCHandle.ToIntPtr(value: GCHandle.Alloc(dataArray));
            Debug.Log("[NativePlugins] Created unmanaged array: " + unmanagedArrayPtr);
            return unmanagedArrayPtr;
        }

        public static void FreeUnmanagedStringArray(IntPtr unmanagedArrayPtr, int count)
        {
            Debug.Log("[NativePlugins] Releasing unmanaged array: " + unmanagedArrayPtr);

            // release each strings allocated in unmanaged space
            GCHandle    unmanagedArrayHandle    = GCHandle.FromIntPtr(unmanagedArrayPtr);
            IntPtr[]    dataArray               = (IntPtr[])unmanagedArrayHandle.Target;
            for (int iter = 0; iter < count; iter ++)
            {
                Marshal.FreeHGlobal(dataArray[iter]);
            }

            // release handle
            unmanagedArrayHandle.Free();
        }

        #endregion
    }
}