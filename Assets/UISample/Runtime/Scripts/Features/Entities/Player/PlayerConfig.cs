using UnityEngine;

namespace UISample.Features
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "UISample/Entities/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public AudioClip[] WalkClips { get; private set; }
        [field: SerializeField] public AudioClip JumpClip { get; private set; }

        public AudioClip GetRandomWalkClip()
        {
            return WalkClips[Random.Range(0, WalkClips.Length)];
        }
    }
}