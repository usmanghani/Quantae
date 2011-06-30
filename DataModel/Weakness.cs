namespace Quantae.DataModel
{
    public class Weakness
    {
        public WeaknessType WeaknessType { get; set; }
        public string UmbrellaTopicName { get; set; }

        public override bool Equals(object obj)
        {
            Weakness w = obj as Weakness;
            return w.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            if (!string.IsNullOrEmpty(this.UmbrellaTopicName))
            {
                return this.UmbrellaTopicName.GetHashCode();
            }
            
            return (int)this.WeaknessType;
        }
    }
}