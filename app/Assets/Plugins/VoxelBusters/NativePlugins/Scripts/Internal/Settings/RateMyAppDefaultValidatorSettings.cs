using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    [Serializable]
    public class RateMyAppDefaultValidatorSettings
    {
        #region Fields

        [SerializeField]
        [Tooltip("The number of hours since first launch, after which user is prompted to rate the app.")]
        private     int         m_showFirstPromptAfterHours             = 2;
        [SerializeField]
        [Tooltip("The number of hours since last time we showed the prompt, after which user is prompted to rate the app.")]
        private     int         m_showSuccessivePromptAfterHours        = 6;
        [SerializeField]
        [Tooltip("The number of times the user must launch the app, after which user is prompted to rate the app.")]
        private     int         m_showSuccessivePromptAfterLaunches     = 5;

        #endregion

        #region Properties

        public int ShowFirstPromptAfterHours
        {
            get
            {
                return m_showFirstPromptAfterHours;
            }
        }

        public int SuccessivePromptAfterHours
        {
            get
            {
                return m_showSuccessivePromptAfterHours;
            }
        }

        public int SuccessivePromptAfterLaunches
        {
            get
            {
                return m_showSuccessivePromptAfterLaunches;
            }
        }

        #endregion
    }
}