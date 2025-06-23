using UISample.Features;
using UnityEngine.Events;

namespace UISample.Infrastructure
{
    public static class EventBus
    {
        public static readonly UnityEvent<EQuestType, EQuestTarget, int> OnAddQuestValue = new();
    }
}