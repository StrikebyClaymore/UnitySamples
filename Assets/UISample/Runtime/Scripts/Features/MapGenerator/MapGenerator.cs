using System.Collections.Generic;
using Plugins.ServiceLocator;
using Pool;
using UISample.Data;
using UISample.Infrastructure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UISample.Features
{
    public class MapGenerator : ILocalService, IInitializable, IUpdate
    {
        private const int TreeSize = 5;
        private readonly MapGeneratorConfig _config;
        private readonly List<MapTree> _trees = new();
        private readonly List<Acorn> _acorns = new();
        private readonly Tilemap _tilemap;
        private Transform _target;
        private int _currentTreeX;
        public MonoPool<Acorn> AcornsPool { get; private set; }
        public IReadOnlyList<MapTree> Trees => _trees;
        public Transform Target
        {
            set => _target = value;
        }
        public MapNode PlayerSpawnNode { get; private set; }
        public int MoveDirection { get; private set; }
        public bool Initialized { get; private set; }

        public MapGenerator(ConfigsContainer configs, Tilemap tilemap)
        {
            _config = configs.MapGeneratorConfig;
            _tilemap = tilemap;
            AcornsPool = new MonoPool<Acorn>(_config.AcornPrefab, 2, new GameObject("Acorns Pool").transform);
            _currentTreeX = 0;
        }

        public void Initialize()
        {
            Generate();
            Initialized = true;
        }

        public void CustomUpdate()
        {
            if (_target == null)
                return;

            var targetPosX = _tilemap.WorldToCell(_target.position).x;

            if (MoveDirection == 1)
            {
                if (targetPosX > _currentTreeX + TreeSize / 2)
                {
                    _currentTreeX += TreeSize + 1;
                    RemoveTree(_trees[0]);
                    _trees.RemoveAt(0);
                    GenerateTree(_currentTreeX + TreeSize + 1);
                }
            }
            else if (MoveDirection == -1)
            {
                if (targetPosX < _currentTreeX - TreeSize / 2)
                {
                    _currentTreeX -= TreeSize + 1;
                    RemoveTree(_trees[2]);
                    _trees.RemoveAt(2);
                    GenerateTree(_currentTreeX - TreeSize - 1);
                }
            }
        }
        
        public void SetDirection(int direction)
        {
            if(MoveDirection == 0)
                MoveDirection = direction;
        }
        
        public Vector3 MapToWorld(Vector3Int position)
        {
            return _tilemap.CellToWorld(position) + _tilemap.cellSize / 2;
        }
        
        private void ResetGame()
        {
            _tilemap.ClearAllTiles();
            if (Application.isPlaying)
                foreach (var acorn in _acorns)
                    AcornsPool.Release(acorn);
            _trees.Clear();
            _currentTreeX = 0;
            MoveDirection = 0;
        }
        
        private void Generate()
        {
            ResetGame();
            GenerateTree(-6);
            CreateHollowTree(0);
            GenerateTree(6);
        }
        
        private void GenerateTree(int startX)
        {
            if (startX != TreeSize + 1 && (startX / (TreeSize + 1) - 1) % _config.HollowSpawnNumber == 0)
            {
                CreateHollowTree(startX);
                return;
            }
            
            var tree = new MapTree(startX);
            if(MoveDirection is 0 or 1)
                _trees.Add(tree);
            else
                _trees.Insert(0, tree);
            
            int y = 0;
            System.Random rnd = new System.Random();
            int branchSections = rnd.Next(_config.BranchesRange.x, _config.BranchesRange.y + 1);
            int? previousDirection = null;
            Vector3Int position;
            
            for (int i = 0; i < branchSections; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    position = new Vector3Int(startX, y++, 0);
                    _tilemap.SetTile(position, _config.Trunk);
                    var node = new MapNode(EMapNodeType.Trunk, position);
                    tree.Nodes.Add(node);
                }

                int direction;
                if (previousDirection == null)
                    direction = rnd.Next(0, 2) == 0 ? -1 : 1;
                else
                    direction = -previousDirection.Value;
                previousDirection = direction;

                position = new Vector3Int(startX + direction, y - 1, 0);
                _tilemap.SetTile(position, _config.Branch);
                tree.Nodes.Add(new MapNode(EMapNodeType.Branch, position));

                TrySpawnAcorn(position);
                
                position = new Vector3Int(startX + direction * 2, y - 1, 0);
                _tilemap.SetTile(position, _config.Leaves);
                tree.Nodes.Add(new MapNode(EMapNodeType.Leaves, position));
            }

            position = new Vector3Int(startX, y++, 0);
            _tilemap.SetTile(position, _config.Trunk);
            tree.Nodes.Add(new MapNode(EMapNodeType.Trunk, position));
            
            position = new Vector3Int(startX, y++, 0);
            _tilemap.SetTile(position, _config.Trunk);
            tree.Nodes.Add(new MapNode(EMapNodeType.Trunk, position));
            
            GenerateCrown(startX, y, 2, tree);
            ConnectNodes(tree);
        }

        private void RemoveTree(MapTree mapTree)
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = -2; x < 3; x++)
                {
                    _tilemap.SetTile(new Vector3Int(mapTree.PositionX + x, y, 0), null);
                }
            }
        }
        
        private void TrySpawnAcorn(Vector3Int position)
        {
            if (Application.isPlaying && Random.Range(0, 100f) > 50f)
            {
                var acorn = AcornsPool.Get();
                acorn.transform.position = MapToWorld(position);
                _acorns.Add(acorn);
            }
        }

        private void GenerateCrown(int centerX, int startY, int branchCount, MapTree mapTree)
        {
            int width = branchCount == 2 ? 5 : 7;
            int height = branchCount == 2 ? 3 : 4;
            int halfWidth = width / 2;
            for (int y = 0; y < height; y++)
            {
                for (int x = -halfWidth; x <= halfWidth; x++)
                {
                    var position = new Vector3Int(centerX + x, startY + y, 0);
                    _tilemap.SetTile(position, _config.Crown);
                    mapTree.Nodes.Add(new MapNode(EMapNodeType.Crown, position));
                }
            }
        }
        
        private void ConnectNodes(MapTree mapTree)
        {
            for (int i = 0; i < mapTree.Nodes.Count; i++)
            {
                var currentNode = mapTree.Nodes[i];
                for (int j = 0; j < mapTree.Nodes.Count; j++)
                {
                    var targetNode = mapTree.Nodes[j];
                    if (CanConnectNodes(currentNode, targetNode))
                    {
                        currentNode.Connections.Add(targetNode);
                    }
                }
            }
        }

        private bool CanConnectNodes(MapNode current, MapNode target)
        {
            if (current.Type is EMapNodeType.Crown || target.Type is EMapNodeType.Crown)
                return false;
            if (Mathf.Abs(current.Position.x - target.Position.x) + Mathf.Abs(current.Position.y - target.Position.y) == 1)
            {
                return true;
            }
            return false;
        }

        private void CreateHollowTree(int startX)
        {
            var tree = new MapTree(0);
            if(MoveDirection is 0 or 1)
                _trees.Add(tree);
            else
                _trees.Insert(0, tree);

            MapNode AddNode(EMapNodeType type, Vector3Int position, TileBase tile)
            {
                _tilemap.SetTile(position, tile);
                var node = new MapNode(type, position);
                tree.Nodes.Add(node);
                return node;
            }

            AddNode(EMapNodeType.Trunk, new Vector3Int(startX, 0, 0), _config.Trunk);
            AddNode(EMapNodeType.Trunk, new Vector3Int(startX, 1, 0), _config.Trunk);
            AddNode(EMapNodeType.Trunk, new Vector3Int(startX, 2, 0), _config.Trunk);
            AddNode(EMapNodeType.Branch, new Vector3Int(startX-1, 2, 0), _config.Branch);
            AddNode(EMapNodeType.Leaves, new Vector3Int(startX-2, 2, 0), _config.Leaves);
            
            if(startX != 0)
                TrySpawnAcorn(new Vector3Int(startX-1, 2, 0));

            var node = AddNode(EMapNodeType.Hollow, new Vector3Int(startX, 3, 0), _config.TrunkHollow);
            PlayerSpawnNode = node;
            AddNode(EMapNodeType.Trunk, new Vector3Int(startX, 4, 0), _config.Trunk);
            AddNode(EMapNodeType.Trunk, new Vector3Int(startX, 5, 0), _config.Trunk);
            AddNode(EMapNodeType.Branch, new Vector3Int(startX+1, 5, 0), _config.Branch);
            AddNode(EMapNodeType.Leaves, new Vector3Int(startX+2, 5, 0), _config.Leaves);
            
            if(startX != 0)
                TrySpawnAcorn(new Vector3Int(startX+1, 5, 0));

            AddNode(EMapNodeType.Trunk, new Vector3Int(startX, 6, 0), _config.Trunk);
            AddNode(EMapNodeType.Trunk, new Vector3Int(startX, 7, 0), _config.Trunk);

            AddNode(EMapNodeType.Crown, new Vector3Int(startX, 8, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX+1, 8, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX+2, 8, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX-1, 8, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX-2, 8, 0), _config.Crown);

            AddNode(EMapNodeType.Crown, new Vector3Int(startX, 9, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX+1, 9, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX+2, 9, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX-1, 9, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX-2, 9, 0), _config.Crown);

            AddNode(EMapNodeType.Crown, new Vector3Int(startX, 10, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX+1, 10, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX+2, 10, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX-1, 10, 0), _config.Crown);
            AddNode(EMapNodeType.Crown, new Vector3Int(startX-2, 10, 0), _config.Crown);
            
            ConnectNodes(tree);
        }
    }
}