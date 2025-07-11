﻿using UnityEngine;
using UnityEngine.UI;

namespace UISample.UI
{
    public class PauseView : BaseView
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button ShadowCloseButton { get; private set; }
        
        [field: SerializeField] public Slider SoundSlider { get; private set; }
        [field: SerializeField] public Slider MusicSlider { get; private set; }
        [field: SerializeField] public Slider UISlider { get; private set; }
        
        [field: SerializeField] public Button HomeButton { get; private set; }
        
        [field: SerializeField] public Image SoundIcon { get; private set; }
        [field: SerializeField] public Image MusicIcon { get; private set; }
        [field: SerializeField] public Image UIIcon { get; private set; }
        
        [field: SerializeField] public Sprite SoundOn { get; private set; }
        [field: SerializeField] public Sprite SoundOff { get; private set; }
        [field: SerializeField] public Sprite MusicOn { get; private set; }
        [field: SerializeField] public Sprite MusicOff { get; private set; }
        [field: SerializeField] public Sprite UIOn { get; private set; }
        [field: SerializeField] public Sprite UIOff { get; private set; }
    }
}