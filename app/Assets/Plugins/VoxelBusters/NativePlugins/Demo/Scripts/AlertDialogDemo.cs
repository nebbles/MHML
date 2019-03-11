using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Demo
{
    public class AlertDialogDemo : DemoBase<AlertDialogDemoActionButton, AlertDialogDemoActionType>
    {
       #region Fields

        [SerializeField]
        private     InputField          m_titleInputField               = null;
        [SerializeField]    
        private     InputField          m_messageInputField             = null;
        [SerializeField]
        private     InputField          m_buttonLabelInputField         = null;
        [SerializeField]
        private     InputField          m_cancelButtonLabelInputField   = null;

        private     AlertDialog         m_activeDialog                  = null;

        #endregion

        #region Base methods

        protected override void Start() 
        {
            base.Start();

            // set default text values
            m_titleInputField.text              = "Demo";
            m_messageInputField.text            = DemoHelper.GetDummyBody();
            m_buttonLabelInputField.text        = "Ok";
            m_cancelButtonLabelInputField.text  = "Cancel";
        }

        protected override string GetCreateInstanceCodeSnippet()
        {
            return "new AlertDialog()";
        }

        protected override void OnButtonPress(AlertDialogDemoActionButton selectedButton)
        {
            switch (selectedButton.ActionType)
            {
               case AlertDialogDemoActionType.New:
                    m_activeDialog = new AlertDialog();
                    break;

                case AlertDialogDemoActionType.SetTitle:
                    if (null == m_activeDialog)
                    {
                        LogMissingInstance();                        
                        return;
                    }
                    m_activeDialog.SetTitle(m_titleInputField.text);
                    break;

                case AlertDialogDemoActionType.GetTitle:
                    if (null == m_activeDialog)
                    {
                        LogMissingInstance();                        
                        return;
                    }
                    Log("Alert title: " + m_activeDialog.Title);
                    break;

                case AlertDialogDemoActionType.SetMessage:
                    if (null == m_activeDialog)
                    {
                        LogMissingInstance();                        
                        return;
                    }
                    m_activeDialog.SetMessage(m_messageInputField.text);
                    break;

                case AlertDialogDemoActionType.GetMessage:
                    if (null == m_activeDialog)
                    {
                        LogMissingInstance();                        
                        return;
                    }
                    Log("Alert message: " + m_activeDialog.Message);
                    break;

                case AlertDialogDemoActionType.AddButton:
                    if (null == m_activeDialog)
                    {
                        LogMissingInstance();                        
                        return;
                    }
                    string buttonLabel0 = m_buttonLabelInputField.text;
                    m_activeDialog.AddButton(buttonLabel0, () => Log(string.Format("Button with label: {0} is selected", buttonLabel0)));
                    break;

                case AlertDialogDemoActionType.AddCancelButton:
                    if (null == m_activeDialog)
                    {
                        LogMissingInstance();                        
                        return;
                    }
                    string buttonLabel1 = m_cancelButtonLabelInputField.text;
                    m_activeDialog.AddCancelButton(buttonLabel1, () => Log(string.Format("Button with label: {0} is selected", buttonLabel1)));
                    break;

                case AlertDialogDemoActionType.Show:
                    if (null == m_activeDialog)
                    {
                        LogMissingInstance();                        
                        return;
                    }
                    m_activeDialog.Show();
                    break;

                case AlertDialogDemoActionType.ResourcePage:
                    Application.OpenURL(Internal.Constants.kAlertDialogResourcePage);
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}