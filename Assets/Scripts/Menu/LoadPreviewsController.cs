using System;
using System.Collections.Generic;
using Common.Database;
using Common.Request;
using Common.Utils;
using Menu.UI;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Menu
{
    public class LoadPreviewsController : IInitializable
    {
        [Inject] private LevelPreviewCreator _levelPreviewCreator;
        [Inject] private DataBase _dataBase;
        [Inject] private RequestHandler _requestHandler;
        [Inject] private ConnectionProblemPopupView _connectionProblemPopupView;
        
        public void Initialize()
        {
            LoadPreviews();
        }

        private void LoadPreviews()
        {
            LoadRequestLevelsData((isSuccess, requestLevelsData) =>
            {
                if (isSuccess)
                {
                    _dataBase.CreateTable();

                    CreatePreviewControllers(requestLevelsData);
                }
                else
                {
                    WaitForRetryLoad(() =>
                    {
                        LoadPreviews();
                    });
                }
            });
        }

        private void LoadRequestLevelsData(Action<bool, List<RequestLevelData>> callback)
        {
            _requestHandler.DoRequest(RequestUrls.LevelsJson, (result, resultString) =>
            {
                if (result == UnityWebRequest.Result.Success)
                {
                    if (resultString.TryParseJson(out List<RequestLevelData> requestLevelsData))
                    {
                        callback(true, requestLevelsData);
                    }
                    else
                    {
                        Debug.LogError($"cant parse: {resultString}");
                        callback(false, null);
                    }
                }
                else
                {
                    callback(false, null);
                }
            });
        }

        private void WaitForRetryLoad(Action callback)
        {
            _connectionProblemPopupView.Show();

            IDisposable disposable = null;
            disposable = _connectionProblemPopupView.RetryConnectButton.OnClickAsObservable().Subscribe(_ =>
            {
                disposable?.Dispose();
                _connectionProblemPopupView.Hide();
                
                callback();
            }).AddTo(_connectionProblemPopupView);
        }

        private void CreatePreviewControllers(List<RequestLevelData> requestLevelsData)
        {
            for (int i = 0; i < requestLevelsData.Count; i++)
            {
                _levelPreviewCreator.CreatePreviewController(requestLevelsData[i]);
            }
        }
    }
}