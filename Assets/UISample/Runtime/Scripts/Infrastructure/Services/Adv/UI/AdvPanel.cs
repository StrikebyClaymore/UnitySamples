using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class AdvPanel : MonoBehaviour, IDisposable
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private Button _rewardButton;
        private const int AdvTime = 3;
        private readonly WaitForSecondsRealtime _advWait = new(1);
        private Coroutine _coroutine;
        private bool _rewardUnlocked;
        public event Action<bool> OnClose;

        private void Awake()
        {
            _closeButton.onClick.AddListener(ClosePressed);
            _rewardButton.onClick.AddListener(RewardPressed);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _coroutine = StartCoroutine(AdvTimer());
        }

        public void Hide()
        {
            Dispose();
            gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _rewardUnlocked = false;
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _advWait.Reset();
            _rewardButton.gameObject.SetActive(false);
        }

        private IEnumerator AdvTimer()
        {
            var seconds = AdvTime;
            while (seconds != 0)
            {
                _timerText.SetText(seconds.ToString());
                yield return _advWait;
                seconds--;
            }
            _timerText.SetText("0");
            _rewardUnlocked = true;
            _rewardButton.gameObject.SetActive(true);
        }

        private void ClosePressed()
        {
            OnClose?.Invoke(_rewardUnlocked);
            Hide();
        }

        private void RewardPressed()
        {
            OnClose?.Invoke(true);
            Hide();
        }
    }
}