namespace SimpleLang.ThreeAddressCode.TacNodes
{
    public abstract class TacNode
    {
        public string Label { get; set; }

        protected bool Equals(TacNode other)
        {
            return string.Equals(Label, other.Label);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TacNode) obj);
        }

        public override int GetHashCode()
        {
            return (Label != null ? Label.GetHashCode() : 0);
        }
    }
}