using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// This interface contains the information retrieved when <see cref="AddressBook.ReadContacts(GenericCallback{IAddressBookReadContactsCallbackResult})"/> operation is completed.
    /// </summary>
    public interface IAddressBookReadContactsCallbackResult : ICallbackResult
    {
        #region Properties

        /// <summary>
        /// Contains the contacts details retrieved from address book.
        /// </summary>
        /// <value>If the requested operation was successful, this property holds an array of <see cref="IAddressBookContact"/> objects; otherwise, this is null.</value>
        IAddressBookContact[] Contacts
        {
            get;
        }

        #endregion
    }
}