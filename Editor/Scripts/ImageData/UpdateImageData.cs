using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    public class UpdateImageData : RequestImageData
    {
        [SerializeField] protected Texture2D _image;
        [SerializeField] protected bool _isIncluded = true;
        [SerializeField] protected bool _needDeleteImage;

        public Texture2D Image => _image;
        public override bool IsIncluded => _isIncluded;
        public override string Name => _image != null ? _image.name : _name;

        public UpdateImageData() { }

        public UpdateImageData(string url, string name) : base(url) => _name = name;
        
        public UpdateImageData(string name, string url, string assetPath = null, Texture2D image = null) 
            : base(image?.name ?? name, url, assetPath)
        {
            _image = image;
        }

        public void UpdateImage(Texture2D image) => _image = image;

        public override void UpdateAssetPath(string savePath)
        {
            _assetPath = AssetDatabase.GetAssetPath(_image);
            if (string.IsNullOrEmpty(_assetPath)) base.UpdateAssetPath(savePath);
        }
    }
}