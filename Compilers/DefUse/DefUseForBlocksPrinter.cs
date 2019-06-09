using System;
using System.Collections.Generic;
using System.Text;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.DefUse
{
    static class DefUseForBlocksPrinter
    {
        static public void Execute(Dictionary<ThreeAddressCode, IExpressionSetsContainer> defUseForBlocks)
        {
            Console.WriteLine("DefUseForBlocksPrinter:");
            foreach (KeyValuePair<ThreeAddressCode, IExpressionSetsContainer> entry in defUseForBlocks)
            {
                Console.WriteLine("block: " + TACodeLinesToString(entry.Key.TACodeLines));
                Console.WriteLine("     def: " + ContainerSetToString(entry.Value.GetSecondSet()));
                Console.WriteLine("     use: " + ContainerSetToString(entry.Value.GetFirstSet()));
            }
        }

        static private string TACodeLinesToString(LinkedList<TacNode> codeLines)
        {
            var s = new StringBuilder();
            foreach (var line in codeLines)
                s.Append(line.ToString().Trim() + "; ");
            return s.ToString();
        }

        static private string ContainerSetToString(HashSet<TacNode> tacNodesHash)
        {
            var s = new StringBuilder();
            s.Append("{ ");
            foreach (var tacNode in tacNodesHash)
            {
                TacNodeVarDecorator varNode = tacNode as TacNodeVarDecorator;
                s.Append(varNode.ToString().Trim() + " ");
            }
            s.Append("}");
            return s.ToString();
        }

    }
}
