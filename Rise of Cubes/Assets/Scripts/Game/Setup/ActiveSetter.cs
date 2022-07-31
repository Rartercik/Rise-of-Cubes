using UnityEngine;

namespace Game.Setup
{
    public class ActiveSetter : MonoBehaviour
    {
        [SerializeField] private bool _deactivateOnStart;

        private void Start()
        {
            gameObject.SetActive(!_deactivateOnStart);
        }
    }
}