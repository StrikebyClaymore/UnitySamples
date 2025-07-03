using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Plugins.ServiceLocator;
using UISample.Features;
using UISample.Infrastructure;
using UnityEngine;

namespace UISample.UI
{
    public class RouletteController : BaseController, IUpdate
    {
        private enum State
        {
            Stop,
            Accelerate,
            Rotate,
            DeAccelerate
        }
        
        private const float StartPanelFadeTime = 0.3f;
        private const float RotateTime = 1.0f;
        private const float RotateSpeed = 10.0f;
        private const float AccelerationTime = 1.5f;
        private readonly RouletteView _view;
        private readonly SkinsManager _skinsManager;
        private readonly List<Skin> _skins = new();
        private int _lastSkinIndex;
        private Vector2 _anchorMinStart;
        private Vector2 _anchorMinEnd;
        private Vector2 _anchorMaxStart;
        private float _rotationTimePassed;
        private float _currentRotationSpeed;
        private int _predictWinnerIndex;
        private float _predictedSlotDistance;
        private State _state;
        private float _deAccelerationDistance;
        private float _slotHeight;
        private bool _slotReplaced;

        public RouletteController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<RouletteView>();
            _skinsManager = ServiceLocator.GetLocal<SkinsManager>();
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
            SetupSkins();
            ResetSlotContainers();
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
            if(_state is State.Stop)
                return;
            RotateRoulette();
        }
        
        public Skin PredictWinner()
        {
            var random = new System.Random(Random.Range(int.MinValue, int.MaxValue));
            _skins.Clear();
            _skins.AddRange(_skinsManager.Skins.Where(x => !x.IsUnlocked)
                .OrderBy(_ => random.Next())
                .ToList());
            _predictWinnerIndex = Random.Range(0, _skins.Count);
            return _skins[_predictWinnerIndex];
        }

