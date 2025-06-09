using BrunoMikoski.AnimationSequencer;
using UnityEngine;

namespace UISample.UI
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField] protected AnimationSequencer _showSequencer;
        [SerializeField] protected AnimationSequencer _hideSequencer;
        
        public virtual void Show(bool instantly = false)
        {
            SetGameObjectActive(true);
            if (instantly)
            {
                _showSequencer?.PlayInstantly();
                return;
            }
            _showSequencer?.Play();
        }
        
        public virtual void Hide(bool instantly = false)
        {
            if (instantly)
            {
                SetGameObjectActive(false);
                return;
            }
            if (_hideSequencer != null)
                _hideSequencer.Play(() => SetGameObjectActive(false));
            else
                SetGameObjectActive(false);
        }

        private void SetGameObjectActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}