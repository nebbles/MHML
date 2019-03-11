using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    public interface IAddressBookRequestAccessCallbackResult : ICallbackResult
    {
        #region Properties

        AddressBookAuthorizationStatus AuthorizationStatus
        {
            get;
        }

        #endregion
    }
}
