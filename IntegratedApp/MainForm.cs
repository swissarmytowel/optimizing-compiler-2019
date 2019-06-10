using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleLang.Visitors;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Optimizations.Interfaces;
using SimpleLang.Optimizations;
using SimpleLang.Optimizations.BooleanOptimization;
using SimpleLang.TACode.TacNodes;
using SimpleLang.TACode;
using SimpleLang.ConstDistrib;
using SimpleLang.TacBasicBlocks;
using SimpleLang.CFG.DominatorsTree;
using SimpleLang.DefUse;
using SimpleLang.CFG;
using SimpleLang.IterationAlgorithms;
using SimpleLang.GenKill.Implementations;
using SimpleLang.E_GenKill.Implementations;


namespace IntegratedApp
{
    public partial class IntegratedApp : Form
    {
#region Init data
        enum OptimizationsByAstTree
        {
            opt1 = 0,   //1) 1 * ex, ex * 1, ex / 1 => ex
            opt2 = 1,   //2) 0 * ex, ex * 0 => 0
            opt3 = 2,   //3) 2 * 3 => 6
            opt4 = 3,   //4) 0 + exp, exp + 0 => exp
            opt5 = 4,   //5) a - a => 0
            opt6 = 5,   //6) 2 < 3 => true
            opt9 = 6,   //9) a > a, a != a => false
            opt10 = 7,  //10) x = x => null
            opt11 = 8,  //11) if (true) st1; else st2; => st1
            opt12 = 9,  //12) if (false) st1; else st2; => st2
            opt13 = 10, //13) if (ex) null; else null; => null
            opt14 = 11, //14) while (false) st; => null
        }

        enum OptimizationsByBasicBlocks
        {
            opt0 = 0,       //Протяжка const в пределах ББл на основе Def-Use
            opt1 = 1,       //Протяжка копий в пределах ББл на основе Def-Use
            opt2 = 2,       //Свертка const
            opt3 = 3,       //Алгебраические тождества
            opt4 = 4,       //Логические тождества
            opt5 = 5,       //Оптимизация общих подвыражений
            opt6 = 6,       //Очистка от пустых опр-ов
            opt7 = 7,       //Устранение переходов через переходы
            opt8 = 8,       //Устранение недостижимого кода
            opt9 = 9,       //Устранение переходов к переходам
            opt10 = 10,     //LVN - алгоритм
        }

        enum OptimizationsByIterationAlgorithm
        {
            //opt0 = 0,       //ИТА для активных переменных
            opt1 = 0,       //Удаление мертвого кода на основе ИТА для активных переменных
            //opt2 = 2,       //ИТА для достигающих переменных
            opt3 = 1,       //Протяжка const на основе ИТА для достигающих переменных
           // opt4 = 4,       //ИТА для доступных выражений
            opt5 = 2,       //Провести оптимизации на основе анализа доступных выражений
           // opt6 = 6,       //ИТА в задаче распространения const
            opt7 = 3,       //Распространение const на основе ИТА
        }

        enum OptimizationsByControlFlowGraph
        {
            opt0 = 0,   //Дерево доминаторов
            opt1 = 1,   //Классификация ребер в CFG
            opt2 = 2,   //Определение глубины в CFG
            opt3 = 3,   //Определение того, явл. ли ребро обратимым
            opt4 = 4,   //Определение того, явл. ли CFG приводимым
            opt5 = 5,   //Определение всех естественных циклов в CFG с информ. об их вложенности
            opt6 = 6,   //Построение глубинного остовного дерева с соотв. нумерацией вершин
            opt7 = 7,   //Ускорение ИТА для задачи о достигающих определениях засчет перенумерации ББЛ
        }

