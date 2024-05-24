using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(StringFilterCondition), true)]
    public class StringFilterConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            SerializedProperty condStringProp = property.FindPropertyRelative("_conditionString");
            condStringProp.stringValue = EditorGUI.TextField(position, new GUIContent(" "), condStringProp.stringValue);
            SerializedProperty isEnabledProp = property.FindPropertyRelative(CommonEditorVars.IsEnabled);
            isEnabledProp.boolValue = !string.IsNullOrEmpty(condStringProp.stringValue);
        }
    }
}