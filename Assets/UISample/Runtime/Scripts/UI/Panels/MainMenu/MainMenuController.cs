using Plugins.ServiceLocator;
using UISample.Data;
using UISample.Infrastructure;
using UnityEngine.SceneManagement;

namespace UISample.UI
{
    public class MainMenuController : BaseController
    {
        private readonly MainMenuView _view;
        
        public MainMenuController(UIContainer uiContainer)
        {
            _view = uiContainer.GetView<MainMenuView>();
            _view.PlayButton.onClick.AddListener(PlayPressed);
            _view.SettingsButton.onClick.AddListener(SettingsPressed);
            var playerData = ServiceLocator.Get<PlayerData>();
            playerData.Acorns.OnValueChanged += UpdateAcorns;
            playerData.Gems.OnValueChanged += UpdateGems;
            UpdateAcorns(playerData.Acorns.Value);
            UpdateGems(playerData.Gems.Value);
        }

        public override void Show(bool instantly = false)
        {
            _view.Show(instantly);
        }

        public override void Hide(bool instantly = false)
        {
            _view.Hide(instantly);
        }

        private void PlayPressed()
        {
            var sceneLoader = ServiceLocator.Get<SceneLoader>();
            sceneLoader.LoadSceneAsync(GameConstants.GameplaySceneIndex, LoadSceneMode.Single);
        }
        
        private void SettingsPressed()
        {
            _sceneUI.ShowController<SettingsController>();
        }
        
        private void UpdateAcorns(int value)
        {
            _view.AcornsText.text = value.ToString();
        }
        
        private void UpdateGems(int value)
        {
            _view.GemsText.text = value.ToString();
        }
    }
}