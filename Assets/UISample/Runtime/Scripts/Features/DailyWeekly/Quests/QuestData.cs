using CustomInspector;
using UnityEngine;

namespace UISample.Features
{
    [System.Serializable]
    public class QuestData
    {
        [field: SerializeField, ReadOnly] public int Id { get; private set; }
        [field: SerializeField, ReadOnly] public EQuestCategory Category { get; private set; } 
        [field: SerializeField] public EQuestType Type { get; private set; } 
        [field: SerializeField] public EQuestTarget Target { get; private set; }
        [field: SerializeField] public int TargetAmount { get; set; }
        [field: SerializeField] public ERewards RewardType { get; private set; }
        [field: SerializeField] public int RewardAmount { get; private set; }
        
#if UNITY_EDITOR
        public void SetData(int id, EQuestCategory category)
        {
            Id = id;
            Category = category;
        }  
#endif
    }
}