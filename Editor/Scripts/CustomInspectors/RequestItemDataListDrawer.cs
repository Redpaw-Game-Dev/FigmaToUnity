using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(RequestImageDataList), true)]
    public class RequestItemDataListDrawer : PropertyDrawer
    {
        private const float LabelWidth = 125f;
        private const float RemoveButtonWidth = 25f;
        private const float ListPadding = 25f;
        private const int SquareButtonFontSize = 20;
        private static readonly float InfoBoxHeight = EditorGUIUtility.singleLineHeight * 1.5f;

        private string[] _typeNames;
        private Type[] _types;
        private int _selectedIndex;
        private ReorderableList _list;
        private SerializedProperty _property;
        private bool _isActionsExpanded;
        
        private GUIStyle _buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = SquareButtonFontSize,
            alignment = TextAnchor.MiddleCenter
        };
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            InitLists(property);
            if (property.isExpanded)
            {
                SerializedProperty filterProp = property.FindPropertyRelative(CommonEditorVars.Filter);
                float height = property.isExpanded 
                    ? _list.GetHeight() + EditorGUIUtility.singleLineHeight
                    : EditorGUIUtility.singleLineHeight;
                height += GetActionsBoxHeight();
                height += EditorGUI.GetPropertyHeight(filterProp);
                if (IsAnyElementWithEmptyProp()) height += InfoBoxHeight;
                return height;
            }
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _property = property;
            InitTypeNamesArray();
            EditorGUI.BeginProperty(position, label, property);
            
            Color defaultBgColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.65f, 0.65f, 0.65f);
            GUI.Box(position, GUIContent.none, EditorStyles.helpBox);
            GUI.backgroundColor = defaultBgColor;
            
            Rect elementRect = new Rect(position.x + CommonEditorVars.ToggleSize, position.y, LabelWidth, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(elementRect, property.isExpanded, property.displayName);
            if(property.isExpanded)
            {
                EditorGUI.indentLevel++;

                SerializedProperty filterProp = property.FindPropertyRelative(CommonEditorVars.Filter);
                
                elementRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                elementRect.width = position.width - elementRect.x * 0.5f;
                elementRect.height = EditorGUI.GetPropertyHeight(filterProp);
                EditorGUI.PropertyField(elementRect, filterProp);
                
                
                elementRect.y += elementRect.height + EditorGUIUtility.standardVerticalSpacing;
                elementRect.height = GetActionsBoxHeight();
                DrawActionsBox(elementRect);

                if (IsAnyElementWithEmptyProp())
                {
                    elementRect.y += elementRect.height + EditorGUIUtility.standardVerticalSpacing;
                    elementRect.height = InfoBoxHeight;
                    DrawInfoBox(elementRect);
                }

                elementRect.x = position.x;
                elementRect.y += elementRect.height + EditorGUIUtility.standardVerticalSpacing;
                elementRect.width = LabelWidth;
                elementRect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.LabelField(elementRect, $"New element type");
            
                elementRect.x += elementRect.width;
                elementRect.width = position.width - LabelWidth - RemoveButtonWidth;
                _selectedIndex = EditorGUI.Popup(elementRect, _selectedIndex, _typeNames);
            
                elementRect.x += elementRect.width;
                elementRect.width = RemoveButtonWidth;
                if (GUI.Button(elementRect, new GUIContent("+", "Add new data to list"), _buttonStyle))
                {
                    AddItemToList(Activator.CreateInstance(_types[_selectedIndex]));
                }

                elementRect.x = position.x;
                elementRect.y += elementRect.height + EditorGUIUtility.standardVerticalSpacing;
                elementRect.width = position.width;
                elementRect.height -= ListPadding;
                _list.DoList(elementRect);

                EditorGUI.EndProperty();
                EditorGUI.indentLevel--;
            }
        }

        private void DrawInfoBox(Rect elementRect)
        {
            string message = "Elements with";
            int count = GetElementCountWithEmptyProp(CommonEditorVars.Name);
            if (count > 0) message += $" empty name: {count};";
            count = GetElementCountWithEmptyProp(CommonEditorVars.URL);
            if (count > 0) message += $" empty URL: {count};";
            count = GetElementCountWithEmptyProp(CommonEditorVars.Image);
            if (count > 0) message += $" missed image: {count};";
            EditorGUI.HelpBox(elementRect, message, MessageType.Warning);
        }

        private int GetElementCountWithEmptyProp(string propName)
        {
            int count = 0;
            for (int i = 0; i < _list.serializedProperty.arraySize; i++)
            {
                SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(i);
                if (IsElementPropValueNullOrEmpty(element.FindPropertyRelative(propName))) count++;
            }
            return count;
        }
        
        private bool IsAnyElementWithEmptyProp()
        {
            for (int i = 0; i < _list.serializedProperty.arraySize; i++)
            {
                SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(i);
                if (IsElementPropValueNullOrEmpty(element.FindPropertyRelative(CommonEditorVars.Name)) ||
                    IsElementPropValueNullOrEmpty(element.FindPropertyRelative(CommonEditorVars.URL)) ||
                    IsElementPropValueNullOrEmpty(element.FindPropertyRelative(CommonEditorVars.Image))) return true;
            }
            return false;
        }

        private bool IsElementPropValueNullOrEmpty(SerializedProperty prop)
        {
            if (prop == null) return false;
            switch (prop.type)
            {
                case "string":
                    return string.IsNullOrEmpty(prop.stringValue);
                case "PPtr<$Texture2D>":
                    return prop.objectReferenceValue == null;
            }
            return false;
        }
        
        private void DrawActionsBox(Rect position)
        {
            GUI.Box(position, GUIContent.none, EditorStyles.helpBox);
            
            Rect elementRect = position;
            elementRect.height = EditorGUIUtility.singleLineHeight;
            _isActionsExpanded = EditorGUI.Foldout(elementRect, _isActionsExpanded, new GUIContent("Actions"));
            
            if (_isActionsExpanded)
            {
                elementRect.y += elementRect.height + EditorGUIUtility.standardVerticalSpacing;
                elementRect.width = position.width * 0.333f;
                if (DoesAnyListItemHaveIsIncludedProp())
                {
                    if (IsAnyListItemExcluded())
                    {
                        if (GUI.Button(elementRect, new GUIContent("Include All", "Include all update data"))) SetIsIncludedForAll(true);
                    }
                    else
                    {
                        if (GUI.Button(elementRect, new GUIContent("Exclude All", "Exclude all update data"))) SetIsIncludedForAll(false);
                    }
                }
                else
                {
                    GUI.enabled = false;
                    if (GUI.Button(elementRect, new GUIContent("Exclude All", "New data can't be excluded"))) SetIsIncludedForAll(false);
                    GUI.enabled = true;
                }
                
                elementRect.x += elementRect.width;
                if (GUI.Button(elementRect, new GUIContent("Remove Items", "Remove all items from list"))) UseMethodForEntireList("RemoveItem");
                
                elementRect.x += elementRect.width;
                if (GUI.Button(elementRect,  new GUIContent("Delete Items", "Delete all images from project"))) UseMethodForEntireList("DeleteItem");
            }
        }

        private void AddItemToList(object item)
        {
            MethodInfo method = typeof(RequestImageDataList).GetMethod("AddItem");
            RequestImageDataList instance = (RequestImageDataList)SerializedPropertyHelper.GetTargetObjectOfProperty(_property);
            method.Invoke(instance, new []{ item });
        }
        
        private void UseMethodForOneItem(string methodName, int index)
        {
            MethodInfo method = typeof(RequestImageDataList).GetMethod(methodName);
            RequestImageDataList instance = (RequestImageDataList)SerializedPropertyHelper.GetTargetObjectOfProperty(_property);
            method.Invoke(instance, new []{ (object)index });
        }
        
        private void UseMethodForEntireList(string methodName)
        {
            MethodInfo method = typeof(RequestImageDataList).GetMethod(methodName);
            RequestImageDataList instance = (RequestImageDataList)SerializedPropertyHelper.GetTargetObjectOfProperty(_property);
            for (int i = _list.serializedProperty.arraySize - 1; i >= 0 ; i--)
            {
                method.Invoke(instance, new []{ (object)i });
            }
        }
        
        private bool DoesAnyListItemHaveIsIncludedProp()
        {
            for (int i = 0; i < _list.serializedProperty.arraySize; i++)
            {
                SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(i);
                SerializedProperty isIncludedProp = element.FindPropertyRelative(CommonEditorVars.IsIncluded);
                if (isIncludedProp != null) return true;
            }
            return false;
        }
        
        private bool IsAnyListItemExcluded()
        {
            for (int i = 0; i < _list.serializedProperty.arraySize; i++)
            {
                SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(i);
                SerializedProperty isIncludedProp = element.FindPropertyRelative(CommonEditorVars.IsIncluded);
                if (isIncludedProp != null && !isIncludedProp.boolValue) return true;
            }
            return false;
        }
        
        private void SetIsIncludedForAll(bool value)
        {
            for (int i = 0; i < _list.serializedProperty.arraySize; i++)
            {
                SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(i);
                SerializedProperty isIncludedProp = element.FindPropertyRelative(CommonEditorVars.IsIncluded);
                if (isIncludedProp != null) isIncludedProp.boolValue = value;
            }
        }
        
        private float GetActionsBoxHeight()
        {
            return _isActionsExpanded
                ? EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing * 2f
                : EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
        
        private void InitLists(SerializedProperty property)
        {
            if (_list != null)
            {
                _list.serializedProperty = FindListProperty(property);
                return;
            }
            SerializedProperty dataProp = FindListProperty(property);
            _list = new ReorderableList(dataProp.serializedObject, dataProp, true, false, false, false);
            _list.drawElementCallback += OnListDrawElementCallback;
            _list.elementHeightCallback += OnListElementHeightCallback;
        }

        private SerializedProperty FindListProperty(SerializedProperty property)
        {
            SerializedProperty isFilterAppliedProp = property.FindPropertyRelative(CommonEditorVars.Filter)
                .FindPropertyRelative(CommonEditorVars.IsFilterApplied);
            return property.FindPropertyRelative(
                isFilterAppliedProp.boolValue ? "_filteredData" : "_data");
        }
        
        private float OnListElementHeightCallback(int index)
        {
            SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(element);
        }
        
        private void OnListDrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            if(index < 0 || index >= _list.serializedProperty.arraySize) return;
            SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(index);
            Rect elementRect = new Rect(rect.x, rect.y, rect.width - RemoveButtonWidth, rect.height);
            bool isNeedDeleteButton = element.FindPropertyRelative(CommonEditorVars.Image) != null;
            if (isNeedDeleteButton) elementRect.width -= RemoveButtonWidth;
            EditorGUI.PropertyField(elementRect, element);
            
            elementRect.x = elementRect.width + RemoveButtonWidth * 1.65f;
            elementRect.y = rect.y + EditorGUIUtility.standardVerticalSpacing;
            elementRect.width = RemoveButtonWidth;
            elementRect.height = rect.height - EditorGUIUtility.standardVerticalSpacing * 2f;
            if (isNeedDeleteButton)
            {
                DrawDeleteButton(elementRect, index);
                elementRect.x += RemoveButtonWidth;
            }
            DrawRemoveButton(elementRect, index);
        }
        
        private void DrawRemoveButton(Rect elementRect, int index)
        {
            if (GUI.Button(elementRect, new GUIContent("-", "Remove from list"), _buttonStyle))
            {
                UseMethodForOneItem("RemoveItem", index);
            }
        }
        
        private void DrawDeleteButton(Rect elementRect, int index)
        {
            if (GUI.Button(elementRect, new GUIContent("×", "Delete from project"), _buttonStyle))
            {
                UseMethodForOneItem("DeleteItem", index);
            }
        }

        private void InitTypeNamesArray()
        {
            if(_types != null && _typeNames != null) return;
            List<string> typeNames = new List<string>();
            List<Type> types = new List<Type>();
            Type parentType = typeof(RequestImageData);
            Type[] allTypes = Assembly.GetAssembly(parentType).GetTypes();
            for (int i = 0; i < allTypes.Length; i++)
            {
                if (allTypes[i].IsClass &&
                    !allTypes[i].IsAbstract &&
                    allTypes[i].IsSubclassOf(parentType))
                {
                    typeNames.Add(allTypes[i].Name);
                    types.Add(allTypes[i]);
                }
            }
            _typeNames = typeNames.ToArray();
            _types = types.ToArray();
        }
    }
}