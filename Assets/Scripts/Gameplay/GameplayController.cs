using System;
using Common;
using Zenject;
using UniRx;
using UnityEngine;

namespace Gameplay
{
    public class GameplayController : IInitializable, IDisposable
    {
        [Inject] private SceneChanger _sceneChanger;
        [Inject] private GameplayLevelView _gameplayLevelView;
        [Inject] private CameraController _cameraController;
        [Inject] private LevelData _levelData;
        [Inject] private Camera _camera;

        private CompositeDisposable _disposables;

        public void Initialize()
        {
            _disposables = new CompositeDisposable();
            
            var texture = _levelData.texture;

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.one * 0.5f,
                100f);

            _gameplayLevelView.SetSprite(sprite);
            _cameraController.SetBounds(sprite.bounds);

            Activate();
            CheckWin();
        }

        private void Activate()
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var origin = _camera.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero);

                    if (hit.collider == _gameplayLevelView.BoxCollider)
                    {
                        _levelData.counter.Value--;
                    }
                }
            }).AddTo(_disposables);
        }

        private void CheckWin()
        {
            _levelData.counter.Subscribe(count =>
            {
                if (count == 0)
                {
                    _sceneChanger.LoadMenuScene();
                }
            }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}