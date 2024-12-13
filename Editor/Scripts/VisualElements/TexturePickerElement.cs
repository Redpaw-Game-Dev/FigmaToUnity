using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace LazyRedpaw.FigmaToUnity
{
    public class TexturePickerElement : BindableElement, INotifyValueChanged<Object>
    {
        public new class UxmlFactory : UxmlFactory<TexturePickerElement, UxmlTraits> { }
        public new class UxmlTraits : BindableElement.UxmlTraits { }
        
        private const string TemplateFileName = "TexturePicker.uxml";
        
        private VisualElement _textureElement;
        private ObjectField _objectField;
        private Texture2D _texture;


        public Object value
        {
            get => _texture;
            set
            {
                if (value == this.value)
                    return;

                var previous = this.value;
                SetValueWithoutNotify(value);

                using (var evt = ChangeEvent<Object>.GetPooled(previous, value))
                {
                    evt.target = this;
                    SendEvent(evt);
                }
            }
        }
        
        public TexturePickerElement()
        {
            VisualTreeAsset tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathHelper.FindFilePath(TemplateFileName));

            TemplateContainer container = tree.CloneTree();
            _textureElement = container.Query<VisualElement>("Texture");
            _objectField = container.Query<ObjectField>("Picker");
            _objectField.RegisterValueChangedCallback(OnObjectFieldValueChanged);
            Add(container);
        }

        public void SetValueWithoutNotify(Object newValue)
        {
            if (newValue == null || newValue is Texture2D)
            {
                _texture = newValue as Texture2D;
                _textureElement.style.backgroundImage = new StyleBackground(_texture);
                _objectField.SetValueWithoutNotify(_texture);
            }
            else throw new ArgumentException($"Expected object of type {typeof(Texture2D)}");
        }
        
        private void OnObjectFieldValueChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }
    }
}