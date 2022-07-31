namespace Game.Tools
{
    public static class LayerTool
    {
        public static bool EqualLayers(int layer1, int layer2)
        {
            return (layer1 & (1 << layer2)) > 0;
        }
    }
}