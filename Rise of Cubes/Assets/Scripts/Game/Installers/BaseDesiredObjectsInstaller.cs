using Game.Resources;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class BaseDesiredObjectsInstaller : MonoInstaller
    {
        [SerializeField] private Collider _baseFloor;
        [SerializeField] private ResourcesVault _vault;
    
        public override void InstallBindings()
        {
            Container.Bind<Collider>().FromInstance(_baseFloor).AsSingle().NonLazy();
            Container.Bind<ResourcesVault>().FromInstance(_vault).AsSingle().NonLazy();
        }
    }
}