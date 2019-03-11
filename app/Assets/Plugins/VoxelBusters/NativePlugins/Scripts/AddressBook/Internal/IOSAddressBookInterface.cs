#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using AOT;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    internal class IOSAddressBookInterface : NativeAddressBookInterfaceBase, INativeAddressBookInterface
    {
        #region Delegates

        internal delegate void RequestAccessNativeCallback(CNAuthorizationStatus status, string error, IntPtr tagPtr);

        internal delegate void ReadContactsNativeCallback(IntPtr contactsPtr, int count, string error, IntPtr tagPtr);

        #endregion

        #region Binding calls

        [DllImport("__Internal")]
        private static extern void NPAddressBookRegisterCallbacks(RequestAccessNativeCallback accessCallback, ReadContactsNativeCallback readContactsCallback);
        
        [DllImport("__Internal")]
        private static extern CNAuthorizationStatus NPAddressBookGetAuthorizationStatus();
        
        [DllImport("__Internal")]
        private static extern void NPAddressBookRequestAccess(IntPtr tagPtr);
        
        [DllImport("__Internal")]
        private static extern void NPAddressBookReadContacts(IntPtr tagPtr);
        
        [DllImport("__Internal")]
        private static extern void NPAddressBookReset();

        #endregion

        #region Constructors

        static IOSAddressBookInterface()
        {
            NPAddressBookRegisterCallbacks(accessCallback: HandleRequestAccessCallbackInternal, readContactsCallback: HandleReadContactsCallbackInternal);
        } 

        #endregion

        #region Base class implementation

        public override AddressBookAuthorizationStatus GetAuthorizationStatus()
        {
            return ConvertToAddressBookAuthorizationStatus(status: NPAddressBookGetAuthorizationStatus());
        }

        public override void RequestAccess(object requestTag)
        {
            // make call
            IntPtr requestTagPtr = MarshalUtility.GetIntPtr(requestTag);
            NPAddressBookRequestAccess(requestTagPtr);
        }

        public override void ReadContacts(object requestTag)
        {
            // make call
            IntPtr requestTagPtr = MarshalUtility.GetIntPtr(requestTag);
            NPAddressBookReadContacts(requestTagPtr);
        }

        protected override void InvalidateInternal()
        {
            base.InvalidateInternal();

            NPAddressBookReset();
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(RequestAccessNativeCallback))]
        private static void HandleRequestAccessCallbackInternal(CNAuthorizationStatus status, string error, IntPtr tagPtr)
        {
            // create result object
            AddressBookRequestAccessCallbackResult  callbackResult  = new AddressBookRequestAccessCallbackResult()
            {
                AuthorizationStatus = ConvertToAddressBookAuthorizationStatus(status),
                Error               = error,
            };

            // send callback
            GCHandle    tagHandle   = GCHandle.FromIntPtr(tagPtr);
            onRequestAccessFinishEvent(callbackResult, tagHandle.Target);

            // release handle
            tagHandle.Free();
        }

        [MonoPInvokeCallback(typeof(ReadContactsNativeCallback))]
        private static void HandleReadContactsCallbackInternal(IntPtr contactsPtr, int count, string error, IntPtr tagPtr)
        {
            // create result object
            AddressBookReadContactsCallbackResult   callbackResult  = new AddressBookReadContactsCallbackResult()
            {
                Contacts    = NativeAddressBookUtility.ConvertNativeArrayDataToContactArray(contactsPtr, count),
                Error       = error,
            };

            // trigger callback
            GCHandle    tagHandle  = GCHandle.FromIntPtr(tagPtr);
            onReadContactsFinishEvent(callbackResult, tagHandle.Target);
            
            // release handle
            tagHandle.Free();
        }

        #endregion

        #region Converter methods

        private static AddressBookAuthorizationStatus ConvertToAddressBookAuthorizationStatus(CNAuthorizationStatus status)
        {
            switch (status)
            {
                case CNAuthorizationStatus.CNAuthorizationStatusNotDetermined:
                    return AddressBookAuthorizationStatus.NotDetermined;

                case CNAuthorizationStatus.CNAuthorizationStatusRestricted:
                    return AddressBookAuthorizationStatus.Restricted;

                case CNAuthorizationStatus.CNAuthorizationStatusDenied:
                    return AddressBookAuthorizationStatus.Denied;

                case CNAuthorizationStatus.CNAuthorizationStatusAuthorized:
                    return AddressBookAuthorizationStatus.Authorized;

                default:
                    throw ErrorCentre.SwitchCaseNotImplementedException(status);
            }
        }

        #endregion

        #region Nested types

        internal enum CNAuthorizationStatus
        {
            CNAuthorizationStatusNotDetermined,
            CNAuthorizationStatusRestricted,
            CNAuthorizationStatusDenied,
            CNAuthorizationStatusAuthorized,
        }

        #endregion
    }
}
#endif