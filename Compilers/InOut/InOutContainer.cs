using SimpleLang.GenKill.Interfaces;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OneBasicBlock = SimpleLang.TACode.ThreeAddressCode;

namespace SimpleLang.InOut
{
    class InOutContainer
    {
        public Dictionary<OneBasicBlock, HashSet<TacNode>> In = new Dictionary<OneBasicBlock, HashSet<TacNode>>();
        public Dictionary<OneBasicBlock, HashSet<TacNode>> Out = new Dictionary<OneBasicBlock, HashSet<TacNode>>();

        /// <summary>
        /// We construct InOutContainer by GenKillContainer in every BasicBlock
        /// </summary>
        /// <param name="bBlocks"> All basic blocks </param>
        /// <param name="genKillContainers"> All gen-kill containers in basic blocks </param>
        public InOutContainer(BasicBlocks bBlocks, Dictionary<OneBasicBlock, IGenKillContainer> genKillContainers)
        {
            for (int i = 0; i < bBlocks.BasicBlockItems.Count; ++i) {
                var curBlock = bBlocks.BasicBlockItems[i];

                if (i == 0) {
                    In[curBlock] = new HashSet<TacNode>();
                } else {
                    var prevBlock = bBlocks.BasicBlockItems[i - 1];
                    In[curBlock] = new HashSet<TacNode>();
                    In[curBlock].UnionWith(In[prevBlock]);
                    In[curBlock].UnionWith(Out[prevBlock]);
                }

                if (genKillContainers.ContainsKey(curBlock)) {
                    Out[curBlock] = new HashSet<TacNode>(genKillContainers[curBlock].GetGen()
                        .Union(In[curBlock]
                        .Except(genKillContainers[curBlock].GetKill())));
                } else {
                    Out[curBlock] = new HashSet<TacNode>(In[curBlock]);
                }
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var numBlock = 0;

            foreach (var inItem in In) {
                builder.Append(string.Format("--- IN {0} :\n", numBlock));
                if (inItem.Value.Count == 0) {
                    builder.Append("null");
                } else {
                    var tmp = 0;
                    foreach (var value in inItem.Value) {
                        builder.Append(string.Format("{0})", tmp++));
                        builder.Append(value.ToString());
                        builder.Append("\n");
                    }
                }
                builder.Append(string.Format("\n--- OUT {0}:\n", numBlock));
                if (Out[inItem.Key].Count == 0) {
                    builder.Append("null");
                } else {
                    var tmp = 0;
                    foreach (var value in Out[inItem.Key]) {
                        builder.Append(string.Format("{0})", tmp++));
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

