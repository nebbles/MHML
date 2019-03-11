using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// An authorization status the user can grant for an app to access the contacts information.
    /// </summary>
    public enum AddressBookAuthorizationStatus
    {
        /// <summary> The user has not yet made a choice regarding whether this app can access the address book data. </summary>
        NotDetermined,

        /// <summary> The application is not authorized to access the address book data. </summary>
        Restricted,

        /// <summary> The user explicitly denied access to address book data for this application. </summary>
        Denied,

        /// <summary> The application is authorized to access address book data. </summary>
        Authorized
    }
}