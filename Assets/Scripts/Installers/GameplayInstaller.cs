using Common;
using Gameplay;
using Gameplay.UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayLevelView gameplayLevelView;
        [SerializeField] private GameScreenView gameScreenView;
        [SerializeField] private Camera camera;
        
        public override void InstallBindings()
        {
            BindGameplay();
            BindCamera();
            BindGameScreen();
        }

        private void BindGameplay()
        {
            Container.Bind<GameplayLevelView>().FromInstance(gameplayLevelView).AsSingle();

            var levelData = Container.Resolve<SceneChanger>().GameplayLevelData;
            Container.Bind<LevelData>().FromInstance(levelData).AsSingle();

            Container.BindInterfacesAndSelfTo<SaveProgressController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayController>().AsSingle().NonLazy();
        }

        private void BindCamera()
        {
            Container.Bind<Camera>().FromInstance(camera).AsSingle();
            Container.Bind<CameraController>().AsSingle();
        }

        private void BindGameScreen()
        {
            Container.Bind<GameScreenView>().FromInstance(gameScreenView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameScreenController>().AsSingle();
        }
    }
}