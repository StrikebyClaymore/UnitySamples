namespace UISample.Features
{
    [System.Serializable]
    public class Skin
    {
        public readonly int Id;
        public bool IsUnlocked;

        public Skin(int id, bool isUnlocked)
        {
            Id = id;
            IsUnlocked = isUnlocked;
        }
    }
}