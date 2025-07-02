using DG.Tweening;
using Plugins.ServiceLocator;
using UISample.Infrastructure;
using UnityEngine;

namespace UISample.UI
{
    public class RouletteController : BaseController, IUpdate
    {
        private const float StartPanelFadeTime = 0.3f;
        private const float RotateSpeed = 10.0f;
        private const float RotateTime = 4.0f;
        private const float AccelerationTime = 1.5f;
        private readonly RouletteView _view;
        private Vector2 _anchorMinStart;
        private Vector2 _anchorMinEnd;
        private Vector2 _anchorMaxStart;
        private Vector2 _anchorMaxEnd;
        private float _rotationTimePassed;
        private float _currentRotationSpeed;
        private float _maxSpeedReached;
        private bool _rotateEnabled;

        public RouletteController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<RouletteView>();
            _view.CloseButton.onClick.AddListener(ClosePressed);
            _view.ShadowCloseButton.onClick.AddListener(ClosePressed);
            _view.StartButton.onClick.AddListener(StartPressed);
            ServiceLocator.Get<ApplicationLoop>().AddUpdatable(this);
            InitRoulette();
        }

        public override void Show(bool instantly = false)
        {
            _view.StartPanel.alpha = 1;
            _view.StartPanel.interactable = true;
            _view.StartPanel.blocksRaycasts = true;
            base.Show(instantly);
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            base.Hide(instantly);
            _view.Hide(instantly);
        }

        public void CustomUpdate()
        {
            if(!_rotateEnabled)
                return;
            RotateRoulette();
        }

        private void ClosePressed()
        {
            Hide();
        }

        private void StartPressed()
        {
            _view.StartPanel.interactable = false;
            _view.StartPanel.blocksRaycasts = false;
            _view.StartPanel.DOFade(0, StartPanelFadeTime)
                .OnComplete(StartRoulette);
        }

        private void InitRoulette()    
        {
            var slot = _view.SlotContainers[0];
            slot.Rect.anchorMin = new Vector2(slot.Rect.anchorMin.x, slot.Rect.anchorMin.y + 1);
            slot.Rect.anchorMax = new Vector2(slot.Rect.anchorMax.x, slot.Rect.anchorMax.y + 1);
            _anchorMinStart = slot.Rect.anchorMin;
            _anchorMaxStart = slot.Rect.anchorMax;
            
            slot = _view.SlotContainers[1];
            _anchorMinEnd = new Vector2(slot.Rect.anchorMin.x, slot.Rect.anchorMin.y - 1);
            _anchorMaxEnd = new Vector2(slot.Rect.anchorMax.x, slot.Rect.anchorMax.y - 1);
        }
        
        private void StartRoulette()
        {
            _rotationTimePassed = 0;
            _maxSpeedReached = 0;
            _currentRotationSpeed = RotateSpeed;
            _rotateEnabled = true;
        }

        private void RotateRoulette()
        {
            float deltaTime = Time.unscaledDeltaTime;

            _rotationTimePassed += deltaTime;
            
            if (_rotationTimePassed <= AccelerationTime)
            {
                float accelerationTime = _rotationTimePassed / AccelerationTime;
                _currentRotationSpeed = Mathf.Lerp(0, RotateSpeed, accelerationTime);
                _maxSpeedReached = _currentRotationSpeed;
            }
            else
            {
                float accelerationTime = (_rotationTimePassed - AccelerationTime) / (RotateTime - AccelerationTime);
                _currentRotationSpeed = Mathf.Lerp(_maxSpeedReached, 0, accelerationTime);
            }

            foreach (var slot in _view.SlotContainers)
            {
                slot.Rect.anchorMin = Vector2.MoveTowards(slot.Rect.anchorMin, _anchorMinEnd, _currentRotationSpeed * deltaTime);
                slot.Rect.anchorMax = Vector2.MoveTowards(slot.Rect.anchorMax, _anchorMaxEnd, _currentRotationSpeed * deltaTime);

                if (slot.Rect.anchorMin.y <= _anchorMinEnd.y || slot.Rect.anchorMax.y <= _anchorMaxEnd.y)
                {
                    slot.Rect.anchorMin = _anchorMinStart;
                    slot.Rect.anchorMax = _anchorMaxStart;
                }
            }

            if (_rotationTimePassed >= RotateTime)
            {
                RouletteFinished();
            }
        }

        private void RouletteFinished()
        {
            _rotateEnabled = false;

            RectTransform center = _view.CenterPoint;
            Vector3 centerWorldPos = center.position;

            RouletteSlot winningSlot = null;
            float closestDistance = float.MaxValue;

            foreach (var slotContainer in _view.SlotContainers)
            {
                foreach (var slot in slotContainer.RouletteSlot)
                {
                    float distance = Vector3.Distance(slot.Rect.position, centerWorldPos);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        winningSlot = slot;
                    }
                }
            }

            if (winningSlot != null)
            {
                Debug.Log("Победивший слот: " + winningSlot.name);
                winningSlot.SetWin();
            }
        }
    }
}