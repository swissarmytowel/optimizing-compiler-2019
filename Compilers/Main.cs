﻿using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using SimpleLang.CFG;
using SimpleLang.TACode;
using SimpleLang.TACode.TacNodes;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;
using SimpleLang.Optimizations;
using SimpleLang.GenKill.Implementations;
using SimpleLang.InOut;
using SimpleLang.DefUse;
using SimpleLang.IterationAlgorithms;
using SimpleLang.TacBasicBlocks;
using SimpleLang.TacBasicBlocks.DefUse;
using SimpleLang.E_GenKill.Implementations;
using SimpleLang.ConstDistrib;
using SimpleLang.CFG.DominatorsTree;
using SimpleLang.CFG.NaturalCycles;

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

                    var avis = new AssignCountVisitor();
                    parser.root.Visit(avis);
                    Console.WriteLine("Количество присваиваний = {0}", avis.Count);
                    Console.WriteLine("-------------------------------");

//                    var operv = new OperatorCountVisitor();
//                    parser.root.Visit(operv);
//                    Console.WriteLine(operv.Result);
//
//                    var maxcv = new MaxOpExprVisitor();
//                    parser.root.Visit(maxcv);
//                    Console.WriteLine(maxcv.Result);
//
//                    var inncycv = new IsInnerCycleVisitor();
//                    parser.root.Visit(inncycv);
//                    Console.WriteLine(inncycv.Result);
//
//                    var innifv = new IsInnerIfCycleVisitor();
//                    parser.root.Visit(innifv);
//                    Console.WriteLine(innifv.Result);
//
//                    var maxdeepv = new MaxDeepCycleVistor();
//                    parser.root.Visit(maxdeepv);
//                    Console.WriteLine(maxdeepv.Result);
//
//                    var parentv = new FillParentVisitor();
//                    parser.root.Visit(parentv);
//
//                    var sameminusv = new SameMinusOptVisitor();
//                    parser.root.Visit(sameminusv);
//
//                    var zeroMulVisitor = new ZeroMulOptVisitor();
//                    parser.root.Visit(zeroMulVisitor);
//
//                    var compareFalseVisitor = new CompareToItselfFalseOptVisitor();
//                    parser.root.Visit(compareFalseVisitor);
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

                    var whilefalsev = new WhileFalseOptVisitor();
                    parser.root.Visit(whilefalsev);

                    var zeroMulVisitor = new ZeroMulOptVisitor();
                    parser.root.Visit(zeroMulVisitor);

                    var compareFalseVisitor = new CompareToItselfFalseOptVisitor();
                    parser.root.Visit(compareFalseVisitor);

                    Console.WriteLine("-------------------------------");
                    //
                    //                    var ifNodeWithBoolExpr = new IfNodeWithBoolExprVisitor();
                    //                    parser.root.Visit(ifNodeWithBoolExpr);

                    //var plusZeroExpr = new PlusZeroExprVisitor();
                    //parser.root.Visit(plusZeroExpr);

                    var alwaysElse = new AlwaysElseVisitor();
                    parser.root.Visit(alwaysElse);

                    //var checkTruth = new CheckTruthVisitor();
                    //parser.root.Visit(checkTruth);

                    //Console.WriteLine("Оптимизированная программа");
                    //printv = new PrettyPrintVisitor(true);
                    //r.Visit(printv);
                    //Console.WriteLine(printv.Text);
                    //Console.WriteLine("-------------------------------");

                    Console.WriteLine("Оптимизированная программа");
                    printv = new PrettyPrintVisitor(true);
                    r.Visit(printv);
                    Console.WriteLine(printv.Text);
                    Console.WriteLine("-------------------------------");

                    var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
                    r.Visit(threeAddressCodeVisitor);
                    threeAddressCodeVisitor.Postprocess();
                   
                    Console.WriteLine("========== TAC ==============");
                    Console.WriteLine(threeAddressCodeVisitor);
                    
                    var cfg = new ControlFlowGraph(threeAddressCodeVisitor.TACodeContainer);

