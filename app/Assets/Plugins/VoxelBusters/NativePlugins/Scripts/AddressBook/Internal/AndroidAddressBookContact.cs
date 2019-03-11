#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace VoxelBusters.NativePlugins
{
    using Internal;

    internal class AndroidAddressBookContact : IAddressBookContact
    {
        #region Fields

        private AndroidJavaObject m_profilePicture;

        #endregion

        #region Properties

        public string FirstName
        {
            get;
            set;
        }

        public string MiddleName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string[] PhoneNumbers
        {
            get;
            set;
        }

        public string[] EmailAddresses
        {
            get;
            set;
        }

        #endregion

        #region Override methods

        public override string ToString()
        {
            return string.Format("First Name : {0}, Last Name : {1}, Email Addresses : [{2}], Phone Numbers : [{3}]", FirstName, LastName, string.Concat(EmailAddresses), string.Concat(PhoneNumbers));
        }

        #endregion

        #region Public methods

        public void LoadImage(GenericCallback<ILoadImageCallbackResult> callback)
        {
            if (callback == null)
                return;

            if (m_profilePicture != null)
            {
                m_profilePicture.Call("load", AndroidPluginUtility.GetContext(), new LoadURIProxyListener((data) =>
                {
                    UnityThreadDispatcher.Instance.Enqueue(() =>
                    {
                        Texture2D texture = null;

                        if (data != null)
                        {
                            texture = new Texture2D(2, 2);
                            ImageConversion.LoadImage(texture, data);
                        }

                        LoadImageCallbackResult result = new LoadImageCallbackResult(texture, null);
                        callback(result);
                    });
                }));
            }
            else
            {
                LoadImageCallbackResult result = new LoadImageCallbackResult(AddressBook.defaultImage, null);
                callback(result);
            }
        }

        #endregion

        #region Internal methods

        internal static AndroidAddressBookContact FromNativeObject(AndroidJavaObject nativeObject)
        {
            AndroidAddressBookContact contact = new AndroidAddressBookContact();
            contact.FirstName = nativeObject.Call<string>("givenName");
            contact.LastName = nativeObject.Call<string>("familyName");

            contact.PhoneNumbers = nativeObject.Call<string[]>("phoneNumbers");
            contact.EmailAddresses = nativeObject.Call<string[]>("emailAddresses");
            contact.SetProfilePicture(nativeObject.Call<AndroidJavaObject>("profilePicture"));

            return contact;
        }

        #endregion

        #region Private methods

        internal void SetProfilePicture(AndroidJavaObject javaObject)
        {
            m_profilePicture = javaObject;
        }

        #endregion
    }
}
#endif
