using Game.PlayerInteraction;
using UnityEngine;
using Game.Tools;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.Buildings
{
    [RequireComponent(typeof(BuildingLimiter))]
    public class BuildingMover : MonoBehaviour
    {
        [SerializeField] private LayerMask _floor;
        [SerializeField] private Material _outLimitsMaterial;
        
        [Inject] private InputEnable _inputEnable;
        [Inject] private EventSystem _eventSystem;
        [Inject] private Camera _camera;

        public readonly float LayerDelta = 0.001f;

        private Transform _transform;
        private Renderer _renderer;
        private BuildingLimiter _limiter;
        private Material _defaultMaterial;
        private bool _builded;
        private bool _moving;
        private Vector3 _startPosition;
        private Vector3 _vectorFromFirstTouch;

        public BuildingLimiter Limiter => _limiter;
        public bool Moved { get; private set; }

        private void Start()
        {
            _transform = transform;
            _limiter = GetComponent<BuildingLimiter>();
            _renderer = GetComponent<Renderer>();
            _defaultMaterial = _renderer.material;
        }

        private void Update()
        {
            if (_inputEnable.Enable == false || (_inputEnable.BuildingNew && _builded)) return;
            
            var mousePosition = Input.mousePosition;
            var positionOnFloor = WorldSpaceTool.GetPointOnFloor(_camera, mousePosition, _floor);
            positionOnFloor.y = _transform.position.y;
            
            if (Input.GetMouseButtonDown(0))
            {
                TrySelect(mousePosition, positionOnFloor);
            }
            if (_moving)
            {
                Move(positionOnFloor);
                if (Input.GetMouseButtonUp(0))
                {
                    EndMoving();
                }
            }
        }
        
        public void SetAsBuilded()
        {
            _builded = true;
        }
        
        public void SetRightMaterial()
        {
            _renderer.material = _limiter.RightPlace ? _defaultMaterial : _outLimitsMaterial;
        }

        private void TrySelect(Vector3 mousePosition, Vector3 positionOnFloor)
        {
            var cameraRay = _camera.ScreenPointToRay(mousePosition);
            
            if (WorldSpaceTool.CheckAnyButtonPressed(_eventSystem, Input.mousePosition)) return;
                
            if (Physics.Raycast(cameraRay, out var hit, Mathf.Infinity, 1 << gameObject.layer))
            {
                if (hit.transform == _transform)
                {
                    Moved = false;
                    _inputEnable.CameraMovementEnable = false;
                    _startPosition = _transform.position;
                    _vectorFromFirstTouch = _transform.position - positionOnFloor;
                    if (_builded)
                        _transform.position += Vector3.up * LayerDelta;
                    _moving = true;
                }
            }
        }
        
        private void Move(Vector3 positionOnFloor)
        {
            positionOnFloor.y = _transform.position.y;
            var newPosition = positionOnFloor + _vectorFromFirstTouch;
            if ((_transform.position - newPosition).magnitude > 0)
            {
                Moved = true;
            }
            _transform.position = positionOnFloor + _vectorFromFirstTouch;
            
            SetRightMaterial();
        }

        private void EndMoving()
        {
            _inputEnable.CameraMovementEnable = true;
            _moving = false;
            
            if (_builded == false) return;
            
            if (_limiter.RightPlace == false)
            {
                _transform.position = _startPosition;
            }
            else
            {
                _transform.position += Vector3.down * LayerDelta;
            }

            _renderer.material = _defaultMaterial;
        }
    }
}