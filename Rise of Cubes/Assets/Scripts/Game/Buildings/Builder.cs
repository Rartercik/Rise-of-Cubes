using Game.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Buildings
{
    public class Builder : MonoBehaviour
    {
        [SerializeField] private PreBuilding _buildingPrefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform _floor;
        [SerializeField] private ResourcesVault _vault;
        [SerializeField] private ResourceType[] _requreResources;
        [SerializeField] private int[] _requreCosts;
        [SerializeField] private UnityEvent _onNotEnoughCost;

        private readonly float _onSpawnY = 0.002f;

        private Cost _cost;

        private void Start()
        {
            _cost = new Cost(_requreResources, _requreCosts);
        }

        public void TryCreate()
        {
            var position = _floor.position;
            position.y = _onSpawnY;
            if (CheckBuyAvailable(_requreResources, _requreCosts))
            {
                var prebuilding = Instantiate(_buildingPrefab, position, Quaternion.identity, _parent);
                prebuilding.SetCost(_cost);
                for (int i = 0; i < _requreResources.Length; i++)
                {
                    _vault.TakeResources(_requreResources[i], _requreCosts[i]);
                }
            }
            else
            {
                _onNotEnoughCost?.Invoke();
            }
        }

        private bool CheckBuyAvailable(ResourceType[] requreResources, int[] requreCost)
        {
            for (int i = 0; i < requreResources.Length; i++)
            {
                if (_vault.GetResourceCount(requreResources[i]) < requreCost[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}