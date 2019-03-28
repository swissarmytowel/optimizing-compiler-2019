namespace SimpleLang.TACode.TacNodes
{
    /// <inheritdoc />
    /// <summary>
    /// TAC-node for a conditional jump (if .. goto) statement
    /// Used to rewrite if-else statement from the language code to TAC
    /// </summary>
    public class TacIfGotoNode : TacGotoNode
    {
        /// <summary>
        /// Condition, under which a jump to TargetLabel(from the base class field) occurs
        /// </summary>
        public string Condition { get; set; }

        #region IDE-generated equality members
        
        private bool Equals(TacIfGotoNode other)
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
        
        #endregion
        
        public override string ToString() => $"{Label}: if {Condition} goto {TargetLabel}";
    }
}