using UnityEngine;
using UnityEngine.UI;

namespace Game.Resources
{
    public class ResourceText : MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private string _textBeforeCount;
        [SerializeField] private Text _text;

        public void TryUpdate(ResourceType type, int count)
        {
            if (type != _resourceType) return;

            _text.text = _textBeforeCount + count;
        }
    }
}