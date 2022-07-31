using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Game.Tools;

namespace Game.PlayerInteraction
{
    [RequireComponent(typeof(Camera))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask _floor;
        [SerializeField] private MeshCollider _floorCollider;
        [SerializeField] private float _limitingSpeed;
        
        [Inject] private InputEnable _inputEnable;
        [Inject] private EventSystem _eventSystem;
        
        private Camera _camera;
        private Transform _transform;
        private Vector3 _firstTouch;
        private bool _moving;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _transform = transform;
        }

        private void Update()
        {
            TryMove();

            CheckMovingEnd();
            
            LimitMovement();
        }

        private void TryMove()
        {
            if (_inputEnable.Enable == false || _inputEnable.CameraMovementEnable == false) return;
            
            var cameraRay = _camera.ScreenPointToRay(Input.mousePosition);
            var looksAtFloor = Physics.Raycast(cameraRay, Mathf.Infinity, _floor);
                
            if (looksAtFloor)
            {
                ProcessMoving();
            }
        }

        private void ProcessMoving()
        {
            var mousePosition = Input.mousePosition;
            var positionOnFloor = WorldSpaceTool.GetPointOnFloor(_camera, mousePosition, _floor);

            if (Input.GetMouseButtonDown(0) &&
                WorldSpaceTool.CheckAnyButtonPressed(_eventSystem, Input.mousePosition) == false)
            {
                _firstTouch = positionOnFloor;
                _moving = true;
            }

            if (Input.GetMouseButton(0) && _moving)
            {
                var vectorFromFirstTouch = positionOnFloor - _firstTouch;
                _transform.position -= vectorFromFirstTouch;
            }
        }

        private void LimitMovement()
        {
            var floorBounds = _floorCollider.bounds;
            var rays = CreateRaysFromCorners();

            if (IntersectTarget(_floorCollider, rays) == false)
            {
                var center = floorBounds.center;
                center.y = _transform.position.y;
                _transform.position = Vector3.MoveTowards(_transform.position, center, _limitingSpeed);
            }
        }

        private void CheckMovingEnd()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _moving = false;
            }
        }

        private Ray[] CreateRaysFromCorners()
        {
            var rays = new Ray[4];

            int index = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var corner = _camera.ViewportToScreenPoint(new Vector3(i, j));
                    rays[index] = _camera.ScreenPointToRay(corner);
                    index++;
                }
            }

            return rays;
        }

        private bool IntersectTarget(MeshCollider collider, Ray[] rays)
        {
            foreach (var ray in rays)
            {
                if (collider.bounds.IntersectRay(ray) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
