using System;
using System.IO;
using System.Linq;
using SimpleLang.CFG;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;

namespace DominationBorder
{
    class Program
    {
        static void Main(string[] args)
        {
            var DirectoryPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string FileName = Path.Combine(DirectoryPath, "a.txt");
            try
            {
                string Text = File.ReadAllText(FileName);
                Text = Text.Replace('\t', ' ');

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                var r = parser.root;
                var printv = new PrettyPrintVisitor(true);
                r.Visit(printv);
                Console.WriteLine("Текст программы:");
                Console.WriteLine(printv.Text);
                Console.WriteLine();
                var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
                r.Visit(threeAddressCodeVisitor);
                threeAddressCodeVisitor.Postprocess();
                var cfg = new ControlFlowGraph(threeAddressCodeVisitor.TACodeContainer);
                Console.WriteLine("CFG: ");
                Console.WriteLine(cfg);
                Console.WriteLine("DominationBorder: ");
                foreach (var vertex in cfg.Vertices)
                {
                    Console.WriteLine();
                    Console.WriteLine("vertex: \n" + vertex);
                    var borderSet = DominationBorder.Execute(cfg, vertex);
                    string borderStr = "";
                    foreach (var el in borderSet)
                        borderStr += el + "\n";
                    if (borderSet.Count() == 0)
                        borderStr = "Empty";
                    Console.WriteLine("borderSet: \n\n" + borderStr);
                    Console.WriteLine();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
            }
        }
    }
}
