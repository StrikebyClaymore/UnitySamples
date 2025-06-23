using UnityEngine;

namespace UISample.Features
{
    [System.Serializable]
    public class DailyRewardData
    {
        [System.Serializable]
        public struct DropInfo
        {
            [field: SerializeField] public int Day { get; private set; }
            [field: SerializeField] public int Chance { get; private set; }
            [field: SerializeField] public Vector2Int Amount { get; private set; }
            [field: SerializeField] public bool Daily { get; private set; }
        }
        
        [field: SerializeField] public ERewards Type { get; private set; }
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public DropInfo[] Drop { get; private set; }
    }
}