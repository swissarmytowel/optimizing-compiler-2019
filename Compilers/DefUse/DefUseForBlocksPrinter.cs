using System;
using System.Collections.Generic;
using System.Text;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.DefUse
{
    public static class DefUseForBlocksPrinter
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

        static public StringBuilder ExecutePrint(Dictionary<ThreeAddressCode, IExpressionSetsContainer> defUseForBlocks)
        {
            StringBuilder text = new StringBuilder();
            text.Append("DefUseForBlocksPrinter:\n\n");
            foreach (KeyValuePair<ThreeAddressCode, IExpressionSetsContainer> entry in defUseForBlocks) {
                text.Append("block: " + TACodeLinesToString(entry.Key.TACodeLines) + "\n");
                text.Append("     def: " + ContainerSetToString(entry.Value.GetSecondSet()) + "\n");
                text.Append("     use: " + ContainerSetToString(entry.Value.GetFirstSet()) + "\n");
            }
            return text;
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
