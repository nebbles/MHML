using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    [Serializable]
    public class PlatformIdSet
    {
        #region Fields

        [SerializeField]
        private         string                      m_id;
        [SerializeField]
        private         RuntimePlatformValue[]      m_platformIds;

        #endregion

        #region Properties

        public string Id
        {
            get
            {
                return m_id;
            }
        }

        #endregion

        #region Constructors

        public PlatformIdSet()
        {
            // set default values
            m_id            = string.Empty;
            m_platformIds   = new RuntimePlatformValue[0];
        }

        public PlatformIdSet(string id, RuntimePlatformValue[] platformIds)
        {
            // set default values
            m_id            = id;
            m_platformIds   = platformIds;
        }

        #endregion

        #region Create methods

        public static PlatformIdSet Create(RuntimePlatformValue platformId)
        {
            // check arguments
            if (null == platformId)
            {
                throw ErrorCentre.ArgumentNullException("platformId");
            }

            // create new object
            return new PlatformIdSet(platformId.ToString(), new RuntimePlatformValue[] { platformId });
        }

        public static PlatformIdSet Create(string id, params RuntimePlatformValue[] platformIds)
        {
            // check arguments
            if (null == id)
            {
                throw ErrorCentre.ArgumentNullException("id");
            }
            if (null == platformIds)
            {
                throw ErrorCentre.ArgumentNullException("platformIds");
            }

            // create new object
            return new PlatformIdSet(id, platformIds);
        }

        #endregion

        #region Platform methods

        public string GetIdForRuntimePlatform(RuntimePlatform platform)
        {
            // find appropriate id
            string  value   = NativePluginsUtility.FindValueForPlatform(m_platformIds, platform);
            if (null == value)
            {
                Debug.LogWarning("[NativePlugins] Couldn't find platform id for " + platform);
            }

            return value;
        }

        public string GetIdForActivePlatform()
        {
            RuntimePlatform activePlatform = NativePluginsUtility.GetActivePlatform();
            return GetIdForRuntimePlatform(activePlatform);
        }

        #endregion
    }
}