        public override void Dispose()
        {
            ServiceLocator.Get<ApplicationLoop>().RemoveUpdatable(this);
            base.Dispose();
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

        private void ResetSlotContainers()
        {
            _view.SlotContainers[0].Rect.anchorMin = _anchorMinStart;
            _view.SlotContainers[0].Rect.anchorMax = _anchorMaxStart;
            
            var rect = _view.SlotContainers[1].Rect;
            rect.anchorMin = new Vector2(rect.anchorMin.x, _anchorMinStart.y - 1);
            rect.anchorMax = new Vector2(rect.anchorMax.x, _anchorMaxStart.y - 1);
        }

        private void SetupSkins()
        {
            _lastSkinIndex = 0;
            foreach (var slotContainer in _view.SlotContainers)
            {
                foreach (var slot in slotContainer.RouletteSlot)
                {
                    if (_lastSkinIndex >= _skins.Count)
                        _lastSkinIndex = 0;
                    slot.SetData(_lastSkinIndex, _skins[_lastSkinIndex].Sprite);
                    slot.SetDefault();
                    _lastSkinIndex++;
                }
            }
        }

        private void InitRoulette()
        {
            var slotContainer = _view.SlotContainers[0];
            _slotHeight = Mathf.Abs(slotContainer.RouletteSlot[0].Rect.anchorMax.y - slotContainer.RouletteSlot[0].Rect.anchorMin.y);
                
            slotContainer.Rect.anchorMin = new Vector2(slotContainer.Rect.anchorMin.x, slotContainer.Rect.anchorMin.y + 1);
            slotContainer.Rect.anchorMax = new Vector2(slotContainer.Rect.anchorMax.x, slotContainer.Rect.anchorMax.y + 1);
            _anchorMinStart = slotContainer.Rect.anchorMin;
            _anchorMaxStart = slotContainer.Rect.anchorMax;
            
            slotContainer = _view.SlotContainers[1];
            _anchorMinEnd = new Vector2(slotContainer.Rect.anchorMin.x, slotContainer.Rect.anchorMin.y - 1);
        }
        
        private void StartRoulette()
        {
            _rotationTimePassed = 0;
            _currentRotationSpeed = RotateSpeed;
            _slotReplaced = false;
            _state = State.Accelerate;
        }
        
        private void RotateRoulette()
        {
            float deltaTime = Time.unscaledDeltaTime;
            _rotationTimePassed += deltaTime;

            if (_state is State.Accelerate)
            {
                float time = _rotationTimePassed / AccelerationTime;
                _currentRotationSpeed = Mathf.Lerp(0, RotateSpeed, time);
                if (_rotationTimePassed >= AccelerationTime)
                {
                    _rotationTimePassed = 0;
                    _state = State.Rotate;
                }
            }
            else if (_state is State.Rotate)
            {
                if (_rotationTimePassed >= RotateTime)
                {
                    _rotationTimePassed = 0;
                    _state = State.DeAccelerate;
                    _deAccelerationDistance = 0.5f * RotateSpeed * AccelerationTime;
                }
            }
            else if (_state is State.DeAccelerate)
            {
                float time = _rotationTimePassed / AccelerationTime;
                _currentRotationSpeed = Mathf.Lerp(RotateSpeed, 0, time);
                TryReplaceSlot(time);
                if (_rotationTimePassed >= AccelerationTime)
                    RouletteFinished();
            }

            UpdateSlots(deltaTime);
        }

        private void TryReplaceSlot(float time)
        {
            if (!_slotReplaced)
            {
                float passedDistance = 0.5f * RotateSpeed * time * AccelerationTime;
                float remainingDistance = _deAccelerationDistance - passedDistance;
                float remainingSlots = remainingDistance / _slotHeight;
                if (remainingSlots <= _view.SlotContainers[0].RouletteSlot.Length + 2)
                {
                    var slotContainer = _view.SlotContainers[0];
                    if (slotContainer.RouletteSlot[2].Index != _predictWinnerIndex)
                    {
                        var temp = slotContainer.RouletteSlot[2].Index;
                            
                        foreach (var slots in _view.SlotContainers)
                        {
                            foreach (var slot in slots.RouletteSlot)
                            {
                                if (slot.Index == _predictWinnerIndex)
                                {
                                    slot.SetData(temp, _skins[temp].Sprite);
                                    break;
                                }
                            }
                        }
                            
                        slotContainer.RouletteSlot[2].SetData(_predictWinnerIndex, _skins[_predictWinnerIndex].Sprite);
                        slotContainer.RouletteSlot[2].SetWin();
                    }
                    _slotReplaced = true;
                }
            }
        }

        private void UpdateSlots(float deltaTime)
        {
            float moveDelta = _currentRotationSpeed * deltaTime;

            foreach (var slotContainer in _view.SlotContainers)
            {
                var rect = slotContainer.Rect;
                Vector2 prevAnchorMin = rect.anchorMin;
                Vector2 prevAnchorMax = rect.anchorMax;
                rect.anchorMin = new Vector2(prevAnchorMin.x, prevAnchorMin.y - moveDelta);
                rect.anchorMax = new Vector2(prevAnchorMax.x, prevAnchorMax.y - moveDelta);
                if (rect.anchorMin.y <= _anchorMinEnd.y)
                {
                    float overshoot = _anchorMinEnd.y - rect.anchorMin.y;
                    
                    rect.anchorMin = new Vector2(_anchorMinStart.x, _anchorMinStart.y - overshoot);
                    rect.anchorMax = new Vector2(_anchorMaxStart.x, _anchorMaxStart.y - overshoot);
                    foreach (var slot in slotContainer.RouletteSlot)
                    {
                        if (_lastSkinIndex >= _skins.Count)
                            _lastSkinIndex = 0;

                        slot.SetData(_lastSkinIndex, _skins[_lastSkinIndex].Sprite);
                        _lastSkinIndex++;
                    }
                }
            }
        }
        
        private RouletteSlot GetWinSlot()
        {
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

            return winningSlot;
        }
        
        private void RouletteFinished()
        {
            _state = State.Stop;
            var winningSlot = GetWinSlot();
            winningSlot.SetWin();
        }
        
    }
}