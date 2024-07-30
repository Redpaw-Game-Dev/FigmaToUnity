using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [Serializable]
    public class RequestImageDataList
    {
        [SerializeReference] private List<RequestImageData> _data;
        [SerializeReference] private List<RequestImageData> _filteredData;
        [SerializeField] private Filter _filter;
        
        public int Count => _filter.IsFilterApplied ? _filteredData.Count : _data.Count;
        public RequestImageData this[int i] => _filter.IsFilterApplied ? _filteredData[i] : _data[i];

        public RequestImageDataList()
        {
            _filter = new Filter();
            _filter.OnFilterApplied += FilterItems;
        }
        
        public void UpdateItemData(Texture2D image, int index)
        {
            if (_filter.IsFilterApplied)
            {
                for (int i = 0; i < _data.Count; i++)
                {
                    if (ReferenceEquals(_data[i], _filteredData[index]))
                    {
                        _filteredData[index] = UpdateListItem(_filteredData[index], image);
                        _data[i] = UpdateListItem(_data[i], image);
                        return;
                    }
                }
            }
            _data[index] = UpdateListItem(_data[index], image);
        }

        public void AddItem(RequestImageData image)
        {
            _data.Add(image);
            if(_filter.IsFilterApplied) FilterItems();
        }

        public void RemoveItem(int index)
        {
            if (_filter.IsFilterApplied)
            {
                for (int i = 0; i < _data.Count; i++)
                {
                    if (ReferenceEquals(_data[i], _filteredData[index]))
                    {
                        _data.RemoveAt(i);
                        _filteredData.RemoveAt(index);
                        return;
                    }
                }
            }
            _data.RemoveAt(index);
        }
        
        public void DeleteItem(int index)
        {
            Texture2D image = null;
            if (_filter.IsFilterApplied)
            {
                if(_filteredData[index] is UpdateImageData data) image = data.Image;
            }
            else if (_data[index] is UpdateImageData data) image = data.Image;
            RemoveItem(index);
            if (image)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(image));
                AssetDatabase.Refresh();
            }
        }

        public void FilterItems()
        {
            if (!_filter.IsAnyConditionEnabled()) ClearFilter();
            if(!_filter.IsFilterApplied) return;
            if(_filteredData.Count > 0) _filteredData.Clear();
            for (int i = 0; i < _data.Count; i++)
            {
                if(_filter.IsFilterMet(_data[i])) _filteredData.Add(_data[i]);
            }
        }
        
        public void ClearFilter()
        {
            if(!_filter.IsFilterApplied) return;
            _filter.Clear();
            _filteredData.Clear();
        }

        private RequestImageData UpdateListItem(RequestImageData item, Texture2D image)
        {
            if (item is NewImageData) item = item.ToUpdateImageData();
            ((UpdateImageData)item).UpdateImage(image);
            return item;
        }
    }
}