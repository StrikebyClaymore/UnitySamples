using Newtonsoft.Json;

namespace UISample.Features
{
    [System.Serializable]
    public class DailyReward
    {
        [JsonProperty("Id")]
        public int Id { get; private set; }
        [JsonProperty("Amount")]
        public int Amount { get; private set; }
        [JsonProperty("State")]
        public EDailyRewardState State { get; set; }

        public DailyReward(int id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}