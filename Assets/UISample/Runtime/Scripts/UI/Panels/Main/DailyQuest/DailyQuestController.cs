using System.Collections.Generic;
using Plugins.ServiceLocator;
using Pool;
using UISample.Data;
using UISample.Features;
using UISample.Infrastructure;
using UnityEngine;

namespace UISample.UI
{
    public class DailyQuestController : BaseController
    {
        private readonly DailyQuestView _view;
        private readonly DailyQuestsConfig _config;
        private readonly DailyQuestsManager _questsManager;
        private readonly MonoPool<QuestSlot> _slotsPool;
        private readonly List<QuestSlot> _slots = new();
        private Dictionary<EQuestCategory, List<Quest>> _quests;
        private EQuestCategory _selectedCategory;

        public DailyQuestController(UIContainer uiContainer, MainMenuConfigs configs)
        {
            _view = uiContainer.GetView<DailyQuestView>();
            _config = configs.DailyQuestsConfig;
            _questsManager = ServiceLocator.Get<DailyQuestsManager>();
            _slotsPool = new MonoPool<QuestSlot>(_config.QuestSlotPrefab, 1, new GameObject("Quests Pool").transform, true);
            _view.CloseButton.onClick.AddListener(ClosePressed);
            _view.ShadowCloseButton.onClick.AddListener(ClosePressed);
            foreach (var pair in _view.Categories)
                pair.Value.OnButtonPress.AddListener(() => SelectCategory(pair.Key));
            DeselectCategories();
            _selectedCategory = EQuestCategory.Daily;
            _view.Categories[_selectedCategory].EnableHighlight();
            _questsManager.OnQuestCompleted += QuestCompleted;
        }

        public override void Show(bool instantly = false)
        {
            _view.Show(instantly);
            DrawCategory(_selectedCategory);
        }

        public override void Hide(bool instantly = false)
        {
            _view.Hide(instantly);
        }

        public void InitializeSlots(Dictionary<EQuestCategory, List<Quest>> quests)
        {
            _quests = quests;
            DrawCategory(_selectedCategory);
        }

        public override void Dispose()
        {
            _slotsPool?.Dispose();
            _questsManager.OnQuestCompleted -= QuestCompleted;
        }

        private void ClosePressed()
        {
            _sceneUI.HideController<DailyQuestController>();
        }

        private void SelectCategory(EQuestCategory category)
        {
            DeselectCategories();
            _selectedCategory = category;
            _view.Categories[category].EnableHighlight();
            DrawCategory(_selectedCategory);
        }

        private void DeselectCategories()
        {
            foreach (var pair in _view.Categories)
                pair.Value.DisableHighlight();
        }

        private void DrawCategory(EQuestCategory category)
        {
            foreach (var slot in _slots)
                _slotsPool.Release(slot);
            _slots.Clear();
            var categoryQuests = _quests[category];
            for (var i = 0; i < categoryQuests.Count; i++)
            {
                var quest = categoryQuests[i];
                var slot = _slotsPool.Get();
                slot.Initialize(quest, _config.GetRewardIcon(quest.Config.RewardType), i, SlotRewardPressed);
                slot.transform.SetParent(_view.QuestContainer);
                _slots.Add(slot);
            }
        }

        private void SlotRewardPressed(int index)
        {
            var slot = _slots[index];
            var quest = _quests[_selectedCategory][index];
            if (quest.Model.State is not EQuestState.Completed)
                return;
            Debug.Log(quest.IsCompleted);
            slot.SetState(EQuestState.Rewarded);
            _questsManager.ClaimReward(quest);
        }
        
        private void QuestCompleted(Quest quest)
        {
            if (quest.Config.Category != _selectedCategory)
                return;
            _slots[quest.Index].SetState(EQuestState.Completed);
        }
    }
}