        /// <summary>
        /// Выбранные оптимизации по AST - дереву
        /// </summary>
        private List<OptimizationsByAstTree> checkedOptimizationsBlock1 = new List<OptimizationsByAstTree>();
        private Dictionary<OptimizationsByAstTree, Visitor> optimizationsBlock1 = new Dictionary<OptimizationsByAstTree, Visitor>() {
            { OptimizationsByAstTree.opt1, new SimplifyMultDivisionByOneVisitor() },
            { OptimizationsByAstTree.opt2, new ZeroMulOptVisitor() },
            { OptimizationsByAstTree.opt3, new ConstFoldingVisitor() },
            { OptimizationsByAstTree.opt4, new PlusZeroExprVisitor() },
            { OptimizationsByAstTree.opt5, new SameMinusOptVisitor() },
            { OptimizationsByAstTree.opt6, new CheckTruthVisitor() },
            { OptimizationsByAstTree.opt9, new CompareToItselfFalseOptVisitor() },
            { OptimizationsByAstTree.opt10, new RemoveSelfAssignment() },
            { OptimizationsByAstTree.opt11, new IfNodeWithBoolExprVisitor() },
            { OptimizationsByAstTree.opt12, new AlwaysElseVisitor() },
            { OptimizationsByAstTree.opt13, new DelOfDeadConditionsVisitor() },
            { OptimizationsByAstTree.opt14, new WhileFalseOptVisitor() },
        };

        /// <summary>
        /// Выбранные оптимизации по ББл
        /// </summary>
        private List<OptimizationsByBasicBlocks> checkedOptimizationsBlock2 = new List<OptimizationsByBasicBlocks>();
        private Dictionary<OptimizationsByBasicBlocks, IOptimizer> optimizationsBlock2 = new Dictionary<OptimizationsByBasicBlocks, IOptimizer>() {
            { OptimizationsByBasicBlocks.opt0, new DefUseConstPropagation() },
            { OptimizationsByBasicBlocks.opt1, new DefUseCopyPropagation() },
            { OptimizationsByBasicBlocks.opt2, new ConvConstOptimization() },
            { OptimizationsByBasicBlocks.opt3, new AlgebraicIdentityOptimization() },
            { OptimizationsByBasicBlocks.opt4, new BooleanOptimizer() },
            { OptimizationsByBasicBlocks.opt5, new CommonSubexprOptimization() },
            { OptimizationsByBasicBlocks.opt6, new EmptyNodeOptimization() },
            { OptimizationsByBasicBlocks.opt7, new GotoOptimization() },
            { OptimizationsByBasicBlocks.opt8, new UnreachableCodeOpt() },
            { OptimizationsByBasicBlocks.opt9, new EliminateTranToTranOpt() },
            { OptimizationsByBasicBlocks.opt10, new LocalValueNumberingOptimization() },
        };

        private readonly HashSet<OptimizationsByBasicBlocks> _optimizationsOnWholeTac = new HashSet<OptimizationsByBasicBlocks> {
            OptimizationsByBasicBlocks.opt8,
            OptimizationsByBasicBlocks.opt9 };

        private bool CheckIfOptimizationIsOnWholeTac(OptimizationsByBasicBlocks optimizationId)
        {
            return _optimizationsOnWholeTac.Contains(optimizationId);
        } 

        private static ThreeAddressCode GetTacFromBBlocks(BasicBlocks bblocks)
        {
            var tac = new ThreeAddressCode();
            foreach(var block in bblocks)
            {
                tac.PushNodes(block.TACodeLines);
            }
            return tac;
        }
        /// <summary>
        /// Выбранные оптимизации, связанные с ИТА
        /// </summary>
        private List<OptimizationsByIterationAlgorithm> checkedOptimizationsBlock3 = new List<OptimizationsByIterationAlgorithm>();
        private Dictionary<OptimizationsByIterationAlgorithm, IIterativeAlgorithmOptimizer<TacNode>> optimizationsBlock3TacNode = new Dictionary<OptimizationsByIterationAlgorithm, IIterativeAlgorithmOptimizer<TacNode>>() {
           
            { OptimizationsByIterationAlgorithm.opt1, new DeadCodeOptimizationWithITA() },
            { OptimizationsByIterationAlgorithm.opt3, new ReachingDefinitionsConstPropagation() },
            { OptimizationsByIterationAlgorithm.opt5, new AvailableExprOptimization() },
        };

