namespace SimpleLang.ThreeAddressCode.TacNodes
{
    public class TacGotoNode : TacNode
    {
        public string TargetLabel { get; set; }

        protected bool Equals(TacGotoNode other)
        {
            return base.Equals(other) && string.Equals(TargetLabel, other.TargetLabel);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TacGotoNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (TargetLabel != null ? TargetLabel.GetHashCode() : 0);
            }
        }
    }
}