using System;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Common.Request
{
    public class RequestHandler : IInitializable, IDisposable
    {
        private CompositeDisposable _disposables;
        
        public void Initialize()
        {
            _disposables = new CompositeDisposable();
        }

        public void DoRequest(string url, Action<UnityWebRequest.Result, string> callback)
        {
            IDisposable requestDisposable = null;
            
            requestDisposable = UnityWebRequest
                .Get(url)
                .SendWebRequest()
                .AsAsyncOperationObservable()
                .Subscribe(result =>
                {
                    requestDisposable?.Dispose();
                    if (result.webRequest.result == UnityWebRequest.Result.Success)
                    {
                        callback(result.webRequest.result, result.webRequest.downloadHandler.text);
                    }
                    else
                    {
                        Debug.LogError($"result: {result.webRequest.result}, url: {url}");
                        callback(result.webRequest.result, null);
                    }
                });

            _disposables.Add(requestDisposable);
        }
        
        public void DoTextureRequest(string url, Action<UnityWebRequest.Result, Texture2D> callback)
        {
            UnityWebRequest web = UnityWebRequestTexture
                .GetTexture(url, true);

            IDisposable requestDisposable = null;
            
            requestDisposable = web
                .SendWebRequest()
                .AsAsyncOperationObservable()
                .Subscribe(result =>
                {
                    requestDisposable?.Dispose();
                    if (result.webRequest.result == UnityWebRequest.Result.Success)
                    {
                        callback(result.webRequest.result, DownloadHandlerTexture.GetContent(web));
                    }
                    else
                    {
                        Debug.LogError($"result: {result.webRequest.result}, url: {url}");
                        callback(result.webRequest.result, null);
                    }
                });
            
            _disposables.Add(requestDisposable);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}