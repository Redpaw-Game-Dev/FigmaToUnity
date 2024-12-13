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
        
        public int Count => _data.Count;
        public RequestImageData this[int i] => _data[i];

        public RequestImageDataList()
        {
            
        }
        
        public void UpdateItemData(Texture2D image, int index)
        {
            _data[index] = UpdateListItem(_data[index], image);
        }

        public void AddItem(RequestImageData image)
        {
            _data.Add(image);
        }

        public void RemoveItem(int index)
        {
            _data.RemoveAt(index);
        }
        
        public void DeleteItem(int index)
        {
            Texture2D image = null;
            if (_data[index] is UpdateImageData data) image = data.Image;
            RemoveItem(index);
            if (image)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(image));
                AssetDatabase.Refresh();
            }
        }

        private RequestImageData UpdateListItem(RequestImageData item, Texture2D image)
        {
            if (item is NewImageData) item = item.ToUpdateImageData();
            ((UpdateImageData)item).UpdateImage(image);
            return item;
        }
    }
}