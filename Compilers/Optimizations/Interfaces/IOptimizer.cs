using SimpleLang.CFG;
using SimpleLang.IterationAlgorithms;
using SimpleLang.TACode;

namespace SimpleLang.Optimizations.Interfaces
{
    public interface IOptimizer
    {
        /// <summary>
        /// Method to run optimization
        /// </summary>
        /// <param name="tac"> Three-address code container</param>
        /// <returns>Bool value, marking if current optimization was applied successfully</returns>
        bool Optimize(ThreeAddressCode tac);
    }

    public interface IBlockOptimizer
    {
        bool Optimize(BasicBlocks bb);
    }

    public interface IAlgorithmOptimizer<T>
    {
        bool Optimize(BasicBlocks bb, IterationAlgorithm<T> ita);
    }
}
