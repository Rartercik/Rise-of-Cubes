using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Tools
{
    public static class WorldSpaceTool
    {
        public static Vector3 GetPointOnFloor (Camera camera, Vector2 screenPoint, LayerMask floor)
        {
            Physics.Raycast(camera.ScreenPointToRay(screenPoint), out var hit, Mathf.Infinity, floor);
            return hit.point;
        }

        public static bool CheckAnyButtonPressed(EventSystem eventSystem, Vector2 mousePosition)
        {
            var pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = mousePosition;
            var UIs = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, UIs);

            return UIs.Count > 0;
        }
    }
}