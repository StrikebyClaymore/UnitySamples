using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISample.UI.UIElements
{
    public class Button : Selectable, ISubmitHandler
    {
        public readonly UnityEvent OnButtonPress = new();
        public readonly UnityEvent OnButtonRelease = new();
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            Press();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            Release();
        }
        
        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();
            if (!IsActive() || !IsInteractable())
                return;
            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }
        
        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            OnButtonPress?.Invoke();
        }

        private void Release()
        {
            if (!IsActive() || !IsInteractable())
                return;
            OnButtonRelease?.Invoke();
        }
        
        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
    }
}