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
    }
}