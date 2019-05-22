using System;
using System.Collections.Generic;

using SimpleLang.Optimizations;
using SimpleLang.GenKill.Interfaces;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;

namespace SimpleLang.GenKill.Implementations
{
    public class GenKillVisitor : IGenKillVisitor
    {
        private Dictionary<TacNode, IGenKillContainer> lineContainer;

        public Dictionary<ThreeAddressCode, IGenKillContainer> GenerateReachingDefinitionForBlocks(BasicBlocks bblocks)
        {
            var lineGenKill = GenerateReachingDefinitionForLine(bblocks);
            var blocksKill = new Dictionary<ThreeAddressCode, HashSet<TacNode>>();
            var blocksGen = new Dictionary<ThreeAddressCode, HashSet<TacNode>>();

            foreach (var bblock in bblocks)
            {
                foreach (var line in bblock)
                {
                    if (line is TacAssignmentNode assignmentNode)
                    {
                        if (!blocksKill.ContainsKey(bblock))
                        {
                            blocksKill.Add(bblock, new HashSet<TacNode>());
                        }

                        if (!blocksGen.ContainsKey(bblock))
                        {
                            blocksGen.Add(bblock, new HashSet<TacNode>());
                        }

                        blocksKill[bblock].UnionWith(lineGenKill[line].GetKill());
                        blocksGen[bblock].UnionWith(lineGenKill[line].GetGen());
                    }
                }
            }

            var resultBlocksGenKill = new Dictionary<ThreeAddressCode, IGenKillContainer>();

            foreach(var resultGKKey in blocksGen.Keys)
            {
                var genKillContainer = GetGenKillContainer();

                foreach (var resVal in blocksKill[resultGKKey])
                {
                    genKillContainer.AddKill(resVal);
                }

                foreach (var resVal in blocksGen[resultGKKey])
                {
                    genKillContainer.AddGen(resVal);
                }

                resultBlocksGenKill.Add(resultGKKey, genKillContainer);
            }

            return resultBlocksGenKill;
        }

        public Dictionary<TacNode, IGenKillContainer> GenerateReachingDefinitionForLine(BasicBlocks bblocks)
        {
            lineContainer = new Dictionary<TacNode, IGenKillContainer>();
            var variablesContainer = new Dictionary<string, HashSet<TacNode>>();

            // Прохождение по каждой строчке кода и нахождение gen и ипсользования переменных
            foreach (var bblock in bblocks) 
            {
                foreach(var line in bblock)
                {
                    if (line is TacAssignmentNode assignmentNode)
                    {
                        if (!lineContainer.ContainsKey(line))
                        {
                            lineContainer.Add(line, GetGenKillContainer());
                        }

                        lineContainer[line].AddGen(line);

                        if (!variablesContainer.ContainsKey(assignmentNode.LeftPartIdentifier))
                        {
                            variablesContainer.Add(assignmentNode.LeftPartIdentifier, new HashSet<TacNode>());
                        }

                        variablesContainer[assignmentNode.LeftPartIdentifier].Add(line);
                    }
                }
            }

            // Прохождение по каждой строчке кода и нахождение kill посредством variablesContainer
            foreach (var bblock in bblocks)
            {
                foreach (var line in bblock)
                {
                    if (line is TacAssignmentNode assignmentNode)
                    {
                        var variableSet = new HashSet<TacNode>();
                        variableSet.UnionWith(variablesContainer[assignmentNode.LeftPartIdentifier]);

                        foreach(var variable in variableSet)
                            if (!lineContainer[line].GetGen().Contains(variable))
                                lineContainer[line].AddKill(variable);
                    }
                }
            }


            return lineContainer;
        }

        public IGenKillContainer GetGenKillContainer() => new GenKillConatainer();
    }
}
