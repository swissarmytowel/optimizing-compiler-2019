using SimpleLang.TACode.TacNodes;

namespace SimpleLang.DefUse
{
    public class TacNodeVarDecorator: TacNode
    {
        public string VarName = null;

        override public string ToString()
        {
            return VarName;
        }

        protected bool Equals(TacNodeVarDecorator other)
        {
            return string.Equals(VarName, other.VarName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TacNodeVarDecorator)obj);
        }

        public override int GetHashCode()
        {
            return (VarName != null ? VarName.GetHashCode() : 0);
        }
    }
}
