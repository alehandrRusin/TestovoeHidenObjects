using Common.Request;
using Menu;
using Menu.UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private LevelPreviewView levelPreviewViewPrefab;
        [SerializeField] private MenuScreenView menuScreenView;
        [SerializeField] private ConnectionProblemPopupView connectionProblemPopupView;

        public override void InstallBindings()
        {
            BindRequestHandler();
            
            BindLevelSystem();
            BindUI();
            
            Container.BindInterfacesAndSelfTo<LoadPreviewsController>().AsSingle();
        }

        private void BindLevelSystem()
        {
            Container.BindFactory<LevelPreviewView, LevelPreviewView.Factory>()
                .FromComponentInNewPrefab(levelPreviewViewPrefab);

            Container
                .BindFactory<LevelPreviewView, RequestLevelData, LevelPreviewController, LevelPreviewController.Factory>();

            Container.Bind<LevelPreviewCreator>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<MenuScreenView>().FromInstance(menuScreenView).AsSingle();
            Container.Bind<ConnectionProblemPopupView>().FromInstance(connectionProblemPopupView).AsSingle();
        }

        private void BindRequestHandler()
        {
            Container.BindInterfacesAndSelfTo<RequestHandler>().AsSingle();
        }
    }
}