//                    Console.WriteLine(cfg);
//                    cfg.SaveToFile(@"cfg.txt");
                    
//                    var dstClassifier = new DstEdgeClassifier(cfg);
//                    dstClassifier.ClassificateEdges(cfg);
//                    Console.WriteLine(dstClassifier);

                    Console.WriteLine(cfg);
                    cfg.SaveToFile(@"cfg.txt");
                    var dstClassifier = new DstEdgeClassifier(cfg);
                    dstClassifier.ClassificateEdges(cfg);
                    Console.WriteLine(dstClassifier);

                    var depth = cfg.GetDepth(dstClassifier.EdgeTypes);
                    Console.WriteLine($"Depth CFG = {depth}");


                    /* -----------------------CFG TASKS START---------------------------------*/
                    Console.WriteLine("\nCFG TASKS START");
                    Console.WriteLine(cfg);
                    var edgeClassifierService = new EdgeClassifierService(cfg);
                    Console.WriteLine("EdgeClassifierService: \n" + edgeClassifierService);
                    bool isReducibility = DSTReducibility.IsReducibility(cfg);
                    Console.WriteLine("IsReducibility: " + isReducibility);
                    var naturalCycles = new CFGNaturalCycles(cfg);
                    Console.WriteLine("\nNaturalCycles: \n" + naturalCycles);
                    //Console.WriteLine("\nNestedCycles: \n" + naturalCycles.NestedLoopsText());
                    Console.WriteLine("\nCFG TASKS END");
                    /* -----------------------CFG TASKS END---------------------------------*/

                    //Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);
                    //var availExprOpt = new AvailableExprOptimization();
                    //availExprOpt.Optimize(cfg);
                    //Console.WriteLine("======= After algebraic identity =======");
                    //Console.WriteLine(cfg);


                    //                    Console.WriteLine("======= DV =======");
                    //                    Console.WriteLine(threeAddressCodeVisitor);
                    //                    var detector = new DefUseDetector();
                    //                    detector.DetectAndFillDefUse(threeAddressCodeVisitor.TACodeContainer);

                    Console.WriteLine();
                    Console.WriteLine("Before optimization");
                    Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);

                    /*Console.WriteLine("======= DV =======");
                    Console.WriteLine(threeAddressCodeVisitor);
                    var detector = new DefUseDetector();
                    detector.DetectAndFillDefUse(threeAddressCodeVisitor.TACodeContainer);

                    //Console.WriteLine("======= Detector 1 =======");
                    //Console.WriteLine(detector);
                    //Console.WriteLine("======= Detector 2 =======");
                    //Console.WriteLine(detector.ToString2());
                    
//                    var constPropagationOptimizer = new DefUseConstPropagation(detector);
//                    var result = constPropagationOptimizer.Optimize(threeAddressCodeVisitor.TACodeContainer);
//
//                    Console.WriteLine("======= After const propagation =======");
//                    Console.WriteLine(threeAddressCodeVisitor);
//
//                    result = constPropagationOptimizer.Optimize(threeAddressCodeVisitor.TACodeContainer);
//                    Console.WriteLine("======= After const propagation =======");
//                    Console.WriteLine(threeAddressCodeVisitor);
//
//                    var copyPropagationOptimizer = new DefUseCopyPropagation(detector);
//                    result = copyPropagationOptimizer.Optimize(threeAddressCodeVisitor.TACodeContainer);
//
//                    Console.WriteLine("======= After copy propagation =======");
//                    Console.WriteLine(threeAddressCodeVisitor);
=======
                    var constPropagationOptimizer = new DefUseConstPropagation(detector);
                    var result = constPropagationOptimizer.Optimize(threeAddressCodeVisitor.TACodeContainer);

                    Console.WriteLine("======= After const propagation =======");
                    Console.WriteLine(threeAddressCodeVisitor);

                    result = constPropagationOptimizer.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    Console.WriteLine("======= After const propagation =======");
                    Console.WriteLine(threeAddressCodeVisitor);

                    var copyPropagationOptimizer = new DefUseCopyPropagation(detector);
                    result = copyPropagationOptimizer.Optimize(threeAddressCodeVisitor.TACodeContainer);

                    Console.WriteLine("======= After copy propagation =======");
                    Console.WriteLine(threeAddressCodeVisitor);
                    */

                    //var bblocks = new BasicBlocks();
                    //bblocks.SplitTACode(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("Разбиение на базовые блоки завершилось");
                    var emptyopt = new EmptyNodeOptimization();
                    emptyopt.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    Console.WriteLine("Empty node optimization");
                    Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);

