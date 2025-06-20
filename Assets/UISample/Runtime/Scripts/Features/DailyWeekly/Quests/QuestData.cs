using UnityEngine;

namespace UISample.Features
{
    [System.Serializable]
    public class QuestData
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public EQuestType Type { get; private set; } 
        [field: SerializeField] public EQuestTarget Target { get; private set; }
        [field: SerializeField] public int TargetAmount { get; set; }
        [field: SerializeField] public ERewards RewardType { get; private set; }
        [field: SerializeField] public int RewardAmount { get; private set; }
        [field: SerializeField] public Sprite RewardIcon { get; private set; }
        
#if UNITY_EDITOR
        public void SetData(int id, Sprite icon)
        {
            Id = id;
            RewardIcon = icon;
        }  
#endif
    }
}