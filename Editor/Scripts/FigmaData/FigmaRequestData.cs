using System;
using System.Collections.Generic;

namespace LazyRedpaw.FigmaToUnity
{
    public class FigmaRequestData
    {
        private static string CommonRequestPart = "https://api.figma.com/v1/images/";
        private static string CommonIdsPart = "?ids=";
        
        private string _fileKey;
        private Dictionary<int, RequestImageData> _indexItemDataDict;
        private string _requestString;

        public string FileKey => _fileKey;
        public int ItemDataCount => _indexItemDataDict.Count;
        public string RequestString => _requestString;

        public FigmaRequestData(string fileKey)
        {
            _fileKey = fileKey;
            _indexItemDataDict = new Dictionary<int, RequestImageData>();
        }

        public void AddItemData(int listIndex, RequestImageData data)
        {
            _indexItemDataDict.Add(listIndex, data);
        }

        public KeyValuePair<int, RequestImageData> GetItemByFigmaId(string figmaId)
        {
            foreach (var indexItemData in _indexItemDataDict)
            {
                if (indexItemData.Value.FigmaId.Equals(figmaId)) return indexItemData;
            }
            return default;
        }

        public void UpdateRequestString()
        {
            _requestString = $"{CommonRequestPart}{_fileKey}{CommonIdsPart}";
            foreach (var indexItemData in _indexItemDataDict)
            {
                indexItemData.Value.UpdateFigmaId();
                _requestString += indexItemData.Value.FigmaId;
                _requestString += ",";
            }

            _requestString = _requestString.Substring(0, _requestString.Length - 1);
        }
        
        public override bool Equals(object obj)
        {
            return _fileKey.Equals(((FigmaRequestData)obj)._fileKey);
        }

        protected bool Equals(FigmaRequestData other)
        {
            return _fileKey == other._fileKey;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_fileKey, _indexItemDataDict);
        }
    }
}