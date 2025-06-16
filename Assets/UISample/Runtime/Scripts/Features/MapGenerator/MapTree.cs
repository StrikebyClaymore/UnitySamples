using System.Collections.Generic;

namespace UISample.Features
{
    public class MapTree
    {
        public readonly int PositionX;
        public readonly List<MapNode> Nodes = new();

        public MapTree(int positionX)
        {
            PositionX = positionX;
        }
    }
}