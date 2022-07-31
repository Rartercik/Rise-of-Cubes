using UnityEngine;
using Zenject;

namespace Game.Initializers
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasInitializer : UnityEngine.MonoBehaviour
    {
        [Inject] private Camera _usingCamera;

        private Canvas _canvas;

        private void Start()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = _usingCamera;
        }
    }
}