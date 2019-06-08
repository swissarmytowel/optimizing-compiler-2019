using SimpleLang.GenKill.Interfaces;
using SimpleLang.Optimizations;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.InOut
{
    public class InOutContainer<T>
    {
        public Dictionary<ThreeAddressCode, HashSet<T>> In = new Dictionary<ThreeAddressCode, HashSet<T>>();

        public Dictionary<ThreeAddressCode, HashSet<T>> Out = new Dictionary<ThreeAddressCode, HashSet<T>>();
        
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