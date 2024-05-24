using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(Filter), true)]
    public class FilterDrawer : PropertyDrawer
    {
        private const float LabelWidth = 125f;
        private const int FilterVersion = 2;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            InitFilter(property);
            SerializedProperty conditionsProp = property.FindPropertyRelative(CommonEditorVars.FilterConditions);
            return property.isExpanded
                ? EditorGUIUtility.singleLineHeight * conditionsProp.arraySize + EditorGUIUtility.standardVerticalSpacing * conditionsProp.arraySize * 0.25f
                : EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty conditionsProp = property.FindPropertyRelative(CommonEditorVars.FilterConditions);
            SerializedProperty isFilterAppliedProp = property.FindPropertyRelative(CommonEditorVars.IsFilterApplied);

            GUI.Box(position, GUIContent.none, EditorStyles.helpBox);

            Rect elementRect = new Rect(position.x, position.y, LabelWidth, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(elementRect, property.isExpanded, property.displayName);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                elementRect.x -= CommonEditorVars.ToggleSize;
                elementRect.y += EditorGUIUtility.standardVerticalSpacing;
                elementRect.width = position.width;
                for (int i = 0; i < 2; i++)
                {
                    elementRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    SerializedProperty element = conditionsProp.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(elementRect, element);
                }
                elementRect.width = position.width * 0.5f;
                for (int i = 2; i < conditionsProp.arraySize; i++)
                {
                    if (i % 2 == 0) elementRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    SerializedProperty element = conditionsProp.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(elementRect, element);
                    if (i % 2 == 0) elementRect.x += elementRect.width;
                    else elementRect.x -= elementRect.width;
                }

                elementRect.x = position.x + CommonEditorVars.SpacingWidth;
                elementRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                elementRect.width = position.width * 0.5f - CommonEditorVars.SpacingWidth;
                if (GUI.Button(elementRect, new GUIContent("Apply Filter", "Apply conditions and filter list")))
                {
                    MethodInfo filterMethod = typeof(Filter).GetMethod("Apply");
                    Filter instance = (Filter)SerializedPropertyHelper.GetTargetObjectOfProperty(property);
                    filterMethod.Invoke(instance, null);
                }

                elementRect.x += elementRect.width;
                if (GUI.Button(elementRect, new GUIContent("Clear Filter", "Disable all conditions and show all data")))
                {
                    if (isFilterAppliedProp.boolValue)
                    {
                        MethodInfo clearFilterMethod = typeof(Filter).GetMethod("Clear");
                        Filter instance = (Filter)SerializedPropertyHelper.GetTargetObjectOfProperty(property);
                        clearFilterMethod.Invoke(instance, null);
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
        
        private void InitFilter(SerializedProperty property)
        {
            SerializedProperty filterVersionProp = property.FindPropertyRelative("_filterVersion");
            SerializedProperty conditionsProp = property.FindPropertyRelative(CommonEditorVars.FilterConditions);
            if (filterVersionProp.intValue < FilterVersion)
            {
                filterVersionProp.intValue = FilterVersion;
                conditionsProp.ClearArray();
                CreateFilterCondition(conditionsProp, typeof(SearchNameFilterCondition), "Search Name");
                CreateFilterCondition(conditionsProp, typeof(SearchUrlFilterCondition), "Search URL");
                CreateFilterCondition(conditionsProp, typeof(IsIncludedFilterCondition), "Included Items");
                CreateFilterCondition(conditionsProp, typeof(IsExcludedFilterCondition), "Excluded Items");
                CreateFilterCondition(conditionsProp, typeof(IsEmptyNameFilterCondition), "Empty Name");
                CreateFilterCondition(conditionsProp, typeof(IsEmptyUrlFilterCondition), "Empty URL");
                CreateFilterCondition(conditionsProp, typeof(IsNewImageDataFilterCondition), "New Data");
                CreateFilterCondition(conditionsProp, typeof(IsUpdateImageDataFilterCondition), "Update Data");
                CreateFilterCondition(conditionsProp, typeof(IsMissedImageFilterCondition), "Missed Image");
            }
        }
        
        private void CreateFilterCondition(SerializedProperty filterProp, Type type, string name)
        {
            filterProp.InsertArrayElementAtIndex(filterProp.arraySize);
            SerializedProperty newElement = filterProp.GetArrayElementAtIndex(filterProp.arraySize - 1);
            newElement.managedReferenceValue = Activator.CreateInstance(type, name);
        }
    }
}