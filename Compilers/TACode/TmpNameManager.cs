namespace SimpleLang.TACode
{
    /// <summary>
    /// Singleton for a temporary naming engine manager
    /// Creates unique identifiers for variable names and labels
    /// </summary>
    public class TmpNameManager
    {
        public static readonly TmpNameManager Instance = new TmpNameManager();

        /// <summary>
        /// Corresponding counters of variables and labels used
        /// </summary>
        private int _currentVariableCounter = 0;
        private int _currentLabelCounter = 0;
        
        /// <summary>
        /// Generate unique temporary variable name (identifier) in format of 'tn'
        /// where n is an int number
        /// </summary>
        /// <returns>unique temporary variable name</returns>
        public string GenerateTmpVariableName() => $"t{++_currentVariableCounter}";
        
        /// <summary>
        /// Generate unique label in format of 'Ln'
        /// where n is an int number
        /// </summary>
        /// <returns>Unique label</returns>
        public string GenerateLabel() => $"L{++_currentLabelCounter}";
    }
}
