namespace SimpleLang.ThreeAddressCode.TacNodes
{
    public class TacIfGotoNode : TacGotoNode
    {
        public string Condition { get; set; }

        protected bool Equals(TacIfGotoNode other)
        {
            return base.Equals(other) && string.Equals(Condition, other.Condition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TacIfGotoNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Condition != null ? Condition.GetHashCode() : 0);
            }
        }
        
        public override string ToString() => $"{Label}: if {Condition} goto {TargetLabel}";
    }
}