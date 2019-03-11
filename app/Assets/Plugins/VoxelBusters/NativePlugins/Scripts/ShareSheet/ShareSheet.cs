using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// The ShareSheet class provides an interface to access standard services from your app.
    /// </summary>
    /// <description>
    /// <para>
    /// The system provides several standard services, such as copying items to the pasteboard, posting content to social media sites, sending items via email or SMS, and more. 
    /// </para>
    /// </description>
    /// <example>
    /// The following code example shows how to use share sheet.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.NativePlugins;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     public void Start()
    ///     {
    ///         new ShareSheet()
    ///             .AddText("Example")
    ///             .AddScreenshot()
    ///             .SetCompletionCallback(OnShareSheetClosed)
    ///             .Show();
    ///     }
    /// 
    ///     private void OnShareSheetClosed(IShareSheetClosedCallbackResult result)
    ///     {
    ///         // add your code
    ///     }
    /// }
    /// </code>
    /// </example>
    public class ShareSheet
    {
        #region Fields

        private     INativeShareSheetInterface                          m_nativeInterface       = null;
        private     GenericCallback<IShareSheetClosedCallbackResult>    m_callback              = null;

        #endregion

        #region Constructors

        static ShareSheet()
        {
            // check whether this feature can be used
            SharingSettings settings = NativePluginsSettings.SharingSettings;
            if ((false == settings.IsEnabled) || (false == settings.UsesShareSheet))
            {
                throw ErrorCentre.FeatureNotAccessibleException(featureName: "Share Sheet");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareSheet"/> class.
        /// </summary>
        public ShareSheet()
        {            
            // set properties
            m_nativeInterface   = CreateNativeInterface();
            m_callback          = null;

            // register for events
            m_nativeInterface.onClose  += HandleOnClose;

            // retain object
            this.RetainObject();
        }

        ~ShareSheet()
        {
            // unset properties
            m_nativeInterface   = null;
            m_callback          = null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the initial text to the share sheet.
        /// </summary>
        /// <param name="value">The text to add.</param>
        public ShareSheet AddText(string value)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            m_nativeInterface.AddText(value);
            return this;
        }

        /// <summary>
        /// Creates a screenshot and adds it to the share sheet.
        /// </summary>
        public ShareSheet AddScreenshot()
        {
            m_nativeInterface.AddScreenshot();
            return this;
        }

        /// <summary>
        /// Adds the specified image to the share sheet.
        /// </summary>
        /// <param name="image">The image to add.</param>
        public ShareSheet AddImage(Texture2D image)
        {
            if (null == image)
            {
                throw new ArgumentNullException("image");
            }

            m_nativeInterface.AddImage(image);
            return this;
        }

        /// <summary>
        /// Adds the URL to the share sheet.
        /// </summary>
        /// <param name="url">The URL to add.</param>
        public ShareSheet AddURL(URLString url)
        {
            if (false == url.IsValid)
            {
                Debug.LogError("[CPNP] Url provided is invalid.");
            }
            else
            {
                m_nativeInterface.AddURL(url);
            }
            return this;
        }

        /// <summary>
        /// Specify the action to execute after the share sheet is dismissed.
        /// </summary>
        /// <param name="callback">The action to be called on completion.</param>
        public ShareSheet SetCompletionCallback(GenericCallback<IShareSheetClosedCallbackResult> callback)
        {
            // save callback reference
            m_callback = callback;
            return this;
        }

        /// <summary>
        /// Shows the share sheet interface, anchored at screen position (0, 0).
        /// </summary>
        public void Show()
        {
            Show(Vector2.zero);
        }

        /// <summary>
        /// Shows the share sheet interface, anchored to given position.
        /// </summary>
        /// <param name="screenPosition">The position (in the coordinate system of screen) at which to anchor the share sheet menu. This property is used in iOS platform only.</param>
        public void Show(Vector2 screenPosition)
        {
            m_nativeInterface.Show(screenPosition);
        }

        #endregion

        #region Private methods

        private static INativeShareSheetInterface CreateNativeInterface()
        {
#if UNITY_EDITOR
            return new EditorShareSheetInterface();
#elif UNITY_IOS
            return new IOSShareSheetInterface();
#elif UNITY_ANDROID
            return new AndroidShareSheetInterface();
#else
            return null;
#endif
        }

        private void Invalidate()
        {
            // reset state
            this.ReleaseObject();

            // unregister from event
            m_nativeInterface.onClose   -= HandleOnClose;
            
            // reset interface properties
            ((NativeInterfaceObject)m_nativeInterface).Invalidate();
        }

        #endregion

        #region Event callback methods

        private void HandleOnClose(IShareSheetClosedCallbackResult result)
        {
            // send callback to original object
            if (m_callback != null)
            {
                m_callback(result);
            }

            Invalidate();
        }

        #endregion
    }
}