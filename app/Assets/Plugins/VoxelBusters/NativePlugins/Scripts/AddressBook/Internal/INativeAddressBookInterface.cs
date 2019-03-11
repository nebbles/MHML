using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    // delegate signatures
    internal delegate void RequestAccessCallback(IAddressBookRequestAccessCallbackResult result, object requestTag);
    
    internal delegate void ReadContactsCallback(IAddressBookReadContactsCallbackResult result, object requestTag);

    internal interface INativeAddressBookInterface
    {
        #region Events

        event RequestAccessCallback onRequestAccessFinish;

        event ReadContactsCallback onReadContactsFinish;

        #endregion

        #region Methods

        AddressBookAuthorizationStatus GetAuthorizationStatus();
        
        void RequestAccess(object requestTag);    
        
        void ReadContacts(object requestTag);

        #endregion
    }
}