using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.NativePlugins.Internal;

using Exception = System.Exception;
using UnityObject = UnityEngine.Object;

namespace VoxelBusters.NativePlugins
{
    internal class UnityUIAlertDialog : NativeAlertDialogInterfaceBase, INativeAlertDialogInterface
    {
        #region Static fields

        private static  GameObject                          alertDialogPrefab   = null;

        #endregion

        #region Fields

        private         GameObject                          m_dialogGO          = null;
        private         Button[]                            m_buttons           = null;

        private         string                              m_title             = null;
        private         string                              m_message           = null;
        private         int                                 m_noOfButtons       = 0;

        #endregion

        #region Constructors

        public UnityUIAlertDialog()
        {
            // set properties
            m_dialogGO  = CreateDialog();
            m_buttons   = m_dialogGO.GetComponentsInChildren<Button>();
            m_dialogGO.SetActive(false);
        }

        ~UnityUIAlertDialog()
        {
            // release
            m_buttons   = null;
        }

        #endregion

        #region Base class implementation

        public override string GetTitle()
        {
            return m_title;
        }

        public override void SetTitle(string value)
        {
            m_title     = value;
        }
            
        public override string GetMessage()
        {
            return m_message;
        }

        public override void SetMessage(string value)
        {
            m_message   = value;
        }
            
        public override void AddButton(string text, bool isCancelType)
        {
            try
            {
                // update button label
                int     buttonIndex = m_noOfButtons;
                Button  button      = m_buttons[m_noOfButtons];
                button.GetComponentInChildren<Text>().text = text;
                button.onClick.AddListener(() => onButtonClickEvent(buttonIndex));

                // update button count
                m_noOfButtons++;
            }
            catch (Exception)
            {}
        }

        public override void Show()
        {
            // set labels
            UnityEngineUtility.FindComponentInChildren<Text>(m_dialogGO, "Title").text      = m_title;
            UnityEngineUtility.FindComponentInChildren<Text>(m_dialogGO, "Message").text    = m_message;

            // hide unused buttons
            for (int iter = m_noOfButtons; iter < m_buttons.Length; iter++)
            {
                m_buttons[iter].gameObject.SetActive(false);
            }

            // enable dialog box
            m_dialogGO.SetActive(true);
        }

        public override void Dismiss()
        {
            // hide dialog box
            m_dialogGO.SetActive(false);
        }

        protected override void InvalidateInternal()
        {
            base.InvalidateInternal();

            // remove ui object
            UnityObject.Destroy(m_dialogGO);
        }

        #endregion

        #region Private static methods

        private static GameObject CreateDialog()
        {
            // find assets
            UICanvas    canvas      = UICanvas.Canvas;
            if (null == alertDialogPrefab)
            {
                alertDialogPrefab   = UnityEngineUtility.LoadAssetInPluginResourcesFolder<GameObject>("NPAlertDialog");
            }

            // create instance
            GameObject  alertClone  = UnityObject.Instantiate(alertDialogPrefab, canvas.transform, false);
            return alertClone;
        }

        #endregion
    }
}