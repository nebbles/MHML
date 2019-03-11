using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    [Serializable]
    public class UsagePermission
    {
        #region Fields

        [SerializeField]
        private     RuntimePlatformValue[]     m_permissionValues      = new RuntimePlatformValue[0];

        #endregion

        #region Static Methods

        public static UsagePermission Create(params RuntimePlatformValue[] values)
        {
            return new UsagePermission()
            {
                m_permissionValues  = values,
            };
        }

        #endregion

        #region Public methods

        public string GetPermission(RuntimePlatform platform)
        {
            RuntimePlatformValue targetValue = Array.Find(m_permissionValues, (item) => item.IsEqualToPlatform(platform));
            if (targetValue != null)
            {
                if (RuntimePlatform.All == targetValue.Platform)
                {
                    return FormatPermissionString(targetValue.Value, platform);
                }
            
                return targetValue.Value;
            }

            return null;
        }

        #endregion

        #region Private methods

        private string FormatPermissionString(string genericPermission, RuntimePlatform targetPlatform)
        {
            switch (targetPlatform)
            {
                case RuntimePlatform.iOS:
                case RuntimePlatform.tvOS:
                    return genericPermission.Replace("$productName", "$(PRODUCT_NAME)");

                case RuntimePlatform.Editor:
                    return genericPermission.Replace("$productName", Application.productName);

                default:
                    return genericPermission;
            }
        }

        #endregion
    }
}