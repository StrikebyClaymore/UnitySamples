using System.Collections.Generic;
using UISample.Data;

namespace UISample.Features
{
    public class QuestsGenerator
    {
        private readonly DailyQuestsConfig _config;
        
        public QuestsGenerator(DailyQuestsConfig config)
        {
            _config = config;
        }
        
        public Dictionary<EQuestCategory, List<Quest>> CreateNewQuests()
        {
            var quests = new Dictionary<EQuestCategory, List<Quest>>();
            foreach (var categoryPair in _config.Quests)
            {
                var category = categoryPair.Key;
                var categoryQuests = new List<Quest>();
                for (var i = 0; i < categoryPair.Value.Count; i++)
                {
                    var questData = categoryPair.Value[i];
                    var questModel = new QuestModel(questData.Id, EQuestState.Process, 0);
                    categoryQuests.Add(new Quest(questData, questModel, i));
                }

                quests[category] = categoryQuests;
            }
            return quests;
        }

        public List<Quest> CreateQuestsForCategory(EQuestCategory category)
        {
            var quests = new List<Quest>();
            if (!_config.Quests.TryGetValue(category, out var questDataList))
                return quests;
            for (int i = 0; i < questDataList.Count; i++)
            {
                var questData = questDataList[i];
                var questModel = new QuestModel(questData.Id, EQuestState.Process, 0);
                quests.Add(new Quest(questData, questModel, i));
            }
            return quests;
        }
    }
}