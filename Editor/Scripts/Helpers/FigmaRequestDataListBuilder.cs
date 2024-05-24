using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LazyRedpaw.FigmaToUnity
{
    public static class FigmaRequestDataListBuilder
    {
        public static List<FigmaRequestData> Build(RequestImageDataList requestImageDataList)
        {
            List<FigmaRequestData> requestDataList = new List<FigmaRequestData>();
            for (int i = 0; i < requestImageDataList.Count; i++)
            {
                if (!requestImageDataList[i].IsIncluded) continue;
                if (string.IsNullOrEmpty(requestImageDataList[i].URL))
                {
                    Debug.LogWarning($"{requestImageDataList[i].Name} URL is missing.");
                    continue;
                }
                string fileKey = requestImageDataList[i].URL.ExtractString("design/", "/");
                FigmaRequestData data = requestDataList.FirstOrDefault(data => data.FileKey == fileKey);
                if (data == null)
                {
                    data = new FigmaRequestData(fileKey);
                    requestDataList.Add(data);
                }
                data.AddItemData(i, requestImageDataList[i]);
            }

            for (int i = 0; i < requestDataList.Count; i++)
            {
                requestDataList[i].UpdateRequestString();
            }
            return requestDataList;
        }
    }
}