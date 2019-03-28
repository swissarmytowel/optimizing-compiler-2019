using ProgramTree;

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
        
        #region IDE-generated equality members

        private bool Equals(TacAssignmentNode other)
        {
            return string.Equals(Label, other.Label)
                   && string.Equals(LeftPartIdentifier, other.LeftPartIdentifier)
                   && string.Equals(FirstOperand, other.FirstOperand)
                   && string.Equals(SecondOperand, other.SecondOperand)
                   && string.Equals(Operation, other.Operation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TacNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Label != null ? Label.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LeftPartIdentifier != null ? LeftPartIdentifier.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FirstOperand != null ? FirstOperand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SecondOperand != null ? SecondOperand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Operation != null ? Operation.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion
        
        public override string ToString()
        {
            var rightPart = (Operation == null) && (SecondOperand == null)
                ? $"{FirstOperand}"
                : $"{FirstOperand} {Operation} {SecondOperand}";
            return $"{Label}: {LeftPartIdentifier} {AssignType.Assign} {FirstOperand} {Operation} {SecondOperand}";
        }
    }
}