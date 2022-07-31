using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Game.Buildings
{
    [RequireComponent(typeof(BuildingMover))]
    public class BuildingInformation : MonoBehaviour, IMaxLevelDepender
    {
        [SerializeField] private GraphicRaycaster _defaultLabel;
        [SerializeField] private GraphicRaycaster _maxLevelLabel;

        [Inject] private Camera _camera;
        [Inject] private EventSystem _eventSystem;

        private GraphicRaycaster _label;
        private Canvas _labelCanvas;
        private Canvas _maxLevelCanvas;
        private Transform _transform;
        private BuildingMover _mover;

        private void Start()
        {
            _label = _defaultLabel;
            _labelCanvas = _label.GetComponent<Canvas>();
            _maxLevelCanvas = _maxLevelLabel.GetComponent<Canvas>();
            _transform = transform;
            _mover = GetComponent<BuildingMover>();
        }

        private void Update()
        {
            var mousePosition = Input.mousePosition;
            
            var cameraRay = _camera.ScreenPointToRay(mousePosition);
            
            if (Input.GetMouseButtonDown(0))
            {
                var pointerEventData = new PointerEventData(_eventSystem);
                pointerEventData.position = mousePosition;
                var UIs = new List<RaycastResult>();
                _label.Raycast(pointerEventData, UIs);

                if (UIs.Count == 0)
                {
                    _labelCanvas.enabled = false;
                }
            }
            else if (Input.GetMouseButtonUp(0) && _mover.Moved == false)
            {
                if (Physics.Raycast(cameraRay, out var hit, Mathf.Infinity, 1 << gameObject.layer))
                {
                    var pointerEventData = new PointerEventData(_eventSystem);
                    pointerEventData.position = Input.mousePosition;
                    var UIs = new List<RaycastResult>();
                    _eventSystem.RaycastAll(pointerEventData, UIs);
                    
                    if (hit.transform == _transform && UIs.Count == 0)
                    {
                        _labelCanvas.enabled = true;
                    }
                }
            }
        }

        public void RealizeMaxLevelActions()
        {
            _label = _maxLevelLabel;
            _labelCanvas = _maxLevelCanvas;
        }
    }
}