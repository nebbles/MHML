using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace VoxelBusters.NativePlugins.Internal
{
    public static class UnityEngineUtility
    {
        #region Texture2d methods

        public static string GetMimeType(TextureEncoding encoding)
        {
            switch (encoding)
            {
                case TextureEncoding.ToJPG:
                    return MimeType.kJPGImage;

                case TextureEncoding.ToPNG:
                    return MimeType.kPNGImage;

                default:
                    throw ErrorCentre.SwitchCaseNotImplementedException(encoding);
            }
        }

        public static byte[] EncodeTexture(this Texture2D texture, out string mimeType)
        {
            switch (texture.format)
            {
                case TextureFormat.Alpha8:
                case TextureFormat.ARGB32:
                case TextureFormat.ARGB4444:
                case TextureFormat.RGBA32:
                case TextureFormat.RGBA4444:
                case TextureFormat.BGRA32:
                case TextureFormat.RGBAHalf:
                case TextureFormat.RGBAFloat:
                case TextureFormat.PVRTC_RGBA2:
                case TextureFormat.PVRTC_RGBA4:
                    mimeType = MimeType.kPNGImage;
                    return texture.EncodeToPNG();

                default:
                    mimeType = MimeType.kJPGImage;
                    return texture.EncodeToJPG();
            }
        }

        public static byte[] EncodeTexture(this Texture2D texture, TextureEncoding encoding)
        {
            switch (encoding)
            {
                case TextureEncoding.ToJPG:
                    return ImageConversion.EncodeToJPG(texture);

                case TextureEncoding.ToPNG:
                    return ImageConversion.EncodeToPNG(texture);

                default:
                    throw new NotImplementedException(encoding.ToString());
            }
        }

        public static T FindComponentInChildren<T>(GameObject gameObject, string name)
        {
            return gameObject.transform.Find(name).GetComponent<T>();
        }

        public static string TakeScreenshot(string fileName)
        {
            string filePath = Application.persistentDataPath + "/" + fileName;

            // Delete existing file
            FileUtility.Delete(filePath);

            // Start Capturing
            ScreenCapture.CaptureScreenshot(fileName);

            return filePath;
        }

        public static Transform[] GetImmediateChildren(GameObject gameObject)
        {
            int         childCount  = gameObject.transform.childCount;
            Transform[] children    = new Transform[childCount];
            for (int iter = 0; iter < childCount; iter++)
            {
                children[iter]      = gameObject.transform.GetChild(iter);
            }
            return children;
        }

        #endregion

        #region Resource methods

        public static T LoadAssetInPluginResourcesFolder<T>(string name) where T : UnityObject
        {
            return Resources.Load<T>(name);
        }

        #endregion

        #region Screen methods

        public static Vector2 InvertScreenPosition(Vector2 position, bool invertX = false, bool invertY = false)
        {
            if (invertX)
            {
                position.x  = Screen.width - position.x;
            }
            if (invertY)
            {
                position.y  = Screen.height - position.y;
            }

            return position;
        }

        #endregion
    }
}