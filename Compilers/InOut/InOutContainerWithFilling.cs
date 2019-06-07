using SimpleLang.GenKill.Interfaces;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TacBasicBlocks;
using OneBasicBlock = SimpleLang.TACode.ThreeAddressCode;

namespace SimpleLang.InOut
{
    class InOutContainerWithFilling
    {
        public Dictionary<OneBasicBlock, HashSet<TacNode>> In = new Dictionary<OneBasicBlock, HashSet<TacNode>>();
        public Dictionary<OneBasicBlock, HashSet<TacNode>> Out = new Dictionary<OneBasicBlock, HashSet<TacNode>>();

        /// <summary>
        /// We construct InOutContainer by GenKillContainer in every BasicBlock
        /// </summary>
        /// <param name="bBlocks"> All basic blocks </param>
        /// <param name="genKillContainers"> All gen-kill containers in basic blocks </param>
        public InOutContainerWithFilling(BasicBlocks bBlocks,
            Dictionary<OneBasicBlock, IExpressionSetsContainer> genKillContainers)
        {
            for (var i = 0; i < bBlocks.BasicBlockItems.Count; ++i)
            {
                var curBlock = bBlocks.BasicBlockItems[i];

                if (i == 0)
                {
                    In[curBlock] = new HashSet<TacNode>();
                }
                else
                {
                    var prevBlock = bBlocks.BasicBlockItems[i - 1];
                    FillInForBasicBlock(curBlock, prevBlock);
                }

                FillOutForBasicBlock(curBlock, genKillContainers);
            }
        }

        /// <summary>
        /// Fill IN for basic block B
        /// </summary>
        /// <param name="curBlock">current basic block</param>
        /// <param name="prevBlock">previous basic block</param>
        public void FillInForBasicBlock(OneBasicBlock curBlock, OneBasicBlock prevBlock)
        {
            In[curBlock] = new HashSet<TacNode>();
            In[curBlock].UnionWith(In[prevBlock]);
            In[curBlock].UnionWith(Out[prevBlock]);
        }

        /// <summary>
        /// Fill OUT for basic block B
        /// </summary>
        /// <param name="curBlock">Current basic block</param>
        /// <param name="genKillContainers">Gen/Kill container</param>
        public void FillOutForBasicBlock(OneBasicBlock curBlock,
            Dictionary<OneBasicBlock, IExpressionSetsContainer> genKillContainers)
        {
            if (genKillContainers.ContainsKey(curBlock))
            {
                Out[curBlock] = new HashSet<TacNode>(genKillContainers[curBlock].GetFirstSet()
                    .Union(In[curBlock]
                        .Except(genKillContainers[curBlock].GetSecondSet())));
            }
            else
            {
                Out[curBlock] = new HashSet<TacNode>(In[curBlock]);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var numBlock = 0;

            foreach (var inItem in In)
            {
                builder.Append($"--- IN {numBlock} :\n");
                if (inItem.Value.Count == 0)
                {
                    builder.Append("null");
                }
                else
                {
                    var tmp = 0;
                    foreach (var value in inItem.Value)
                    {
                        builder.Append($"{tmp++})");
                        builder.Append(value.ToString());
                        builder.Append("\n");
                    }
                }

                builder.Append($"\n--- OUT {numBlock}:\n");
                if (Out[inItem.Key].Count == 0)
                {
                    builder.Append("null");
                }
                else
                {
                    var tmp = 0;
                    foreach (var value in Out[inItem.Key])
                    {
                        builder.Append($"{tmp++})");
                        builder.Append(value.ToString());
                        builder.Append("\n");
                    }
                }

                builder.Append("\n");
                numBlock++;
            }

            return builder.ToString();
        }
    }
}