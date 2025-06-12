using DG.Tweening;
using Plugins.ServiceLocator;
using UISample.Infrastructure;
using UISample.UI;
using UnityEngine;

namespace UISample.Features
{
    public class PlayerMovement : IUpdate
    {
        private readonly Transform _transform;
        private readonly MapGeneratorMono _mapGenerator;
        private readonly int _moveDistance = 1;
        private readonly float _moveInTreeDuration = 0.3f;
        private readonly float _moveBetweenTreeDuration = 0.7f;
        private Vector3Int _direction = Vector3Int.zero;
        private bool _isMoving;
        private MapGeneratorMono.Tree _currentTree;
        private MapGeneratorMono.Node _currentNode;
        
        public PlayerMovement(PlayerView view, MapGeneratorMono mapGenerator)
        {
            _transform = view.transform;
            _mapGenerator = mapGenerator;
            _currentTree = _mapGenerator.Trees[1];
            _currentNode = _mapGenerator.PlayerSpawnNode;
            _transform.position = _mapGenerator.MapToWorld(_currentNode.Position);
            var controls = ServiceLocator.Get<GameplaySceneUI>().GetController<ControlsController>();
            controls.OnControlPressed.AddListener(HandleControlPressed);
            controls.OnControlReleased.AddListener(HandleControlReleased);
        }

        public void CustomUpdate()
        {
            if (_isMoving || _direction == Vector3Int.zero)
                return;

            if (CanMoveInTree())
            {
                _isMoving = true;
                MoveInTree();
                _transform.DOMove(_mapGenerator.MapToWorld(_currentNode.Position), _moveInTreeDuration).OnComplete(() =>
                {
                    _isMoving = false;
                });
            }
            else if (CanMoveBetweenTree())
            {
                _isMoving = true;
                MoveBetweenTree();
                _transform.DOMove(_mapGenerator.MapToWorld(_currentNode.Position), _moveBetweenTreeDuration).OnComplete(() =>
                {
                    _isMoving = false;
                });
            }
        }

        private void HandleControlPressed(Vector3Int direction)
        {
            if(_direction != Vector3Int.zero)
                return;
            _direction = direction;
        }
        
        private void HandleControlReleased(Vector3Int direction)
        {
            if(direction != _direction)
                return;
            _direction = Vector3Int.zero;
        }

        private void MoveInTree()
        {
            foreach (var node in _currentNode.Connections)
            {
                if (node.Position == _currentNode.Position + _direction)
                {
                    Debug.Log($"Move to {node.Type} {node.Position}");
                    _currentNode = node;
                    break;
                }
            }
        }

        private void MoveBetweenTree()
        {
            if (_currentNode.Type is MapGeneratorMono.ENodeType.Leaves)
            {
                foreach (var tree in _mapGenerator.Trees)
                {
                    if(tree == _currentTree)
                        continue;
                    foreach (var node in tree.Nodes)
                    {
                        if (node.Type is MapGeneratorMono.ENodeType.Leaves &&
                            Mathf.Abs(_currentNode.Position.x - node.Position.x) +
                            Mathf.Abs(_currentNode.Position.y - node.Position.y) < 6)
                        {
                            Debug.Log($"Move to next tree {node.Type} {node.Position}");
                            _currentTree = tree;
                            _currentNode = node;
                        }
                    }
                }
            }
        }
        
        private bool CanMoveInTree()
        {
            foreach (var node in _currentNode.Connections)
            {
                if (node.Position == _currentNode.Position + _direction)
                {
                    Debug.Log($"Can move to {node.Type} {node.Position}");
                    return true;
                }
            }
            return false;
        }

        private bool CanMoveBetweenTree()
        {
            if (_currentNode.Type is MapGeneratorMono.ENodeType.Leaves)
            {
                foreach (var tree in _mapGenerator.Trees)
                {
                    if (tree == _currentTree)
                        continue;
                    foreach (var node in tree.Nodes)
                    {
                        if (node.Type is MapGeneratorMono.ENodeType.Leaves &&
                            Mathf.Abs(_currentNode.Position.x - node.Position.x) +
                            Mathf.Abs(_currentNode.Position.y - node.Position.y) < 6)
                        {
                            Debug.Log($"Can move to next tree {node.Type} {node.Position}");
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}