namespace ASCOM.CelestronAdvancedBlueTooth
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SelectedComPort = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectedBluetooth = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CheckScope = new System.Windows.Forms.Button();
            this.TrackingMode = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.HasGPS = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Obstruction = new System.Windows.Forms.NumericUpDown();
            this.Focal = new System.Windows.Forms.NumericUpDown();
            this.Apperture = new System.Windows.Forms.NumericUpDown();
            this.ObstructionLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ScopeSelection = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Elevation = new System.Windows.Forms.NumericUpDown();
            this.Latitude = new System.Windows.Forms.TextBox();
            this.Longitude = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LatSuff = new System.Windows.Forms.ComboBox();
            this.LonSuff = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.ShowHandControl = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Obstruction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Focal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Apperture)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Elevation)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(4, 228);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(4, 256);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.CelestronAdvancedBlueTooth.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(15, 3);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Comm Port";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(133, 16);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(335, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(21, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "..";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(475, 59);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SelectedComPort);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(467, 33);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "COM";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SelectedComPort
            // 
            this.SelectedComPort.FormattingEnabled = true;
            this.SelectedComPort.Location = new System.Drawing.Point(73, 6);
            this.SelectedComPort.Name = "SelectedComPort";
            this.SelectedComPort.Size = new System.Drawing.Size(121, 21);
            this.SelectedComPort.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.SelectedBluetooth);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(467, 33);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "BlueTooth";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Selected device:";
            // 
            // SelectedBluetooth
            // 
            this.SelectedBluetooth.Location = new System.Drawing.Point(101, 6);
            this.SelectedBluetooth.Name = "SelectedBluetooth";
            this.SelectedBluetooth.ReadOnly = true;
            this.SelectedBluetooth.Size = new System.Drawing.Size(228, 20);
            this.SelectedBluetooth.TabIndex = 1;
            this.SelectedBluetooth.Click += new System.EventHandler(this.SelectedBluetooth_Click);
            this.SelectedBluetooth.TextChanged += new System.EventHandler(this.SelectedBluetooth_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CheckScope);
            this.panel1.Controls.Add(this.picASCOM);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(475, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(66, 284);
            this.panel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(475, 225);
            this.panel2.TabIndex = 12;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TrackingMode);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.HasGPS);
            this.groupBox3.Location = new System.Drawing.Point(220, 136);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(247, 86);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Telescope Settings";
            // 
            // CheckScope
            // 
            this.CheckScope.Enabled = false;
            this.CheckScope.Location = new System.Drawing.Point(4, 202);
            this.CheckScope.Name = "CheckScope";
            this.CheckScope.Size = new System.Drawing.Size(59, 23);
            this.CheckScope.TabIndex = 16;
            this.CheckScope.Text = "Check";
            this.CheckScope.UseVisualStyleBackColor = true;
            this.CheckScope.Click += new System.EventHandler(this.CheckScope_Click);
            // 
            // TrackingMode
            // 
            this.TrackingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TrackingMode.FormattingEnabled = true;
            this.TrackingMode.Items.AddRange(new object[] {
            "None",
            "AltAzm",
            "Eq N",
            "Eq S"});
            this.TrackingMode.Location = new System.Drawing.Point(90, 36);
            this.TrackingMode.Name = "TrackingMode";
            this.TrackingMode.Size = new System.Drawing.Size(79, 21);
            this.TrackingMode.TabIndex = 1;
            this.TrackingMode.SelectedIndexChanged += new System.EventHandler(this.Field_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 39);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Tracking Mode";
            // 
            // HasGPS
            // 
            this.HasGPS.AutoSize = true;
            this.HasGPS.Checked = true;
            this.HasGPS.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.HasGPS.Location = new System.Drawing.Point(9, 19);
            this.HasGPS.Name = "HasGPS";
            this.HasGPS.Size = new System.Drawing.Size(68, 17);
            this.HasGPS.TabIndex = 0;
            this.HasGPS.Text = "has GPS";
            this.HasGPS.UseVisualStyleBackColor = true;
            this.HasGPS.CheckedChanged += new System.EventHandler(this.HasGPS_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Obstruction);
            this.groupBox2.Controls.Add(this.Focal);
            this.groupBox2.Controls.Add(this.Apperture);
            this.groupBox2.Controls.Add(this.ObstructionLabel);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.ScopeSelection);
            this.groupBox2.Location = new System.Drawing.Point(220, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 124);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Telescope Properties";
            // 
            // Obstruction
            // 
            this.Obstruction.Location = new System.Drawing.Point(90, 98);
            this.Obstruction.Name = "Obstruction";
            this.Obstruction.Size = new System.Drawing.Size(79, 20);
            this.Obstruction.TabIndex = 18;
            this.Obstruction.ValueChanged += new System.EventHandler(this.Field_ValueChanged);
            // 
            // Focal
            // 
            this.Focal.Location = new System.Drawing.Point(90, 72);
            this.Focal.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Focal.Name = "Focal";
            this.Focal.Size = new System.Drawing.Size(79, 20);
            this.Focal.TabIndex = 17;
            this.Focal.ValueChanged += new System.EventHandler(this.Field_ValueChanged);
            // 
            // Apperture
            // 
            this.Apperture.Location = new System.Drawing.Point(90, 46);
            this.Apperture.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.Apperture.Name = "Apperture";
            this.Apperture.Size = new System.Drawing.Size(79, 20);
            this.Apperture.TabIndex = 16;
            this.Apperture.ValueChanged += new System.EventHandler(this.Field_ValueChanged);
            // 
            // ObstructionLabel
            // 
            this.ObstructionLabel.AutoSize = true;
            this.ObstructionLabel.Location = new System.Drawing.Point(6, 100);
            this.ObstructionLabel.Name = "ObstructionLabel";
            this.ObstructionLabel.Size = new System.Drawing.Size(81, 13);
            this.ObstructionLabel.TabIndex = 15;
            this.ObstructionLabel.Text = "Obstruction (%):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Focal (mm):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Apperture (mm):";
            // 
            // ScopeSelection
            // 
            this.ScopeSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScopeSelection.FormattingEnabled = true;
            this.ScopeSelection.Items.AddRange(new object[] {
            "Celestrone Advanced C6-NGT",
            "Celestrone Advanced C6-SCT",
            "Celestrone Advanced C8-NGT",
            "Celestrone Advanced C8-SCT"});
            this.ScopeSelection.Location = new System.Drawing.Point(6, 19);
            this.ScopeSelection.Name = "ScopeSelection";
            this.ScopeSelection.Size = new System.Drawing.Size(234, 21);
            this.ScopeSelection.TabIndex = 0;
            this.ScopeSelection.SelectedIndexChanged += new System.EventHandler(this.ScopeSelection_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Elevation);
            this.groupBox1.Controls.Add(this.Latitude);
            this.groupBox1.Controls.Add(this.Longitude);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.LatSuff);
            this.groupBox1.Controls.Add(this.LonSuff);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(4, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 124);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Location";
            // 
            // Elevation
            // 
            this.Elevation.Location = new System.Drawing.Point(79, 72);
            this.Elevation.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Elevation.Name = "Elevation";
            this.Elevation.Size = new System.Drawing.Size(79, 20);
            this.Elevation.TabIndex = 17;
            this.Elevation.ValueChanged += new System.EventHandler(this.Field_ValueChanged);
            // 
            // Latitude
            // 
            this.Latitude.Location = new System.Drawing.Point(79, 19);
            this.Latitude.Name = "Latitude";
            this.Latitude.Size = new System.Drawing.Size(79, 20);
            this.Latitude.TabIndex = 8;
            this.Latitude.Validated += new System.EventHandler(this.GPS_Validated);
            // 
            // Longitude
            // 
            this.Longitude.Location = new System.Drawing.Point(79, 45);
            this.Longitude.Name = "Longitude";
            this.Longitude.Size = new System.Drawing.Size(79, 20);
            this.Longitude.TabIndex = 7;
            this.Longitude.Validated += new System.EventHandler(this.GPS_Validated);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Elevation (m):";
            // 
            // LatSuff
            // 
            this.LatSuff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LatSuff.FormattingEnabled = true;
            this.LatSuff.Items.AddRange(new object[] {
            "N",
            "S"});
            this.LatSuff.Location = new System.Drawing.Point(164, 19);
            this.LatSuff.Name = "LatSuff";
            this.LatSuff.Size = new System.Drawing.Size(38, 21);
            this.LatSuff.TabIndex = 1;
            // 
            // LonSuff
            // 
            this.LonSuff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LonSuff.FormattingEnabled = true;
            this.LonSuff.Items.AddRange(new object[] {
            "E",
            "W"});
            this.LonSuff.Location = new System.Drawing.Point(164, 45);
            this.LonSuff.Name = "LonSuff";
            this.LonSuff.Size = new System.Drawing.Size(38, 21);
            this.LonSuff.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Longitude:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Latitude:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 284);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(541, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar1.Visible = false;
            // 
            // ShowHandControl
            // 
            this.ShowHandControl.AutoSize = true;
            this.ShowHandControl.Location = new System.Drawing.Point(11, 16);
            this.ShowHandControl.Name = "ShowHandControl";
            this.ShowHandControl.Size = new System.Drawing.Size(88, 17);
            this.ShowHandControl.TabIndex = 19;
            this.ShowHandControl.Text = "Hand Control";
            this.ShowHandControl.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ShowHandControl);
            this.groupBox4.Controls.Add(this.chkTrace);
            this.groupBox4.Location = new System.Drawing.Point(4, 136);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(210, 86);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Misc";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 306);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Celestron ASCOM Driver Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Obstruction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Focal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Apperture)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Elevation)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox LonSuff;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox LatSuff;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox SelectedComPort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox ScopeSelection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SelectedBluetooth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label ObstructionLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox TrackingMode;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox HasGPS;
        private System.Windows.Forms.TextBox Latitude;
        private System.Windows.Forms.TextBox Longitude;
        private System.Windows.Forms.Button CheckScope;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.NumericUpDown Obstruction;
        private System.Windows.Forms.NumericUpDown Focal;
        private System.Windows.Forms.NumericUpDown Apperture;
        private System.Windows.Forms.NumericUpDown Elevation;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox ShowHandControl;
    }
}