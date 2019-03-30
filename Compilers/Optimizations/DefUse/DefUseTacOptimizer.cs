using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.Optimizations.DefUse
{
    public class DefUseTacOptimizer
    {
        private readonly DefUseDetector _defUseDetector = new DefUseDetector();
            
        public void DeadVariablesOptimization(ThreeAddressCode threeAddressCode)
        {
            _defUseDetector.DetectAndFillDefUse(threeAddressCode);
            foreach (var definition in _defUseDetector.Definitions)
            {
                if (definition.Value == null || definition.Value.Count == 0)
                {
                    threeAddressCode.RemoveNodeByLabel(definition.Key.Item2);
                }
            }
        }
    }
}