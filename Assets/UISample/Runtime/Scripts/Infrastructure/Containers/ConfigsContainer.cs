using UISample.Datas;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class ConfigsContainer : MonoBehaviour
    {
        [field: SerializeField] public AudioConfig AudioConfig { get; private set; }
    }
}