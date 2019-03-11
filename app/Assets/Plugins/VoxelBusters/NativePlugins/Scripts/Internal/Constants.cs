using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    public static class Constants
    {
        #region Constants

        // file paths
        public const string kPluginAndroidProjectPath                   = "Assets/Plugins/Android/cross_platform_native_plugins/";
        public const string kPluginiOSSourcePath                        = "Assets/Plugins/iOS/VoxelBusters/NativePlugins";
        public const string kPluginCodebasePath                         = "Assets/Plugins/VoxelBusters/NativePlugins";
        public const string kPluginEditorPath                           = "Assets/Editor/VoxelBusters/NativePlugins";
        public const string kEditorResourcesPath                        = kPluginEditorPath + "/EditorResources";
        public const string kResourcesPath                              = kPluginCodebasePath + "/Resources";

        // file names
        public const string kPluginSettingsFileName                     = "NativePluginsSettings.asset";
        public const string kPluginSettingsFileNameWithoutExtension     = "NativePluginsSettings";
        
        // product information
        public const string kProductDisplayName                         = "Cross Platform Native Plugins";
        public const string kProductVersion                             = "Version 1.0";
        public const string kProductCopyrights                          = "Copyright © 2019 Voxel Busters Interactive LLP.";

        // website links
        public const string kAddressBookResourcePage                    = "https://assetstore.nativeplugins.voxelbusters.com";
        public const string kAlertDialogResourcePage                    = "https://assetstore.nativeplugins.voxelbusters.com";
        public const string kMailComposerResourcePage                   = "https://assetstore.nativeplugins.voxelbusters.com";
        public const string kMessageComposerResourcePage                = "https://assetstore.nativeplugins.voxelbusters.com";
        public const string kShareSheetResourcePage                     = "https://assetstore.nativeplugins.voxelbusters.com";
        public const string kSocialShareComposerResourcePage            = "https://assetstore.nativeplugins.voxelbusters.com";
        public const string kRateMyAppResourcePage                      = "https://assetstore.nativeplugins.voxelbusters.com";

        #endregion
    }
}