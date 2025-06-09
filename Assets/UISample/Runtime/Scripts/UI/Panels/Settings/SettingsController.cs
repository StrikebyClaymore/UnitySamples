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
            _view.CloseButton.onClick.AddListener(ClosePressed);
            _audioSettings = ServiceLocator.Get<AudioSettings>();
            
            _audioSettings.OnSoundVolumeChanged.AddListener(SoundVolumeChanged);
            _audioSettings.OnMusicVolumeChanged.AddListener(MusicVolumeChanged);
            _audioSettings.OnUIVolumeChanged.AddListener(UIVolumeChanged);
            
            _view.SoundSlider.onValueChanged.AddListener(SoundSliderChanged);
            _view.MusicSlider.onValueChanged.AddListener(MusicSliderChanged);
            _view.UISlider.onValueChanged.AddListener(UISliderChanged);
        }

        public override void Show(bool instantly = false)
        {
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            _view.Hide(instantly);
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
            _view.AudioSource.volume = value;
            _view.UISlider.SetValueWithoutNotify(value);
            _view.UIIcon.sprite = value > 0 ? _view.SoundOn : _view.SoundOff;
        }
    }
}