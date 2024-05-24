using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(NewImageData), true)]
    public class NewImageDataDrawer : PropertyDrawer
    {
        private const float LabelWidth = 45f;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty nameProp = property.FindPropertyRelative(CommonEditorVars.Name);
            SerializedProperty urlProp = property.FindPropertyRelative(CommonEditorVars.URL);
            
            Rect elementRect = new Rect(position.x, position.y + EditorGUIUtility.standardVerticalSpacing,
                LabelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(elementRect, "Name");
                    
            float halfWidth = position.width * 0.5f;
            elementRect.x += LabelWidth;
            elementRect.width = halfWidth - LabelWidth - CommonEditorVars.SpacingWidth;
            nameProp.stringValue = EditorGUI.TextField(elementRect, nameProp.stringValue);

            elementRect.x += elementRect.width + CommonEditorVars.SpacingWidth;
            elementRect.width = LabelWidth;
            EditorGUI.LabelField(elementRect, "URL");
                    
            elementRect.x += LabelWidth;
            elementRect.width = halfWidth - LabelWidth;

            urlProp.stringValue = EditorGUI.TextField(elementRect, urlProp.stringValue);
        }
    }
}