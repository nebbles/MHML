using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal class AddressBookRequestAccessCallbackResult : CallbackResultBase, IAddressBookRequestAccessCallbackResult
    {
        #region IAddressBookRequestAccessCallbackResult implementation

        public AddressBookAuthorizationStatus AuthorizationStatus
        {
            get;
            internal set;
        }

        #endregion
    }
}