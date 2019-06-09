using SimpleLang.Optimizations;
using SimpleLang.TACode;

namespace TextTests.Handlers
{
    public class UnreachableCodeHandler : TextTestsHandler
    {
        public UnreachableCodeHandler(string directoryName) : base(directoryName) { }
        protected override ThreeAddressCode ProcessTAC(ThreeAddressCode tacContainer)
        {
            var optimization = new UnreachableCodeOpt();
            optimization.Optimize(tacContainer);
            return tacContainer;
        }
    }
}
