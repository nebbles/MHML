using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.NativePlugins;

namespace VoxelBusters.NativePlugins.Demo
{
	public class ShareSheetDemo : DemoBase<ShareSheetDemoActionButton, ShareSheetDemoActionType>
	{
        #region Fields

        [SerializeField]    
        private     Texture2D           m_image                 = null;
        [SerializeField]
        private     InputField          m_textInputField        = null;
        [SerializeField]
        private     InputField          m_urlInputField         = null;
        [SerializeField]
        private     Toggle              m_isLocalPathToggle     = null;

        private     ShareSheet          m_shareSheet	        = null;

        #endregion
        
        #region Base class methods

        protected override void Start() 
        {
            base.Start();

            // set default text values
            m_textInputField.text       = DemoHelper.GetDummyBody();
        }

        #endregion

        #region UI callback methods

        protected override string GetCreateInstanceCodeSnippet()
        {
            return "new ShareSheet()";
        }

        protected override void OnButtonPress(ShareSheetDemoActionButton selectedButton)
        {
            switch (selectedButton.ActionType)
            {
                case ShareSheetDemoActionType.New:
                    m_shareSheet	= new ShareSheet();
                    m_shareSheet.SetCompletionCallback((result) =>
                    {
                        Log("Composer dismissed with result code: " + result.ResultCode);
                        Log("Releasing composer.");
                        Log("Call new() to create new instance.");
                    });
                    break;

                case ShareSheetDemoActionType.AddText:
                    if (null == m_shareSheet)
                    {
                        LogMissingInstance();
                        return;
                    }
                    m_shareSheet.AddText(m_textInputField.text);
                    break;

                case ShareSheetDemoActionType.AddScreenshot:
                    if (null == m_shareSheet)
                    {
                        LogMissingInstance();
                        return;
                    }
                    m_shareSheet.AddScreenshot();
                    break;

                case ShareSheetDemoActionType.AddImage:
                    if (null == m_shareSheet)
                    {
                        LogMissingInstance();
                        return;
                    }
                    m_shareSheet.AddImage(m_image);
                    break;

                case ShareSheetDemoActionType.AddURL:
                    if (null == m_shareSheet)
                    {
                        LogMissingInstance();
                        return;
                    }
					URLString url = m_isLocalPathToggle.isOn 
                        ? URLString.FileURLWithPath(m_urlInputField.text) 
                        : URLString.URLWithPath(m_urlInputField.text);
                    m_shareSheet.AddURL(url);
					break;

                case ShareSheetDemoActionType.Show:
                    if (null == m_shareSheet)
                    {
                        LogMissingInstance();
                        return;
                    }
                    m_shareSheet.Show();
                    break;

                case ShareSheetDemoActionType.ResourcePage:
                    Application.OpenURL(Internal.Constants.kShareSheetResourcePage);
                    break;

                default:
                    break;
            }
        }

        #endregion
	}
}
