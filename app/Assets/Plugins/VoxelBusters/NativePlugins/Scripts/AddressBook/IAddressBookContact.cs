using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// This interface contains contact properties, such as contact’s name, image, phone numbers etc.  
    /// </summary>
    public interface IAddressBookContact
    {
        #region Properties

        /// <summary>
        /// The first name of the contact. (read-only)
        /// </summary>
        string FirstName
        {
            get;
        }

        /// <summary>
        /// The middle name of the contact. (read-only)
        /// </summary>
        string MiddleName
        {
            get;
        }

        /// <summary>
        /// The last name of the contact. (read-only)
        /// </summary>
        string LastName
        {
            get;
        }

        /// <summary>
        /// An array of phone numbers of the contact. (read-only)
        /// </summary>
        string[] PhoneNumbers
        {
            get;
        }

        /// <summary>
        /// An array of email addresses of the contact. (read-only)
        /// </summary>
        string[] EmailAddresses
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Asynchronously loads the profile picture of a contact.
        /// </summary>
        /// <param name="callback">The callback to be executed when request is completed.</param>
        void LoadImage(GenericCallback<ILoadImageCallbackResult> callback);

        #endregion
    }
}