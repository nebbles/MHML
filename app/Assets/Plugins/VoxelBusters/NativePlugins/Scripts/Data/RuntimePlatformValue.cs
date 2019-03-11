using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// The RuntimePlatformValue class represents an immutable, read-only object that combines a string value with a platform.
    /// </summary>
    [Serializable]
    public class RuntimePlatformValue
    {
        #region Fields

        [SerializeField]
        private     RuntimePlatform     m_platform      = RuntimePlatform.Unknown;
        [SerializeField]
        private     string              m_value         = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the runtime platform associated with string value.
        /// </summary>
        /// <value>The enum value indicates the platform to which string value belongs.</value>
        public RuntimePlatform Platform
        {
            get
            {
                return m_platform;
            }
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public string Value
        {
            get
            {
                return m_value;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns a new instance of <see cref="RuntimePlatformValue"/>, containing a string value functional only on iOS platform.
        /// </summary>
        /// <returns>The instance of <see cref="RuntimePlatformValue"/>.</returns>
        /// <param name="value">The string value associated with iOS platform.</param>
        public static RuntimePlatformValue iOS(string value)
        {
            return new RuntimePlatformValue()
            {
                m_platform  = RuntimePlatform.iOS,
                m_value     = value,
            };
        }

        /// <summary>
        /// Returns a new instance of <see cref="RuntimePlatformValue"/>, containing a string value functional only on tvOS platform.
        /// </summary>
        /// <returns>The instance of <see cref="RuntimePlatformValue"/>.</returns>
        /// <param name="value">The string value associated with tvOS platform.</param>
        public static RuntimePlatformValue tvOS(string value)
        {
            return new RuntimePlatformValue()
            {
                m_platform  = RuntimePlatform.tvOS,
                m_value     = value,
            };
        }

        /// <summary>
        /// Returns a new instance of <see cref="RuntimePlatformValue"/>, containing a string value functional only on Android platform.
        /// </summary>
        /// <returns>The instance of <see cref="RuntimePlatformValue"/>.</returns>
        /// <param name="value">The string value associated with Android platform.</param>
        public static RuntimePlatformValue Android(string value)
        {
            return new RuntimePlatformValue()
            {
                m_platform  = RuntimePlatform.Android,
                m_value     = value,
            };
        }

        /// <summary>
        /// Returns a new instance of <see cref="RuntimePlatformValue"/>, containing a string value functional on all supported platform.
        /// </summary>
        /// <returns>The instance of <see cref="RuntimePlatformValue"/>.</returns>
        /// <param name="value">The string value associated with all supported platforms.</param>
        public static RuntimePlatformValue All(string value)
        {
            return new RuntimePlatformValue()
            {
                m_platform  = RuntimePlatform.All,
                m_value     = value,
            };
        }

        #endregion

        #region Public methods

        public bool IsEqualToPlatform(RuntimePlatform other)
        {
            return ((other & m_platform) != 0);
        }

        #endregion

        #region Object methods

        public override string ToString()
        {
            return m_value;
        }

        #endregion
    }
}