namespace SimpleLang.ThreeAddressCode.TacNodes
{
    public class TacAssignmentNode : TacNode
    {
        public string RightPart { get; set; }
        public string FirstOperand { get; set; }
        public string SecondOperand { get; set; }
        public string Operation { get; set; }

        private bool Equals(TacAssignmentNode other)
        {
            return string.Equals(Label, other.Label)
                   && string.Equals(RightPart, other.RightPart)
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
                hashCode = (hashCode * 397) ^ (RightPart != null ? RightPart.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FirstOperand != null ? FirstOperand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SecondOperand != null ? SecondOperand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Operation != null ? Operation.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}