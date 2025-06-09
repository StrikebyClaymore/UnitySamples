namespace UnitySamples.Editor
{
    public struct DropdownItem<T>
    {
        public string Text;
        public T Value;
        
        public DropdownItem(string text, T value)
        {
            this.Text = text;
            this.Value = value;
        }
    }
}