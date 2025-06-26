using UISample.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class MenuCharacter : MonoBehaviour
    {
        [SerializeField] private Image _hatImage;

        public void SetHatSprite(Sprite sprite)
        {
            _hatImage.sprite = sprite;
            _hatImage.SetActive(sprite != null);
        }
    }
}