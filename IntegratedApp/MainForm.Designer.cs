namespace IntegratedApp
{
    partial class IntegratedApp
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InfoStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkedListBox4 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox3 = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ClearOutButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.ClearInButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.TacItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CfgItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BasicBlocksItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.genBKillBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defUseДляББлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.OutputTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.InputTextBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(879, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadStripMenuItem,
            this.SaveStripMenuItem,
            this.InfoStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
            this.toolStripMenuItem1.Text = "Файл";
            // 
            // LoadStripMenuItem
            // 
            this.LoadStripMenuItem.Name = "LoadStripMenuItem";
            this.LoadStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.LoadStripMenuItem.Text = "Загрузить";
            this.LoadStripMenuItem.Click += new System.EventHandler(this.LoadStripMenuItem_Click);
            // 
            // SaveStripMenuItem
            // 
            this.SaveStripMenuItem.Name = "SaveStripMenuItem";
            this.SaveStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.SaveStripMenuItem.Text = "Сохранить";
            this.SaveStripMenuItem.Click += new System.EventHandler(this.SaveStripMenuItem_Click);
            // 
            // InfoStripMenuItem
            // 
            this.InfoStripMenuItem.Name = "InfoStripMenuItem";
            this.InfoStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.InfoStripMenuItem.Text = "Справка";
            this.InfoStripMenuItem.Click += new System.EventHandler(this.InfoStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 373);
            this.panel1.TabIndex = 11;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox3.Location = new System.Drawing.Point(187, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(519, 373);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Оптимизации";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.checkedListBox4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkedListBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkedListBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkedListBox3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(513, 354);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // checkedListBox4
            // 
            this.checkedListBox4.CheckOnClick = true;
            this.checkedListBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox4.FormattingEnabled = true;
            this.checkedListBox4.HorizontalScrollbar = true;
            this.checkedListBox4.Items.AddRange(new object[] {
            "Дерево доминаторов",
            "Классификация ребер в CFG",
            "Определение глубины в CFG",
            "Определение того, явл. ли CFG приводимым",
            "Определение всех естественных циклов в CFG с информ. об их вложенности",
            "Построение глубинного остовного дерева с соотв. нумерацией вершин"});
            this.checkedListBox4.Location = new System.Drawing.Point(259, 180);
            this.checkedListBox4.Name = "checkedListBox4";
            this.checkedListBox4.Size = new System.Drawing.Size(251, 171);
            this.checkedListBox4.TabIndex = 3;
            this.checkedListBox4.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OptimizationsByControlFlowGraph_ItemCheck);
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.CheckOnClick = true;
            this.checkedListBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.HorizontalScrollbar = true;
            this.checkedListBox2.Items.AddRange(new object[] {
            "Удаление мертвого кода на основе ИТА для активных переменных",
            "Протяжка const на основе ИТА для достигающих переменных",
            "Провести оптимизации на основе анализа доступных выражений",
            "Распространение const на основе ИТА"});
            this.checkedListBox2.Location = new System.Drawing.Point(3, 180);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(250, 171);
            this.checkedListBox2.TabIndex = 4;
            this.checkedListBox2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OptimizationsByIterationAlgorithm_ItemCheck);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "1) 1 * ex, ex * 1, ex / 1 => ex",
            "2) 0 * ex, ex * 0 => 0",
            "3) 2 * 3 => 6",
            "4) 0 + exp, exp + 0 => exp",
            "5) a - a => 0",
            "6) 2 < 3 => true",
            "9) a > a, a != a => false",
            "10) x = x => null",
            "11) if (true) st1; else st2; => st1",
            "12) if (false) st1; else st2; => st2",
            "13) if (ex) null; else null; => null",
            "14) while (false) st; => null"});
            this.checkedListBox1.Location = new System.Drawing.Point(3, 3);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(250, 171);
            this.checkedListBox1.TabIndex = 3;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OptimizationsByAstTree_ItemCheck);
            // 
            // checkedListBox3
            // 
            this.checkedListBox3.CheckOnClick = true;
            this.checkedListBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox3.FormattingEnabled = true;
            this.checkedListBox3.HorizontalScrollbar = true;
            this.checkedListBox3.Items.AddRange(new object[] {
            "Протяжка const в пределах ББл на основе Def-Use",
            "Протяжка копий в пределах ББл на основе Def-Use",
            "Свертка const",
            "Алгебраические тождества",
            "Логические тождества",
            "Оптимизация общих подвыражений",
            "Очистка от пустых опр-ов ",
            "Устранение переходов через переходы",
            "Устранение недостижимого кода",
            "Устранение переходов к переходам",
            "LVN - алгоритм"});
            this.checkedListBox3.Location = new System.Drawing.Point(259, 3);
            this.checkedListBox3.Name = "checkedListBox3";
            this.checkedListBox3.ScrollAlwaysVisible = true;
            this.checkedListBox3.Size = new System.Drawing.Size(251, 171);
            this.checkedListBox3.TabIndex = 3;
            this.checkedListBox3.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OptimizationsByBasicBlocks_ItemCheck);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.menuStrip2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(184, 373);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Меню";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ClearOutButton);
            this.panel2.Controls.Add(this.ResetButton);
            this.panel2.Controls.Add(this.ClearInButton);
            this.panel2.Controls.Add(this.RunButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 312);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(178, 58);
            this.panel2.TabIndex = 3;
            // 
            // ClearOutButton
            // 
            this.ClearOutButton.Location = new System.Drawing.Point(89, 26);
            this.ClearOutButton.Margin = new System.Windows.Forms.Padding(2);
            this.ClearOutButton.Name = "ClearOutButton";
            this.ClearOutButton.Size = new System.Drawing.Size(83, 23);
            this.ClearOutButton.TabIndex = 5;
            this.ClearOutButton.Text = "Очистить Out";
            this.ClearOutButton.UseVisualStyleBackColor = true;
            this.ClearOutButton.Click += new System.EventHandler(this.ClearOutButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(2, 26);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(2);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(83, 23);
            this.ResetButton.TabIndex = 4;
            this.ResetButton.Text = "Сбросить";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // ClearInButton
            // 
            this.ClearInButton.Location = new System.Drawing.Point(89, 3);
            this.ClearInButton.Margin = new System.Windows.Forms.Padding(2);
            this.ClearInButton.Name = "ClearInButton";
            this.ClearInButton.Size = new System.Drawing.Size(83, 23);
            this.ClearInButton.TabIndex = 2;
            this.ClearInButton.Text = "Очистить In";
            this.ClearInButton.UseVisualStyleBackColor = true;
            this.ClearInButton.Click += new System.EventHandler(this.ClearInButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(3, 3);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(83, 23);
            this.RunButton.TabIndex = 3;
            this.RunButton.Text = "Запуск";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TacItem,
            this.CfgItem,
            this.BasicBlocksItem,
            this.inoutToolStripMenuItem,
            this.genBKillBToolStripMenuItem,
            this.defUseToolStripMenuItem,
            this.defUseДляББлToolStripMenuItem});
            this.menuStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.menuStrip2.Location = new System.Drawing.Point(3, 16);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(178, 354);
            this.menuStrip2.TabIndex = 2;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // TacItem
            // 
            this.TacItem.Name = "TacItem";
            this.TacItem.Size = new System.Drawing.Size(171, 19);
            this.TacItem.Text = "Трехадресный код";
            this.TacItem.Click += new System.EventHandler(this.TacItem_Click);
            // 
            // CfgItem
            // 
            this.CfgItem.Name = "CfgItem";
            this.CfgItem.Size = new System.Drawing.Size(171, 19);
            this.CfgItem.Text = "Control-flow graph";
            this.CfgItem.Click += new System.EventHandler(this.CfgItem_Click);
            // 
            // BasicBlocksItem
            // 
            this.BasicBlocksItem.Name = "BasicBlocksItem";
            this.BasicBlocksItem.Size = new System.Drawing.Size(171, 19);
            this.BasicBlocksItem.Text = "Базовые блоки";
            // 
            // inoutToolStripMenuItem
            // 
            this.inoutToolStripMenuItem.Name = "inoutToolStripMenuItem";
            this.inoutToolStripMenuItem.Size = new System.Drawing.Size(171, 19);
            this.inoutToolStripMenuItem.Text = "In-Out";
            // 
            // genBKillBToolStripMenuItem
            // 
            this.genBKillBToolStripMenuItem.Name = "genBKillBToolStripMenuItem";
            this.genBKillBToolStripMenuItem.Size = new System.Drawing.Size(171, 19);
            this.genBKillBToolStripMenuItem.Text = "Gen_B / Kill_B";
            // 
            // defUseToolStripMenuItem
            // 
            this.defUseToolStripMenuItem.Name = "defUseToolStripMenuItem";
            this.defUseToolStripMenuItem.Size = new System.Drawing.Size(171, 19);
            this.defUseToolStripMenuItem.Text = "Def - Use";
            // 
            // defUseДляББлToolStripMenuItem
            // 
            this.defUseДляББлToolStripMenuItem.Name = "defUseДляББлToolStripMenuItem";
            this.defUseДляББлToolStripMenuItem.Size = new System.Drawing.Size(171, 19);
            this.defUseДляББлToolStripMenuItem.Text = "Def - Use для ББл";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.94798F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.05202F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(706, 24);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(173, 373);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // groupBox4
            // 
            this.groupBox4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox4.Controls.Add(this.OutputTextBox);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(59, 2);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox4.Size = new System.Drawing.Size(112, 369);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Вывод";
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.OutputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputTextBox.Location = new System.Drawing.Point(3, 16);
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.ReadOnly = true;
            this.OutputTextBox.Size = new System.Drawing.Size(106, 350);
            this.OutputTextBox.TabIndex = 1;
            this.OutputTextBox.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.InputTextBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(51, 367);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ввод";
            // 
            // InputTextBox
            // 
            this.InputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputTextBox.Location = new System.Drawing.Point(3, 16);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(45, 348);
            this.InputTextBox.TabIndex = 1;
            this.InputTextBox.Text = "";
            // 
            // IntegratedApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(879, 397);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "IntegratedApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IntegratedApp";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem LoadStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InfoStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem TacItem;
        private System.Windows.Forms.ToolStripMenuItem CfgItem;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckedListBox checkedListBox4;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox3;
        private System.Windows.Forms.Button ClearInButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.ToolStripMenuItem BasicBlocksItem;
        private System.Windows.Forms.ToolStripMenuItem inoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem genBKillBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defUseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defUseДляББлToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox OutputTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox InputTextBox;
        private System.Windows.Forms.Button ClearOutButton;
    }
}

