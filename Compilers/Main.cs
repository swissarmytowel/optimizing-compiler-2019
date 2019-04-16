using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using SimpleLang.CFG;
using SimpleLang.TACode.TacNodes;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;
using SimpleLang.Optimizations;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
            var DirectoryPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string FileName = Path.Combine(DirectoryPath, "a.txt");
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                var r = parser.root;
                // Console.WriteLine(r);
                Console.WriteLine("Исходный текст программы");
                var printv = new PrettyPrintVisitor(true);
                r.Visit(printv);
                Console.WriteLine(printv.Text);
                Console.WriteLine("-------------------------------");

                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                    Console.WriteLine("Синтаксическое дерево построено");

                    // TODO: add loop through all tree optimizations

                    var avis = new AssignCountVisitor();
                    parser.root.Visit(avis);
                    Console.WriteLine("Количество присваиваний = {0}", avis.Count);
                    Console.WriteLine("-------------------------------");

                    var operv = new OperatorCountVisitor();
                    parser.root.Visit(operv);
                    Console.WriteLine(operv.Result);

                    var maxcv = new MaxOpExprVisitor();
                    parser.root.Visit(maxcv);
                    Console.WriteLine(maxcv.Result);

                    var inncycv = new IsInnerCycleVisitor();
                    parser.root.Visit(inncycv);
                    Console.WriteLine(inncycv.Result);

                    var innifv = new IsInnerIfCycleVisitor();
                    parser.root.Visit(innifv);
                    Console.WriteLine(innifv.Result);

                    var maxdeepv = new MaxDeepCycleVistor();
                    parser.root.Visit(maxdeepv);
                    Console.WriteLine(maxdeepv.Result);

                    var parentv = new FillParentVisitor();
                    parser.root.Visit(parentv);

                    var sameminusv = new SameMinusOptVisitor();
                    parser.root.Visit(sameminusv);

                    var zeroMulVisitor = new ZeroMulOptVisitor();
                    parser.root.Visit(zeroMulVisitor);

                    var compareFalseVisitor = new CompareToItselfFalseOptVisitor();
                    parser.root.Visit(compareFalseVisitor);

                    Console.WriteLine("-------------------------------");

                    var ifNodeWithBoolExpr = new IfNodeWithBoolExprVisitor();
                    parser.root.Visit(ifNodeWithBoolExpr);

                    var plusZeroExpr = new PlusZeroExprVisitor();
                    parser.root.Visit(plusZeroExpr);

                    var alwaysElse = new AlwaysElseVisitor();
                    parser.root.Visit(alwaysElse);

                    var checkTruth = new CheckTruthVisitor();
                    parser.root.Visit(checkTruth);

                    Console.WriteLine("Оптимизированная программа");
                    printv = new PrettyPrintVisitor(true);
                    r.Visit(printv);
                    Console.WriteLine(printv.Text);
                    Console.WriteLine("-------------------------------");


                    var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
                    r.Visit(threeAddressCodeVisitor);
                    Console.WriteLine(threeAddressCodeVisitor);

                    var gotoOpt = new GotoOptimization();
                    gotoOpt.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    Console.WriteLine("goto optimization");
                    Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);

                    var algOpt = new AlgebraicIdentityOptimization();
                    algOpt.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    Console.WriteLine("algebraic identity optimization");
                    Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);

                    var bblocks = new BasicBlocks();
                    bblocks.SplitTACode(threeAddressCodeVisitor.TACodeContainer);
                    Console.WriteLine("Разбиение на базовые блоки завершилось");
                    Console.WriteLine();

                    var cfg = new ControlFlowGraph();
                    cfg.Construct(threeAddressCodeVisitor.TACodeContainer);
                    Console.WriteLine(cfg);
                    cfg.SaveToFile(@"cfg.txt");
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

            Console.ReadLine();
        }

    }
}
