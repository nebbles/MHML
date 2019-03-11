using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;


namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// The AddressBook class provides cross-platform interface to access the contact information.
    /// </summary>
    /// <description> 
    /// <para>
    /// In iOS platform, users can grant or deny access to contact data on a per-application basis. 
    /// And the user is prompted only the first time <see cref="ReadContacts"/> is requested; any subsequent calls use the existing permissions.
    /// You can provide custom usage description in Address Book settings of NativePluginsSettings window.
    /// </para>
    /// </description> 
    public static class AddressBook
    {
        #region Static fields

        public  readonly    static Texture2D                    defaultImage        = null;
        private             static INativeAddressBookInterface  nativeInterface     = null;

        #endregion

        #region Static Constructors

        static AddressBook()
        {
            AddressBookSettings settings    = NativePluginsSettings.AddressBookSettings;
            if (false == settings.IsEnabled)
            {
                throw ErrorCentre.FeatureNotAccessibleException(featureName: "Address Book");
            }

            // set values
            defaultImage    = settings.DefaultImage;
            nativeInterface = CreateNativeInterface();

            // register for events
            nativeInterface.onRequestAccessFinish     += HandleOnRequestAccessFinish;
            nativeInterface.onReadContactsFinish      += HandleOnReadContactsFinish;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns the current authorization status provided to access the contact data.
        /// </summary>
        /// <description>
        /// To see different authorization status, see <see cref="AddressBookAuthorizationStatus"/>.
        /// </description>
        /// <returns>The current authorization status to access the contact data.</returns>
        public static AddressBookAuthorizationStatus GetAuthorizationStatus()
        {
            return nativeInterface.GetAuthorizationStatus();
        }

        /// <summary>
        /// Retrieves all the contact information saved in address book database.
        /// </summary>
        /// <param name="callback">The callback that will be executed after the operation is completed.</param>
        /// <example>
        /// The following code example demonstrates how to read contacts information.
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using VoxelBusters.NativePlugins;
        /// 
        /// public class ExampleClass : MonoBehaviour 
        /// {       
        ///     public void Start()
        ///     {
        ///         // initiate request to read contacts data
        ///         AddressBook.ReadContacts(OnReadContactsFinished); 
        ///     }
        /// 
        ///     // callback method executed when read request is finished
        ///     private void OnReadContactsFinished(IAddressBookReadContactsCallbackResult result)
        ///     {
        ///         if (null == result.Error)
        ///         {
        ///             IAddressBookContact[] contacts  = result.Contacts;
        ///             for (int iter = 0; iter < contacts.Length; iter++)
        ///             {
        ///                 Debug.Log(contacts[iter]);
        ///             }
        ///         }
        ///         else
        ///         {
        ///             // user didn't provide necessary permission
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        public static void ReadContacts(GenericCallback<IAddressBookReadContactsCallbackResult> callback)
        {
            // check current permission level
            AddressBookAuthorizationStatus currentAuthStatus = GetAuthorizationStatus();
            if (AddressBookAuthorizationStatus.NotDetermined == currentAuthStatus)
            {
                GenericCallback<IAddressBookRequestAccessCallbackResult> requestAccessCallback = (requestAccessResult) =>
                {
                    nativeInterface.ReadContacts(callback);
                };
                nativeInterface.RequestAccess(requestTag: requestAccessCallback);
            }
            else
            {
                nativeInterface.ReadContacts(requestTag: callback);
            }
        }

        #endregion

        #region Internal methods

        private static INativeAddressBookInterface CreateNativeInterface()
        {
#if UNITY_EDITOR
            return new EditorAddressBookInterface();
#elif UNITY_IOS
            return new IOSAddressBookInterface();
#elif UNITY_ANDROID
            return new AndroidAddressBookInterface();
#else
            return null;
#endif
        }

        #endregion

        #region Event callback methods

        private static void HandleOnRequestAccessFinish(IAddressBookRequestAccessCallbackResult result, object requestTag)
        {
            GenericCallback<IAddressBookRequestAccessCallbackResult>    requestAccessCallback  = (GenericCallback<IAddressBookRequestAccessCallbackResult>)requestTag;
            requestAccessCallback.Invoke(result);
        }

        private static void HandleOnReadContactsFinish(IAddressBookReadContactsCallbackResult result, object requestTag)
        {
            GenericCallback<IAddressBookReadContactsCallbackResult>     readContactsCallback   = (GenericCallback<IAddressBookReadContactsCallbackResult>)requestTag;
            readContactsCallback.Invoke(result);
        }

        #endregion
    }
}