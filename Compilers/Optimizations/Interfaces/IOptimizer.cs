using SimpleLang.TACode;

namespace SimpleLang.Optimizations.Interfaces
{
    public interface IOptimizer
    {
        void Optimize(ThreeAddressCode tac);
    }
}
