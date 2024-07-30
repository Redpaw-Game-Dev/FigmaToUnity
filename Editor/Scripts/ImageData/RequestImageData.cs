using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [Serializable]
    public abstract class RequestImageData
    {
        [SerializeField] protected string _url;
        [SerializeField, HideInInspector] protected string _figmaId;
        [SerializeField, HideInInspector] protected string _assetPath;
        [SerializeField] protected string _name;
        
        public string URL => _url;
        public string FigmaId => _figmaId;
        public string AssetPath => _assetPath;
        public virtual string Name => _name;
        public abstract bool IsIncluded { get; }
        protected RequestImageData() { }

        protected RequestImageData(string url) => _url = url;

        public virtual void UpdateAssetPath(string savePath)
        {
            if (string.IsNullOrEmpty(_assetPath))
            {
                _assetPath = AssetDatabase.GenerateUniqueAssetPath($"{savePath}/{_name}.png");
                _name = Path.GetFileNameWithoutExtension(_assetPath);
            }
        }
        
        public void UpdateFigmaId()
        {
            _figmaId = _url.ExtractString("node-id=", "&t=").Replace("-", ":");
        }

        public UpdateImageData ToUpdateImageData() => new(_url, _name);
    }
}

