using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [Serializable]
    public class Filter
    {
        [SerializeReference] private List<FilterCondition> _filterConditions;
        [SerializeField] private bool _areAllConditionsRequired;
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

        public bool IsFilterMet(RequestImageData imageData)
        {
            return _areAllConditionsRequired ? AreAllConditionsMet(imageData) : AreAnyConditionsMet(imageData);
        }
        
        private bool AreAllConditionsMet(RequestImageData imageData)
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
        
        private bool AreAnyConditionsMet(RequestImageData imageData)
        {
            for (int i = 0; i < _filterConditions.Count; i++)
            {
                if (_filterConditions[i].IsEnabled && _filterConditions[i].IsTrue(imageData))
                {
                    return true;
                }
            }
            return false;
        }
    }
}