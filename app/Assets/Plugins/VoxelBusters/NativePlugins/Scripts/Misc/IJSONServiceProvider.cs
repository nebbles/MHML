using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    public interface IJsonServiceProvider
    {
        #region Methods

        string ToJSON(object obj);

        object FromJSON(string jsonString);

        #endregion
    }
}