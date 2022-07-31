using System;
using System.Collections;
using System.Collections.Generic;
using Game.Resources;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Game.Buildings
{
    public class LevelIncreaser : MonoBehaviour
    {
        [SerializeField] private ResourceType _requreResourceType;
        [SerializeField] private UnityEvent _onStartIncreasing;
        [SerializeField] private UnityEvent _onEndIncreasing;
        [SerializeField] private UnityEvent _onNoEnoughResources;
        [SerializeField] private GameObject _levelIncreasingFence;
        [SerializeField] private int _levelsCount;
        [SerializeField] private int _currentLevel;
        [Tooltip("In minutes")]
        [SerializeField] private float[] _timeToLevelUp;
        [SerializeField] private int[] _resourcesCosts;

        [Inject] private ResourcesVault _vault;
        [Inject] private Animator _noResources;
        
        private IIncreasingBuilding[] _increasingBuildings;
        private IMaxLevelDepender[] _maxLevelDependers;
        
        public event Action OnLevelIncrease;

        public int LevelsCount => _levelsCount;
        public int CurrentLevel => _currentLevel;
        public IEnumerable<int> ResourcesCosts => _resourcesCosts;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_timeToLevelUp.Length != _levelsCount)
            {
                throw new ArgumentException("Time To Level Up array should have the same length as levels count");
            }

            if (_resourcesCosts.Length != _levelsCount - 1)
            {
                throw new ArgumentException("Resources Costs array should have the same length as levels count - 1");
            }
        }
#endif

        private void Awake()
        {
            _increasingBuildings = GetComponents<IIncreasingBuilding>();
            _maxLevelDependers = GetComponents<IMaxLevelDepender>();
        }

        private void Start()
        {
            if (_currentLevel <= 0)
            {
                throw new ArgumentException("Current level must be greater than zero");
            }
        }

        public void Initialize(bool buildedAtStart, UnityEvent onBuilded)
        {
            if (enabled) throw new InvalidOperationException("Can't initialize when already enabled");
            enabled = true;
            
            if (buildedAtStart)
            {
                SetStartLevel(onBuilded);
            }
            else
            {
                StartBuildingCurrentLevel(onBuilded);
            }
        }
        
        public void TryStartIncreaseLevel()
        {
            var requreCost = _resourcesCosts[_currentLevel - 1];

            if (_vault.GetResourceCount(_requreResourceType) < requreCost)
            {
                _onNoEnoughResources?.Invoke();
                _noResources.Play("Show");
                return;
            }
            
            _vault.TakeResources(_requreResourceType, requreCost);
            
            _levelIncreasingFence.SetActive(true);
            _onStartIncreasing?.Invoke();
            foreach (var increasing in _increasingBuildings)
            {
                increasing.SetEnabled(false);
            }
            var timeInSeconds = _timeToLevelUp[_currentLevel - 1] * 60f;
            StartCoroutine(Develop(timeInSeconds, IncreaseLevel));
        }

        private void StartBuildingCurrentLevel(UnityEvent onBuilded)
        {
            foreach (var increasing in _increasingBuildings)
            {
                increasing.SetEnabled(false);
            }
            _levelIncreasingFence.SetActive(true);
            var timeInSeconds = _timeToLevelUp[_currentLevel - 1] * 60f;
            StartCoroutine(Develop(timeInSeconds, SetStartLevel, onBuilded));
        }
        
        private void SetStartLevel(UnityEvent onBuilded)
        {
            foreach (var increasing in _increasingBuildings)
            {
                increasing.SetEnabled(true);
            }
            onBuilded?.Invoke();
            OnLevelIncrease?.Invoke();
        }

        private void IncreaseLevel()
        {
            _currentLevel++;
            foreach (var increasing in _increasingBuildings)
            {
                increasing.SetEnabled(true);
            }

            foreach (var maxLevelDepender in _maxLevelDependers)
            {
                if (_currentLevel == _levelsCount)
                {
                    maxLevelDepender.RealizeMaxLevelActions();
                }
            }
            OnLevelIncrease?.Invoke();
        }

        private IEnumerator Develop(float timeInSeconds, Action onEndDevelop)
        {
            yield return new WaitForSeconds(timeInSeconds);
            onEndDevelop.Invoke();
            _onEndIncreasing?.Invoke();
            _levelIncreasingFence.SetActive(false);
        }
        
        private IEnumerator Develop<T>(float timeInSeconds, Action<T> onEndDevelop, T value)
        {
            yield return new WaitForSeconds(timeInSeconds);
            onEndDevelop.Invoke(value);
            _levelIncreasingFence.SetActive(false);
        }
    }
}