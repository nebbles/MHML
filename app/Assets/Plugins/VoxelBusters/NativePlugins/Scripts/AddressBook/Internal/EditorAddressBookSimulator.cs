#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
	public class EditorAddressBookSimulator 
	{
		#region Constants

		// preference keys
        private 	const 				string    					kAddressBookAuthPrefKey    = "$AB-Auth";

		// messages
        private 	const 				string    					kUnauthorizedAccessError   = "Unauthorized access! Check permission before accessing contacts database.";
        private 	const 				string    					kAlreadyAuthorizedError    = "Permission for accessing contacts is already provided. Please check AuthorizationStatus before requesting access.";

        #endregion

        #region Static fields

        private     static	readonly 	IAddressBookContact[]		contacts					= null;

        #endregion

        #region Constructors

        static EditorAddressBookSimulator()
        {
            // set properties
            contacts = GetDummyContacts();
        }

        #endregion

        #region Static methods

        private static IAddressBookContact[] GetDummyContacts()
        {
            // create fake contacts
            int                     randCount   = Random.Range(10, 20);
            IAddressBookContact[]   newContacts = new IAddressBookContact[randCount];
            for (int iter = 0; iter < randCount; iter++)
            {
                newContacts[iter]   = new EditorAddressBookContact()
                {
                    FirstName       = Path.GetRandomFileName(),
                    LastName        = Path.GetRandomFileName(),
                    PhoneNumbers    = new string[] { (9876543200 + iter).ToString() },
                };
            }
            return newContacts;
        }

        #endregion

        #region Simulator methods

        public static AddressBookAuthorizationStatus GetAuthorizationStatus()
        {
            return (AddressBookAuthorizationStatus)PlayerPrefs.GetInt(kAddressBookAuthPrefKey, (int)AddressBookAuthorizationStatus.NotDetermined);
        }

        public static void RequestAccess(GenericCallback<IAddressBookRequestAccessCallbackResult> callback)
        {
            // check whether required permission is already granted
            AddressBookAuthorizationStatus authorizationStatus = GetAuthorizationStatus();
            if (AddressBookAuthorizationStatus.Authorized == authorizationStatus)
            {
                IAddressBookRequestAccessCallbackResult  callbackResult	= new AddressBookRequestAccessCallbackResult()
                {
                    AuthorizationStatus = AddressBookAuthorizationStatus.Authorized,
                    Error               = kAlreadyAuthorizedError,
                };
                callback(callbackResult);
            }
            else
            {
                // show prompt to user asking for required permission
                new AlertDialog()
                    .SetTitle("Address Book")
                    .SetMessage(NativePluginsSettings.AddressBookSettings.UsagePermission.GetPermission(RuntimePlatform.Editor))
                    .AddButton("Authorise", () => 
                    { 
                        // save selection
                        PlayerPrefs.SetInt(kAddressBookAuthPrefKey, (int)AddressBookAuthorizationStatus.Authorized);

                        // send callback
                        IAddressBookRequestAccessCallbackResult  callbackResult	= new AddressBookRequestAccessCallbackResult()
                        {
                            AuthorizationStatus = AddressBookAuthorizationStatus.Authorized,
                            Error               = null,
                        };
                        callback(callbackResult);
                    })
                    .AddCancelButton("Cancel", () => 
                    { 
                        // save selection
                        PlayerPrefs.SetInt(kAddressBookAuthPrefKey, (int)AddressBookAuthorizationStatus.Denied);

                        // send callback
                        IAddressBookRequestAccessCallbackResult  callbackResult	= new AddressBookRequestAccessCallbackResult()
                        {
                            AuthorizationStatus = AddressBookAuthorizationStatus.Denied,
                            Error               = null,
                        };
                        callback(callbackResult);
                    })
                    .Show();
            }
        }

        public static void ReadContacts(GenericCallback<IAddressBookReadContactsCallbackResult> callback)
        {
            IAddressBookReadContactsCallbackResult callbackResult	= null;

            // read status and fetch appropriate result
            AddressBookAuthorizationStatus authorizationStatus = GetAuthorizationStatus();
            if (AddressBookAuthorizationStatus.Authorized == authorizationStatus)
            {
                callbackResult  = new AddressBookReadContactsCallbackResult()
                {
                    Contacts    = contacts,
                    Error       = null,
                };
            }
            else
            {
                callbackResult  = new AddressBookReadContactsCallbackResult()
                {
                    Contacts    = null,
                    Error       = kUnauthorizedAccessError,
                };
            }
            
            // send callback
            callback(callbackResult);
        }

		public static void Reset() 
		{
			PlayerPrefs.DeleteKey(kAddressBookAuthPrefKey);
		}

        #endregion
	}
}
#endif
