using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using SimpleLang.Optimizations.DefUse;
using SimpleLang.CFG;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;
using SimpleLang.Optimizations;
using System.Linq;
using SimpleLang.GenKill.Implementations;


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
                Text = Text.Replace('\t', ' ');

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

                    //var avis = new AssignCountVisitor();
                    //parser.root.Visit(avis);
                    //Console.WriteLine("Количество присваиваний = {0}", avis.Count);
                    //Console.WriteLine("-------------------------------");

                    //var operv = new OperatorCountVisitor();
                    //parser.root.Visit(operv);
                    //Console.WriteLine(operv.Result);

                    //var maxcv = new MaxOpExprVisitor();
                    //parser.root.Visit(maxcv);
                    //Console.WriteLine(maxcv.Result);

                    //var inncycv = new IsInnerCycleVisitor();
                    //parser.root.Visit(inncycv);
                    //Console.WriteLine(inncycv.Result);

                    //var innifv = new IsInnerIfCycleVisitor();
                    //parser.root.Visit(innifv);
                    //Console.WriteLine(innifv.Result);

                    //var maxdeepv = new MaxDeepCycleVistor();
                    //parser.root.Visit(maxdeepv);
                    //Console.WriteLine(maxdeepv.Result);

                    //var parentv = new FillParentVisitor();
                    //parser.root.Visit(parentv);

                    //var sameminusv = new SameMinusOptVisitor();
                    //parser.root.Visit(sameminusv);

                    //var zeroMulVisitor = new ZeroMulOptVisitor();
                    //parser.root.Visit(zeroMulVisitor);

                    //var compareFalseVisitor = new CompareToItselfFalseOptVisitor();
                    //parser.root.Visit(compareFalseVisitor);

                    //Console.WriteLine("-------------------------------");

                    //var ifNodeWithBoolExpr = new IfNodeWithBoolExprVisitor();
                    //parser.root.Visit(ifNodeWithBoolExpr);

                    //var plusZeroExpr = new PlusZeroExprVisitor();
                    //parser.root.Visit(plusZeroExpr);

                    //var alwaysElse = new AlwaysElseVisitor();
                    //parser.root.Visit(alwaysElse);

                    //var checkTruth = new CheckTruthVisitor();
                    //parser.root.Visit(checkTruth);

                    //Console.WriteLine("Оптимизированная программа");
                    //printv = new PrettyPrintVisitor(true);
                    //r.Visit(printv);
                    //Console.WriteLine(printv.Text);
                    //Console.WriteLine("-------------------------------");

                    //Console.WriteLine("Оптимизированная программа");
                    //printv = new PrettyPrintVisitor(true);
                    //r.Visit(printv);
                    //Console.WriteLine(printv.Text);
                    //Console.WriteLine("-------------------------------");

                    var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
                    r.Visit(threeAddressCodeVisitor);

                    var cfg = new ControlFlowGraph(threeAddressCodeVisitor.TACodeContainer);
                    Console.WriteLine(cfg);
                    cfg.SaveToFile(@"cfg.txt");

                    var bblock_1 = new ThreeAddressCode();
                    bblock_1.PushNode(new TacAssignmentNode {
                        LeftPartIdentifier="i",
                        FirstOperand="m",
                        Operation="-",
                        SecondOperand="1"
                    });
                    bblock_1.PushNode(new TacAssignmentNode
                    {
                        LeftPartIdentifier = "j",
                        FirstOperand = "n",
                    });
                    bblock_1.PushNode(new TacAssignmentNode
                    {
                        LeftPartIdentifier = "a",
                        FirstOperand = "u1"
                    });

                    var bblock_2 = new ThreeAddressCode();
                    bblock_2.PushNode(new TacAssignmentNode
                    {
                        LeftPartIdentifier = "i",
                        FirstOperand = "i",
                        Operation = "+",
                        SecondOperand = "1"
                    });
                    bblock_2.PushNode(new TacAssignmentNode
                    {
                        LeftPartIdentifier = "j",
                        FirstOperand = "j",
                        Operation = "-",
                        SecondOperand = "1"
                    });

                    var bblock_3 = new ThreeAddressCode();
                    bblock_3.PushNode(new TacAssignmentNode
                    {
                        LeftPartIdentifier = "a",
                        FirstOperand = "u2",
                    });

                    var bblock_4 = new ThreeAddressCode();
                    bblock_4.PushNode(new TacAssignmentNode
                    {
                        LeftPartIdentifier = "i",
                        FirstOperand = "u3",
                    });

                    var tmp_bblocks = new BasicBlocks
                    {
                        BasicBlockItems = new List<ThreeAddressCode> { bblock_1, bblock_2, bblock_3, bblock_4 }
                    };

                    var gen_kill_visitor = new GenKillVisitor();
                    var gen_kill_container = gen_kill_visitor.GenerateReachingDefinitionForBlocks(tmp_bblocks);

                    var blocks = new List<ThreeAddressCode> { bblock_1, bblock_2, bblock_3, bblock_4 };
                    var blocksIn = new HashSet<TacNode>();
                    var tf_composition = new TFByComposition(gen_kill_container);

                    foreach (var block in blocks)
                    {
                        blocksIn = tf_composition.Calculate(blocksIn, block);
                    };

                    //var cfg = new ControlFlowGraph();
                    //cfg.Construct(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine(cfg);
                    //cfg.SaveToFile(@"cfg.txt");

                    //Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);
                    //var availExprOpt = new AvailableExprOptimization();
                    //availExprOpt.Optimize(cfg);
                    //Console.WriteLine("======= After algebraic identity =======");
                    //Console.WriteLine(cfg);

                    //Console.WriteLine("======= DV =======");
                    //Console.WriteLine(threeAddressCodeVisitor);
                    //var detector = new DefUseDetector();
                    //detector.DetectAndFillDefUse(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("======= Detector 1 =======");
                    //Console.WriteLine(detector);
                    //Console.WriteLine("======= Detector 2 =======");
                    //Console.WriteLine(detector.ToString2());
                    //var constPropagationOptimizer = new DefUseConstPropagation(detector);
                    //var result = constPropagationOptimizer.Optimize(threeAddressCodeVisitor.TACodeContainer);

                    //Console.WriteLine("======= After const propagation =======");
                    //Console.WriteLine(threeAddressCodeVisitor);

                    //result = constPropagationOptimizer.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("======= After const propagation =======");
                    //Console.WriteLine(threeAddressCodeVisitor);

                    //var copyPropagationOptimizer = new DefUseCopyPropagation(detector);
                    //result = copyPropagationOptimizer.Optimize(threeAddressCodeVisitor.TACodeContainer);

                    //Console.WriteLine("======= After copy propagation =======");
                    //Console.WriteLine(threeAddressCodeVisitor);

                    //var bblocks = new BasicBlocks();
                    //bblocks.SplitTACode(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("Разбиение на базовые блоки завершилось");
                    //var emptyopt = new EmptyNodeOptimization();
                    //emptyopt.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("Empty node optimization");
                    //Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);

                    //var gotoOpt = new GotoOptimization();
                    //gotoOpt.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("Goto optimization");
                    //Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);

                    //var elimintaion = new EliminateTranToTranOpt();
                    //elimintaion.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("Удаление переходов к переходам завершилось");

                    //var unreachableCode = new UnreachableCodeOpt();
                    //var res = unreachableCode.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("Оптимизация для недостижимых блоков");

                    //var algOpt = new AlgebraicIdentityOptimization();
                    //algOpt.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("algebraic identity optimization");
                    //Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);

                    //var bblocks = new BasicBlocks();
                    //bblocks.SplitTACode(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("Разбиение на базовые блоки завершилось");
                    //Console.WriteLine();

                    var defUseSet = new DefUseSetForBlocks(bblocks);
                    Console.WriteLine("DefUSeSet для базовых блоков");
                    Console.WriteLine(defUseSet);
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
