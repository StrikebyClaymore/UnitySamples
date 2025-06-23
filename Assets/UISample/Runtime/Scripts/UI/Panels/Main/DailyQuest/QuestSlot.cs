using System;
using TMPro;
using UISample.Features;
using UISample.Utility;
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
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TMP_Text _rewardAmountText;
        [SerializeField] private GameObject _rewardedImage;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _completedColor;
        private int _index;
        private Action<int> _onClick;

        private void Awake()
        {
            _rewardButton.onClick.AddListener(Click);
        }

        public void Initialize(Quest quest, Sprite rewardIcon, int index, Action<int> onClick)
        {
            _index = index;
            _onClick = onClick;
            _questText.text = $"{quest.Config.Type} {quest.Config.Target}";
            _progressText.text = $"{quest.Model.Progress}/{quest.Config.TargetAmount}";
            _progressSlider.maxValue = quest.Config.TargetAmount;
            _progressSlider.value = quest.Model.Progress;
            _rewardIcon.sprite = rewardIcon;
            _rewardAmountText.text = quest.Config.RewardAmount.ToString();
            SetState(quest.Model.State);
        }

        public void SetState(EQuestState state)
        {
            switch (state)
            {
                case EQuestState.Process:
                    Process();
                    break;
                case EQuestState.Completed:
                    Complete();
                    break;
                case EQuestState.Rewarded:
                    Reward();
                    break;
            }
        }
        
        private void Process()
        {
            _fillImage.color = _defaultColor;
            _rewardImage.color = _defaultColor;
            _rewardedImage.SetActive(false);
        }
        
        private void Complete()
        {
            _fillImage.color = _completedColor;
            _rewardImage.color = _completedColor;
            _rewardedImage.SetActive(false);
        }

        private void Reward()
        {
            _rewardAmountText.SetActive(false);
            _fillImage.color = _completedColor;
            _rewardImage.color = _completedColor;
            _rewardedImage.SetActive(true);
        }

        private void Click()
        {
            _onClick?.Invoke(_index);
        }
    }
}