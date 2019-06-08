using SimpleLang.Optimizations;
using SimpleLang.TACode;

namespace TextTests.Handlers
{
    public class CommonSubexprTestsHandler : TextTestsHandler
    {
        public CommonSubexprTestsHandler(string directoryName) : base(directoryName) { }

        protected override ThreeAddressCode ProcessTAC(ThreeAddressCode tacContainer)
        {
            var optimization = new CommonSubexprOptimization();
            optimization.Optimize(tacContainer);
            return tacContainer;
        }
    }
}
