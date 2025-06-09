using UnityEngine;

namespace UISample.Datas
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "UISample/Audio Config")]
    public class AudioConfig : ScriptableObject
    {
        [field: SerializeField] public AudioSource AudioSourcePrefab { get; private set; }
        [field: SerializeField] public float DefaultSoundVolume { get; private set; } = 0.5f;
        [field: SerializeField] public float DefaultMusicVolume { get; private set; } = 0.5f;
        [field: SerializeField] public float DefaultUIVolume { get; private set; } = 0.5f;
        [field: SerializeField] public AudioClip MainMusicClip { get; private set; }
        [field: SerializeField] public AudioClip TestClip { get; private set; }
    }
}