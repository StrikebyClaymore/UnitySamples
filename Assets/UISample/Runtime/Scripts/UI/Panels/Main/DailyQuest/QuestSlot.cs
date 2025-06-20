using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class QuestSlot : MonoBehaviour
    {
        [SerializeField] private TMP_Text _questText;
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TMP_Text _rewardAmountText;
        [SerializeField] private TMP_Text _completedText;
        [SerializeField] private GameObject _rewardedImage;
    }
}