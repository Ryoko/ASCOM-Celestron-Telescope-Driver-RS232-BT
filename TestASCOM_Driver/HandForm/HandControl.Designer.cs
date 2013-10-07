namespace ASCOM.CelestronAdvancedBlueTooth.HandForm
{
    partial class HandControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ControlButtons = new System.Windows.Forms.GroupBox();
            this.bMode = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Ra_n = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.Dec_n = new System.Windows.Forms.Button();
            this.Ra_p = new System.Windows.Forms.Button();
            this.Dec_p = new System.Windows.Forms.Button();
            this.RateBar = new System.Windows.Forms.TrackBar();
            this.ConstMove = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.GuideItvl = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.GuideRate = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.GuideW = new System.Windows.Forms.Button();
            this.GuideS = new System.Windows.Forms.Button();
            this.GuideE = new System.Windows.Forms.Button();
            this.GuideN = new System.Windows.Forms.Button();
            this.Coordinates = new System.Windows.Forms.GroupBox();
            this.Mode = new System.Windows.Forms.Label();
            this.lMode = new System.Windows.Forms.Label();
            this.Azm = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Alt = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Dec = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Ra = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bgw = new System.ComponentModel.BackgroundWorker();
            this.ControlButtons.SuspendLayout();
            this.bMode.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RateBar)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GuideItvl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GuideRate)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.Coordinates.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlButtons
            // 
            this.ControlButtons.Controls.Add(this.bMode);
            this.ControlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlButtons.Enabled = false;
            this.ControlButtons.Location = new System.Drawing.Point(0, 122);
            this.ControlButtons.Name = "ControlButtons";
            this.ControlButtons.Size = new System.Drawing.Size(174, 301);
            this.ControlButtons.TabIndex = 6;
            this.ControlButtons.TabStop = false;
            this.ControlButtons.Text = "Controls";
            // 
            // bMode
            // 
            this.bMode.Controls.Add(this.tabPage1);
            this.bMode.Controls.Add(this.tabPage2);
            this.bMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.bMode.Location = new System.Drawing.Point(3, 16);
            this.bMode.Margin = new System.Windows.Forms.Padding(0);
            this.bMode.Name = "bMode";
            this.bMode.Padding = new System.Drawing.Point(0, 0);
            this.bMode.SelectedIndex = 0;
            this.bMode.Size = new System.Drawing.Size(168, 186);
            this.bMode.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.LightGray;
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Controls.Add(this.RateBar);
            this.tabPage1.Controls.Add(this.ConstMove);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(160, 160);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Button";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.Ra_n, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Stop, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Dec_n, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Ra_p, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.Dec_p, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(109, 100);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // Ra_n
            // 
            this.Ra_n.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ra_n.Location = new System.Drawing.Point(0, 33);
            this.Ra_n.Margin = new System.Windows.Forms.Padding(0);
            this.Ra_n.Name = "Ra_n";
            this.Ra_n.Size = new System.Drawing.Size(36, 33);
            this.Ra_n.TabIndex = 2;
            this.Ra_n.Text = "<";
            this.Ra_n.UseVisualStyleBackColor = true;
            this.Ra_n.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.Ra_n.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // Stop
            // 
            this.Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Stop.Location = new System.Drawing.Point(36, 33);
            this.Stop.Margin = new System.Windows.Forms.Padding(0);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(36, 33);
            this.Stop.TabIndex = 5;
            this.Stop.Text = "0";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // Dec_n
            // 
            this.Dec_n.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dec_n.Location = new System.Drawing.Point(36, 66);
            this.Dec_n.Margin = new System.Windows.Forms.Padding(0);
            this.Dec_n.Name = "Dec_n";
            this.Dec_n.Size = new System.Drawing.Size(36, 34);
            this.Dec_n.TabIndex = 1;
            this.Dec_n.Text = "v";
            this.Dec_n.UseVisualStyleBackColor = true;
            this.Dec_n.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.Dec_n.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // Ra_p
            // 
            this.Ra_p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ra_p.Location = new System.Drawing.Point(72, 33);
            this.Ra_p.Margin = new System.Windows.Forms.Padding(0);
            this.Ra_p.Name = "Ra_p";
            this.Ra_p.Size = new System.Drawing.Size(37, 33);
            this.Ra_p.TabIndex = 3;
            this.Ra_p.Text = ">";
            this.Ra_p.UseVisualStyleBackColor = true;
            this.Ra_p.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.Ra_p.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // Dec_p
            // 
            this.Dec_p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dec_p.Location = new System.Drawing.Point(36, 0);
            this.Dec_p.Margin = new System.Windows.Forms.Padding(0);
            this.Dec_p.Name = "Dec_p";
            this.Dec_p.Size = new System.Drawing.Size(36, 33);
            this.Dec_p.TabIndex = 0;
            this.Dec_p.Text = "^";
            this.Dec_p.UseVisualStyleBackColor = true;
            this.Dec_p.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.Dec_p.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // RateBar
            // 
            this.RateBar.Location = new System.Drawing.Point(110, 7);
            this.RateBar.Maximum = 9;
            this.RateBar.Minimum = 1;
            this.RateBar.Name = "RateBar";
            this.RateBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.RateBar.Size = new System.Drawing.Size(45, 100);
            this.RateBar.TabIndex = 4;
            this.RateBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.RateBar.Value = 7;
            // 
            // ConstMove
            // 
            this.ConstMove.AutoSize = true;
            this.ConstMove.Location = new System.Drawing.Point(7, 113);
            this.ConstMove.Name = "ConstMove";
            this.ConstMove.Size = new System.Drawing.Size(75, 17);
            this.ConstMove.TabIndex = 6;
            this.ConstMove.Text = "Constantly";
            this.ConstMove.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.LightGray;
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.GuideItvl);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.GuideRate);
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(160, 160);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Guide";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Itvl (ms.)";
            // 
            // GuideItvl
            // 
            this.GuideItvl.Location = new System.Drawing.Point(56, 137);
            this.GuideItvl.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.GuideItvl.Name = "GuideItvl";
            this.GuideItvl.Size = new System.Drawing.Size(59, 20);
            this.GuideItvl.TabIndex = 11;
            this.GuideItvl.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Rate (%)";
            // 
            // GuideRate
            // 
            this.GuideRate.Location = new System.Drawing.Point(56, 113);
            this.GuideRate.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.GuideRate.Name = "GuideRate";
            this.GuideRate.Size = new System.Drawing.Size(59, 20);
            this.GuideRate.TabIndex = 9;
            this.GuideRate.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.GuideRate.ValueChanged += new System.EventHandler(this.GideRate_ValueChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.GuideW, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.GuideS, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.GuideE, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.GuideN, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(109, 100);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // GuideW
            // 
            this.GuideW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GuideW.Location = new System.Drawing.Point(0, 33);
            this.GuideW.Margin = new System.Windows.Forms.Padding(0);
            this.GuideW.Name = "GuideW";
            this.GuideW.Size = new System.Drawing.Size(36, 33);
            this.GuideW.TabIndex = 2;
            this.GuideW.Text = "W";
            this.GuideW.UseVisualStyleBackColor = true;
            this.GuideW.Click += new System.EventHandler(this.GuideBtn_Click);
            // 
            // GuideS
            // 
            this.GuideS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GuideS.Location = new System.Drawing.Point(36, 66);
            this.GuideS.Margin = new System.Windows.Forms.Padding(0);
            this.GuideS.Name = "GuideS";
            this.GuideS.Size = new System.Drawing.Size(36, 34);
            this.GuideS.TabIndex = 1;
            this.GuideS.Text = "S";
            this.GuideS.UseVisualStyleBackColor = true;
            this.GuideS.Click += new System.EventHandler(this.GuideBtn_Click);
            // 
            // GuideE
            // 
            this.GuideE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GuideE.Location = new System.Drawing.Point(72, 33);
            this.GuideE.Margin = new System.Windows.Forms.Padding(0);
            this.GuideE.Name = "GuideE";
            this.GuideE.Size = new System.Drawing.Size(37, 33);
            this.GuideE.TabIndex = 3;
            this.GuideE.Text = "E";
            this.GuideE.UseVisualStyleBackColor = true;
            this.GuideE.Click += new System.EventHandler(this.GuideBtn_Click);
            // 
            // GuideN
            // 
            this.GuideN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GuideN.Location = new System.Drawing.Point(36, 0);
            this.GuideN.Margin = new System.Windows.Forms.Padding(0);
            this.GuideN.Name = "GuideN";
            this.GuideN.Size = new System.Drawing.Size(36, 33);
            this.GuideN.TabIndex = 0;
            this.GuideN.Text = "N";
            this.GuideN.UseVisualStyleBackColor = true;
            this.GuideN.Click += new System.EventHandler(this.GuideBtn_Click);
            // 
            // Coordinates
            // 
            this.Coordinates.Controls.Add(this.Mode);
            this.Coordinates.Controls.Add(this.lMode);
            this.Coordinates.Controls.Add(this.Azm);
            this.Coordinates.Controls.Add(this.label6);
            this.Coordinates.Controls.Add(this.Alt);
            this.Coordinates.Controls.Add(this.label8);
            this.Coordinates.Controls.Add(this.Dec);
            this.Coordinates.Controls.Add(this.label4);
            this.Coordinates.Controls.Add(this.Ra);
            this.Coordinates.Controls.Add(this.label1);
            this.Coordinates.Dock = System.Windows.Forms.DockStyle.Top;
            this.Coordinates.Location = new System.Drawing.Point(0, 0);
            this.Coordinates.Name = "Coordinates";
            this.Coordinates.Size = new System.Drawing.Size(174, 122);
            this.Coordinates.TabIndex = 5;
            this.Coordinates.TabStop = false;
            this.Coordinates.Text = "Coordinates";
            // 
            // Mode
            // 
            this.Mode.AutoSize = true;
            this.Mode.Location = new System.Drawing.Point(99, 96);
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(51, 13);
            this.Mode.TabIndex = 9;
            this.Mode.Text = "unknown";
            // 
            // lMode
            // 
            this.lMode.AutoSize = true;
            this.lMode.Location = new System.Drawing.Point(8, 96);
            this.lMode.Name = "lMode";
            this.lMode.Size = new System.Drawing.Size(37, 13);
            this.lMode.TabIndex = 8;
            this.lMode.Text = "Mode:";
            // 
            // Azm
            // 
            this.Azm.AutoSize = true;
            this.Azm.Location = new System.Drawing.Point(99, 74);
            this.Azm.Name = "Azm";
            this.Azm.Size = new System.Drawing.Size(58, 13);
            this.Azm.TabIndex = 7;
            this.Azm.Text = "00:00:00.0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Azimuth:";
            // 
            // Alt
            // 
            this.Alt.AutoSize = true;
            this.Alt.Location = new System.Drawing.Point(99, 61);
            this.Alt.Name = "Alt";
            this.Alt.Size = new System.Drawing.Size(58, 13);
            this.Alt.TabIndex = 5;
            this.Alt.Text = "00:00:00.0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Altitude:";
            // 
            // Dec
            // 
            this.Dec.AutoSize = true;
            this.Dec.Location = new System.Drawing.Point(99, 39);
            this.Dec.Name = "Dec";
            this.Dec.Size = new System.Drawing.Size(58, 13);
            this.Dec.TabIndex = 3;
            this.Dec.Text = "00:00:00.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Declination:";
            // 
            // Ra
            // 
            this.Ra.AutoSize = true;
            this.Ra.Location = new System.Drawing.Point(99, 26);
            this.Ra.Name = "Ra";
            this.Ra.Size = new System.Drawing.Size(58, 13);
            this.Ra.TabIndex = 1;
            this.Ra.Text = "00:00:00.0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Right Ascension:";
            // 
            // bgw
            // 
            this.bgw.WorkerReportsProgress = true;
            this.bgw.WorkerSupportsCancellation = true;
            this.bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_DoWork);
            this.bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgw_ProgressChanged);
            this.bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgw_RunWorkerCompleted);
            // 
            // HandControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(174, 435);
            this.Controls.Add(this.ControlButtons);
            this.Controls.Add(this.Coordinates);
            this.Name = "HandControl";
            this.Text = "HandControl";
            this.ControlButtons.ResumeLayout(false);
            this.bMode.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RateBar)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GuideItvl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GuideRate)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.Coordinates.ResumeLayout(false);
            this.Coordinates.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ControlButtons;
        private System.Windows.Forms.Button Ra_p;
        private System.Windows.Forms.Button Ra_n;
        private System.Windows.Forms.Button Dec_n;
        private System.Windows.Forms.Button Dec_p;
        private System.Windows.Forms.GroupBox Coordinates;
        private System.Windows.Forms.Label Azm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label Alt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label Dec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Ra;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Mode;
        private System.Windows.Forms.Label lMode;
        private System.ComponentModel.BackgroundWorker bgw;
        private System.Windows.Forms.TrackBar RateBar;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.CheckBox ConstMove;
        private System.Windows.Forms.TabControl bMode;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown GuideItvl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button GuideW;
        private System.Windows.Forms.Button GuideS;
        private System.Windows.Forms.Button GuideE;
        private System.Windows.Forms.Button GuideN;
        public System.Windows.Forms.NumericUpDown GuideRate;
    }
}