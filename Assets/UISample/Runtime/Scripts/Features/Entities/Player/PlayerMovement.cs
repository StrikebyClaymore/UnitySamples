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
        private readonly GameplaySceneUI _sceneUI;
        private Vector3Int _direction = Vector3Int.zero;
        private bool _isMoving;
        private MapGeneratorMono.Tree _currentTree;
        private MapGeneratorMono.Node _currentNode;
        private MapGeneratorMono.Node _visitedHollow;
        
        public PlayerMovement(PlayerView view, MapGeneratorMono mapGenerator)
        {
            _transform = view.transform;
            _mapGenerator = mapGenerator;
            _currentTree = _mapGenerator.Trees[1];
            _currentNode = _mapGenerator.PlayerSpawnNode;
            _visitedHollow = _currentNode;
            _transform.position = _mapGenerator.MapToWorld(_currentNode.Position);
            var controls = ServiceLocator.Get<GameplaySceneUI>().GetController<ControlsController>();
            _sceneUI = ServiceLocator.Get<GameplaySceneUI>();
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
                    if (_direction == Vector3Int.zero && _currentNode != _visitedHollow && _currentNode.Type is MapGeneratorMono.ENodeType.Hollow)
                    {
                        _visitedHollow = _currentNode;
                        _sceneUI.ShowController<HollowController>();
                    }
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
                    _currentNode = node;
                    return;
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
                            _currentTree = tree;
                            _currentNode = node;
                            _mapGenerator.SetDirection(_direction.x);
                            return;
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
                    return true;
                }
            }
            return false;
        }

        private bool CanMoveBetweenTree()
        {
            if (_mapGenerator.MoveDirection != 0 && _direction.x != _mapGenerator.MoveDirection)
                return false;
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
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}