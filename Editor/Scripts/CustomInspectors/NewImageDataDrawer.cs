using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static LazyRedpaw.FigmaToUnity.Constants;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(NewImageData))]
    public class NewImageDataDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            // VisualTreeAsset tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathHelper.FindFilePath(NewImageDataUXML));
            VisualTreeAsset tree = Resources.Load<VisualTreeAsset>(NewImageDataUXML);
            tree.CloneTree(root);
            EnumField enumDataType = root.Q<EnumField>(EnumDataType);
            enumDataType.value = ImageDataType.NewImageData;
            enumDataType.RegisterValueChangedCallback(evt => OnDataTypeChanged(property, evt));
            return root;
        }
        
        private void OnDataTypeChanged(SerializedProperty property, ChangeEvent<Enum> evt)
        {
            if (evt.newValue.ToString() != ImageDataType.NewImageData.ToString())
            {
                string name = property.FindPropertyRelative(NameProp).stringValue;
                string url = property.FindPropertyRelative(UrlProp).stringValue;
                string assetPath = property.FindPropertyRelative(AssetPathProp).stringValue;
                Texture2D image = null;
                if (!string.IsNullOrEmpty(assetPath))
                {
                    image = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                }
                property.managedReferenceValue = new UpdateImageData(name, url, assetPath, image);
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}