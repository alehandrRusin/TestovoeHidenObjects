using System;
using Common;
using UniRx;
using Zenject;

namespace Gameplay.UI
{
    public class GameScreenController : IInitializable, IDisposable
    {
        [Inject] private GameScreenView _gameScreenView;
        [Inject] private SceneChanger _sceneChanger;
        [Inject] private LevelData _levelData;

        private IDisposable _disposable;
        
        public void Initialize()
        {
            _gameScreenView.ExitToMenuButton.onClick.AddListener(ExitToMenuButtonOnClick);
            
            _gameScreenView.SetLevelName(_levelData.imageName);

            _disposable = _levelData.counter.Subscribe(count =>
            {
                _gameScreenView.RefreshProgress(count);
            });
        }

        private void ExitToMenuButtonOnClick()
        {
            _sceneChanger.LoadMenuScene();
        }

        public void Dispose()
        {
            _gameScreenView.ExitToMenuButton.onClick.RemoveListener(ExitToMenuButtonOnClick);
            _disposable?.Dispose();
        }
    }
}