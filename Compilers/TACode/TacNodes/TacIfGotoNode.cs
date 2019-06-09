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

        public override string ToString() => (Label != null ? Label + ": " : "") + $"if {Condition} goto {TargetLabel}";
    }
}