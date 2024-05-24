using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(BoolFilterCondition), true)]
    public class BoolFilterConditionDrawer : FilterConditionDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            SerializedProperty isEnabledProp = property.FindPropertyRelative(CommonEditorVars.IsEnabled);
            isEnabledProp.boolValue = EditorGUI.Toggle(position, new GUIContent(" "), isEnabledProp.boolValue);
        }
    }
}