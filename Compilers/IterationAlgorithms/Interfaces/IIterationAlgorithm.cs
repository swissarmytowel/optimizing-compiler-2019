using SimpleLang.InOut;

namespace SimpleLang.IterationAlgorithms.Interfaces
{
    public interface IIterationAlgorithm<T>
    {
        InOutContainer<T> InOut { get; set; }
    }
}
