using SimpleLang.Optimizations;
using SimpleLang.TACode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OneBasicBlock = SimpleLang.TACode.ThreeAddressCode;

namespace SimpleLang.InOut
{
    class InOutContainer
    {

#region
        // TO DO :: Заменить на GenKill Миши и Димы
        class GenKill
        {
            Dictionary<OneBasicBlock, HashSet<string>> gen = new Dictionary<OneBasicBlock, HashSet<string>>();
            Dictionary<OneBasicBlock, HashSet<string>> kill = new Dictionary<OneBasicBlock, HashSet<string>>();

            public Dictionary<OneBasicBlock, HashSet<string>> Gen { get { return gen; } }
            public Dictionary<OneBasicBlock, HashSet<string>> Kill { get { return kill; } }
        }
#endregion

        Dictionary<OneBasicBlock, HashSet<string>> In = new Dictionary<OneBasicBlock, HashSet<string>>();
        Dictionary<OneBasicBlock, HashSet<string>> Out = new Dictionary<OneBasicBlock, HashSet<string>>();

        InOutContainer(BasicBlocks bBlocks, GenKill genKillContainer)
        {
            for (int i = 0; i < bBlocks.BasicBlockItems.Count; ++i) {
                var curBlock = bBlocks.BasicBlockItems[i];

                if (i == 0) {
                    In[curBlock] = new HashSet<string>();
                } else {
                    var prevBlock = bBlocks.BasicBlockItems[i - 1];
                    In[curBlock].UnionWith(In[prevBlock]);
                    In[curBlock].UnionWith(Out[prevBlock]);
                }
                
                Out[curBlock] = new HashSet<string>(genKillContainer.Gen[curBlock]
                    .Union(In[curBlock]
                    .Except(genKillContainer.Kill[curBlock])));
            }
        }
    }
}
