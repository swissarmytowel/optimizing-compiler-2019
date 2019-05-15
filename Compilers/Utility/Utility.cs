using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Utility
{
    public class Utility
    {
        /// <summary>
        /// Checking if an operand (expression) is a variable, not a bool, int or double value
        /// </summary>
        /// <param name="expression">operand to be checked</param>
        /// <returns>If operand is a variable</returns>
        public static bool IsVariable(string expression) => int.TryParse(expression, out _) == false
                                                      && double.TryParse(expression, out _) == false
                                                      && bool.TryParse(expression, out _) == false;


        /// <summary>
        /// Checking if an operand (expression) is a number (int or double value)
        /// </summary>
        /// <param name="expression">operand to be checked</param>
        /// <returns>If operand is a number</returns>
        public static bool IsNum(string expression) => int.TryParse(expression, out _) == true
            || double.TryParse(expression, out _) == true;

    }
}
