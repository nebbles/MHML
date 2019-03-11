#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal class EditorAddressBookContact : IAddressBookContact
    {
        #region System methods

        public override string ToString()
        {
            return string.Format("First name: {0} Last name: {1}", FirstName, LastName);
        }

        #endregion

        #region IAddressBookContact implementation

        public string FirstName
        {
            get; 
            internal set;
        }

        public string MiddleName
        {
            get; 
            internal set;
        }

        public string LastName
        {
            get; 
            internal set;
        }

        public string[] PhoneNumbers
        {
            get; 
            internal set;
        }

        public string[] EmailAddresses
        {
            get; 
            internal set;
        }

        public void LoadImage(GenericCallback<ILoadImageCallbackResult> callback)
        {
            LoadImageCallbackResult callbackResult = new LoadImageCallbackResult(AddressBook.defaultImage, null);
            if (callback != null)
            {
                callback(callbackResult);
            }
        }

        #endregion
    }
}
#endif