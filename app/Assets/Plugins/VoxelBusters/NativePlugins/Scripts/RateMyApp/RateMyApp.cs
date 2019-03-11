using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// The RateMyApp class provides an unique way to prompt user to review the app. 
    /// </summary>
    /// <description>
    /// By default, prompt system makes use of configuration available in RateMyApp section of NativePluginsSettings. 
    /// These values can be adjusted according to developer preference.
    /// </description>
    public class RateMyApp : MonoBehaviour
    {
        #region Static fields

        private     static  RateMyApp               sharedInstance      = null;
        private     static  IRateMyAppValidator     validator           = null;
        private     static  RateMyAppSettings       settings            = null;
        private     static  bool                    isShowingPrompt     = false;
        
        #endregion

        #region Load methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AddRateMyAppToScene()
        {
            if (null == sharedInstance)
            {
                RateMyAppSettings   settingsLocal   = NativePluginsSettings.RateMyAppSettings;
                if (settingsLocal.IsEnabled)
                {
                    if (null == FindObjectOfType<RateMyApp>())
                    {
                        CreateInstance();
                    }
                }
            }
        }

        private static GameObject CreateInstance()
        {
            GameObject  prefab  = UnityEngineUtility.LoadAssetInPluginResourcesFolder<GameObject>("RateMyApp");
            if (null == prefab)
            {
                throw ErrorCentre.NullReferenceException("prefab");
            }
            return Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Immediately prompts user to review. This method ignores IRateMyAppValidator conditions to be satisfied.
        /// </summary>
        public static void AskForReviewNow()
        {
            if (false == NativePluginsSettings.RateMyAppSettings.IsEnabled)
            {
                throw ErrorCentre.FeatureNotAccessibleException(featureName: "Rate My App");
            }
            if (null == sharedInstance)
            {
                throw ErrorCentre.FeatureIsNotReadyException();
            }

            ShowPromptWindow();
        }

        #endregion

        #region Unity methods

        private void Awake()
        {
            // check whether this feature is available
            if (false == NativePluginsSettings.RateMyAppSettings.IsEnabled)
            {
                throw ErrorCentre.FeatureNotAccessibleException(featureName: "Rate My App");
            }
            
            // check whether this instance is duplicate
            if (sharedInstance != null)
            {
                Destroy(gameObject);
                return;
            }

            // check whether this feature is available for use
            settings    = NativePluginsSettings.RateMyAppSettings;
            if (false == settings.IsEnabled)
            {
                throw ErrorCentre.FeatureNotAccessibleException(featureName: "RateMyApp");
            }

            // check whether validator is added
            IRateMyAppValidator attachedValidator   = (settings.UsesCustomValidator) 
                ? GetComponent<IRateMyAppValidator>()
                : gameObject.AddComponent<RateMyAppDefaultValidator>();
            if (null == attachedValidator)
            {
                throw ErrorCentre.NullReferenceException(variableName: "customValidator");
            }

            // update properties
            sharedInstance  = this;
            validator       = attachedValidator;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (isShowingPrompt)
            {
                return;
            }
            if (validator.CanShowRateMyApp())
            {
                ShowPromptWindow();
            }
        }

        #endregion

        #region Private methods

        private static void ShowPromptWindow()
        {
            // mark that we are showing window
            isShowingPrompt = true;

            // create prompt
            AlertDialog alertDialog = new AlertDialog()
                .SetTitle(settings.PromptTitle)
                .SetMessage(settings.PromptDescription)
                .AddButton(settings.OkButtonLabel, () => OnPromptButtonPressed(PromptButtonType.Ok))
                .AddCancelButton(settings.CancelButtonLabel, () => OnPromptButtonPressed(PromptButtonType.Cancel));
            if (settings.CanShowRemindMeLaterButton)
            {
                alertDialog.AddButton(settings.RemindLaterButtonLabel, () => OnPromptButtonPressed(PromptButtonType.RemindLater));
            }
            alertDialog.Show();
        }

        private static void OnPromptButtonPressed(PromptButtonType selectedButtonType)
        {
            // reset flag
            isShowingPrompt = false;
            switch (selectedButtonType)
            {
                case PromptButtonType.RemindLater:
                    validator.DidClickOnRemindLaterButton();
                    break;

                case PromptButtonType.Cancel:
                    validator.DidClickOnCancelButton();
                    break;

                case PromptButtonType.Ok:
                    validator.DidClickOnOkButton();
                    ShowReviewWindow();
                    break;
            }
        }

        private static void ShowReviewWindow()
        {
#if UNITY_EDITOR
            ShowReviewWindow_Editor();
#elif UNITY_IOS
            ShowReviewWindow_iOS();
            return;
#endif
        }

        #endregion

        #region Platform specific methods

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern bool NPStoreReviewCanUseDeepLinking();
        
        [DllImport("__Internal")]
        private static extern void NPStoreReviewRequestReview();

        private static void ShowReviewWindow_iOS()
        {
            if (NPStoreReviewCanUseDeepLinking())
            {
                string  storeId     = NativePluginsSettings.ApplicationSettings.GetAppStoreIdForActivePlatform();
                string  storeURL    = string.Format("itms-apps://itunes.apple.com/app/id{0}?action=write-review", storeId);
                Application.OpenURL(storeURL);
            }
            else
            {
                NPStoreReviewRequestReview();
            }
        }
#endif

#if UNITY_EDITOR
        private static void ShowReviewWindow_Editor()
        {}
#endif

        #endregion

        #region Nested types

        private enum PromptButtonType
        {
            RemindLater,
            Cancel,
            Ok,
        }

        #endregion
    }
}