using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    public class UICanvas : MonoBehaviour
    {
        #region Static properties

        internal static UICanvas Canvas
        {
            get;
            private set;
        }

        #endregion

        #region Private methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void LoadCanvas()
        {
            // create canvas
            GameObject  canvasPrefab    = UnityEngineUtility.LoadAssetInPluginResourcesFolder<GameObject>("NPCanvas");
            if (null == canvasPrefab)
            {
                throw ErrorCentre.NullReferenceException("canvasPrefab");
            }

            // instantiate copy
            Canvas = Instantiate(canvasPrefab, parent: null, worldPositionStays: false).GetComponent<UICanvas>();
            Canvas.transform.SetAsLastSibling();
        }

        #endregion
    }
}