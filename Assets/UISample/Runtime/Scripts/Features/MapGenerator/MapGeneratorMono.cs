using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UISample.Features
{
    [ExecuteAlways]
    public class MapGeneratorMono : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private int _chunkWitdh = 22;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _ground, _trunk, _stick, _leaves;
        [SerializeField] private RuleTile _crown;
        [SerializeField] private Vector2Int _branchesRange = new Vector2Int(2, 3);
        
        [SerializeField] private int _moveDirection = 1;
        [SerializeField] private int _targetX;
        [SerializeField] private int _currentTreeX;
        [SerializeField] private int _treeSize = 5;
        
        [SerializeField] private List<int> _generatedTreePositions = new List<int>();
        
        private void Awake()
        {
            _currentTreeX = 0;
        }

        private void Update()
        {
            if(_target is null)
                return;
            var targetPosX = _tilemap.WorldToCell(_target.position).x;
            _targetX = targetPosX;
            if (targetPosX > _currentTreeX + _treeSize / 2)
            {
                _currentTreeX += _treeSize + 1;
                RemoveTree(_generatedTreePositions[0]);
                _generatedTreePositions.RemoveAt(0);
                GenerateTree(_currentTreeX + _treeSize + 1);
            }
        }

        private void RemoveTree(int startX)
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = -2; x < 3; x++)
                {
                    _tilemap.SetTile(new Vector3Int(startX + x, y, 0), null);
                }
            }
        }

        [Button("Generate")]
        private void Generate()
        {
            _tilemap.ClearAllTiles();
            GenerateTree(-6);
            GenerateTree(0);
            GenerateTree(6);
        }
        
        private void GenerateTree(int x)
        {
            _generatedTreePositions.Add(x);
            
            int y = 0;
            System.Random rnd = new System.Random();
            int branchSections = rnd.Next(_branchesRange.x, _branchesRange.y + 1);
            int? previousDirection = null;

            for (int i = 0; i < branchSections; i++)
            {
                for (int j = 0; j < 3; j++)
                    _tilemap.SetTile(new Vector3Int(x, y++, 0), _trunk);

                int direction;
                if (previousDirection == null)
                    direction = rnd.Next(0, 2) == 0 ? -1 : 1;
                else
                    direction = -previousDirection.Value;
                previousDirection = direction;

                _tilemap.SetTile(new Vector3Int(x + direction, y - 1, 0), _stick);
                _tilemap.SetTile(new Vector3Int(x + direction * 2, y - 1, 0), _leaves);
            }

            _tilemap.SetTile(new Vector3Int(x, y++, 0), _trunk);
            _tilemap.SetTile(new Vector3Int(x, y++, 0), _trunk);
            GenerateCrown(x, y, 2);
        }


        private void GenerateCrown(int centerX, int startY, int branchCount)
        {
            int width = branchCount == 2 ? 5 : 7;
            int height = branchCount == 2 ? 3 : 4;
            int halfWidth = width / 2;
            for (int y = 0; y < height; y++)
            {
                for (int x = -halfWidth; x <= halfWidth; x++)
                {
                    _tilemap.SetTile(new Vector3Int(centerX + x, startY + y, 0), _crown);
                }
            }
        }

        private void CreateStartTree()
        {
            _tilemap.SetTile(new Vector3Int(0, 0, 0), _ground);
            
            _tilemap.SetTile(new Vector3Int(0, 1, 0), _trunk);
            _tilemap.SetTile(new Vector3Int(0, 2, 0), _trunk);
            _tilemap.SetTile(new Vector3Int(0, 3, 0), _trunk);
            _tilemap.SetTile(new Vector3Int(-1, 3, 0), _stick);
            _tilemap.SetTile(new Vector3Int(-2, 3, 0), _leaves);
            
            _tilemap.SetTile(new Vector3Int(0, 4, 0), _trunk);
            _tilemap.SetTile(new Vector3Int(0, 5, 0), _trunk);
            _tilemap.SetTile(new Vector3Int(0, 6, 0), _trunk);
            _tilemap.SetTile(new Vector3Int(1, 6, 0), _stick);
            _tilemap.SetTile(new Vector3Int(2, 6, 0), _leaves);
            _tilemap.SetTile(new Vector3Int(0, 7, 0), _trunk);
            _tilemap.SetTile(new Vector3Int(0, 8, 0), _trunk);
            
            _tilemap.SetTile(new Vector3Int(0, 9, 0), _crown);
            _tilemap.SetTile(new Vector3Int(1, 9, 0), _crown);
            _tilemap.SetTile(new Vector3Int(2, 9, 0), _crown);
            _tilemap.SetTile(new Vector3Int(-1, 9, 0), _crown);
            _tilemap.SetTile(new Vector3Int(-2, 9, 0), _crown);
            
            _tilemap.SetTile(new Vector3Int(0, 10, 0), _crown);
            _tilemap.SetTile(new Vector3Int(1, 10, 0), _crown);
            _tilemap.SetTile(new Vector3Int(2, 10, 0), _crown);
            _tilemap.SetTile(new Vector3Int(-1, 10, 0), _crown);
            _tilemap.SetTile(new Vector3Int(-2, 10, 0), _crown);
            
            _tilemap.SetTile(new Vector3Int(0, 11, 0), _crown);
            _tilemap.SetTile(new Vector3Int(1, 11, 0), _crown);
            _tilemap.SetTile(new Vector3Int(2, 11, 0), _crown);
            _tilemap.SetTile(new Vector3Int(-1, 11, 0), _crown);
            _tilemap.SetTile(new Vector3Int(-2, 11, 0), _crown);
        }
    }
}