        private Dictionary<OptimizationsByIterationAlgorithm, IIterativeAlgorithmOptimizer<SemilatticeStreamValue>> optimizationsBlock3SemilatticeStreamValue = new Dictionary<OptimizationsByIterationAlgorithm, IIterativeAlgorithmOptimizer<SemilatticeStreamValue>>() {
            { OptimizationsByIterationAlgorithm.opt7, new ConstDistributionOptimization() },
        };

        /// <summary>
        /// Выбранные оптимизации, связанные с CFG
        /// </summary>
        private List<OptimizationsByControlFlowGraph> checkedOptimizationsBlock4 = new List<OptimizationsByControlFlowGraph>();
#endregion

        public IntegratedApp()
        {
            InitializeComponent();
        }

#region callback StripMenuItems
        private void LoadStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                StreamReader sr = new StreamReader(openFileDialog.FileName);
                InputTextBox.Text = sr.ReadToEnd();
                sr.Close();
                InputTextBox.SelectionStart = InputTextBox.Text.Length;
            }
        }

        private void SaveStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                sw.WriteLine(InputTextBox.Text);
                sw.Close();
            }
        }

        private void InfoStripMenuItem_Click(object sender, EventArgs e)
        {
            AdditionalWindow tacWindow = new AdditionalWindow("Справка");
            tacWindow.Show();
        }
        #endregion

