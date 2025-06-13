using UnityEngine;
using UnityEngine.Events;

namespace UISample.Features
{
    public class PlayerView : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        public readonly UnityEvent<Collider2D> OnTriggerEnter = new();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerEnter?.Invoke(collision);
        }
    }
}