namespace SimpleLang.TACode.TacNodes
{
    /// <summary>
    /// Abstract three-address code (TAC) node class
    /// Object-oriented TAC code line representation 
    /// </summary>
    public abstract class TacNode
    {
        /// <summary>
        /// Current TAC code line label
        /// </summary>
        public string Label { get; set; }

        #region IDE-generated equality members
        
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

        #endregion
        
        public override string ToString() => $"{Label}:";
    }
}