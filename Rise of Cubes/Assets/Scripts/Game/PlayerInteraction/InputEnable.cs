using UnityEngine;

namespace Game.PlayerInteraction
{
    [CreateAssetMenu]
    public class InputEnable : ScriptableObject
    {
        public bool Enable { get; private set; } = true;
        
        public bool CameraMovementEnable { get; set; } = true;
        public bool BuildingNew { get; private set; }

        public void SwitchEnable(bool enable)
        {
            Enable = enable;
        }

        public void SetBuildingNew(bool buildingNew)
        {
            BuildingNew = buildingNew;
        }
    }
}