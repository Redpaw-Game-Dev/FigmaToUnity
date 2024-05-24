using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(UpdateImageData), true)]
    public class UpdateImageDataDrawer : PropertyDrawer
    {
        private const float UpdateElementLabelWidth = 90f;
        private const float TextureFieldSize = 48f;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return TextureFieldSize + EditorGUIUtility.standardVerticalSpacing * 2f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty imageProp = property.FindPropertyRelative(CommonEditorVars.Image);
            SerializedProperty isIncludedProp = property.FindPropertyRelative(CommonEditorVars.IsIncluded);
            SerializedProperty urlProp = property.FindPropertyRelative(CommonEditorVars.URL);
            CheckEmptyName(property, imageProp);
            
            Rect elementRect = new Rect(position.x, position.y + EditorGUIUtility.standardVerticalSpacing,
                TextureFieldSize, TextureFieldSize);
            Texture2D selectedTexture = (Texture2D)EditorGUI.ObjectField(elementRect, imageProp.objectReferenceValue,
                typeof(Texture2D), false);
            if (EditorGUI.EndChangeCheck())
            {
                imageProp.objectReferenceValue = selectedTexture;
                imageProp.serializedObject.ApplyModifiedProperties();
            }

            elementRect.x += elementRect.width + CommonEditorVars.SpacingWidth;
            elementRect.y += EditorGUIUtility.singleLineHeight * 0.33f;
            elementRect.width = UpdateElementLabelWidth;
            elementRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(elementRect, "Is Included");
            
            elementRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.LabelField(elementRect, "URL");

            elementRect.x += elementRect.width;
            elementRect.width = position.width - TextureFieldSize - UpdateElementLabelWidth - CommonEditorVars.SpacingWidth;
            urlProp.stringValue = EditorGUI.TextField(elementRect, urlProp.stringValue);

            elementRect.y -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            elementRect.width = CommonEditorVars.ToggleSize;
            isIncludedProp.boolValue = EditorGUI.Toggle(elementRect, isIncludedProp.boolValue);
        }

        private void CheckEmptyName(SerializedProperty property, SerializedProperty imageProp)
        {
            SerializedProperty nameProp = property.FindPropertyRelative(CommonEditorVars.Name);
            if (imageProp.objectReferenceValue == null && string.IsNullOrEmpty(nameProp.stringValue))
            {
                nameProp.stringValue = "MISSED_IMAGE";
            }
        }
    }
}