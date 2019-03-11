using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal static class ErrorCentre
    {
        #region Exception methods

        public static Exception PluginNotConfiguredException()
        {
            return new Exception("Please configure your NativePlugins before you start using it in your project.");
        }

        public static Exception CreateNativeObjectFailedException()
        {
            return new Exception("Failed to create native object.");
        }

        public static Exception SwitchCaseNotImplementedException(object value)
        {
            return new NullReferenceException(string.Format("Switch case for {0} is not implemented.", value));
        }

        public static Exception ArgumentNullException(string variableName)
        {
            return new ArgumentNullException(variableName);
        }

        public static Exception NullReferenceException(string variableName)
        {
            return new NullReferenceException(variableName);
        }

        public static Exception NotImplementedException()
        {
            return new NotImplementedException();
        }

        public static Exception NotSupportedInEditorException()
        {
            return new Exception("This feature is not supported by simulator.");
        }

        public static Exception FeatureNotAccessibleException(string featureName = "This")
        {
            return new Exception(string.Format("{0} feature is marked as not required. Please use NativePluginsSettings for enabling it.", featureName));
        }

        public static Exception FeatureIsNotReadyException()
        {
            return new Exception("This feature is not yet ready.");
        }

        public static Exception CreateSingletonFailedException()
        {
            return new Exception("You cannot create multiple instances of singleton type.");
        }

        public static Exception ObjectInvalidatedException()
        {
            return new Exception("The user is trying to invalidate object which is already invalidated.");
        }

        public static Exception ObjectNotFound()
        {
            return new NotImplementedException("The requested object not found.");
        }

        public static Exception ObjectNotFound(string name)
        {
            return new NotImplementedException(string.Format("{0} object not found.", name));
        }

        #endregion

        #region Print methods

        public static void LogNotSupportedInEditor(string featureName = "This")
        {
            Debug.LogWarning(string.Format("[CPNP] {0} feature is not supported by simulator.", featureName));
        }

        public static void LogNotSupported(string featureName = "This")
        {
            Debug.LogWarning(string.Format("[CPNP] {0} feature is not supported.", featureName));
        }

        #endregion
    }
}