using System;
using System.Reflection;
using ModestTree;
using System.Linq;
using UnityEngine;

namespace Game.Buildings
{
    public class FactoryIncreasableText : IncreasableText
    {
        [SerializeField] private ResourceFactory _factory;
        [SerializeField] private ResourceFactoryIncreasableType _type;
        [SerializeField] private bool _showingNextLevel;

        private int[] _increasableArray;

        private void Start()
        {
            _increasableArray = (int[])_factory
                .GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(f => CheckFieldHasRequreType(f, _type))
                .GetValue(_factory);
            
            ReasizeTextUpdate(Increaser.CurrentLevel);
        }

        protected override void UpdateText()
        {
            var currentLevel = Increaser.CurrentLevel;
            
            if (currentLevel == Increaser.LevelsCount) return;
            
            ReasizeTextUpdate(currentLevel);
        }

        private void ReasizeTextUpdate(int currentLevel)
        {
            var additionalText = _increasableArray[currentLevel - 1].ToString();
            
            if (_showingNextLevel)
            {
                additionalText += $"({_increasableArray[currentLevel]})";
            }

            Text.text = DefaultText + additionalText;
        }

        private bool CheckFieldHasRequreType(FieldInfo field, ResourceFactoryIncreasableType type)
        {
            var requreAttribute = field.GetCustomAttributes()
                .OfType<ArrayOfIncreasablesAttribute>()
                .FirstOrDefault();
            return requreAttribute != null && requreAttribute.Type == type;
        }
    }
}