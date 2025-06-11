using System.Linq;
using UISample.Infrastructure;
using UISample.Utility;
using UnityEngine;

namespace UISample.Features
{
    public class Parallax : MonoBehaviour, IInitializable, IUpdatable
    {
        [System.Serializable]
        private struct ParallaxLayer
        {
            [field: SerializeField] public Transform[] Transforms { get; private set; }
            [field: SerializeField] public float Speed { get; private set; }
        }
        
        [SerializeField] private Transform _target;
        [SerializeField] private EUpdateMode _updateMode = EUpdateMode.LateUpdate;
        [SerializeField] private float _backgroundWidth = 15.9f;
        [SerializeField] private ParallaxLayer[] _parallaxLayers;
        private Vector3 _lastTargetPosition;
        public Transform Target
        {
            set
            {
                _target = value;
                if(_target != null)
                    _lastTargetPosition = _target.position;
            }
        }

        public bool Initialized { get; private set; }

        public void Initialize()
        {
            if(_target != null)
                _lastTargetPosition = _target.position;
            Initialized = true;
        }
        
        public void CustomUpdate()
        {
            if(_updateMode is EUpdateMode.Update)
                UpdateParallax();
        }

        public void CustomFixedUpdate()
        {
            if(_updateMode is EUpdateMode.FixedUpdate)
                UpdateParallax();
        }

        public void CustomLateUpdate()
        {
            if(_updateMode is EUpdateMode.LateUpdate)
                UpdateParallax();
        }
        
        private void UpdateParallax()
        {
            if (_target == null)
                return;
            Vector3 delta = _target.position - _lastTargetPosition;
            foreach (var layer in _parallaxLayers)
                UpdateParallaxLayer(layer, delta);
            _lastTargetPosition = _target.position;
        }
        
        private void UpdateParallaxLayer(ParallaxLayer layer, Vector3 delta)
        {
            foreach (var transform in layer.Transforms)
                transform.position += new Vector3(delta.x * layer.Speed, 0f, 0f);
            float viewCenterX = _target.position.x;
            float halfSpan = _backgroundWidth * layer.Transforms.Length / 2f;
            foreach (var transform in layer.Transforms)
            {
                float distance = viewCenterX - transform.position.x;
                if (Mathf.Abs(distance) > halfSpan)
                {
                    float direction = Mathf.Sign(distance);
                    float extremeX = direction > 0
                        ? layer.Transforms.Max(tr => tr.position.x)
                        : layer.Transforms.Min(tr => tr.position.x);
                    transform.position = new Vector3(
                        extremeX + direction * _backgroundWidth,
                        transform.position.y,
                        transform.position.z
                    );
                }
            }
        }
    }
}