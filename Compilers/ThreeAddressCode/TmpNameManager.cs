namespace SimpleLang.ThreeAddressCode
{
    public class TmpNameManager
    {
        public static TmpNameManager Instance = new TmpNameManager();

        private int _currentVariableCounter = 0;
        private int _currentLabelCounter = 0;

        public string GenerateTmpVariableName() => $"t{++_currentVariableCounter}";
        public string GenerateLabel() => $"L{++_currentLabelCounter}";
    }
}
