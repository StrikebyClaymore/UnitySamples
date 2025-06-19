#if DOTWEEN_ENABLED
using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace BrunoMikoski.AnimationSequencer
{
    [Serializable]
    public class AnchoredMinMaxMoveDOTweenAction : DOTweenActionBase
    {
        public override Type TargetComponentType => typeof(RectTransform);
        public override string DisplayName => "Move To Anchored MinMax";
        public Vector2 Min
        {
            get => min;
            set => min = value;
        }
        public Vector2 Max
        {
            get => max;
            set => max = value;
        }
        [SerializeField] private Vector2 min;
        [SerializeField] private Vector2 max;
        private RectTransform rectTransform;
        private Vector2 previousMin;
        private Vector2 previousMax;
        
        protected override Tweener GenerateTween_Internal(GameObject target, float duration)
        {
            rectTransform = target.transform as RectTransform;
            if (rectTransform == null)
            {
                Debug.LogError($"{target.name} does not have a RectTransform component");
                return null;
            }

            // Чтение текущих значений прямо при создании твина
            TweenerCore<Vector4, Vector4, VectorOptions> tween = DOTween
                .To(() =>
                    {
                        Vector2 aMin = rectTransform.anchorMin;
                        Vector2 aMax = rectTransform.anchorMax;
                        return new Vector4(aMin.x, aMin.y, aMax.x, aMax.y);
                    },
                    x =>
                    {
                        rectTransform.anchorMin = new Vector2(x.x, x.y);
                        rectTransform.anchorMax = new Vector2(x.z, x.w);
                    },
                    new Vector4(min.x, min.y, max.x, max.y),
                    duration);

            return tween;
        }

        public override void ResetToInitialState()
        {
            if (rectTransform == null)
                return;

            rectTransform.anchorMin = previousMin;
            rectTransform.anchorMax = previousMax;
        }
    }
}
#endif