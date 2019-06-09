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

        public override string ToString() => $"{base.ToString()}goto {TargetLabel}";
    }
}
