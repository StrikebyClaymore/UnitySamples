using System.Linq;
using UISample.Features;
using UISample.Utility;
using UnityEngine;

namespace UISample.Data
{
    [CreateAssetMenu(fileName = "ShopConfig", menuName = "UISample/ShopConfig")]
    public class ShopConfig : ScriptableObject
    {
        [field: SerializeField] public Pair<EProducts, Sprite>[] ProductIcons { get; private set; }
        [field: SerializeField] public Pair<ECurrency, Sprite>[] CurrencyIcons { get; private set; }
        [field: SerializeField] public ProductData[] Products { get; private set; }

        public ProductData GetProduct(int index)
        {
            return Products[index];
        }
        
        public ProductData GetProduct(string id)
        {
            return Products.FirstOrDefault(x => x.Id == id);
        }
        
        public Sprite GetIcon(EProducts productType)
        {
            return ProductIcons.FirstOrDefault(x => x.Key == productType).Value;
        }
        
        public Sprite GetIcon(ECurrency currencyType)
        {
            return CurrencyIcons.FirstOrDefault(x => x.Key == currencyType).Value;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            for (int i = 0; i < Products.Length; i++)
            {
                Products[i].SetData(i);
            }
        }
#endif
    }
}