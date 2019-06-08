using SimpleLang.InOut;

namespace SimpleLang.IterationAlgorithms.Interfaces
{
    interface IIterationAlgorithm<T>
    {
        InOutContainer<T> InOut { get; set; }
    }
}
