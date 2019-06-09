using System;
using SimpleLang.TACode;
using SimpleLang.Optimizations;

namespace TextTests.Handlers
{
    public class EliminateTranToTranTestsHandler : TextTestsHandler
    {
        public EliminateTranToTranTestsHandler(string directoryName) : base(directoryName) { }

        protected override ThreeAddressCode ProcessTAC(ThreeAddressCode tacContainer)
        {
            var elimintaion = new EliminateTranToTranOpt();
            elimintaion.Optimize(tacContainer);
            return tacContainer;
        }
    }
}
