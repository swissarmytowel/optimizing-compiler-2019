namespace SimpleLang.TACode.TacNodes
{
    /// <inheritdoc />
    /// <summary>
    /// TAC-node for an assignment statement
    /// </summary>
    public class TacAssignmentNode : TacNode
    {
        /// <summary>
        /// An identifier representation from the left part of the assignment operation
        /// </summary>
        public string LeftPartIdentifier { get; set; }
        /// <summary>
        /// First (left) operand representation from the right part of the assignment operation
        /// </summary>
        public string FirstOperand { get; set; }
        /// <summary>
        /// Second (right) operand representation from the right part of the assignment operation
        /// </summary>
        public string SecondOperand { get; set; } = null;
        /// <summary>
        /// Operation in between the operands
        /// </summary>
        public string Operation { get; set; } = null;
        
        public override string ToString()
        {
            var rightPart = (Operation == null) && (SecondOperand == null)
                ? $"{FirstOperand}"
                : $"{FirstOperand} {Operation} {SecondOperand}";
            return $"{base.ToString()}{LeftPartIdentifier} = {FirstOperand} {Operation} {SecondOperand}";
        }
    }
}
