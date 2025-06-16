using UISample.Data;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class GameplayConfigs : MonoBehaviour
    {
        [field: SerializeField] public MapGeneratorConfig MapGeneratorConfig { get; private set; }
    }
}