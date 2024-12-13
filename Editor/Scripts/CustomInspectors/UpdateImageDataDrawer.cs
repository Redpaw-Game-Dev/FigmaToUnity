using System;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using static LazyRedpaw.FigmaToUnity.Constants;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(UpdateImageData))]
    public class UpdateImageDataDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            VisualTreeAsset tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathHelper.FindFilePath(UpdateImageDataUXML));
            tree.CloneTree(root);
            EnumField enumDataType = root.Q<EnumField>(EnumDataType);
            enumDataType.value = ImageDataType.UpdateImageData;
            enumDataType.RegisterValueChangedCallback(evt => OnDataTypeChanged(property, evt));
            TexturePickerElement texturePicker = root.Q<TexturePickerElement>(TexturePicker);
            texturePicker.RegisterValueChangedCallback(evt => OnTextureChanged(property, evt));
            return root;
        }

        private void OnTextureChanged(SerializedProperty property, ChangeEvent<Object> evt)
        {
            SerializedProperty imageProp = property.FindPropertyRelative(ImageProp);
            if (evt.newValue != imageProp.objectReferenceValue)
            {
                SerializedProperty assetPathProp = property.FindPropertyRelative(AssetPathProp);
                assetPathProp.stringValue = AssetDatabase.GetAssetPath(evt.newValue);
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnDataTypeChanged(SerializedProperty property, ChangeEvent<Enum> evt)
        {
            if (evt.newValue.ToString() != ImageDataType.UpdateImageData.ToString())
            {
                SerializedProperty imageProp = property.FindPropertyRelative(ImageProp);
                string name = string.Empty;
                if (imageProp.objectReferenceValue != null)
                {
                    name = imageProp.objectReferenceValue.name;
                }
                string url = property.FindPropertyRelative(UrlProp).stringValue;
                string assetPath = property.FindPropertyRelative(AssetPathProp).stringValue;
                property.managedReferenceValue = new NewImageData(name, url, assetPath);
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}