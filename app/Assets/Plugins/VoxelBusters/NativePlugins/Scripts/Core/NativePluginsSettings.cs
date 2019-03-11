using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    public class NativePluginsSettings : ScriptableObject
    {
        #region Static fields

        private static      NativePluginsSettings       sharedInstance                  = null;

        #endregion

        #region Fields

        [SerializeField]
        private             ApplicationSettings         m_applicationSettings           = new ApplicationSettings();
        [SerializeField]
        private             AddressBookSettings         m_addressBookSettings           = new AddressBookSettings();
        [SerializeField]
        private             MobilePopupSettings         m_mobilePopupSettings           = new MobilePopupSettings();
        [SerializeField]
        private             SharingSettings             m_sharingSettings               = new SharingSettings();
        [SerializeField]
        private             RateMyAppSettings           m_rateMyAppSettings             = new RateMyAppSettings();
        
        #endregion

        #region Properties

        public static ApplicationSettings ApplicationSettings
        {
            get
            {
                return GetSharedInstance().m_applicationSettings;
            }
        }

        public static AddressBookSettings AddressBookSettings
        {
            get
            {
                return GetSharedInstance().m_addressBookSettings;
            }
        }

        public static MobilePopupSettings MobilePopupSettings
        {
            get
            {
                return GetSharedInstance().m_mobilePopupSettings;
            }
        }

        public static SharingSettings SharingSettings
        {
            get
            {
                return GetSharedInstance().m_sharingSettings;
            }
        }

        public static RateMyAppSettings RateMyAppSettings
        {
            get
            {
                return GetSharedInstance().m_rateMyAppSettings;
            }
        }

        #endregion

        #region Private static methods

        private static NativePluginsSettings GetSharedInstance()
        {
            if (null == sharedInstance)
            {
                // check whether we are accessing in edit or play mode
                NativePluginsSettings settings  = null;
                if (Application.isPlaying)
                {
                    settings    = UnityEngineUtility.LoadAssetInPluginResourcesFolder<NativePluginsSettings>(Constants.kPluginSettingsFileNameWithoutExtension);
                }
#if UNITY_EDITOR
                else
                {
                    settings    = UnityEditorUtility.LoadAssetInResourcesFolder<NativePluginsSettings>(Constants.kPluginSettingsFileName);
                }
#endif
                if (null == settings)
                {
                    throw ErrorCentre.PluginNotConfiguredException();
                }

                // store reference
                sharedInstance = settings;
            }

            return sharedInstance;
        }

        #endregion
    }
}