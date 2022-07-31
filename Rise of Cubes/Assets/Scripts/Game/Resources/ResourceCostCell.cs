namespace Game.Resources
{
    public struct ResourceCostCell
    {
        public readonly ResourceType Type;
        public readonly int Cost;
        
        public ResourceCostCell(ResourceType type, int cost)
        {
            Type = type;
            Cost = cost;
        }
    }
}