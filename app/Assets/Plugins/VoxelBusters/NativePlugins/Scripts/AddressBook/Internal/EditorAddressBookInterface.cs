#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    internal class EditorAddressBookInterface : NativeAddressBookInterfaceBase, INativeAddressBookInterface
    {
        #region Base class implementation

        public override AddressBookAuthorizationStatus GetAuthorizationStatus()
        {
            return EditorAddressBookSimulator.GetAuthorizationStatus();
        }

        public override void RequestAccess(object requestTag)
        {
            EditorAddressBookSimulator.RequestAccess((result) => onRequestAccessFinishEvent(result, requestTag));
        }

        public override void ReadContacts(object requestTag)
        {
            EditorAddressBookSimulator.ReadContacts((result) => onReadContactsFinishEvent(result, requestTag));
        }

        #endregion
    }
}
#endif