#region callback Buttons
        private void RunButton_Click(object sender, EventArgs e)
        {
            if (InputTextBox.Text == "") {
                MessageBox.Show("Нужен текст программы");
                return;
            }

            if (checkedOptimizationsBlock1.Count == 0 && checkedOptimizationsBlock2.Count == 0 
                && checkedOptimizationsBlock3.Count == 0 && checkedOptimizationsBlock4.Count == 0) {
                MessageBox.Show("Не выбрана ни одна оптимизация");
                return;
            }

            OutputTextBox.Text = "";
            TmpNameManager.Instance.Drop();

            Scanner scanner = new Scanner();
            scanner.SetSource(InputTextBox.Text, 0);
            Parser parser = new Parser(scanner);
            var b = parser.Parse();

            var parentv = new FillParentVisitor();
            parser.root.Visit(parentv);
            

#region Optimizations by AST
            if (checkedOptimizationsBlock1.Count != 0) {
                foreach (var opt in checkedOptimizationsBlock1) {
                    parser.root.Visit(optimizationsBlock1[opt]);
                }

                var printv = new PrettyPrintVisitor(true);
                parser.root.Visit(printv);
                OutputTextBox.Text += printv.Text;
                OutputTextBox.Text += "\n";
            }
            
#endregion
            var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
            parser.root.Visit(threeAddressCodeVisitor);
            threeAddressCodeVisitor.Postprocess();

            var cfg = new ControlFlowGraph(threeAddressCodeVisitor.TACodeContainer);

  #region Optimizations by Basic blocks
            if (checkedOptimizationsBlock2.Count != 0) {
                OutputTextBox.Text += "=== BEFORE BBlocks OPTIMIZATIONS === \n" + cfg.SourceCode;
                for (int i = 0; i < cfg.SourceBasicBlocks.BasicBlockItems.Count; ++i) {
                    OutputTextBox.Text += string.Format("===== Three address code for Block #{0} =====\n", i);
                    OutputTextBox.Text += cfg.SourceBasicBlocks.BasicBlockItems[i].ToString();
                    OutputTextBox.Text += "\n";

                    int ind, iteration = 0;
                    for (ind = 0; ind < checkedOptimizationsBlock2.Count; ind++) {
                        var opt = checkedOptimizationsBlock2[ind];
                        var isOnWholeTac = CheckIfOptimizationIsOnWholeTac(opt);
                        var result = optimizationsBlock2[opt].Optimize(isOnWholeTac
                                                                       ? cfg.SourceCode 
                                                                       : cfg.SourceBasicBlocks.BasicBlockItems[i]);


                        if (result) {
                            ind = -1;
                            continue;
                        }
                        
                        OutputTextBox.Text += string.Format("===== Block #{0} Optimization {1} iteration #{2} =====\n\n", i, optimizationsBlock2[opt].GetType(), iteration++);
                        OutputTextBox.Text += isOnWholeTac
                                              ? cfg.SourceCode.ToString()
                                              : cfg.SourceBasicBlocks.BasicBlockItems[i].ToString();
                        OutputTextBox.Text += "\n";
                        Console.WriteLine("sosi");
                    }
                }
                OutputTextBox.Text += "=== AFTER BBlocks OPTIMIZATIONS === \n";

                cfg.Rebuild(GetTacFromBBlocks(cfg.SourceBasicBlocks));
                OutputTextBox.Text += cfg.SourceCode;

            }
            #endregion

            #region Optimizations by Iteration Algorithm

            if (checkedOptimizationsBlock3.Count != 0) {
                var counter = 0;
                var ind = 0;
                for(ind = 0; ind < checkedOptimizationsBlock3.Count; ind++)
                {
                    var opt = checkedOptimizationsBlock3[ind];
                    string typeOpt = "";
                    bool isOptimized = false;
                    switch(opt)
                    {
                        case OptimizationsByIterationAlgorithm.opt1:
                            var defUseContainers = DefUseForBlocksGenerator.Execute(cfg.SourceBasicBlocks);
                            var ita1 = new ActiveVariablesITA(cfg, defUseContainers);
                            isOptimized = new DeadCodeOptimizationWithITA().Optimize(ita1);
                            typeOpt = "Dead code optimization";
                            break;
                        case OptimizationsByIterationAlgorithm.opt3:
                            GenKillVisitor genKillVisitor = new GenKillVisitor();
                            var genKillContainers = genKillVisitor.GenerateReachingDefinitionForBlocks(cfg.SourceBasicBlocks);
                            var ita3 = new ReachingDefinitionsITA(cfg, genKillContainers);
                            isOptimized = new ReachingDefinitionsConstPropagation().Optimize(ita3);
                            typeOpt = "Const propagation by reaching definition";
                            break;
                        case OptimizationsByIterationAlgorithm.opt5:
                            E_GenKillVisitor availExprVisitor = new E_GenKillVisitor();
                            var availExprContainers = availExprVisitor.GenerateAvailableExpressionForBlocks(cfg.SourceBasicBlocks);
                            var availableExpressionsITA = new AvailableExpressionsITA(cfg, availExprContainers);
                            var availableExprOptimization = new AvailableExprOptimization();
                            isOptimized = availableExprOptimization.Optimize(availableExpressionsITA);
                            typeOpt = "Available expr optimization";
                            break;
                        case OptimizationsByIterationAlgorithm.opt7:
                            var constDistITA = new ConstDistributionITA(cfg);
                            var constDistOpt = new ConstDistributionOptimization();
                            isOptimized = constDistOpt.Optimize(constDistITA);
                            typeOpt = "Const distribution";
                            break;
                    }

                    if(isOptimized)
                    {
                        ind = -1;
                        continue;
                    }

                    OutputTextBox.Text += $"===== Optimization {typeOpt} iteration #{counter++} =====\n\n";
                    OutputTextBox.Text += cfg.SourceCode.ToString();
                    OutputTextBox.Text += "\n";
                }

            }

            #endregion

            cfg.Rebuild(threeAddressCodeVisitor.TACodeContainer);

            #region Optimizations by CFG

            if (checkedOptimizationsBlock4.Count != 0) {
                foreach(var item in checkedOptimizationsBlock4) {
                    switch (item) {
                        case OptimizationsByControlFlowGraph.opt0:
                            OutputTextBox.Text += string.Format("===== Three address code =====\n");
                            OutputTextBox.Text += threeAddressCodeVisitor.TACodeContainer;
                            OutputTextBox.Text += "\n";
                            OutputTextBox.Text += cfg;

                            var dominatorService = new DominatorsService(cfg);
                            OutputTextBox.Text += dominatorService;
                            OutputTextBox.Text += "\n";
                            break;
                        case OptimizationsByControlFlowGraph.opt1:
                            break;
                        case OptimizationsByControlFlowGraph.opt2:
                            break;
                        case OptimizationsByControlFlowGraph.opt3:
                            break;
                        case OptimizationsByControlFlowGraph.opt4:
                            break;
                        case OptimizationsByControlFlowGraph.opt5:
                            break;
                        case OptimizationsByControlFlowGraph.opt6:
                            break;
                        case OptimizationsByControlFlowGraph.opt7:
                            break;
                    }
                }
            }

#endregion
        }
        
        private void ResetButton_Click(object sender, EventArgs e)
        {
            foreach (int elem in checkedListBox1.CheckedIndices) {
                checkedListBox1.SetItemCheckState(elem, CheckState.Unchecked);
            }
            foreach (int elem in checkedListBox2.CheckedIndices) {
                checkedListBox2.SetItemCheckState(elem, CheckState.Unchecked);
            }
            foreach (int elem in checkedListBox3.CheckedIndices) {
                checkedListBox3.SetItemCheckState(elem, CheckState.Unchecked);
            }
            foreach (int elem in checkedListBox4.CheckedIndices) {
                checkedListBox4.SetItemCheckState(elem, CheckState.Unchecked);
            }
            checkedListBox1.ClearSelected();
            checkedListBox2.ClearSelected();
            checkedListBox3.ClearSelected();
            checkedListBox4.ClearSelected();
        }
        
        private void ClearInButton_Click(object sender, EventArgs e)
        {
            InputTextBox.Clear();
        }

        private void ClearOutButton_Click(object sender, EventArgs e)
        {
            OutputTextBox.Clear();
        }
