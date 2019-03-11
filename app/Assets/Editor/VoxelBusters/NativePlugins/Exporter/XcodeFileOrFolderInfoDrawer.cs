using UnityEngine;
using UnityEditor;

namespace VoxelBusters.NativePlugins
{
    [CustomPropertyDrawer(typeof(XcodeFileOrFolderInfo))]
    public class XcodeFileOrFolderInfoDrawer : PropertyDrawer 
    {
        #region Drawer Methods
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
        {
            return EditorGUIUtility.singleLineHeight;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            // show property name label
            label       = EditorGUI.BeginProperty(position, label, property);
            position    = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

            // show property attributes
            Rect    pathRect        = new Rect(position.x, position.y, position.width * 0.6f, position.height);
            Rect    flagsRect       = new Rect(pathRect.xMax + 5f, position.y, position.width - pathRect.width - 5f, position.height);
            int     indentLevel     = EditorGUI.indentLevel;

            EditorGUI.indentLevel   = 0;
            EditorGUI.PropertyField(pathRect, property.FindPropertyRelative("m_path"), GUIContent.none);
            EditorGUI.PropertyField(flagsRect, property.FindPropertyRelative("m_compileFlags"), GUIContent.none);
            EditorGUI.indentLevel   = indentLevel;
            
            EditorGUI.EndProperty();
        }
        
        #endregion
    }
}