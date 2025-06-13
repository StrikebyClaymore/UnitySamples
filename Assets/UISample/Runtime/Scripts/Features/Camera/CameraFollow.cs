using System;
using NaughtyAttributes;
using UISample.Infrastructure;
using UISample.Utility;
using UnityEngine;

namespace UISample.Features
{
    public class CameraFollow : MonoBehaviour, IUpdatable
    {
        [Serializable]
        private struct FollowBounds
        {
            public bool Enabled;
            public float MinX;
            public float MaxX;
            public float MinY;
            public float MaxY;
        }   
        
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;
        [SerializeField] private EUpdateMode _updateMode = EUpdateMode.LateUpdate;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private FollowBounds _bounds;
        public Transform Target { set => _target = value; }

        public void CustomUpdate()
        {
            if(_updateMode is EUpdateMode.Update)
                Follow();
        }

        public void CustomFixedUpdate()
        {
            if(_updateMode is EUpdateMode.FixedUpdate)
                Follow();
        }

        public void CustomLateUpdate()
        {
            if(_updateMode is EUpdateMode.LateUpdate)
                Follow();
        }
        
        private void Follow()
        {
            if(_target == null)
                return;
            var targetPosition = _target.position + _offset;
            if (_bounds.Enabled)
                targetPosition = ApplyBounds(targetPosition);
            transform.position = targetPosition;
        }

        private Vector3 ApplyBounds(Vector3 targetPosition)
        {
            float cameraHeight = 2f * _camera.orthographicSize;
            float cameraWidth = cameraHeight * _camera.aspect;
            Vector3 cameraHalfSize = new Vector3(cameraWidth * 0.5f, cameraHeight * 0.5f, 0f);
            if (_bounds.MinX != _bounds.MaxX)
            {
                float minX = _bounds.MinX + cameraHalfSize.x;
                float maxX = _bounds.MaxX - cameraHalfSize.x;
                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            }
            if (_bounds.MinY != _bounds.MaxY)
            {
                float minY = _bounds.MinY + cameraHalfSize.y;
                float maxY = _bounds.MaxY - cameraHalfSize.y;
                targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            }
            targetPosition.z = _target.position.z + _offset.z;
            return targetPosition;
        }
        
#if UNITY_EDITOR
        [Button("Update Follow")]
        private void UpdateFollow()
        {
            Follow();
        }

        private void OnValidate()
        {
            
        }
#endif
    }
}