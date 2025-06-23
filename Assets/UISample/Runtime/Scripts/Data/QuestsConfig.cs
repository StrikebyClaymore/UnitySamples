using System.Collections.Generic;
using UISample.Features;
using UISample.UI;
using UISample.Utility;
using UnityEngine;

namespace UISample.Data
{
    [CreateAssetMenu(fileName = "DailyQuestsConfig", menuName = "UISample/DailyQuestsConfig")]
    public class DailyQuestsConfig : ScriptableObject
    {
        [field: SerializeField] public List<QuestReward> Rewards { get; private set; }
        [field: SerializeField] public PairCollection<EQuestCategory, List<QuestData>> Quests { get; private set; }
        [field: SerializeField] public QuestSlot QuestSlotPrefab { get; private set; }

        public QuestData GetQuestData(int id)
        {
            foreach (var pair in Quests)
            {
                foreach (var quest in pair.Value)
                {
                    if (quest.Id == id)
                        return quest;
                }
            }
            
            return null;
        }
        
        public Sprite GetRewardIcon(ERewards reward)
        {
            foreach (var pair in Rewards)
            {
                if (reward == pair.Type)
                    return pair.Icon;
            }
            
            return null;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            var index = 0;
            foreach (var pair in Quests)
            {
                var category = pair.Key;
                foreach (var quest in pair.Value)
                {
                    quest.SetData(index, category);
                    index++;
                }
            }
        }
#endif
    }
}