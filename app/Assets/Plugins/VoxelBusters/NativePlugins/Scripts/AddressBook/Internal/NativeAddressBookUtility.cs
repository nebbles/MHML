using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal class NativeAddressBookUtility
    {
        #region Methods

        public static NativeAddressBookContact[] ConvertNativeArrayDataToContactArray(IntPtr contactsPtr, int length)
        {
            if (IntPtr.Zero == contactsPtr)
            {
                return null;
            }
           
            // create original data array from native data
            NativeAddressBookContact[]    contacts  = new NativeAddressBookContact[length];
            int             sizeOfNativeData        = Marshal.SizeOf(typeof(NativeAddressBookContactData));
            int             offset                  = 0;
            for (int iter = 0; iter < length; iter++)
            {
                NativeAddressBookContactData unmanagedItem = (NativeAddressBookContactData)Marshal.PtrToStructure(new IntPtr(contactsPtr.ToInt64() + offset), typeof(NativeAddressBookContactData));
                contacts[iter]   = new NativeAddressBookContact(unmanagedItem);
                offset          += sizeOfNativeData;
            }
            return contacts;
        }

        #endregion
    }
}