//                    var gotoOpt = new GotoOptimization();
//                    gotoOpt.Optimize(threeAddressCodeVisitor.TACodeContainer);
//                    Console.WriteLine("Goto optimization");
//                    Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);

                    //var elimintaion = new EliminateTranToTranOpt();
                    //elimintaion.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    //Console.WriteLine("Удаление переходов к переходам завершилось");

//                   var unreachableCode = new UnreachableCodeOpt();
//                   var res = unreachableCode.Optimize(threeAddressCodeVisitor.TACodeContainer);
//                   Console.WriteLine("Оптимизация для недостижимых блоков");

                    var algOpt = new AlgebraicIdentityOptimization();
                    algOpt.Optimize(threeAddressCodeVisitor.TACodeContainer);
                    Console.WriteLine("algebraic identity optimization");
                    Console.WriteLine(threeAddressCodeVisitor.TACodeContainer);

                    var bblocks = new BasicBlocks();
                    bblocks.SplitTACode(threeAddressCodeVisitor.TACodeContainer);
                    Console.WriteLine("Разбиение на базовые блоки завершилось");
                    Console.WriteLine();

                    GenKillVisitor genKillVisitor = new GenKillVisitor();
                    var genKillContainers = genKillVisitor.GenerateReachingDefinitionForBlocks(cfg.SourceBasicBlocks);

                    //start 
                    var commonTf = new TFByCommonWay(genKillContainers);
                    var composTf = new TFByComposition(genKillContainers);

                    Console.WriteLine("=== Compos ===");
                    Console.WriteLine(composTf.Calculate(new HashSet<TacNode>(), cfg.SourceBasicBlocks.BasicBlockItems.First()));
                   
                    Console.WriteLine("=== Common ===");
                    Console.WriteLine(commonTf.Calculate(new HashSet<TacNode>(), cfg.SourceBasicBlocks.BasicBlockItems.First()));

                    //end
//                    InOutContainerWithFilling inOutContainers =
//                        new InOutContainerWithFilling(cfg.SourceBasicBlocks, genKillContainers);
//                    Console.WriteLine("=== InOut для базовых блоков ===");
//                    Console.WriteLine(inOutContainers.ToString());

                    var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
                    DefUseForBlocksPrinter.Execute(defUseContainers);

                    var reachingDefenitionsITA = new ReachingDefinitionsITA(cfg, genKillContainers);
                    Console.WriteLine("=== InOut после итерационного алгоритма для достигающих определения ===");
                    Console.WriteLine(reachingDefenitionsITA.InOut);
                    Console.WriteLine("=======================================================================");
                    var reachingDefConstPropagation = new ReachingDefinitionsConstPropagation();
                    Console.WriteLine(threeAddressCodeVisitor);

                    reachingDefConstPropagation.Optimize(reachingDefenitionsITA);
                    reachingDefConstPropagation.Optimize(reachingDefenitionsITA);

                    
                    foreach (var bblock in bblocks)
                    {
                        Console.Write(bblock);
                    }

                    Console.WriteLine("============ Dominators ============");
                    var dominators = new DominatorsFinder(cfg);
                    for(var i = 0; i < dominators.Dominators.Count; ++i)
                    {
                        Console.WriteLine(i + ": ");
                        foreach (var tacNode in dominators.Dominators.ElementAt(i).Value)
                        {
                            Console.WriteLine(tacNode);
                        }
                    }
                    Console.WriteLine("============ Immediate Dominators ============");
                    for (var i = 0; i < dominators.ImmediateDominators.Count; ++i) {
                        Console.WriteLine(i + ": ");
                        if (dominators.ImmediateDominators.ElementAt(i).Value == null) {
                            Console.WriteLine("null");
                        } else {
                            foreach (var tacNode in dominators.ImmediateDominators.ElementAt(i).Value) {
                                Console.WriteLine(tacNode);
                            }
                        }
                        Console.WriteLine();
                    }


