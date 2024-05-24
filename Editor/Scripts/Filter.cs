using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [Serializable]
    public class Filter
    {
        [SerializeReference] private List<FilterCondition> _filterConditions;
        [SerializeField] private bool _isFilterApplied;
        [SerializeField] private int _filterVersion;

        public event Action OnFilterApplied; 
        
        public bool IsFilterApplied => _isFilterApplied;

        public bool IsAnyConditionEnabled()
        {
            for (int i = 0; i < _filterConditions.Count; i++)
            {
                if (_filterConditions[i].IsEnabled) return true;
            }
            return false;
        }
        
        public bool IsItemApproved(RequestImageData imageData)
        {
            for (int i = 0; i < _filterConditions.Count; i++)
            {
                if (_filterConditions[i].IsEnabled && !_filterConditions[i].IsTrue(imageData))
                {
                    return false;
                }
            }
            return true;
        }
        
        public void Apply()
        {
            _isFilterApplied = true;
            OnFilterApplied?.Invoke();
        }

        public void Clear()
        {
            if(!_isFilterApplied) return;
            _isFilterApplied = false;
            for (int i = 0; i < _filterConditions.Count; i++)
            {
                _filterConditions[i].Clear();
            }
        }
    }
}