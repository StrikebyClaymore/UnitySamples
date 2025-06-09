using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class MainMenuView : BaseView
    {
        [field: SerializeField] public Button SettingsButton { get; private set; }
    }
}