using System;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [Serializable]
    public abstract class FilterCondition
    {
        [SerializeField] protected string _inspectorName;
        [SerializeField] protected bool _isEnabled;
        protected FilterCondition(string inspectorName)
        {
            _inspectorName = inspectorName;
        }

        public bool IsEnabled => _isEnabled;
        
        public abstract bool IsTrue(RequestImageData imageData);
        public abstract void Clear();
    }
}