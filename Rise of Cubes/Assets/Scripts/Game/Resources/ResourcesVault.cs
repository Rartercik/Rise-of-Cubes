using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Resources
{
    public class ResourcesVault : MonoBehaviour
    {
        [SerializeField] private ResourceType[] _keys;
        [SerializeField] private int[] _values;
        [SerializeField] private UnityEvent<ResourceType, int> _onResourceCountChange;
        
        private Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();

        private void Awake()
        {
            var resourcesLength = Enum.GetValues(typeof(ResourceType)).Length;

            for (int i = 0; i < resourcesLength; i++)
            {
                var type = _keys[i];
                var count = _values[i];
                _resources.Add(type, count);
                _onResourceCountChange?.Invoke(type, _resources[type]);
            }
        }

        public void TakeResources(ResourceType type, int count)
        {
            var resourceCount = _resources[type];
            if (resourceCount < count || count < 0)
            {
                throw new ArgumentException();
            }

            _resources[type] -= count;
            _onResourceCountChange?.Invoke(type, _resources[type]);
        }

        public void GiveResources(ResourceType type, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException();
            }

            _resources[type] += count;
            _onResourceCountChange?.Invoke(type, _resources[type]);
        }

        public int GetResourceCount(ResourceType type)
        {
            return _resources[type];
        }
    }
}