#endregion

#region callback Windows
        private void TacItem_Click(object sender, EventArgs e)
        {
            AdditionalWindow tacWindow = new AdditionalWindow("TAC Window");
            tacWindow.Show();
        }

        private void CfgItem_Click(object sender, EventArgs e)
        {
            AdditionalWindow cfgWindow = new AdditionalWindow("CFG Window");
            cfgWindow.Show();
        }
#endregion

#region Item Check
        private void OptimizationsByAstTree_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) {
                checkedOptimizationsBlock1.Add((OptimizationsByAstTree)e.Index);
            } else if (e.NewValue == CheckState.Unchecked) {
                checkedOptimizationsBlock1.Remove((OptimizationsByAstTree)e.Index);
            }
        }

        private void OptimizationsByBasicBlocks_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) {
                checkedOptimizationsBlock2.Add((OptimizationsByBasicBlocks)e.Index);
            } else if (e.NewValue == CheckState.Unchecked) {
                checkedOptimizationsBlock2.Remove((OptimizationsByBasicBlocks)e.Index);
            }
        }

        private void OptimizationsByIterationAlgorithm_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) {
                checkedOptimizationsBlock3.Add((OptimizationsByIterationAlgorithm)e.Index);
            } else if (e.NewValue == CheckState.Unchecked) {
                checkedOptimizationsBlock3.Remove((OptimizationsByIterationAlgorithm)e.Index);
            }
        }

        private void OptimizationsByControlFlowGraph_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) {
                checkedOptimizationsBlock4.Add((OptimizationsByControlFlowGraph)e.Index);
            } else if (e.NewValue == CheckState.Unchecked) {
                checkedOptimizationsBlock4.Remove((OptimizationsByControlFlowGraph)e.Index);
            }
        }
#endregion

    }
}
