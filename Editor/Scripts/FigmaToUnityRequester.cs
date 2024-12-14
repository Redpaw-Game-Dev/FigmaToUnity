using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [CreateAssetMenu(fileName = "FigmaToUnity", menuName = "LazyRedpaw/FigmaToUnity")]
    public class FigmaToUnityRequester : ScriptableObject
    {
        [SerializeField] private string _figmaToken;
        [SerializeField] private string _savePath = "Assets";
        [SerializeField] private RequestImageDataList _requestImageDataList;

        [SerializeField, HideInInspector] private float _requestProgress = -1f;
        [SerializeField, HideInInspector] private bool _isRequestProcessing;
        [SerializeField, HideInInspector] private string _requestProgressText;
        private float _progressStep;

        public async void TrySendRequest()
        {
            try
            {
                List<FigmaRequestData> requestsData = FigmaRequestDataListBuilder.Build(_requestImageDataList);
                if (requestsData == null || requestsData.Count == 0) Debug.LogWarning($"No data to send.");
                else await ProcessRequest(requestsData);
            }
            catch (Exception e)
            {
                _isRequestProcessing = false;
                _requestProgressText = "Error";
                throw;
            }
        }

        private async Task ProcessRequest(List<FigmaRequestData> requestsData)
        {
            _isRequestProcessing = true;
            int asyncOpsCount = 0;
            for (int i = 0; i < requestsData.Count; i++) asyncOpsCount += requestsData[i].ItemDataCount + 1;
            _progressStep = 1f / asyncOpsCount;
            _requestProgress = 0f;
            for (int i = 0; i < requestsData.Count; i++)
            {
                _requestProgressText = "Requesting data from Figma";
                string jsonResponse = await WebRequestHelper.GetJsonDataFromFigma(requestsData[i].RequestString, _figmaToken);
                _requestProgress += _progressStep;
                FigmaResponseData data = JsonConvert.DeserializeObject<FigmaResponseData>(jsonResponse);
                foreach (var imageIdUrlPair in data.Images)
                {
                    KeyValuePair<int, RequestImageData> indexImageData = requestsData[i].GetItemByFigmaId(imageIdUrlPair.Key);
                    _requestProgressText = $"Loading {indexImageData.Value.Name} {_requestProgress * 100f:0}%";
                    Texture2D image = await WebRequestHelper.DownloadImageByUrl(imageIdUrlPair.Value);
                    _requestProgress += _progressStep;
                    indexImageData.Value.UpdateAssetPath(_savePath);
                    image.SaveToFile(indexImageData.Value.AssetPath, true);
                    AssetDatabase.Refresh();
                    Texture2D textureAsset = AssetDatabase.LoadAssetAtPath<Texture2D>(indexImageData.Value.AssetPath);
                    TextureImporter importer = AssetImporter.GetAtPath(indexImageData.Value.AssetPath) as TextureImporter;
                    importer.textureType = TextureImporterType.Sprite;
                    AssetDatabase.WriteImportSettingsIfDirty(indexImageData.Value.AssetPath);
                    _requestImageDataList.UpdateItemData(textureAsset, indexImageData.Key);
                    AssetDatabase.Refresh();
                }
            }
            _isRequestProcessing = false;
            EditorUtility.SetDirty(this);
            _requestProgressText = "Completed";
        }
    }
}