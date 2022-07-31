using System;
using Game.Tools;
using UnityEngine;
using Zenject;

namespace Game.Buildings
{
    public class BuildingLimiter : MonoBehaviour
    {
        [SerializeField] private LayerMask _obstacle;
        
        [Inject] private Collider _buildingZone;

        private bool _inZone;
        private int _obstaclesCount;
        
        public bool RightPlace => _obstaclesCount == 0 && _inZone;

        private void OnTriggerEnter(Collider other)
        {
            if (other == _buildingZone)
            {
                _inZone = true;
            }
            
            if (LayerTool.EqualLayers(_obstacle, other.gameObject.layer))
            {
                _obstaclesCount++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other == _buildingZone)
            {
                _inZone = false;
            }
            
            if (LayerTool.EqualLayers(_obstacle, other.gameObject.layer))
            {
                _obstaclesCount--;
            }
        }
    }
}