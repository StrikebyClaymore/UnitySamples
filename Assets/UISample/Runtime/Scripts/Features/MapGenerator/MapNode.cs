using System.Collections.Generic;
using UnityEngine;

namespace UISample.Features
{
    public class MapNode
    {
        public readonly EMapNodeType Type;
        public readonly Vector3Int Position;
        public readonly List<MapNode> Connections = new();

        public MapNode(EMapNodeType type, Vector3Int position)
        {
            Type = type;
            Position = position;
        }
    }
}