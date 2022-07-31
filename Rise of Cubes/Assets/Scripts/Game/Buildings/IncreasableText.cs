using UnityEngine;
using UnityEngine.UI;

namespace Game.Buildings
{
    [RequireComponent(typeof(Text))]
    public abstract class IncreasableText : MonoBehaviour
    {
        [SerializeField] protected LevelIncreaser Increaser;
        [SerializeField] protected string DefaultText;
        
        protected Text Text;

        private void OnEnable()
        {
            Text = GetComponent<Text>();
            Increaser.OnLevelIncrease += UpdateText;
        }

        protected abstract void UpdateText();
    }
}