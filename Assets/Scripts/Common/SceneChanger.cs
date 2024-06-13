using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Common
{
    public class SceneChanger : IInitializable, IDisposable
    {
        private const int MenuSceneId = 0;
        private const int GameplaySceneId = 1;

        [Inject] private TransitionScreenView _transitionScreenView;

        public LevelData GameplayLevelData { get; private set; }
        
        private Scene _menuScene;
        private Scene _gameplayScene;
        
        private List<GameObject> _menuSceneActiveGameObjects;

        private CompositeDisposable _disposables;
        
        public void Initialize()
        {
            _menuScene = SceneManager.GetSceneAt(MenuSceneId);

            _menuSceneActiveGameObjects = new List<GameObject>();
            _disposables = new CompositeDisposable();
        }
        
        public void LoadGameplayScene(LevelData levelData)
        {
            GameplayLevelData = levelData;
            
            _transitionScreenView.Show(() =>
            {
                SceneManager.LoadSceneAsync(GameplaySceneId, LoadSceneMode.Additive).AsObservable().Subscribe(_ =>
                {
                    _gameplayScene = SceneManager.GetSceneAt(GameplaySceneId);
                    
                    CollectMenuSceneRootGameObjects();

                    _transitionScreenView.Hide();
                }).AddTo(_disposables);
            });
        }
        
        public void LoadMenuScene()
        {
            GameplayLevelData = null;
            
            _transitionScreenView.Show(() =>
            {
                SceneManager.UnloadSceneAsync(_gameplayScene).AsObservable().Subscribe(_ =>
                {
                    SetActiveMenuSceneRootGameObjects();
                    SceneManager.SetActiveScene(_menuScene);
                    
                    _transitionScreenView.Hide();
                }).AddTo(_disposables);
            });
        }
        
        private void CollectMenuSceneRootGameObjects()
        {
            _menuSceneActiveGameObjects.Clear();

            var rootGameObjects = _menuScene.GetRootGameObjects();

            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                var rootGameObject = rootGameObjects[i];
                if (rootGameObject.activeSelf)
                {
                    rootGameObject.SetActive(false);
                    _menuSceneActiveGameObjects.Add(rootGameObject);
                }
            }
        }
        
        private void SetActiveMenuSceneRootGameObjects()
        {
            if (_menuSceneActiveGameObjects != null)
            {
                _menuSceneActiveGameObjects.ForEach(x => x.SetActive(true));
            }
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}