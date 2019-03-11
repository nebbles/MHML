using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    public class RateMyAppDefaultValidator : MonoBehaviour, IRateMyAppValidator
    {
        #region Constants

        private const       string      kVersionLastRated          = "np-version-last-rated";
        private const       string      kShowPromptAfter           = "np-show-prompt-after";
        private const       string      kPromptLastShown           = "np-prompt-last-shown";
        private const       string      kDontShow                  = "np-dont-show";
        private const       string      kAppUsageCount             = "np-app-usage-count";

        #endregion

        #region Fields

        private     RateMyAppDefaultValidatorSettings   m_validatorSettings     = null;

        #endregion

        #region Unity methods

        private void Awake()
        {
            // initialise component
            m_validatorSettings = NativePluginsSettings.RateMyAppSettings.DefaultValidatorSettings;

            // set first time launch properties
            if (IsFirstTime())
            {
                // Set hours after which rate me is prompted for first time
                PlayerPrefs.SetInt(kShowPromptAfter, m_validatorSettings.ShowFirstPromptAfterHours);
                PlayerPrefs.SetString(kPromptLastShown, DateTime.UtcNow.ToString());
                PlayerPrefs.Save();
            }
            RecordAppLaunch();
        }

        #endregion

        #region Private methods

        private bool IsFirstTime()
        {
            return (-1 == PlayerPrefs.GetInt(kAppUsageCount, -1));
        }

        private void RecordAppLaunch()
        {
            // save to disk 
            PlayerPrefs.SetInt(kAppUsageCount, GetAppLaunchCount() + 1);
            PlayerPrefs.Save();
        }

        private int GetAppLaunchCount()
        {
            return PlayerPrefs.GetInt(kAppUsageCount, 0);
        }

        private bool CheckIfValidatorConditionsAreSatisfied()
        {
            // check for conditions
            DateTime    now                         = DateTime.UtcNow ;
            string      promptLastShownOnStr        = PlayerPrefs.GetString(kPromptLastShown);
            DateTime    promptLastShownOn           = DateTime.Parse(promptLastShownOnStr);
            float       hoursSincePromptLastShown   = (float)(now- promptLastShownOn).TotalHours;

            if (m_validatorSettings.SuccessivePromptAfterHours < hoursSincePromptLastShown)
            {
                int     appLaunchCount  = GetAppLaunchCount();
                if (appLaunchCount <= m_validatorSettings.SuccessivePromptAfterLaunches)
                {
                    PlayerPrefs.SetInt(kAppUsageCount, 0);
                    PlayerPrefs.SetString(kPromptLastShown, now.ToString());
                    return true;
                }
            }
            
            return false;
        }

        #endregion

        #region IRateMyAppValidator implementation

        public bool CanShowRateMyApp()
        {
            // check if user has denied to show
            if (PlayerPrefs.GetInt(kDontShow, 0) == 1)
            {
                return false;
            }
            
            // check if rating is provided already
            string  versionLastReviewed   = PlayerPrefs.GetString(kVersionLastRated);
            if (!string.IsNullOrEmpty(versionLastReviewed))
            {
                string  currentVersion    = ApplicationUtility.GetBundleVersion();
                // check if version matches, then it means app is already reviewed for this version
                if (currentVersion.CompareTo(versionLastReviewed) <= 0)
                {
                    return false;
                }
            }
            
            return CheckIfValidatorConditionsAreSatisfied();        
        }

        public void DidClickOnRemindLaterButton()
        {
            PlayerPrefs.SetInt(kShowPromptAfter, m_validatorSettings.SuccessivePromptAfterHours);
        }

        public void DidClickOnCancelButton()
        {
            PlayerPrefs.SetInt(kDontShow, 1);
        }

        public void DidClickOnOkButton()
        {
            string currentVersion  = ApplicationUtility.GetBundleVersion();
            PlayerPrefs.SetString(kVersionLastRated, currentVersion);
        }

        #endregion
    }
}