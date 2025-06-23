using System;
using UISample.Infrastructure;

namespace UISample.Features
{
    public class Quest : BaseObjective
    {
        public QuestData Config { get; }
        public QuestModel Model { get; }
        public int Index { get; }
        public event Action<Quest> OnProgressChanged;
        public event Action<Quest> OnQuestCompleted;
        
        public Quest(QuestData config, QuestModel model, int index)
        {
            Config = config;
            Model = model;
            Index = index;
            Subscribe();
        }
        
        public override void Dispose()
        {
            OnProgressChanged = null;
            OnQuestCompleted = null;
            base.Dispose();
        }
        
        protected sealed override void Subscribe()
        {
            EventBus.OnAddQuestValue.AddListener(Update);
        }

        protected override void Unsubscribe()
        {
            EventBus.OnAddQuestValue.RemoveListener(Update);
        }

        protected override void TryComplete()
        {
            if (Model.Progress >= Config.TargetAmount)
                Complete();
        }

        protected override void Complete()
        {
            OnQuestCompleted?.Invoke(this);
            Model.State = EQuestState.Completed;
            base.Complete();
        }
        
        private void Update(EQuestType type, EQuestTarget target, int value)
        {
            if(Config.Type != type || Config.Target != target)
                return;
            Model.Progress = Math.Min(Model.Progress + value, Config.TargetAmount);
            OnProgressChanged?.Invoke(this);
            TryComplete();
        }
    }
}