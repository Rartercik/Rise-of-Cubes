using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Resources
{
    public struct Cost
    {
        private readonly List<ResourceCostCell> _resources;

        public Cost(ResourceCostCell[] cells)
        {
            _resources = cells.ToList();
        }

        public Cost(ResourceType[] resourceTypes, int[] resourceCosts)
        {
            _resources = new List<ResourceCostCell>();
            
            if (resourceTypes.Distinct().Count() != resourceTypes.Length)
            {
                throw new ArgumentException("There must not be the same types");
            }
            if (resourceTypes.Length != resourceCosts.Length)
            {
                throw new ArgumentException("Types and costs should have the same length");
            }
            
            for (int i = 0; i < resourceTypes.Length; i++)
            {
                _resources.Add(new ResourceCostCell(resourceTypes[i], resourceCosts[i]));
            }
        }

        public IEnumerable<ResourceCostCell> Resources => _resources;
    }
}