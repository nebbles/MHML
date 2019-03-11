#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;

using UnityObject = UnityEngine.Object;

namespace VoxelBusters.NativePlugins.Internal
{
    public static class UnityEditorUtility
    {
        #region SerializationProperty methods

        public static RuntimePlatform GetRuntimePlatform()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7 
                case BuildTarget.iPhone:
                    return RuntimePlatform.iOS;
#else
                case BuildTarget.iOS:
                    return RuntimePlatform.iOS;
#endif
                case BuildTarget.tvOS:
                    return RuntimePlatform.tvOS;

                case BuildTarget.Android:
                    return RuntimePlatform.Android;

                default:
                    return RuntimePlatform.Unknown;
            }
        }

        #endregion

        #region Resources methods

        public static void CreateAssetInResourcesFolder(UnityObject asset, string fileName)
        {
            string path = string.Format("{0}/{1}", Constants.kResourcesPath, fileName);
            AssetDatabase.CreateAsset(asset, path);
        }

        public static T LoadAssetInResourcesFolder<T>(string fileName) where T : UnityObject
        {
            string path = string.Format("{0}/{1}", Constants.kResourcesPath, fileName);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public static T LoadAssetInEditorResourcesFolder<T>(string fileName) where T : UnityObject
        {
            string path = string.Format("{0}/{1}", Constants.kEditorResourcesPath, fileName);
            return (T)EditorGUIUtility.Load(path);
        }

        #endregion

        #region Plist extensions

        public static bool TryGetElement<T>(this PlistElementDict dict, string key, out T element) where T : PlistElement
        {
            IDictionary<string, PlistElement>   dictionary  = dict.values;
            PlistElement                        value;
            if (dictionary.TryGetValue(key, out value))
            {
                element = (T)value;
                return true;
            }

            element     = default(T);
            return false;
        }

        public static bool Contains(this PlistElementArray array, string value)
        {
            List<PlistElement>   valueList  = array.values;
            for (int iter = 0; iter < valueList.Count; iter++)
            {
                if (valueList[iter].AsString() == value)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
#endif