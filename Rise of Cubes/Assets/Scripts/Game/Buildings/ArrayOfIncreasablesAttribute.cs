using System;

namespace Game.Buildings
{
    public class ArrayOfIncreasablesAttribute : Attribute
    {
        public readonly ResourceFactoryIncreasableType Type;

        public ArrayOfIncreasablesAttribute(ResourceFactoryIncreasableType type)
        {
            Type = type;
        }
    }
}