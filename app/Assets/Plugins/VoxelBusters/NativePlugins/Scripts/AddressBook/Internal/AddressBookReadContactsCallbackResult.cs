using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal class AddressBookReadContactsCallbackResult : CallbackResultBase, IAddressBookReadContactsCallbackResult
    {
        #region IAddressBookReadContactsCallbackResult implementation

        public IAddressBookContact[] Contacts
        {
            get;
            internal set;
        }

        #endregion
    }
}