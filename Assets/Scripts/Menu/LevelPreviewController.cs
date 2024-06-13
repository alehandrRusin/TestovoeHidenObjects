using System;
using Common;
using Common.Database;
using Common.Request;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Menu
{
    public class LevelPreviewController : IInitializable, IDisposable
    {
        [Inject] private RequestHandler _requestHandler;
        [Inject] private SceneChanger _sceneChanger;
        [Inject] private DataBase _dataBase;
        
        //factory data
        [Inject] private LevelPreviewView _levelPreviewView;
        [Inject] private RequestLevelData _requestLevelData;

        private LevelData _levelData;

        private IDisposable _disposable;

        public void Initialize()
        {
            LoadLevelImage((isSuccess,texture) =>
            {
                if (isSuccess)
                {
                    _levelPreviewView.Button.onClick.AddListener(ViewButtonOnClick);
                    
                    CreateLevelData(texture);
                    _levelPreviewView.Initialize(_levelData.texture, _levelData.imageName);
                    RefreshViewProgress();
                }
                else
                {
                    _levelPreviewView.ShowUnknownErrorLabel();
                }
            });
        }

        private void CreateLevelData(Texture2D texture)
        {
            _levelData = new LevelData
            {
                id = _requestLevelData.id,
                imageName = _requestLevelData.imageName,
                texture = texture
            };

            if (_dataBase.TryLoadData(_levelData.id, out var dbLevelData))
            {
                _levelData.counter = new IntReactiveProperty(dbLevelData.counter);
            }
            else
            {
                _levelData.counter = new IntReactiveProperty(_requestLevelData.counter);
            }
        }

        private void RefreshViewProgress()
        {
            _disposable = _levelData.counter.Subscribe(count =>
            {
                _levelPreviewView.RefreshProgress(count);

                if (count == 0)
                {
                    _levelPreviewView.Button.onClick.RemoveListener(ViewButtonOnClick);
                    
                    _levelPreviewView.SetCompleted();
                    
                    _disposable?.Dispose();
                }
            });
        }

        private void LoadLevelImage(Action<bool, Texture2D> callback)
        {
            _levelPreviewView.ShowLoadingIndicator();
            _requestHandler.DoTextureRequest(_requestLevelData.imageUrl, (result, texture2D) =>
            {
                _levelPreviewView.HideLoadingIndicator();
                callback(result == UnityWebRequest.Result.Success, texture2D);
            });
        }

        private void ViewButtonOnClick()
        {
            _sceneChanger.LoadGameplayScene(_levelData);
        }
        
        public void Dispose()
        {
            _levelPreviewView.Button.onClick.RemoveListener(ViewButtonOnClick);
            _disposable?.Dispose();
        }

        public class Factory : PlaceholderFactory<LevelPreviewView, RequestLevelData, LevelPreviewController>
        {
            [Inject] private DisposableManager _disposableManager;
            
            public override LevelPreviewController Create(LevelPreviewView param, RequestLevelData param2)
            {
                var instance = base.Create(param, param2);
                instance.Initialize();
                _disposableManager.Add(instance);

                return instance;
            }
        }
    }
}