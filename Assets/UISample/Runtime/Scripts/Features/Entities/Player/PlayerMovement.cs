using DG.Tweening;
using Plugins.ServiceLocator;
using UISample.Infrastructure;
using UISample.UI;
using UnityEngine;

namespace UISample.Features
{
    public class PlayerMovement : IUpdate
    {
        private readonly PlayerView _view;
        private readonly Transform _transform;
        private readonly MapGenerator _mapGenerator;
        private readonly float _moveInTreeDuration = 0.3f;
        private readonly float _moveBetweenTreeDuration = 0.7f;
        private readonly SceneUI _sceneUI;
        private Vector3Int _direction = Vector3Int.zero;
        private bool _isMoving;
        private MapTree _currentTree;
        private MapNode _currentNode;
        private MapNode _visitedHollow;
        
        public PlayerMovement(PlayerView view, MapGenerator mapGenerator)
        {
            _view = view;
            _transform = _view.transform;
            _mapGenerator = mapGenerator;
            _currentTree = _mapGenerator.Trees[1];
            _currentNode = _mapGenerator.PlayerSpawnNode;
            _visitedHollow = _currentNode;
            _transform.position = _mapGenerator.MapToWorld(_currentNode.Position);
            _sceneUI = ServiceLocator.Get<SceneUI>();
            var controls = _sceneUI.GetController<ControlsController>();
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
                    if (_direction == Vector3Int.zero && _currentNode != _visitedHollow && _currentNode.Type is EMapNodeType.Hollow)
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
            if(CanMoveInTree())
                _view.Flip(_direction);
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
            if (_currentNode.Type is EMapNodeType.Leaves)
            {
                foreach (var tree in _mapGenerator.Trees)
                {
                    if (tree == _currentTree || (_mapGenerator.MoveDirection != 0 && (int)Mathf.Sign(tree.PositionX - _currentTree.PositionX) != _mapGenerator.MoveDirection))
                        continue;
                    foreach (var node in tree.Nodes)
                    {
                        if (node.Type is not EMapNodeType.Leaves)
                            continue;
                        var distance = node.Position - _currentNode.Position;
                        if(Mathf.Abs(distance.x) != 2)
                            continue;
                        if (_direction.x != 0)
                        {
                            _mapGenerator.SetDirection(tree.PositionX > _currentTree.PositionX ? 1 : -1);
                            _currentTree = tree;
                            _currentNode = node;
                            return;
                        }
                        if (_direction.y == 0 || node.Position.y == _currentNode.Position.y)
                            continue;
                        if (_direction.y == 1 && distance.y > 0)
                        {
                            _mapGenerator.SetDirection(tree.PositionX > _currentTree.PositionX ? 1 : -1);
                            _currentTree = tree;
                            _currentNode = node;
                            return;
                        }
                        if (_direction.y == -1 && distance.y < 0)
                        {
                            _mapGenerator.SetDirection(tree.PositionX > _currentTree.PositionX ? 1 : -1);
                            _currentTree = tree;
                            _currentNode = node;
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
            if (_currentNode.Type is EMapNodeType.Leaves)
            {
                foreach (var tree in _mapGenerator.Trees)
                {
                    if (tree == _currentTree || (_mapGenerator.MoveDirection != 0 && (int)Mathf.Sign(tree.PositionX - _currentTree.PositionX) != _mapGenerator.MoveDirection))
                        continue;
                    foreach (var node in tree.Nodes)
                    {
                        if (node.Type is not EMapNodeType.Leaves)
                            continue;
                        var distance = node.Position - _currentNode.Position;
                        if(Mathf.Abs(distance.x) != 2)
                            continue;
                        if (_direction.x != 0)
                            return true;
                        if (_direction.y == 0 || node.Position.y == _currentNode.Position.y)
                            continue;
                        if (_direction.y == 1 && distance.y > 0)
                            return true;
                        if (_direction.y == -1 && distance.y < 0)
                            return true;
                    }
                }
            }

            return false;
        }
    }
}