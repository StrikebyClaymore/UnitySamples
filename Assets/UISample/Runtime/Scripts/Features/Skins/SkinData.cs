using CustomInspector;
using UnityEngine;

namespace UISample.Features
{
    [System.Serializable]
    public class SkinData
    {
        [field: SerializeField, ReadOnly] public int Id { get; private set; }

        [field: SerializeField] public Sprite Sprite { get; private set; }

        public SkinData(int id, Sprite sprite)
        {
            Id = id;
            Sprite = sprite;
        }

#if UNITY_EDITOR
        public void SetData(int id)
        {
            Id = id;
        }
#endif
    }
}