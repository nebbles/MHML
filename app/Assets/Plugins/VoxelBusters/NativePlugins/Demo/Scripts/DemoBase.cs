using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.NativePlugins.Demo
{
    public class DemoBase<TActionButton, TActionType> : MonoBehaviour where TActionButton : DemoActionButton<TActionType> where TActionType : struct, System.IConvertible
    {
        #region Constants

        private const string kLogCreateInstance = "Create instance by calling {0})";

        #endregion

        #region Fields

        private     ConsoleRect         m_consoleRect       = null;
        private     TActionButton[]     m_actionButtons	    = null;

        #endregion

        #region Unity methods

        protected virtual void Awake()
        {
            // set properties
            m_consoleRect   = GetComponentInChildren<ConsoleRect>();
            m_actionButtons = GetComponentsInChildren<TActionButton>();
        }

        protected virtual void Start() 
        {
            // set button property
            foreach (TActionButton actionButton in m_actionButtons)
            {
                Button button = actionButton.Button;
                button.onClick.AddListener(() => OnButtonPress(button));
            }
        }

        #endregion

        #region Protected Methods

        protected void Log(string message, bool append = true)
        {
            m_consoleRect.Log(message, append);
        }

        protected void LogMissingInstance(bool append = true)
        {
            m_consoleRect.Log(string.Format(kLogCreateInstance, GetCreateInstanceCodeSnippet()), append);
        }

        #endregion

        #region Private methods

        protected virtual string GetCreateInstanceCodeSnippet()
        {
            throw ErrorCentre.NotImplementedException();
        }

        #endregion

        #region UI callback methods

        public void OnButtonPress(Button selectedButton)
        {
            TActionButton selectedActionButton = System.Array.Find(m_actionButtons, (item) => (selectedButton == item.Button));
            if (selectedActionButton)
            {
                OnButtonPress(selectedActionButton);
            }
        }

        protected virtual void OnButtonPress(TActionButton selectedButton)
        {}

        #endregion
    }
}