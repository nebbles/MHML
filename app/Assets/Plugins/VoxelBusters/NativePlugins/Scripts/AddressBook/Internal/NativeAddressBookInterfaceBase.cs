using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal abstract class NativeAddressBookInterfaceBase : NativeInterfaceObject, INativeAddressBookInterface
    {
        #region INativeAddressBookInterface implementation

        protected static RequestAccessCallback onRequestAccessFinishEvent;
        public event RequestAccessCallback onRequestAccessFinish
        {
            add
            {
                onRequestAccessFinishEvent  += value;
            }
            remove
            {
                onRequestAccessFinishEvent  -= value;
            }
        }

        protected static ReadContactsCallback onReadContactsFinishEvent;
        public event ReadContactsCallback onReadContactsFinish
        {
            add
            {
                onReadContactsFinishEvent   += value;
            }
            remove
            {
                onReadContactsFinishEvent   -= value;
            }
        }

        public abstract AddressBookAuthorizationStatus GetAuthorizationStatus();

        public abstract void RequestAccess(object requestTag);    
        
        public abstract void ReadContacts(object requestTag);

        #endregion
    }
}