using Common;
using Common.Database;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private TransitionScreenView transitionScreenView;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneChanger>().AsSingle();
            Container.Bind<DataBase>().AsSingle();

            Container.Bind<TransitionScreenView>().FromInstance(transitionScreenView);
        }
    }
}