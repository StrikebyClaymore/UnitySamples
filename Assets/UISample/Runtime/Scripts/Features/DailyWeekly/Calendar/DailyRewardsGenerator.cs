using System.Collections.Generic;
using UISample.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UISample.Features
{
    public class DailyRewardsGenerator
    {
        private readonly DailyCalendarConfig _config;
        
        public DailyRewardsGenerator(DailyCalendarConfig config)
        {
            _config = config;
        }
        
        public DailyReward[] GenerateRewards()
        {
            var rewards = new DailyReward[_config.SlotsCount];
            var pool = new List<DailyDropItem>[_config.SlotsCount];

            GenerateDropPool();
            NormalizeDropChances();
            GenerateItems();
            
            void GenerateDropPool()
            {
                for (int i = 0; i < _config.SlotsCount; i++)
                {
                    var day = i + 1;
                    pool[i] = new List<DailyDropItem>();
                    foreach (var rewardData in _config.Data)
                    {
                        foreach (var drop in rewardData.Drop)
                        {
                            if (IsDropValid(day, drop))
                                AddDropChance(i, rewardData.Id, drop);
                        }
                    }
                }
            }
            
            void NormalizeDropChances()
            {
                foreach (var items in pool)
                {
                    float totalChance = 0f;
                    foreach (var item in items)
                    {
                        totalChance += item.Chance;
                    }

                    if (totalChance <= 0f)
                    {
                        Debug.LogWarning("Reward chance out of range");
                        continue;
                    }
                    
                    float scaleFactor = 100f / totalChance;
                    for (var j = 0; j < items.Count; j++)
                    {
                        var item = items[j];
                        var newItem = new DailyDropItem()
                        {
                            Id = item.Id,
                            Chance = item.Chance * scaleFactor,
                            Amount = item.Amount,
                        };
                        items[j] = newItem;
                    }
                }
            }

            void GenerateItems()
            {
                for (int i = 0; i < rewards.Length; i++)
                {
                    var item = GetRandomItem(i);
                    if (item == null)
                        continue;
                    var id = item.Value.Id;
                    var amount = Random.Range(item.Value.Amount.x, item.Value.Amount.y);
                    var reward = new DailyReward(id, amount);
                    rewards[i] = reward;
                }
            }

            bool IsDropValid(int day, DailyRewardData.DropInfo drop)
            {
                if (day == _config.SlotsCount)
                    return drop.Day == day;
                if (drop.Daily && (day % 7 == drop.Day % 7))
                    return true;
                return day == drop.Day;
            }
            
            void AddDropChance(int index, int id, DailyRewardData.DropInfo info)
            {
                pool[index].Add(new DailyDropItem
                {
                    Id = id,
                    Chance = info.Chance,
                    Amount = info.Amount
                });
            }
            
            DailyDropItem? GetRandomItem(int i)
            {
                var items = pool[i];
                float randomValue = Random.Range(0f, 100f);
                float cumulative = 0f;

                foreach (var item in items)
                {
                    cumulative += item.Chance;
                    if (randomValue <= cumulative)
                        return item;
                }

                return null;
            }

            return rewards;
        }
    }
}