using UnityEngine;

namespace UISample.Features
{
    [System.Serializable]
    public struct QuestReward
    {
        [field: SerializeField] public ERewards Type { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}