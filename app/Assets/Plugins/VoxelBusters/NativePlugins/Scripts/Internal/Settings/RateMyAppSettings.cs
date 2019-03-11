using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    [Serializable]
    public class RateMyAppSettings : NativeFeatureSettingsBase
    {
        #region Fields

        [SerializeField]
        private         string                              m_promptTitle                   = "Rate My App";
        [SerializeField, TextArea]
        private         string                              m_promptDescription             = "If you enjoy using Native Plugins would you mind taking a moment to rate it? It wont take more than a minute. Thanks for your support.";
        [SerializeField]
        private         string                              m_okButtonLabel                 = "Ok";
        [SerializeField]
        private         string                              m_cancelButtonLabel             = "Cancel";
        [SerializeField]
        private         string                              m_remindLaterButtonLabel        = "Remind Me Later";
        [SerializeField]
        private         bool                                m_canShowRemindMeLaterButton    = true;
        [SerializeField]
        private         bool                                m_usesCustomValidator           = false;
        [SerializeField]
        private         RateMyAppDefaultValidatorSettings   m_defaultValidatorSettings      = new RateMyAppDefaultValidatorSettings();

        #endregion

        #region Properties

        public string PromptTitle
        {
            get
            {
                return m_promptTitle;
            }
        }

        public string PromptDescription
        {
            get
            {
                return m_promptDescription;
            }
        }

        public string OkButtonLabel
        {
            get
            {
                return m_okButtonLabel;
            }
        }

        public string CancelButtonLabel
        {
            get
            {
                return m_cancelButtonLabel;
            }
        }

        public string RemindLaterButtonLabel
        {
            get
            {
                return m_remindLaterButtonLabel;
            }
        }

        public bool CanShowRemindMeLaterButton
        {
            get
            {
                return m_canShowRemindMeLaterButton;
            }
        }

        public bool UsesCustomValidator
        {
            get
            {
                return m_usesCustomValidator;
            }
        }

        public RateMyAppDefaultValidatorSettings DefaultValidatorSettings
        {
            get
            {
                return m_defaultValidatorSettings;
            }
        }

        #endregion
    }
}