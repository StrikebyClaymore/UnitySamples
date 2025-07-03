using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UISample.Features;
using UISample.UI;
using UnityEngine;

namespace UISample.Data
{
    [CreateAssetMenu(fileName = "SkinsConfig", menuName = "UISample/SkinsConfig")]
    public class SkinsConfig : ScriptableObject
    {
        [field: SerializeField] public SkinSlot SkinSlotPrefab { get; private set; }
        [field: SerializeField] public List<SkinData> Skins { get; private set; }
        
#if UNITY_EDITOR
        [SerializeField] private bool _edit;
        [ShowIf("_edit")]
        [SerializeField] private Sprite[] _sprites;
        
        private void OnValidate()
        {
            for (var i = 0; i < Skins.Count; i++)
            {
                Skins[i].SetData(i);
            }
        }

        [ShowIf("_edit")]
        [Button]
        private void GenerateSkins()
        {
            Skins.Clear();
            for (int i = 0; i < _sprites.Length; i++)
            {
                Skins.Add(new SkinData(i, _sprites[i]));
            }
        }
#endif
    }
}