using Game.PlayerInteraction;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Game.Installers
{
    public class PlayerDesiredObjectsInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private InputEnable _inputEnable;
        [SerializeField] private Button[] _buildingPreventers;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Animator _onNotEnoughResources;
        
        public override void InstallBindings()
        {
            Container.Bind<InputEnable>().FromScriptableObject(_inputEnable).AsSingle().NonLazy();
            Container.Bind<Camera>().FromInstance(_camera).AsSingle().NonLazy();
            Container.Bind<CameraMovement>().FromInstance(_cameraMovement).AsSingle().NonLazy();
            Container.Bind<Button[]>().FromInstance(_buildingPreventers).NonLazy();
            Container.Bind<EventSystem>().FromInstance(_eventSystem).AsSingle().NonLazy();
            Container.Bind<Animator>().FromInstance(_onNotEnoughResources).AsSingle().NonLazy();
        }
    }
}