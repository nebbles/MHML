using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// The AlertDialog class provides an interface to display an alert message to the user.
    /// </summary>
    /// <example>
    /// The following code example shows how to configure and present an alert dialog.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.NativePlugins;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     public void Start()
    ///     {
    ///         
    ///         new AlertDialog()
    ///             .SetTitle(title)
    ///             .SetMessage(message)
    ///             .AddButton(button, OnAlertButtonClicked)
    ///             .Show();
    ///     }
    /// 
    ///     private void OnAlertButtonClicked()
    ///     {
    ///         // add your code
    ///     }
    /// }
    /// </code>
    /// </example>
    public class AlertDialog
    {
        #region Fields

        private     INativeAlertDialogInterface     m_nativeInterface;
        private     List<ButtonMeta>                m_buttonMetaList;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the title of the alert.
        /// </summary>
        /// <value>The title of the alert.</value>
        public string Title
        {
            get
            {
                return m_nativeInterface.GetTitle();
            }
        }

        /// <summary>
        /// Gets the message of the alert.
        /// </summary>
        /// <value>The message of the alert.</value>
        public string Message
        {
            get
            {
                return m_nativeInterface.GetMessage();
            }
        }

        #endregion

        #region Constructors

        static AlertDialog()
        {
            // check whether this feature can be used
            MobilePopupSettings settings = NativePluginsSettings.MobilePopupSettings;
            if (false == settings.IsEnabled)
            {
                throw ErrorCentre.FeatureNotAccessibleException(featureName: "Mobile Popup");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertDialog"/> class.
        /// </summary>
        public AlertDialog()
        {
            MobilePopupSettings settings = NativePluginsSettings.MobilePopupSettings;

            // initialise properties
            m_nativeInterface   = CreateNativeInterface();
            m_buttonMetaList    = new List<ButtonMeta>(capacity: 3);

            // register for events
            m_nativeInterface.onButtonClick += HandleOnButtonClick;

            // retain object to avoid unncessary release
            this.RetainObject();
        }

        ~AlertDialog()
        {
            m_nativeInterface   = null;
            m_buttonMetaList.Clear();
            m_buttonMetaList    = null;
        }

        #endregion

        #region Create methods

        /// <summary>
        /// Creates a new alert dialog with specified values.
        /// </summary>
        /// <param name="title">The title of the alert.</param>
        /// <param name="message">The descriptive text that provides more details.</param>
        /// <param name="button">The title of the button.</param>
        /// <param name="callback">The method to execute when the user selects this button.</param>
        public static void Show(string title, string message, string button = "Ok", GenericCallback callback = null)
        {
            new AlertDialog()
                .SetTitle(title)
                .SetMessage(message)
                .AddButton(button, callback)
                .Show();
        }

        #endregion

        #region Setter methods

        /// <summary>
        /// Sets the title of the alert.
        /// </summary>
        /// <param name="value">The title of the alert.</param>
        public AlertDialog SetTitle(string value)
        {
            m_nativeInterface.SetTitle(value);
            return this;
        }

        /// <summary>
        /// Sets the message of the alert.
        /// </summary>
        /// <param name="value">The descriptive text that provides more details about the reason for the alert.</param>
        public AlertDialog SetMessage(string value)
        {
            m_nativeInterface.SetMessage(value);
            return this;
        }

        /// <summary>
        /// Attaches an action button to the alert. Here, the default style is used.
        /// </summary>
        /// <param name="text">The title of the button.</param>
        /// <param name="callback">The method to execute when the user selects this button.</param>
        public AlertDialog AddButton(string text, GenericCallback callback)
        {
            if (null == text)
            {
                throw new ArgumentNullException("text");
            }

            m_buttonMetaList.Add(new ButtonMeta() { label = text, onClick = callback });
            m_nativeInterface.AddButton(text, false);
            return this;
        }

        /// <summary>
        /// Attaches action button to the alert. This style type indicates the action cancels the operation and leaves things unchanged.
        /// </summary>
        /// <param name="text">The title of the button.</param>
        /// <param name="callback">The method to execute when the user selects this button.</param>
        public AlertDialog AddCancelButton(string text, GenericCallback callback)
        {
            if (null == text)
            {
                throw new ArgumentNullException("text");
            }

            m_buttonMetaList.Add(new ButtonMeta() { label = text, onClick = callback });
            m_nativeInterface.AddButton(text, true);
            return this;
        }

        /// <summary>
        /// Shows the alert dialog to the user.
        /// </summary>
        public void Show()
        {
            m_nativeInterface.Show();
        }

        /// <summary>
        /// Dismisses the alert dialog before user selects an action.
        /// </summary>
        public void Dismiss()
        {
            m_nativeInterface.Dismiss();
            Invalidate();
        }

        #endregion

        #region Private methods

        private static INativeAlertDialogInterface CreateNativeInterface()
        {
#if UNITY_EDITOR
            return new UnityUIAlertDialog();
#elif UNITY_IOS
            return new IOSAlertDialogInterface();
#elif UNITY_ANDROID
            return new AndroidAlertDialogInterface();
#else
            return null;
#endif
        }

        private void Invalidate()
        {
            // reset state
            this.ReleaseObject();

            // unregister from event
            m_nativeInterface.onButtonClick   -= HandleOnButtonClick;
            
            // reset interface properties
            ((NativeInterfaceObject)m_nativeInterface).Invalidate();
        }

        #endregion

        #region Event callback methods

        private void HandleOnButtonClick(int selectedButtonIndex)
        {
            try
            {
                GenericCallback onClickCallback = m_buttonMetaList[selectedButtonIndex].onClick;
                onClickCallback();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                Dismiss();
            }
        }

        #endregion

        #region Nested types

        private struct ButtonMeta
        {
            public  string              label;
            public  GenericCallback     onClick;
        }

        #endregion
    }
}