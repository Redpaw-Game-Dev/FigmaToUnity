using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace LazyRedpaw.FigmaToUnity
{
    public static class WebRequestHelper
    {
        public static async Task<string> GetJsonDataFromFigma(string url, string figmaToken)
        {
            UnityWebRequest request = new UnityWebRequest(url, "GET");
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("X-FIGMA-TOKEN", figmaToken);
            request.SendWebRequest();
            while (!request.isDone) await Task.Yield();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                return null;
            }

            return request.downloadHandler.text;
        }

        public static async Task<Texture2D> DownloadImageByUrl(string url)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            request.downloadHandler = new DownloadHandlerTexture();
            request.SendWebRequest();
            while (!request.isDone) await Task.Yield();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error + "\n" + request.url);
                return null;
            }
            return DownloadHandlerTexture.GetContent(request);
        }
    }
}