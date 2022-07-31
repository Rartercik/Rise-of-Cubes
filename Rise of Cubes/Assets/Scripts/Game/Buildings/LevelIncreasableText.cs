using UnityEngine;

namespace Game.Buildings
{
    public class LevelIncreasableText : IncreasableText
    {
        [SerializeField] private bool _showingNextLevel;

        private void Start()
        {
            RealizeTextUpdate();
        }

        protected override void UpdateText()
        {
            RealizeTextUpdate();
        }

        private void RealizeTextUpdate()
        {
            var currentLevel = Increaser.CurrentLevel;
            var additionalText = currentLevel.ToString();
            if (_showingNextLevel)
            {
                additionalText += $"({currentLevel + 1})";
            }

            Text.text = DefaultText + additionalText;
        }
    }
}