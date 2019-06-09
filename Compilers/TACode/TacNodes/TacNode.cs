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
        public string Label { get; set; } = null;
        /// <summary>
        /// Indicates if a label was generated as a part of utility generation
        /// (if/while/for, anything that RESULTS in if-goto or goto TAC commands but are not that in actual code)
        /// </summary>
        public bool IsUtility { get; set; } = false;
        
        public override string ToString() => Label != null ? Label + ": " : "";
    }
}