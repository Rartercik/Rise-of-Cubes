using Game.PlayerInteraction;
using Game.Resources;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Game.Buildings
{
    [RequireComponent(typeof(LevelIncreaser))]
    [RequireComponent(typeof(BuildingMover))]
    public class PreBuilding : MonoBehaviour
    {
        [SerializeField] private bool _builded;
        [SerializeField] private GameObject _label;
        [SerializeField] private UnityEvent _onBuilded;

        [Inject] private InputEnable _inputEnable;
        [Inject] private Button[] _buildingPreventers;
        [Inject] private ResourcesVault _vault;

        private readonly string _buildingName = "Building";
        private readonly string _preBuildingName = "PreBuilding";

        private LevelIncreaser _levelIncreaser;
        private BuildingMover _mover;
        private Cost _cost;

        private void Start()
        {
            _mover = GetComponent<BuildingMover>();
            _levelIncreaser = GetComponent<LevelIncreaser>();
            foreach (var button in _buildingPreventers)
            {
                button.onClick.AddListener(CancelBuilding);
            }
            gameObject.layer = LayerMask.NameToLayer(_preBuildingName);
            if (_builded)
            {
                ConfirmBuilding(_builded);
            }
            else
            {
                _inputEnable.SetBuildingNew(true);
                transform.position += Vector3.up * _mover.LayerDelta;
            }
        }
        
        private void Update()
        {
            _mover.SetRightMaterial();
        }
        
        public void SetCost(Cost cost)
        {
            _cost = cost;
        }

        public void TryConfirmBuilding()
        {
            if (_mover.Limiter.RightPlace == false) return;
        
            ConfirmBuilding(_builded);
        }

        public void CancelBuilding()
        {
            _label.SetActive(false);
            _inputEnable.SetBuildingNew(false);
            foreach (var button in _buildingPreventers)
            {
                button.onClick.RemoveListener(CancelBuilding);
            }

            foreach (var cost in _cost.Resources)
            {
                _vault.GiveResources(cost.Type, cost.Cost);
            }
            Destroy(gameObject);
        }

        private void ConfirmBuilding(bool builded)
        {
            _label.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer(_buildingName);
            if (builded == false)
            {
                transform.position += Vector3.down * _mover.LayerDelta;
            }
            _mover.SetAsBuilded();
            _inputEnable.SetBuildingNew(false);
            _levelIncreaser.Initialize(builded, _onBuilded);
            Destroy(this);
        }
    }
}
