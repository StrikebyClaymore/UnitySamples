using System.Collections.Generic;
using System.Linq;
using UISample.Features;
using UnityEngine;

namespace UISample.Data
{
    [CreateAssetMenu(fileName = "DailyQuestsConfig", menuName = "UISample/DailyQuestsConfig")]
    public class DailyQuestsConfig : ScriptableObject
    {
        [field: SerializeField] public List<QuestReward> Rewards { get; private set; }
        [field: SerializeField] public List<QuestData> Quests { get; private set; }

        public QuestData GetQuestData(int id)
        {
            foreach (var quest in Quests)
            {
                if (quest.Id == id)
                    return quest;
            }

            return null;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < Quests.Count; i++)
            {
                var quest = Quests[i];
                quest.SetData(i, Rewards.FirstOrDefault(x => x.Type == quest.RewardType).Icon);
            }
        }
#endif
    }
}