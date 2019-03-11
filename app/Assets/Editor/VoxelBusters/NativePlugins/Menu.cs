using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    internal static class Menu
    {
        #region Constants

        private const string kMenuItemPath = "Window/Voxel Busters/Native Plugins/";

        #endregion

        #region Menu items

        [MenuItem(kMenuItemPath + "Open Settings")]
        public static void OpenSettings()
        {
            NativePluginsSettings   settings    = UnityEditorUtility.LoadAssetInResourcesFolder<NativePluginsSettings>(Constants.kPluginSettingsFileName);
            // create if asset is not found
            if (null == settings)
            {
                settings    = ScriptableObject.CreateInstance<NativePluginsSettings>();
                UnityEditorUtility.CreateAssetInResourcesFolder(settings, Constants.kPluginSettingsFileName);
                AssetDatabase.Refresh();
            }

            // ping this object
            Selection.activeObject = settings;
            EditorGUIUtility.PingObject(settings);
        }

        [MenuItem(kMenuItemPath + "Uninstall")]
        public static void Uninstall()
        {
            UninstallPlugin.Uninstall();
        }

#if NATIVE_PLUGINS_DEVELOPMENT_MODE
        [MenuItem(kMenuItemPath + "Create Exporter")]
        public static void CreateExporterSettings()
        {
            string  selectedPath    = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (AssetDatabase.IsValidFolder(selectedPath))
            {
                NativeFeatureExporter   exporter    = ScriptableObject.CreateInstance<NativeFeatureExporter>();
                selectedPath                        = Path.Combine(selectedPath, "NativeFeatureExporter.asset");
                AssetDatabase.CreateAsset(exporter, selectedPath);
                AssetDatabase.Refresh();
            }
        }
#endif

        #endregion
    }
}