//                    var activeVariablesITA = new ActiveVariablesITA(cfg, defUseContainers);
//                    Console.WriteLine("=== InOut после итерационного алгоритма для активных переменных ===");
//                    Console.WriteLine(activeVariablesITA.InOut);
                    var activeVariablesITA = new ActiveVariablesITA(cfg, defUseContainers);
                    Console.WriteLine("=== InOut после итерационного алгоритма для активных переменных ===");
                    Console.WriteLine(activeVariablesITA.InOut);

                    /* -----------------------AvailableExpressions START---------------------------------*/
                    Console.WriteLine("AvailableExpressions Optimization");

                    Console.WriteLine("Before AvailableExprOptimization");
                    Console.WriteLine(cfg.SourceBasicBlocks
                        .BasicBlockItems.Select((bl, ind) => $"BLOCK{ind}:\n" + bl.ToString()).Aggregate((b1, b2) => b1 + b2));

                    E_GenKillVisitor availExprVisitor = new E_GenKillVisitor();
                    var availExprContainers = availExprVisitor.GenerateAvailableExpressionForBlocks(cfg.SourceBasicBlocks);

                    var availableExpressionsITA = new AvailableExpressionsITA(cfg, availExprContainers);
                    Console.WriteLine("=== InOut после итерационного алгоритма для доступных выражений ===");
                    Console.WriteLine(availableExpressionsITA.InOut);
                    var inData = availableExpressionsITA.InOut.In;

                    var availableExprOptimization = new AvailableExprOptimization();
                    bool isUsed = availableExprOptimization.Optimize(availableExpressionsITA);
                    Console.WriteLine("AvailableExprOptimization isUsed: " + isUsed);
                    isUsed = availableExprOptimization.Optimize(availableExpressionsITA);
                    Console.WriteLine("AvailableExprOptimization isUsed: " + isUsed);
                    Console.WriteLine(cfg.SourceBasicBlocks
                        .BasicBlockItems.Select((bl, ind) => $"BLOCK{ind}:\n" + bl.ToString()).Aggregate((b1, b2) => b1 + b2));
                    /* -----------------------AvailableExpressions END---------------------------------*/

                    /* -----------------------ConstDistribOptimization START---------------------------------*/
                    Console.WriteLine("ConstDistributionOptimization: Before");
                    Console.WriteLine(cfg.SourceBasicBlocks
                        .BasicBlockItems.Select((bl, ind) => $"BLOCK{ind}:\n" + bl.ToString()).Aggregate((b1, b2) => b1 + b2));

                    var constDistITA = new ConstDistributionITA(cfg);
                    var constDistOpt = new ConstDistributionOptimization();
                    var isConstDistApplied = constDistOpt.Optimize(constDistITA);
                    Console.WriteLine("ConstDistributionOptimization isUsed: " + isConstDistApplied);
                    Console.WriteLine(cfg.SourceBasicBlocks
                        .BasicBlockItems.Select((bl, ind) => $"BLOCK{ind}:\n" + bl.ToString()).Aggregate((b1, b2) => b1 + b2));
                    /* -----------------------ConstDistrib END---------------------------------*/
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

 