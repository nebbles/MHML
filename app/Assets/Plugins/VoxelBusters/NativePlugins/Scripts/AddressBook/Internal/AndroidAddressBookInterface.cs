#if UNITY_ANDROID
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    using Internal;
    internal partial class AndroidAddressBookInterface : NativeAddressBookInterfaceBase
    {
        #region Properties

        private AndroidJavaObject Plugin
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        static AndroidAddressBookInterface()
        {
            using (AndroidJavaClass javaClass = AndroidPluginUtility.CreateJavaClass(Native.kClassName))
            {
                javaClass.CallStatic(Native.Method.kInitialise, "USAGE DESCRIPTION HERE");//TODO Set the description here
            }
        }

        public AndroidAddressBookInterface()
        {
            Plugin = AndroidPluginUtility.CreateJavaInstance(Native.kClassName);
        }

        #endregion

        #region IAddressBookNativeInterface implementation

        public override AddressBookAuthorizationStatus GetAuthorizationStatus()
        {
            bool _accessGranted = Plugin.Call<bool>(Native.Method.kIsAuthorized);

            if (_accessGranted)
            {
                return AddressBookAuthorizationStatus.Authorized;
            }
            else
            {
                return AddressBookAuthorizationStatus.Denied;
            }
        }

        public override void ReadContacts(object requestTag)
        {
            Plugin.Call(Native.Method.kReadContacts, new ReadContactsProxyListener(onReadContactsFinishEvent, requestTag));
        }

        public override void RequestAccess(object requestTag)
        {
            Plugin.Call(Native.Method.kRequestPermission, new PermissionRequestProxyListener(onRequestAccessFinishEvent, requestTag));
        }

        #endregion
    }
}
#endif