using SquarifiedTreemapForge.WinForms;

namespace SquarifiedTreemapWinForms
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            numericDispDepth = new NumericUpDown();
            label18 = new Label();
            groupBox6 = new GroupBox();
            label5 = new Label();
            label4 = new Label();
            buttonGroupDown = new Button();
            listBoxGroupSelectable = new ListBox();
            buttonGroupDelete = new Button();
            buttonGroupUp = new Button();
            buttonGroupAdd = new Button();
            listBoxGroupSelected = new ListBox();
            groupBox3 = new GroupBox();
            radioAT = new RadioButton();
            radioRB = new RadioButton();
            radioRT = new RadioButton();
            radioLB = new RadioButton();
            radioLT = new RadioButton();
            groupBox4 = new GroupBox();
            radioD = new RadioButton();
            radioA = new RadioButton();
            numericDepth = new NumericUpDown();
            label1 = new Label();
            groupBox2 = new GroupBox();
            groupBox7 = new GroupBox();
            numericMaxBri = new NumericUpDown();
            numericMinBri = new NumericUpDown();
            numericHueNegative = new NumericUpDown();
            numericHuePositive = new NumericUpDown();
            numericLegendHeight = new NumericUpDown();
            numericSat = new NumericUpDown();
            numericLegendFontSize = new NumericUpDown();
            numericLegendWidth = new NumericUpDown();
            numericExplode = new NumericUpDown();
            numericMaxPer = new NumericUpDown();
            numericMinPer = new NumericUpDown();
            numericLegendSteps = new NumericUpDown();
            label19 = new Label();
            label16 = new Label();
            checkLegendOrder = new CheckBox();
            label15 = new Label();
            checkShowPlusSign = new CheckBox();
            label13 = new Label();
            label11 = new Label();
            label14 = new Label();
            label12 = new Label();
            label9 = new Label();
            label10 = new Label();
            checkShow = new CheckBox();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            textBoxRootName = new TextBox();
            labelRootName = new Label();
            textBoxTitle = new TextBox();
            labelTitle = new Label();
            groupBox5 = new GroupBox();
            buttonSave = new Button();
            numericWidth = new NumericUpDown();
            numericHeight = new NumericUpDown();
            label3 = new Label();
            label2 = new Label();
            splitContainer1 = new SplitContainer();
            panel1 = new Panel();
            label17 = new Label();
            treemapControl1 = new TreemapControl();
            tableLayoutPanel2 = new TableLayoutPanel();
            miniToolStrip = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            statusStrip1 = new StatusStrip();
            toolStripSplitButton1 = new ToolStripSplitButton();
            toolStripOpen = new ToolStripMenuItem();
            toolStripClose = new ToolStripMenuItem();
            toolStripLoad = new ToolStripMenuItem();
            tableLayoutMain = new TableLayoutPanel();
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog1 = new OpenFileDialog();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericDispDepth).BeginInit();
            groupBox6.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericDepth).BeginInit();
            groupBox2.SuspendLayout();
            groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericMaxBri).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericMinBri).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericHueNegative).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericHuePositive).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericLegendHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericSat).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericLegendFontSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericLegendWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericExplode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericMaxPer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericMinPer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericLegendSteps).BeginInit();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel1.SuspendLayout();
            statusStrip1.SuspendLayout();
            tableLayoutMain.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(441, 687);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(numericDispDepth);
            groupBox1.Controls.Add(label18);
            groupBox1.Controls.Add(groupBox6);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox4);
            groupBox1.Controls.Add(numericDepth);
            groupBox1.Controls.Add(label1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(435, 337);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // numericDispDepth
            // 
            numericDispDepth.Location = new Point(229, 302);
            numericDispDepth.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            numericDispDepth.Name = "numericDispDepth";
            numericDispDepth.Size = new Size(62, 29);
            numericDispDepth.TabIndex = 4;
            numericDispDepth.TextAlign = HorizontalAlignment.Right;
            numericDispDepth.Value = new decimal(new int[] { 32, 0, 0, 0 });
            numericDispDepth.ValueChanged += numeric_ValueChanged;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(198, 276);
            label18.Name = "label18";
            label18.Size = new Size(93, 23);
            label18.TabIndex = 5;
            label18.Text = "depth limit";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(label5);
            groupBox6.Controls.Add(label4);
            groupBox6.Controls.Add(buttonGroupDown);
            groupBox6.Controls.Add(listBoxGroupSelectable);
            groupBox6.Controls.Add(buttonGroupDelete);
            groupBox6.Controls.Add(buttonGroupUp);
            groupBox6.Controls.Add(buttonGroupAdd);
            groupBox6.Controls.Add(listBoxGroupSelected);
            groupBox6.Location = new Point(7, 17);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(282, 257);
            groupBox6.TabIndex = 1;
            groupBox6.TabStop = false;
            groupBox6.Text = "aggregation columns";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(191, 26);
            label5.Name = "label5";
            label5.Size = new Size(85, 23);
            label5.TabIndex = 8;
            label5.Text = "selectable";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(32, 26);
            label4.Name = "label4";
            label4.Size = new Size(72, 23);
            label4.TabIndex = 7;
            label4.Text = "selected";
            // 
            // buttonGroupDown
            // 
            buttonGroupDown.Location = new Point(64, 226);
            buttonGroupDown.Name = "buttonGroupDown";
            buttonGroupDown.Size = new Size(48, 28);
            buttonGroupDown.TabIndex = 6;
            buttonGroupDown.Text = "down";
            buttonGroupDown.UseVisualStyleBackColor = true;
            buttonGroupDown.Click += buttonGroupDown_Click;
            // 
            // listBoxGroupSelectable
            // 
            listBoxGroupSelectable.FormattingEnabled = true;
            listBoxGroupSelectable.ItemHeight = 21;
            listBoxGroupSelectable.Items.AddRange(new object[] { "***", "***", "***" });
            listBoxGroupSelectable.Location = new Point(167, 46);
            listBoxGroupSelectable.Name = "listBoxGroupSelectable";
            listBoxGroupSelectable.Size = new Size(100, 172);
            listBoxGroupSelectable.TabIndex = 4;
            // 
            // buttonGroupDelete
            // 
            buttonGroupDelete.Location = new Point(120, 147);
            buttonGroupDelete.Name = "buttonGroupDelete";
            buttonGroupDelete.Size = new Size(41, 51);
            buttonGroupDelete.TabIndex = 3;
            buttonGroupDelete.Text = ">>>";
            buttonGroupDelete.UseVisualStyleBackColor = true;
            buttonGroupDelete.Click += buttonGroupDelete_Click;
            // 
            // buttonGroupUp
            // 
            buttonGroupUp.Location = new Point(16, 226);
            buttonGroupUp.Name = "buttonGroupUp";
            buttonGroupUp.Size = new Size(48, 28);
            buttonGroupUp.TabIndex = 5;
            buttonGroupUp.Text = "up";
            buttonGroupUp.UseVisualStyleBackColor = true;
            buttonGroupUp.Click += buttonGroupUp_Click;
            // 
            // buttonGroupAdd
            // 
            buttonGroupAdd.Location = new Point(120, 66);
            buttonGroupAdd.Name = "buttonGroupAdd";
            buttonGroupAdd.Size = new Size(41, 51);
            buttonGroupAdd.TabIndex = 2;
            buttonGroupAdd.Text = "<<<";
            buttonGroupAdd.UseVisualStyleBackColor = true;
            buttonGroupAdd.Click += buttonGroupAdd_Click;
            // 
            // listBoxGroupSelected
            // 
            listBoxGroupSelected.FormattingEnabled = true;
            listBoxGroupSelected.ItemHeight = 21;
            listBoxGroupSelected.Items.AddRange(new object[] { "***", "***", "***" });
            listBoxGroupSelected.Location = new Point(15, 46);
            listBoxGroupSelected.Name = "listBoxGroupSelected";
            listBoxGroupSelected.Size = new Size(100, 172);
            listBoxGroupSelected.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(radioAT);
            groupBox3.Controls.Add(radioRB);
            groupBox3.Controls.Add(radioRT);
            groupBox3.Controls.Add(radioLB);
            groupBox3.Controls.Add(radioLT);
            groupBox3.Location = new Point(296, 17);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(151, 201);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "node layout algorithm";
            // 
            // radioAT
            // 
            radioAT.AutoSize = true;
            radioAT.Location = new Point(22, 161);
            radioAT.Name = "radioAT";
            radioAT.Size = new Size(99, 27);
            radioAT.TabIndex = 4;
            radioAT.TabStop = true;
            radioAT.Text = "alternate";
            radioAT.UseVisualStyleBackColor = true;
            radioAT.CheckedChanged += radio_CheckedChanged;
            // 
            // radioRB
            // 
            radioRB.AutoSize = true;
            radioRB.Location = new Point(22, 127);
            radioRB.Name = "radioRB";
            radioRB.Size = new Size(129, 27);
            radioRB.TabIndex = 3;
            radioRB.TabStop = true;
            radioRB.Text = "right bottom";
            radioRB.UseVisualStyleBackColor = true;
            radioRB.CheckedChanged += radio_CheckedChanged;
            // 
            // radioRT
            // 
            radioRT.AutoSize = true;
            radioRT.Location = new Point(22, 93);
            radioRT.Name = "radioRT";
            radioRT.Size = new Size(98, 27);
            radioRT.TabIndex = 2;
            radioRT.TabStop = true;
            radioRT.Text = "right top";
            radioRT.UseVisualStyleBackColor = true;
            radioRT.CheckedChanged += radio_CheckedChanged;
            // 
            // radioLB
            // 
            radioLB.AutoSize = true;
            radioLB.Location = new Point(22, 59);
            radioLB.Name = "radioLB";
            radioLB.Size = new Size(117, 27);
            radioLB.TabIndex = 1;
            radioLB.TabStop = true;
            radioLB.Text = "left bottom";
            radioLB.UseVisualStyleBackColor = true;
            radioLB.CheckedChanged += radio_CheckedChanged;
            // 
            // radioLT
            // 
            radioLT.AutoSize = true;
            radioLT.Location = new Point(22, 25);
            radioLT.Name = "radioLT";
            radioLT.Size = new Size(86, 27);
            radioLT.TabIndex = 0;
            radioLT.TabStop = true;
            radioLT.Text = "left top";
            radioLT.UseVisualStyleBackColor = true;
            radioLT.CheckedChanged += radio_CheckedChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(radioD);
            groupBox4.Controls.Add(radioA);
            groupBox4.Location = new Point(296, 232);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(139, 86);
            groupBox4.TabIndex = 1;
            groupBox4.TabStop = false;
            groupBox4.Text = "sort";
            // 
            // radioD
            // 
            radioD.AutoSize = true;
            radioD.Location = new Point(22, 53);
            radioD.Name = "radioD";
            radioD.Size = new Size(123, 27);
            radioD.TabIndex = 2;
            radioD.TabStop = true;
            radioD.Text = "descending ";
            radioD.UseVisualStyleBackColor = true;
            radioD.CheckedChanged += radio_CheckedChanged;
            // 
            // radioA
            // 
            radioA.AutoSize = true;
            radioA.Location = new Point(22, 26);
            radioA.Name = "radioA";
            radioA.Size = new Size(113, 27);
            radioA.TabIndex = 1;
            radioA.TabStop = true;
            radioA.Text = "ascending ";
            radioA.UseVisualStyleBackColor = true;
            radioA.CheckedChanged += radio_CheckedChanged;
            // 
            // numericDepth
            // 
            numericDepth.Location = new Point(131, 302);
            numericDepth.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            numericDepth.Name = "numericDepth";
            numericDepth.Size = new Size(62, 29);
            numericDepth.TabIndex = 2;
            numericDepth.TextAlign = HorizontalAlignment.Right;
            numericDepth.Value = new decimal(new int[] { 32, 0, 0, 0 });
            numericDepth.ValueChanged += numeric_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(98, 276);
            label1.Name = "label1";
            label1.Size = new Size(93, 23);
            label1.TabIndex = 3;
            label1.Text = "depth limit";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox7);
            groupBox2.Controls.Add(textBoxRootName);
            groupBox2.Controls.Add(labelRootName);
            groupBox2.Controls.Add(textBoxTitle);
            groupBox2.Controls.Add(labelTitle);
            groupBox2.Controls.Add(groupBox5);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 346);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(435, 338);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(numericMaxBri);
            groupBox7.Controls.Add(numericMinBri);
            groupBox7.Controls.Add(numericHueNegative);
            groupBox7.Controls.Add(numericHuePositive);
            groupBox7.Controls.Add(numericLegendHeight);
            groupBox7.Controls.Add(numericSat);
            groupBox7.Controls.Add(numericLegendFontSize);
            groupBox7.Controls.Add(numericLegendWidth);
            groupBox7.Controls.Add(numericExplode);
            groupBox7.Controls.Add(numericMaxPer);
            groupBox7.Controls.Add(numericMinPer);
            groupBox7.Controls.Add(numericLegendSteps);
            groupBox7.Controls.Add(label19);
            groupBox7.Controls.Add(label16);
            groupBox7.Controls.Add(checkLegendOrder);
            groupBox7.Controls.Add(label15);
            groupBox7.Controls.Add(checkShowPlusSign);
            groupBox7.Controls.Add(label13);
            groupBox7.Controls.Add(label11);
            groupBox7.Controls.Add(label14);
            groupBox7.Controls.Add(label12);
            groupBox7.Controls.Add(label9);
            groupBox7.Controls.Add(label10);
            groupBox7.Controls.Add(checkShow);
            groupBox7.Controls.Add(label8);
            groupBox7.Controls.Add(label7);
            groupBox7.Controls.Add(label6);
            groupBox7.Location = new Point(7, 76);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(450, 178);
            groupBox7.TabIndex = 6;
            groupBox7.TabStop = false;
            groupBox7.Text = "legend";
            // 
            // numericMaxBri
            // 
            numericMaxBri.Location = new Point(229, 144);
            numericMaxBri.Name = "numericMaxBri";
            numericMaxBri.Size = new Size(55, 29);
            numericMaxBri.TabIndex = 24;
            numericMaxBri.TextAlign = HorizontalAlignment.Right;
            numericMaxBri.Value = new decimal(new int[] { 88, 0, 0, 0 });
            numericMaxBri.ValueChanged += numeric_ValueChanged;
            // 
            // numericMinBri
            // 
            numericMinBri.Location = new Point(229, 114);
            numericMinBri.Name = "numericMinBri";
            numericMinBri.Size = new Size(55, 29);
            numericMinBri.TabIndex = 22;
            numericMinBri.TextAlign = HorizontalAlignment.Right;
            numericMinBri.Value = new decimal(new int[] { 8, 0, 0, 0 });
            numericMinBri.ValueChanged += numeric_ValueChanged;
            // 
            // numericHueNegative
            // 
            numericHueNegative.Location = new Point(229, 82);
            numericHueNegative.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            numericHueNegative.Name = "numericHueNegative";
            numericHueNegative.Size = new Size(55, 29);
            numericHueNegative.TabIndex = 19;
            numericHueNegative.TextAlign = HorizontalAlignment.Right;
            numericHueNegative.Value = new decimal(new int[] { 205, 0, 0, 0 });
            numericHueNegative.ValueChanged += numeric_ValueChanged;
            // 
            // numericHuePositive
            // 
            numericHuePositive.Location = new Point(229, 51);
            numericHuePositive.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            numericHuePositive.Name = "numericHuePositive";
            numericHuePositive.Size = new Size(55, 29);
            numericHuePositive.TabIndex = 17;
            numericHuePositive.TextAlign = HorizontalAlignment.Right;
            numericHuePositive.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numericHuePositive.ValueChanged += numeric_ValueChanged;
            // 
            // numericLegendHeight
            // 
            numericLegendHeight.Location = new Point(363, 144);
            numericLegendHeight.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numericLegendHeight.Name = "numericLegendHeight";
            numericLegendHeight.Size = new Size(55, 29);
            numericLegendHeight.TabIndex = 29;
            numericLegendHeight.TextAlign = HorizontalAlignment.Right;
            numericLegendHeight.Value = new decimal(new int[] { 22, 0, 0, 0 });
            numericLegendHeight.ValueChanged += numeric_ValueChanged;
            // 
            // numericSat
            // 
            numericSat.Location = new Point(363, 82);
            numericSat.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            numericSat.Name = "numericSat";
            numericSat.Size = new Size(55, 29);
            numericSat.TabIndex = 26;
            numericSat.TextAlign = HorizontalAlignment.Right;
            numericSat.Value = new decimal(new int[] { 205, 0, 0, 0 });
            numericSat.ValueChanged += numeric_ValueChanged;
            // 
            // numericLegendFontSize
            // 
            numericLegendFontSize.DecimalPlaces = 1;
            numericLegendFontSize.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericLegendFontSize.Location = new Point(363, 51);
            numericLegendFontSize.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericLegendFontSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericLegendFontSize.Name = "numericLegendFontSize";
            numericLegendFontSize.Size = new Size(55, 29);
            numericLegendFontSize.TabIndex = 9;
            numericLegendFontSize.TextAlign = HorizontalAlignment.Right;
            numericLegendFontSize.Value = new decimal(new int[] { 7, 0, 0, 0 });
            numericLegendFontSize.ValueChanged += numeric_ValueChanged;
            // 
            // numericLegendWidth
            // 
            numericLegendWidth.Location = new Point(363, 114);
            numericLegendWidth.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numericLegendWidth.Name = "numericLegendWidth";
            numericLegendWidth.Size = new Size(55, 29);
            numericLegendWidth.TabIndex = 7;
            numericLegendWidth.TextAlign = HorizontalAlignment.Right;
            numericLegendWidth.Value = new decimal(new int[] { 250, 0, 0, 0 });
            numericLegendWidth.ValueChanged += numeric_ValueChanged;
            // 
            // numericExplode
            // 
            numericExplode.Location = new Point(66, 82);
            numericExplode.Maximum = new decimal(new int[] { 32, 0, 0, 0 });
            numericExplode.Name = "numericExplode";
            numericExplode.Size = new Size(55, 29);
            numericExplode.TabIndex = 31;
            numericExplode.TextAlign = HorizontalAlignment.Right;
            numericExplode.ValueChanged += numeric_ValueChanged;
            // 
            // numericMaxPer
            // 
            numericMaxPer.Location = new Point(66, 144);
            numericMaxPer.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            numericMaxPer.Name = "numericMaxPer";
            numericMaxPer.Size = new Size(55, 29);
            numericMaxPer.TabIndex = 15;
            numericMaxPer.TextAlign = HorizontalAlignment.Right;
            numericMaxPer.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numericMaxPer.ValueChanged += numeric_ValueChanged;
            // 
            // numericMinPer
            // 
            numericMinPer.Location = new Point(66, 114);
            numericMinPer.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            numericMinPer.Name = "numericMinPer";
            numericMinPer.Size = new Size(55, 29);
            numericMinPer.TabIndex = 13;
            numericMinPer.TextAlign = HorizontalAlignment.Right;
            numericMinPer.Value = new decimal(new int[] { 10, 0, 0, int.MinValue });
            numericMinPer.ValueChanged += numeric_ValueChanged;
            // 
            // numericLegendSteps
            // 
            numericLegendSteps.Location = new Point(66, 51);
            numericLegendSteps.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
            numericLegendSteps.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            numericLegendSteps.Name = "numericLegendSteps";
            numericLegendSteps.Size = new Size(55, 29);
            numericLegendSteps.TabIndex = 4;
            numericLegendSteps.TextAlign = HorizontalAlignment.Right;
            numericLegendSteps.Value = new decimal(new int[] { 16, 0, 0, 0 });
            numericLegendSteps.ValueChanged += numeric_ValueChanged;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(-4, 86);
            label19.Name = "label19";
            label19.Size = new Size(70, 23);
            label19.TabIndex = 32;
            label19.Text = "explode";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(305, 148);
            label16.Name = "label16";
            label16.Size = new Size(59, 23);
            label16.TabIndex = 30;
            label16.Text = "height";
            // 
            // checkLegendOrder
            // 
            checkLegendOrder.AutoSize = true;
            checkLegendOrder.Location = new Point(123, 24);
            checkLegendOrder.Name = "checkLegendOrder";
            checkLegendOrder.Size = new Size(159, 27);
            checkLegendOrder.TabIndex = 28;
            checkLegendOrder.Text = "legend order asc";
            checkLegendOrder.UseVisualStyleBackColor = true;
            checkLegendOrder.CheckedChanged += check_CheckedChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(283, 86);
            label15.Name = "label15";
            label15.Size = new Size(87, 23);
            label15.TabIndex = 27;
            label15.Text = "saturation";
            // 
            // checkShowPlusSign
            // 
            checkShowPlusSign.AutoSize = true;
            checkShowPlusSign.Location = new Point(252, 24);
            checkShowPlusSign.Name = "checkShowPlusSign";
            checkShowPlusSign.Size = new Size(143, 27);
            checkShowPlusSign.TabIndex = 21;
            checkShowPlusSign.Text = "show plus sign";
            checkShowPlusSign.UseVisualStyleBackColor = true;
            checkShowPlusSign.CheckedChanged += check_CheckedChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(124, 148);
            label13.Name = "label13";
            label13.Size = new Size(126, 23);
            label13.TabIndex = 25;
            label13.Text = "max brightness";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(124, 86);
            label11.Name = "label11";
            label11.Size = new Size(109, 23);
            label11.TabIndex = 20;
            label11.Text = "hue negative";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(124, 118);
            label14.Name = "label14";
            label14.Size = new Size(123, 23);
            label14.TabIndex = 23;
            label14.Text = "min brightness";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(124, 55);
            label12.Name = "label12";
            label12.Size = new Size(102, 23);
            label12.TabIndex = 18;
            label12.Text = "hue positive";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(-4, 148);
            label9.Name = "label9";
            label9.Size = new Size(72, 23);
            label9.TabIndex = 16;
            label9.Text = "max per";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(-1, 118);
            label10.Name = "label10";
            label10.Size = new Size(69, 23);
            label10.TabIndex = 14;
            label10.Text = "min per";
            // 
            // checkShow
            // 
            checkShow.AutoSize = true;
            checkShow.Location = new Point(20, 24);
            checkShow.Name = "checkShow";
            checkShow.Size = new Size(128, 27);
            checkShow.TabIndex = 11;
            checkShow.Text = "show legend";
            checkShow.UseVisualStyleBackColor = true;
            checkShow.CheckedChanged += check_CheckedChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(296, 55);
            label8.Name = "label8";
            label8.Size = new Size(69, 23);
            label8.TabIndex = 10;
            label8.Text = "fontsize";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(305, 118);
            label7.Name = "label7";
            label7.Size = new Size(52, 23);
            label7.TabIndex = 8;
            label7.Text = "width";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(19, 55);
            label6.Name = "label6";
            label6.Size = new Size(49, 23);
            label6.TabIndex = 5;
            label6.Text = "steps";
            // 
            // textBoxRootName
            // 
            textBoxRootName.Location = new Point(82, 45);
            textBoxRootName.Name = "textBoxRootName";
            textBoxRootName.Size = new Size(348, 29);
            textBoxRootName.TabIndex = 5;
            // 
            // labelRootName
            // 
            labelRootName.AutoSize = true;
            labelRootName.Location = new Point(5, 49);
            labelRootName.Name = "labelRootName";
            labelRootName.Size = new Size(92, 23);
            labelRootName.TabIndex = 4;
            labelRootName.Text = "RootName";
            // 
            // textBoxTitle
            // 
            textBoxTitle.Location = new Point(82, 18);
            textBoxTitle.Name = "textBoxTitle";
            textBoxTitle.Size = new Size(348, 29);
            textBoxTitle.TabIndex = 1;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Location = new Point(44, 21);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(42, 23);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Title";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(buttonSave);
            groupBox5.Controls.Add(numericWidth);
            groupBox5.Controls.Add(numericHeight);
            groupBox5.Controls.Add(label3);
            groupBox5.Controls.Add(label2);
            groupBox5.Location = new Point(7, 273);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(321, 60);
            groupBox5.TabIndex = 0;
            groupBox5.TabStop = false;
            groupBox5.Text = "save image";
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(233, 18);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(82, 36);
            buttonSave.TabIndex = 8;
            buttonSave.Text = "save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // numericWidth
            // 
            numericWidth.Location = new Point(62, 22);
            numericWidth.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numericWidth.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numericWidth.Name = "numericWidth";
            numericWidth.Size = new Size(50, 29);
            numericWidth.TabIndex = 4;
            numericWidth.TextAlign = HorizontalAlignment.Right;
            numericWidth.Value = new decimal(new int[] { 3840, 0, 0, 0 });
            // 
            // numericHeight
            // 
            numericHeight.Location = new Point(179, 22);
            numericHeight.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numericHeight.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numericHeight.Name = "numericHeight";
            numericHeight.Size = new Size(50, 29);
            numericHeight.TabIndex = 6;
            numericHeight.TextAlign = HorizontalAlignment.Right;
            numericHeight.Value = new decimal(new int[] { 2160, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(116, 25);
            label3.Name = "label3";
            label3.Size = new Size(59, 23);
            label3.TabIndex = 7;
            label3.Text = "height";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 25);
            label2.Name = "label2";
            label2.Size = new Size(52, 23);
            label2.TabIndex = 5;
            label2.Text = "width";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel1);
            splitContainer1.Panel1.Controls.Add(treemapControl1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel1);
            splitContainer1.Size = new Size(1002, 687);
            splitContainer1.SplitterDistance = 553;
            splitContainer1.SplitterWidth = 8;
            splitContainer1.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.Controls.Add(label17);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(553, 687);
            panel1.TabIndex = 4;
            // 
            // label17
            // 
            label17.Dock = DockStyle.Fill;
            label17.Location = new Point(0, 0);
            label17.Name = "label17";
            label17.Size = new Size(553, 687);
            label17.TabIndex = 0;
            label17.Text = "Specify the original data json file from the menu in the lower left corner.";
            label17.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // treemapControl1
            // 
            treemapControl1.Dock = DockStyle.Fill;
            treemapControl1.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            treemapControl1.Location = new Point(0, 0);
            treemapControl1.Margin = new Padding(0);
            treemapControl1.Name = "treemapControl1";
            treemapControl1.Size = new Size(553, 687);
            treemapControl1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.Size = new Size(558, 193);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // miniToolStrip
            // 
            miniToolStrip.AccessibleName = "新しい項目の選択";
            miniToolStrip.AccessibleRole = AccessibleRole.ButtonDropDown;
            miniToolStrip.AutoSize = false;
            miniToolStrip.Dock = DockStyle.None;
            miniToolStrip.GripMargin = new Padding(0);
            miniToolStrip.ImageScalingSize = new Size(20, 20);
            miniToolStrip.Location = new Point(140, 6);
            miniToolStrip.Name = "miniToolStrip";
            miniToolStrip.Size = new Size(786, 32);
            miniToolStrip.TabIndex = 3;
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            toolStripStatusLabel1.Margin = new Padding(25, 3, 3, 3);
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Padding = new Padding(1);
            toolStripStatusLabel1.Size = new Size(173, 30);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // statusStrip1
            // 
            statusStrip1.Dock = DockStyle.Fill;
            statusStrip1.GripMargin = new Padding(0);
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripSplitButton1, toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 693);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1008, 36);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.Alignment = ToolStripItemAlignment.Right;
            toolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripSplitButton1.DropDownItems.AddRange(new ToolStripItem[] { toolStripOpen, toolStripClose, toolStripLoad });
            toolStripSplitButton1.Image = (Image)resources.GetObject("toolStripSplitButton1.Image");
            toolStripSplitButton1.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Padding = new Padding(8, 0, 8, 0);
            toolStripSplitButton1.Size = new Size(81, 34);
            toolStripSplitButton1.Text = "Menu";
            // 
            // toolStripOpen
            // 
            toolStripOpen.Name = "toolStripOpen";
            toolStripOpen.Size = new Size(214, 26);
            toolStripOpen.Text = "Open Setting Area";
            toolStripOpen.Click += toolStripOpen_Click;
            // 
            // toolStripClose
            // 
            toolStripClose.Name = "toolStripClose";
            toolStripClose.Size = new Size(214, 26);
            toolStripClose.Text = "Close Setting Area";
            toolStripClose.Click += toolStripClose_Click;
            // 
            // toolStripLoad
            // 
            toolStripLoad.Name = "toolStripLoad";
            toolStripLoad.Size = new Size(214, 26);
            toolStripLoad.Text = "Load Json";
            toolStripLoad.Click += toolStripLoad_Click;
            // 
            // tableLayoutMain
            // 
            tableLayoutMain.ColumnCount = 1;
            tableLayoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutMain.Controls.Add(statusStrip1, 0, 1);
            tableLayoutMain.Controls.Add(splitContainer1, 0, 0);
            tableLayoutMain.Dock = DockStyle.Fill;
            tableLayoutMain.Location = new Point(0, 0);
            tableLayoutMain.Name = "tableLayoutMain";
            tableLayoutMain.RowCount = 2;
            tableLayoutMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutMain.Size = new Size(1008, 729);
            tableLayoutMain.TabIndex = 4;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 729);
            Controls.Add(tableLayoutMain);
            Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SquarifiedTreemapLayers";
            Shown += Form1_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericDispDepth).EndInit();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericDepth).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericMaxBri).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericMinBri).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericHueNegative).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericHuePositive).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericLegendHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericSat).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericLegendFontSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericLegendWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericExplode).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericMaxPer).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericMinPer).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericLegendSteps).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericHeight).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tableLayoutMain.ResumeLayout(false);
            tableLayoutMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private RadioButton radioLB;
        private RadioButton radioLT;
        private RadioButton radioAT;
        private RadioButton radioRB;
        private RadioButton radioRT;
        private GroupBox groupBox4;
        private RadioButton radioD;
        private RadioButton radioA;
        private Label label1;
        private NumericUpDown numericDepth;
        private SplitContainer splitContainer1;
        private TreemapControl treemapControl1;
        private TableLayoutPanel tableLayoutPanel2;
        private StatusStrip miniToolStrip;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private StatusStrip statusStrip1;
        private ToolStripSplitButton toolStripSplitButton1;
        private ToolStripMenuItem toolStripOpen;
        private ToolStripMenuItem toolStripClose;
        private TableLayoutPanel tableLayoutMain;
        private Label label3;
        private NumericUpDown numericHeight;
        private Label label2;
        private NumericUpDown numericWidth;
        private GroupBox groupBox5;
        private Button buttonSave;
        private SaveFileDialog saveFileDialog1;
        private TextBox textBoxRootName;
        private Label labelRootName;
        private TextBox textBoxTitle;
        private Label labelTitle;
        private GroupBox groupBox6;
        private Button buttonGroupDelete;
        private Button buttonGroupAdd;
        private ListBox listBoxGroupSelected;
        private ListBox listBoxGroupSelectable;
        private Button buttonGroupDown;
        private Button buttonGroupUp;
        private Label label5;
        private Label label4;
        private GroupBox groupBox7;
        private Label label6;
        private NumericUpDown numericLegendSteps;
        private Label label8;
        private NumericUpDown numericLegendFontSize;
        private Label label7;
        private NumericUpDown numericLegendWidth;
        private CheckBox checkShow;
        private NumericUpDown numericMaxPer;
        private NumericUpDown numericMinPer;
        private Label label9;
        private Label label10;
        private NumericUpDown numericHueNegative;
        private NumericUpDown numericHuePositive;
        private Label label11;
        private Label label12;
        private CheckBox checkShowPlusSign;
        private NumericUpDown numericMaxBri;
        private NumericUpDown numericMinBri;
        private Label label13;
        private Label label14;
        private NumericUpDown numericSat;
        private Label label15;
        private CheckBox checkLegendOrder;
        private Label label16;
        private NumericUpDown numericLegendHeight;
        private Panel panel1;
        private Label label17;
        private ToolStripMenuItem toolStripLoad;
        private OpenFileDialog openFileDialog1;
        private NumericUpDown numericDispDepth;
        private Label label18;
        private Label label19;
        private NumericUpDown numericExplode;
    }
}
