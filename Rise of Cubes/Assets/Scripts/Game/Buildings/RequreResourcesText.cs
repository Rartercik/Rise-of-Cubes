using System.Linq;

namespace Game.Buildings
{
    public class RequreResourcesText : IncreasableText
    {
        protected override void UpdateText()
        {
            var currentLevel = Increaser.CurrentLevel;
            
            if (currentLevel == Increaser.LevelsCount) return;
            
            var requreCosts = Increaser.ResourcesCosts.ToArray();
            Text.text = DefaultText + $"({requreCosts[currentLevel-1]})";
        }
    }
}