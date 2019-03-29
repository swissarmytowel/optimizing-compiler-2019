namespace SimpleLang.TACode.TacNodes
{
    /// <inheritdoc />
    /// <summary>
    /// TAC-node for an unconditional jump (goto) statement
    /// </summary>
    public class TacGotoNode : TacNode
    {
        /// <summary>
        /// Current jumps' target code line, denoted by label
        /// </summary>
        public string TargetLabel { get; set; }

        #region IDE-generated equality members

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
        
        #endregion
        
        public override string ToString() => $"{Label}: goto {TargetLabel}";
    }
}
