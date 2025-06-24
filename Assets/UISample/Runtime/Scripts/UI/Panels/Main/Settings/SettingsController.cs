using Plugins.ServiceLocator;
using UISample.Infrastructure;

namespace UISample.UI
{
    public class SettingsController : BaseController
    {
        private readonly SettingsView _view;
        private readonly AudioSettings _audioSettings;
        
        public SettingsController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<SettingsView>();
            _audioSettings = ServiceLocator.Get<AudioSettings>();
            
            _view.CloseButton.onClick.AddListener(ClosePressed);
            _view.ShadowCloseButton.onClick.AddListener(ClosePressed);
            
            _audioSettings.OnSoundVolumeChanged.AddListener(SoundVolumeChanged);
            _audioSettings.OnMusicVolumeChanged.AddListener(MusicVolumeChanged);
            _audioSettings.OnUIVolumeChanged.AddListener(UIVolumeChanged);
            
            _view.SoundSlider.onValueChanged.AddListener(SoundSliderChanged);
            _view.MusicSlider.onValueChanged.AddListener(MusicSliderChanged);
            _view.UISlider.onValueChanged.AddListener(UISliderChanged);

            SoundVolumeChanged(_audioSettings.SoundVolume);
            MusicVolumeChanged(_audioSettings.MusicVolume);
            UIVolumeChanged(_audioSettings.UIVolume);
        }

        public override void Show(bool instantly = false)
        {
            base.Show(instantly);
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            base.Hide(instantly);
            _view.Hide(instantly);
        }

        public override void Dispose()
        {
            base.Dispose();
            _audioSettings.OnSoundVolumeChanged.RemoveListener(SoundVolumeChanged);
            _audioSettings.OnMusicVolumeChanged.RemoveListener(MusicVolumeChanged);
            _audioSettings.OnUIVolumeChanged.RemoveListener(UIVolumeChanged);
        }

        private void ClosePressed()
        {
            _sceneUI.HideController<SettingsController>();
        }
        
        private void SoundSliderChanged(float value)
        {
            _audioSettings.SetSoundVolume(value);
        }
        
        private void MusicSliderChanged(float value)
        {
            _audioSettings.SetMusicVolume(value);
        }
        
        private void UISliderChanged(float value)
        {
            _audioSettings.SetUIVolume(value);
        }
        
        private void SoundVolumeChanged(float value)
        {
            _view.SoundSlider.SetValueWithoutNotify(value);
            _view.SoundIcon.sprite = value > 0 ? _view.SoundOn : _view.SoundOff;
        } 
        
        private void MusicVolumeChanged(float value)
        {
            _view.MusicSlider.SetValueWithoutNotify(value);
            _view.MusicIcon.sprite = value > 0 ? _view.MusicOn : _view.MusicOff;
        }
        
        private void UIVolumeChanged(float value)
        {
            _view.UISlider.SetValueWithoutNotify(value);
            _view.UIIcon.sprite = value > 0 ? _view.UIOn : _view.UIOff;
        }
    }
}