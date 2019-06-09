﻿using System;
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

namespace IntegratedApp
{
    public partial class IntegratedApp : Form
    {
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

            //if (checkedOptimizationsBlock1.Count == 0 && checkedOptimizationsBlock2.Count == 0 && checkedOptimizationsBlock3.Count == 0 && checkedOptimizationsBlock4.Count == 0) {
            //    MessageBox.Show("Не выбрана ни одна оптимизация");
            //    return;
            //}

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
            }
            #endregion

#region Optimizations by Basic blocks
            if (checkedOptimizationsBlock2.Count != 0) {
                var threeAddressCodeVisitor = new ThreeAddressCodeVisitor();
                parser.root.Visit(threeAddressCodeVisitor);
                threeAddressCodeVisitor.Postprocess();
                OutputTextBox.Text += threeAddressCodeVisitor.TACodeContainer.ToString();
                OutputTextBox.Text += "\n";

                int ind, iteration = 0;
                for (ind = 0; ind < checkedOptimizationsBlock2.Count; ind++) {
                    var opt = checkedOptimizationsBlock2[ind];
                    var result = optimizationsBlock2[opt].Optimize(threeAddressCodeVisitor.TACodeContainer);

                    if (result) {
                        ind = -1;
                        continue;
                    }

                    OutputTextBox.Text += string.Format("===== Optimization {0} iteration {1} =====\n\n", optimizationsBlock2[opt].GetType(), iteration++);
                    OutputTextBox.Text += threeAddressCodeVisitor.TACodeContainer.ToString();
                    OutputTextBox.Text += "\n";
                }
            }
#endregion

            }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            InputTextBox.Clear();
            OutputTextBox.Clear();
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

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) {
                checkedOptimizationsBlock1.Add((OptimizationsByAstTree)e.Index);
            } else if (e.NewValue == CheckState.Unchecked) {
                checkedOptimizationsBlock1.Remove((OptimizationsByAstTree)e.Index);
            }
        }

        private void checkedListBox3_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked) {
                checkedOptimizationsBlock2.Add((OptimizationsByBasicBlocks)e.Index);
            } else if (e.NewValue == CheckState.Unchecked) {
                checkedOptimizationsBlock2.Remove((OptimizationsByBasicBlocks)e.Index);
            }
        }
    }
}
