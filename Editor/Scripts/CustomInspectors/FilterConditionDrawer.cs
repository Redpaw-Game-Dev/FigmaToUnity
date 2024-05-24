using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(FilterCondition), true)]
    public class FilterConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty inspectorNameProp = property.FindPropertyRelative("_inspectorName");
            EditorGUI.LabelField(position, inspectorNameProp.stringValue);
        }
    }
}