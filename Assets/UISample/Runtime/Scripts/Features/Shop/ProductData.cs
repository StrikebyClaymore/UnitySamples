using CustomInspector;
using UnityEngine;

namespace UISample.Features
{
    [System.Serializable]
    public class ProductData
    {
        [field: SerializeField, ReadOnly] public int Index { get; private set; }
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public EProducts Type { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
        [field: SerializeField] public ECurrency Currency { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        
#if UNITY_EDITOR
        public void SetData(int index)
        {
            Index = index;
        }  
#endif
    }
}