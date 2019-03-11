#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    using Internal;
    internal partial class AndroidAddressBookInterface : NativeAddressBookInterfaceBase
    {
        #region Helper classes

        internal class Native
        {
            #region Constant fields

            internal const string kPackage                          = "com.voxelbusters.nativeplugins.v2.features.addressbook";
            internal const string kClassName                        = kPackage + "." + "AddressBookHandler";
            internal const string kReadContactsListenerInterface    = kPackage + "." + "IAddressBookHandler$IReadContactsListener";
            internal const string kRequestAccessListenerInterface   = kPackage + "." + "IAddressBookHandler$IRequestContactsPermissionListener";

            #endregion

            #region Nested types

            internal class Method
            {
                internal const string kInitialise           = "initialise";
                internal const string kReadContacts         = "readContacts";
                internal const string kRequestPermission    = "requestPermission";
                internal const string kIsAuthorized         = "isAuthorized";
            }

            #endregion
        }

        internal class ReadContactsProxyListener : NativeProxy<ReadContactsCallback>
        {
            #region Fields

            private object m_requestTag;

            #endregion

            #region Constructors

            public ReadContactsProxyListener(ReadContactsCallback m_callback, object requestTag) : base(m_callback, Native.kReadContactsListenerInterface)
            {
                m_requestTag = requestTag;
            }

            #endregion

            #region Callbacks

            private void onReadContactsComplete(List<AndroidAddressBookContact> contacts, string error)
            {
                if (m_callback != null)
                {
                    AddressBookReadContactsCallbackResult callbackResult = new AddressBookReadContactsCallbackResult()
                    {
                        Contacts = null,
                        Error = null,
                    };

                    if (string.IsNullOrEmpty(error))
                    {
                        callbackResult.Contacts = contacts.ToArray();
                    }

                    Action action = () => m_callback(callbackResult, m_requestTag);
                    DispatchOnMainThread(action);
                }
            }

            public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
            {
                if (methodName == "onReadContactsComplete")
                {
                    List<AndroidAddressBookContact> list = javaArgs[0].GetList(AndroidAddressBookContact.FromNativeObject);
                    string error = javaArgs[1].GetString();

                    onReadContactsComplete(list, error);
                    return null;
                }
                else
                {
                    return base.Invoke(methodName, javaArgs);
                }
            }

            #endregion
        }

        internal class PermissionRequestProxyListener : NativeProxy<RequestAccessCallback>
        {
            #region Fields

            private object m_requestTag;

            #endregion


            #region Constructors

            public PermissionRequestProxyListener(RequestAccessCallback callback, object requestTag) : base(callback, Native.kRequestAccessListenerInterface)
            {
                m_requestTag = requestTag;
            }

            #endregion

            #region Callbacks

            void onPermissionRequestComplete(bool success, string error)
            {
                if (m_callback != null)
                {
                    AddressBookRequestAccessCallbackResult callbackResult = new AddressBookRequestAccessCallbackResult()
                    {
                        AuthorizationStatus = success ? AddressBookAuthorizationStatus.Authorized : AddressBookAuthorizationStatus.Denied,
                        Error = error,
                    };

                    Action action = () => m_callback(callbackResult, m_requestTag);
                    DispatchOnMainThread(action);
                }
            }

            #endregion
        }

        #endregion
    }
}
#endif
