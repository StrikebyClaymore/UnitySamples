using Newtonsoft.Json;
using UnityEngine;

namespace UISample.Features
{
    [System.Serializable]
    public class Skin
    {
        public readonly int Id;
        public bool IsUnlocked;
        [JsonIgnore]
        public Sprite Sprite;

        public Skin(int id, bool isUnlocked)
        {
            Id = id;
            IsUnlocked = isUnlocked;
        }
    }
}