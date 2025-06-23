using System;

namespace UISample.Features
{
    [Serializable]
    public class QuestModel
    {
        public int Id { get; private set; }

        public int Progress { get; set; }

        public EQuestState State { get; set; }

        public QuestModel(int id, EQuestState state, int progress)
        {
            Id = id;
            State = state;
            Progress = progress;
        }
    }
}