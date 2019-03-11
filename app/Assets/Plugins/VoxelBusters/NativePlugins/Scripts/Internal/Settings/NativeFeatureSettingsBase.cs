using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    [SerializeField]
    public abstract class NativeFeatureSettingsBase
    {
        #region Fields

        [SerializeField]
        private     bool        m_isEnabled     = true;

        #endregion

        #region Properties

        public bool IsEnabled
        {
            get
            {
                return m_isEnabled;
            }
        }

        #endregion
    }
}