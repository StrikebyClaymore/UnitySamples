using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class LoadingView : BaseView
    {
        [field: SerializeField] public Slider ProgressSlider { get; set; }
    }
}