using System;
using System.Linq;
using System.Reflection;
using ModestTree;
using UnityEngine;
using Zenject;
using Game.Resources;

namespace Game.Buildings
{
    [RequireComponent(typeof(LevelIncreaser))]
    public class ResourceFactory : MonoBehaviour, IIncreasingBuilding
    {
        [SerializeField] private ResourceType _resourceType;
        [Range(0, 1)]
        [SerializeField] private float _minimalCoeficientToShow;
        [SerializeField] private GameObject _takeResourcesIcon;
        [Header("Levels")]
        [ArrayOfIncreasables(ResourceFactoryIncreasableType.Capacity)]
        [SerializeField] private int[] _capacitys;
        [ArrayOfIncreasables(ResourceFactoryIncreasableType.Perfomance)]
        [SerializeField] private int[] _perfomancesPerMinute;

        [Inject] private ResourcesVault _vault;
        
        private LevelIncreaser _levelIncreaser;
        private int _capacity;
        private int _perfomancePerMinute;
        private float _currentResources;

#if  UNITY_EDITOR
        private void OnValidate()
        {
            var levelIncreaser = GetComponent<LevelIncreaser>();
            var increasableFields = GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.HasAttribute(typeof(ArrayOfIncreasablesAttribute)));
            foreach (var field in increasableFields)
            {
                var arrayOfIncreasables = (Array)(field.GetValue(this));
                if (arrayOfIncreasables.Length != levelIncreaser.LevelsCount)
                {
                    throw new ArgumentException("Level parameters count should be the same as in the level increser");
                }
            }
        }
#endif

        private void OnEnable()
        {
            _levelIncreaser = GetComponent<LevelIncreaser>();
            _levelIncreaser.OnLevelIncrease += IncreaseLevel;
        }

        private void OnDisable()
        {
            _levelIncreaser.OnLevelIncrease -= IncreaseLevel;
        }

        private void Update()
        {
            _currentResources += Time.deltaTime * (_perfomancePerMinute / 60f);

            var minimalCountToShow = _minimalCoeficientToShow * _capacity;
            if (_currentResources >= minimalCountToShow)
            {
                _takeResourcesIcon.SetActive(true);
            }
            
            if (_currentResources >= _capacity)
            {
                _currentResources = _capacity;
            }
        }

        private void IncreaseLevel()
        {
            _capacity = _capacitys[_levelIncreaser.CurrentLevel - 1];
            _perfomancePerMinute = _perfomancesPerMinute[_levelIncreaser.CurrentLevel - 1];
        }

        public void SetEnabled(bool enable)
        {
            enabled = enable;
        }

        public void GiveResourcesToVault()
        {
            var resources = (int)_currentResources;
            _vault.GiveResources(_resourceType, resources);
            _currentResources -= resources;
        }
    }
}
