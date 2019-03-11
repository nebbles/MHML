using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    [CustomEditor(typeof(NativePluginsSettings))]
    public class NativePluginsSettingsEditor : Editor
    {
        #region Fields

        // internal properties
        private     SerializedProperty      m_appSettingsProperty           = null;
        private     SettingsGroupMeta[]     m_settingsGroupMetaArray        = new SettingsGroupMeta[]
        {
            new SettingsGroupMeta() { displayName = "Address Book",     serializedPropertyName = "m_addressBookSettings", afterDrawInternal = DrawAddressBookMenuControls, beforeDrawInternal = null },
            new SettingsGroupMeta() { displayName = "Billing",          serializedPropertyName = "m_billingSettings" },
            new SettingsGroupMeta() { displayName = "Cloud Services",   serializedPropertyName = "m_cloudServicesSettings" },
            new SettingsGroupMeta() { displayName = "Game Services",    serializedPropertyName = "m_gameServicesSettings" },
            new SettingsGroupMeta() { displayName = "Network Services", serializedPropertyName = "m_networkServicesSettings" },
            new SettingsGroupMeta() { displayName = "Mobile Popup",     serializedPropertyName = "m_mobilePopupSettings" },
            new SettingsGroupMeta() { displayName = "Sharing",          serializedPropertyName = "m_sharingSettings" },
            new SettingsGroupMeta() { displayName = "Rate My App",      serializedPropertyName = "m_rateMyAppSettings" },
        };
        private     SerializedProperty[]    m_settingsProperties            = null;
        private     int                     m_settingsPropertyCount         = 0;

        // custom gui styles
        private     GUIStyle                m_groupBackgroundStyle          = null;
        private     GUIStyle                m_headerStyle                   = null;
        private     GUIStyle                m_headerFoldoutStyle            = null;
        private     GUIStyle                m_headerLabelStyle              = null;
        private     GUIStyle                m_headerToggleStyle             = null;

        // assets
        private     Texture2D               m_logoIcon                      = null;
        private     Texture2D               m_toggleOnIcon                  = null;
        private     Texture2D               m_toggleOffIcon                 = null;

        #endregion

        #region Unity methods

        private void OnEnable()
        {
            // set properties
            m_appSettingsProperty   = serializedObject.FindProperty("m_applicationSettings");
            m_settingsProperties    = Array.ConvertAll(m_settingsGroupMetaArray, (element) => serializedObject.FindProperty(element.serializedPropertyName));
            m_settingsPropertyCount = m_settingsProperties.Length;

            // load assets
            m_logoIcon              = UnityEditorUtility.LoadAssetInEditorResourcesFolder<Texture2D>("logo.png");
            m_toggleOnIcon          = UnityEditorUtility.LoadAssetInEditorResourcesFolder<Texture2D>("toggleOn.png");
            m_toggleOffIcon         = UnityEditorUtility.LoadAssetInEditorResourcesFolder<Texture2D>("toggleOff.png");
        }

        public override void OnInspectorGUI()
        {
            LoadStyles();

            // draw controls
            DrawProductInfoSection();
            EditorGUI.BeginChangeCheck();
            DrawControlGroup(m_appSettingsProperty, "Application Settings");
            for (int iter = 0; iter < m_settingsPropertyCount; iter++)
            {
                SerializedProperty currentProperty = m_settingsProperties[iter];
                if (null == currentProperty)
                {
                    continue;
                }
                DrawSettingsGroup(settingsProperty: currentProperty, settingsMeta: m_settingsGroupMetaArray[iter]);
            }
            // save changes
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }

        #endregion

        #region Product info methods

        private void DrawProductInfoSection()
        {
            GUILayout.BeginHorizontal(m_groupBackgroundStyle);

            // logo section
            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(m_logoIcon, GUILayout.Height(64f), GUILayout.Width(64f));
            GUILayout.Space(2f);
            GUILayout.EndVertical();

            // product info
            GUILayout.BeginVertical();
            GUILayout.Label(Constants.kProductDisplayName, "HeaderLabel");
            GUILayout.Label(Constants.kProductVersion, "MiniLabel");
            GUILayout.Label(Constants.kProductCopyrights, "MiniLabel");
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Controls methods

        private void DrawControlGroup(SerializedProperty property, string displayName)
        {
            EditorGUILayout.BeginVertical(m_groupBackgroundStyle);
            if (DrawControlHeader(property, displayName))
            {
                // show internal properties
                EditorGUI.indentLevel++;
                DrawControlInternalProperties(property);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        private bool DrawControlHeader(SerializedProperty property, string displayName)
        {
            // draw rect
            Rect rect       = EditorGUILayout.GetControlRect(false, 30f);
            GUI.Box(rect, "", m_headerStyle);

            // draw foldable control
            Rect    foldOutRect     = new Rect(rect.x, rect.y, 50f, rect.height);
            if (GUI.Button(foldOutRect, property.isExpanded ? "-" : "+", m_headerFoldoutStyle))
            {
                property.isExpanded = !property.isExpanded;
            }

            // draw label 
            Rect    labelRect       = new Rect(rect.x + 25f, rect.y, rect.width - 100f, rect.height);
            EditorGUI.LabelField(labelRect, displayName, m_headerLabelStyle);

            return property.isExpanded;
        }

        private static void DrawControlInternalProperties(SerializedProperty property)
        {
            // move pointer to first element
            SerializedProperty currentProperty  = property.Copy();
            SerializedProperty endProperty      = null;

            // start iterating through the properties
            bool    firstTime   = true;
            while (currentProperty.NextVisible(enterChildren: firstTime))
            {
                if (firstTime)
                {
                    endProperty = property.GetEndProperty();
                    firstTime   = false;
                }
                if (SerializedProperty.EqualContents(currentProperty, endProperty))
                {
                    break;
                }
                EditorGUILayout.PropertyField(currentProperty, true);
            }
        }

        #endregion

        #region Settings group methods

        private void DrawSettingsGroup(SerializedProperty settingsProperty, SettingsGroupMeta settingsMeta)
        {
            EditorGUILayout.BeginVertical(m_groupBackgroundStyle);
            if (DrawSettingsHeader(settingsProperty, settingsMeta.displayName))
            {
                SerializedProperty  enabledProperty = settingsProperty.FindPropertyRelative("m_isEnabled");

                // update gui state
                bool originalGUIState   = GUI.enabled;
                GUI.enabled             = enabledProperty.boolValue;

                // show internal properties
                EditorGUI.indentLevel++;
                if (settingsMeta.beforeDrawInternal != null)
                {
                    settingsMeta.beforeDrawInternal();
                }
                DrawSettingsInternalProperties(settingsProperty);
                if (settingsMeta.afterDrawInternal != null)
                {
                    settingsMeta.afterDrawInternal();
                }
                EditorGUI.indentLevel--;

                // reset gui state
                GUI.enabled             = originalGUIState;
            }
            EditorGUILayout.EndVertical();
        }

        private bool DrawSettingsHeader(SerializedProperty settingsProperty, string displayName)
        {
            // draw rect
            Rect rect       = EditorGUILayout.GetControlRect(false, 30f);
            GUI.Box(rect, "", m_headerStyle);

            // draw foldable control
            Rect    foldOutRect     = new Rect(rect.x, rect.y, 50f, rect.height);
            if (GUI.Button(foldOutRect, settingsProperty.isExpanded ? "-" : "+", m_headerFoldoutStyle))
            {
                settingsProperty.isExpanded     = !settingsProperty.isExpanded;
            }

            // draw label 
            Rect    labelRect       = new Rect(rect.x + 25f, rect.y, rect.width - 100f, rect.height);
            EditorGUI.LabelField(labelRect, displayName, m_headerLabelStyle);

            // draw enable button
            SerializedProperty  enabledProperty = settingsProperty.FindPropertyRelative("m_isEnabled");
            Rect    toggleRect                  = new Rect(rect.xMax - 64f, rect.y, 64f, 25f);
            if (GUI.Button(toggleRect, enabledProperty.boolValue ? m_toggleOnIcon : m_toggleOffIcon, m_headerToggleStyle))
            {
                //enabledProperty.boolValue       = !enabledProperty.boolValue;
                enabledProperty.boolValue       = true;
            }

            return settingsProperty.isExpanded;
        }

        private static void DrawSettingsInternalProperties(SerializedProperty settingsProperty)
        {
            // move pointer to first element
            SerializedProperty currentProperty  = settingsProperty.Copy();
            currentProperty.NextVisible(enterChildren: true);
            SerializedProperty endProperty      = settingsProperty.GetEndProperty();

            // start iterating through the properties
            while (currentProperty.NextVisible(enterChildren: false))
            {
                if (SerializedProperty.EqualContents(currentProperty, endProperty))
                {
                    break;
                }
                EditorGUILayout.PropertyField(currentProperty, true);
            }
        }

        private static void DrawAddressBookMenuControls()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Simulator"))
            {
                EditorAddressBookSimulator.Reset();
            }
            GUILayout.EndHorizontal();
        }

        private void LoadStyles()
        {
            // check whether styles are already loaded
            if (null != m_groupBackgroundStyle)
            {
                return;
            }

            // bg style
            m_groupBackgroundStyle          = new GUIStyle("TE NodeBoxSelected");
            RectOffset bgOffset             = m_groupBackgroundStyle.margin;
            bgOffset.bottom                 = 5;
            m_groupBackgroundStyle.margin   = bgOffset;

            // header style
            m_headerStyle                   = new GUIStyle("PreButton");
            m_headerStyle.fixedHeight       = 0;

            // foldout style
            m_headerFoldoutStyle            = new GUIStyle("WhiteBoldLabel");
            m_headerFoldoutStyle.fontSize   = 20;
            m_headerFoldoutStyle.alignment  = TextAnchor.MiddleLeft;

            // label style
            m_headerLabelStyle              = new GUIStyle("WhiteBoldLabel");
            m_headerLabelStyle.fontSize     = 12;
            m_headerLabelStyle.alignment    = TextAnchor.MiddleLeft;

            // enabled style
            m_headerToggleStyle             = new GUIStyle("InvisibleButton");
        }

        #endregion

        #region Nested types

        private struct SettingsGroupMeta
        {
            public string serializedPropertyName;
            public string displayName;
            public Action beforeDrawInternal;
            public Action afterDrawInternal;
        }

        #endregion
    }
}