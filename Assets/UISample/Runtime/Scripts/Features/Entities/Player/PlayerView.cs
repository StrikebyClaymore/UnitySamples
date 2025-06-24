using UnityEngine;
using UnityEngine.Events;

namespace UISample.Features
{
    public class PlayerView : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        [SerializeField] private Sprite _spriteHorizontal;
        [SerializeField] private Sprite _spriteVertical;
        public readonly UnityEvent<Collider2D> OnTriggerEnter = new();

        public void Flip(Vector3Int direction)
        {
            SpriteRenderer.flipX = direction.x == -1;
            SpriteRenderer.flipY = direction.y == -1;
            if(direction.x != 0)
                SpriteRenderer.sprite = _spriteHorizontal;
            else if (direction.y != 0)
                SpriteRenderer.sprite = _spriteVertical;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerEnter?.Invoke(collision);
        }
    }
}