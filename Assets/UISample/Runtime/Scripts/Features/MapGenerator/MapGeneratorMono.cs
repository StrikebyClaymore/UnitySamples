using System.Collections.Generic;
using NaughtyAttributes;
using UISample.Infrastructure;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace UISample.Features
{
    [ExecuteAlways]
    public class MapGeneratorMono : MonoBehaviour, IInitializable
    {
        public enum ENodeType
        {
            Trunk,
            Branch,
            Leaves,
            Crown,
            Hollow,
        }
        
        public class Node
        {
            public readonly ENodeType Type;
            public readonly Vector3Int Position;
            public readonly List<Node> Connections = new();

            public Node(ENodeType type, Vector3Int position)
            {
                Type = type;
                Position = position;
            }
        }

        public class Tree
        {
            public readonly int PositionX;
            public readonly List<Node> Nodes = new();

            public Tree(int positionX)
            {
                PositionX = positionX;
            }
        }
        
        [SerializeField] private Transform _target;
        [SerializeField] private int _chunkWitdh = 19;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _ground, _trunk, _branch, _leaves, _trunkHollow;
        [SerializeField] private RuleTile _crown;
        [SerializeField] private Vector2Int _branchesRange = new(2, 3);
        [SerializeField] private int _currentTreeX;
        [SerializeField] private int _treeSize = 5;
        [SerializeField] private int _hoolowSpawnNumber = 10;
        private readonly List<Tree> _trees = new();
        public IReadOnlyList<Tree> Trees => _trees;
        public Transform Target
        {
            set => _target = value;
        }
        public Node PlayerSpawnNode { get; private set; }
        public int MoveDirection { get; private set; }
        public bool Initialized { get; private set; }

        private void Awake()
        {
            _currentTreeX = 0;
        }

        public void Initialize()
        {
            Generate();
            Initialized = true;
        }

        public void SetDirection(int direction)
        {
            if(MoveDirection == 0)
                MoveDirection = direction;

        }

        private void Update()
        {
            if (_target == null)
                return;

            var targetPosX = _tilemap.WorldToCell(_target.position).x;

            if (MoveDirection == 1)
            {
                if (targetPosX > _currentTreeX + _treeSize / 2)
                {
                    _currentTreeX += _treeSize + 1;
                    RemoveTree(_trees[0]);
                    _trees.RemoveAt(0);
                    GenerateTree(_currentTreeX + _treeSize + 1);
                }
            }
            else if (MoveDirection == -1)
            {
                if (targetPosX < _currentTreeX - _treeSize / 2)
                {
                    _currentTreeX -= _treeSize + 1;
                    RemoveTree(_trees[2]);
                    _trees.RemoveAt(2);
                    GenerateTree(_currentTreeX - _treeSize - 1);
                }
            }
        }

        private void RemoveTree(Tree tree)
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = -2; x < 3; x++)
                {
                    _tilemap.SetTile(new Vector3Int(tree.PositionX + x, y, 0), null);
                }
            }
        }

        [Button("Reset")]
        private void ResetGame()
        {
            _tilemap.ClearAllTiles();
            _trees.Clear();
            _currentTreeX = 0;
            MoveDirection = 0;
        }
        
        [Button("Generate")]
        private void Generate()
        {
            _tilemap.ClearAllTiles();
            _trees.Clear();
            GenerateTree(-6);
            CreateHollowTree(0);
            GenerateTree(6);
        }
        
        private void GenerateTree(int startX)
        {
            if (startX != _treeSize + 1 && (startX / (_treeSize + 1) - 1) % _hoolowSpawnNumber == 0)
            {
                CreateHollowTree(startX);
                return;
            }
            
            var tree = new Tree(startX);
            if(MoveDirection is 0 or 1)
                _trees.Add(tree);
            else
                _trees.Insert(0, tree);
            
            int y = 0;
            System.Random rnd = new System.Random();
            int branchSections = rnd.Next(_branchesRange.x, _branchesRange.y + 1);
            int? previousDirection = null;
            Vector3Int position;
            
            for (int i = 0; i < branchSections; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    position = new Vector3Int(startX, y++, 0);
                    _tilemap.SetTile(position, _trunk);
                    var node = new Node(ENodeType.Trunk, position);
                    tree.Nodes.Add(node);
                }

                int direction;
                if (previousDirection == null)
                    direction = rnd.Next(0, 2) == 0 ? -1 : 1;
                else
                    direction = -previousDirection.Value;
                previousDirection = direction;

                position = new Vector3Int(startX + direction, y - 1, 0);
                _tilemap.SetTile(position, _branch);
                tree.Nodes.Add(new Node(ENodeType.Branch, position));
                
                position = new Vector3Int(startX + direction * 2, y - 1, 0);
                _tilemap.SetTile(position, _leaves);
                tree.Nodes.Add(new Node(ENodeType.Leaves, position));
            }

            position = new Vector3Int(startX, y++, 0);
            _tilemap.SetTile(position, _trunk);
            tree.Nodes.Add(new Node(ENodeType.Trunk, position));
            
            position = new Vector3Int(startX, y++, 0);
            _tilemap.SetTile(position, _trunk);
            tree.Nodes.Add(new Node(ENodeType.Trunk, position));
            
            GenerateCrown(startX, y, 2, tree);
            ConnectNodes(tree);
        }

        private void GenerateCrown(int centerX, int startY, int branchCount, Tree tree)
        {
            int width = branchCount == 2 ? 5 : 7;
            int height = branchCount == 2 ? 3 : 4;
            int halfWidth = width / 2;
            for (int y = 0; y < height; y++)
            {
                for (int x = -halfWidth; x <= halfWidth; x++)
                {
                    var position = new Vector3Int(centerX + x, startY + y, 0);
                    _tilemap.SetTile(position, _crown);
                    tree.Nodes.Add(new Node(ENodeType.Crown, position));
                }
            }
        }
        
        private void ConnectNodes(Tree tree)
        {
            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                var currentNode = tree.Nodes[i];
                for (int j = 0; j < tree.Nodes.Count; j++)
                {
                    var targetNode = tree.Nodes[j];
                    if (CanConnectNodes(currentNode, targetNode))
                    {
                        currentNode.Connections.Add(targetNode);
                    }
                }
            }
        }

        private bool CanConnectNodes(Node current, Node target)
        {
            if (current.Type is ENodeType.Crown || target.Type is ENodeType.Crown)
                return false;
            if (Mathf.Abs(current.Position.x - target.Position.x) + Mathf.Abs(current.Position.y - target.Position.y) == 1)
            {
                return true;
            }
            return false;
        }

        private void CreateHollowTree(int startX)
        {
            var tree = new Tree(0);
            _trees.Add(tree);

            Node AddNode(ENodeType type, Vector3Int position, TileBase tile)
            {
                _tilemap.SetTile(position, tile);
                var node = new Node(type, position);
                tree.Nodes.Add(node);
                return node;
            }

            AddNode(ENodeType.Trunk, new Vector3Int(startX, 0, 0), _trunk);
            AddNode(ENodeType.Trunk, new Vector3Int(startX, 1, 0), _trunk);
            AddNode(ENodeType.Trunk, new Vector3Int(startX, 2, 0), _trunk);
            AddNode(ENodeType.Branch, new Vector3Int(startX-1, 2, 0), _branch);
            AddNode(ENodeType.Leaves, new Vector3Int(startX-2, 2, 0), _leaves);

            var node = AddNode(ENodeType.Hollow, new Vector3Int(startX, 3, 0), _trunkHollow);
            PlayerSpawnNode = node;
            AddNode(ENodeType.Trunk, new Vector3Int(startX, 4, 0), _trunk);
            AddNode(ENodeType.Trunk, new Vector3Int(startX, 5, 0), _trunk);
            AddNode(ENodeType.Branch, new Vector3Int(startX+1, 5, 0), _branch);
            AddNode(ENodeType.Leaves, new Vector3Int(startX+2, 5, 0), _leaves);

            AddNode(ENodeType.Trunk, new Vector3Int(startX, 6, 0), _trunk);
            AddNode(ENodeType.Trunk, new Vector3Int(startX, 7, 0), _trunk);

            AddNode(ENodeType.Crown, new Vector3Int(startX, 8, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX+1, 8, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX+2, 8, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX-1, 8, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX-2, 8, 0), _crown);

            AddNode(ENodeType.Crown, new Vector3Int(startX, 9, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX+1, 9, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX+2, 9, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX-1, 9, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX-2, 9, 0), _crown);

            AddNode(ENodeType.Crown, new Vector3Int(startX, 10, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX+1, 10, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX+2, 10, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX-1, 10, 0), _crown);
            AddNode(ENodeType.Crown, new Vector3Int(startX-2, 10, 0), _crown);
            
            ConnectNodes(tree);
        }

        public Vector3 MapToWorld(Vector3Int position)
        {
            return _tilemap.CellToWorld(position) + _tilemap.cellSize / 2;
        }
    }
}