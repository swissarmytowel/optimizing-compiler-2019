using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Optimizations
{
    class DefUseSetForBlocks 
    {
        private BasicBlocks bb;
        private IEnumerable<string> vars;
        public List<string> DefSet { get; private set; }
        public List<string> UseSet { get; private set; }
        DefUseSetForBlocks(BasicBlocks bb, IEnumerable<string> vars)
        {
            this.bb = bb;
            this.vars = vars;
            DefSet = new List<string>();
            UseSet = new List<string>();
        }

        

    }
}
