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
            this.TrackingMode = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.HasGPS = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.Obstruction = new System.Windows.Forms.Label();
            this.Focal = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Apperture = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ScopeSelection = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Latitude = new System.Windows.Forms.TextBox();
            this.Longitude = new System.Windows.Forms.TextBox();
            this.Altitude = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LatSuff = new System.Windows.Forms.ComboBox();
            this.LonSuff = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(4, 251);
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
            this.cmdCancel.Location = new System.Drawing.Point(4, 279);
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
            this.chkTrace.Location = new System.Drawing.Point(73, 37);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(367, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Select Devices";
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
            this.tabControl1.Size = new System.Drawing.Size(474, 86);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SelectedComPort);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.chkTrace);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(466, 60);
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
            this.tabPage2.Size = new System.Drawing.Size(466, 60);
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
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.picASCOM);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(474, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(66, 307);
            this.panel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 86);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(474, 221);
            this.panel2.TabIndex = 12;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TrackingMode);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.HasGPS);
            this.groupBox3.Location = new System.Drawing.Point(4, 116);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
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
            this.TrackingMode.Location = new System.Drawing.Point(93, 40);
            this.TrackingMode.Name = "TrackingMode";
            this.TrackingMode.Size = new System.Drawing.Size(91, 21);
            this.TrackingMode.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 43);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Tracking Mode";
            // 
            // HasGPS
            // 
            this.HasGPS.AutoSize = true;
            this.HasGPS.Location = new System.Drawing.Point(93, 19);
            this.HasGPS.Name = "HasGPS";
            this.HasGPS.Size = new System.Drawing.Size(68, 17);
            this.HasGPS.TabIndex = 0;
            this.HasGPS.Text = "has GPS";
            this.HasGPS.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.Obstruction);
            this.groupBox2.Controls.Add(this.Focal);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.Apperture);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.ScopeSelection);
            this.groupBox2.Location = new System.Drawing.Point(220, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 212);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scope";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(93, 119);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(79, 20);
            this.textBox4.TabIndex = 3;
            // 
            // Obstruction
            // 
            this.Obstruction.AutoSize = true;
            this.Obstruction.Location = new System.Drawing.Point(9, 122);
            this.Obstruction.Name = "Obstruction";
            this.Obstruction.Size = new System.Drawing.Size(81, 13);
            this.Obstruction.TabIndex = 15;
            this.Obstruction.Text = "Obstruction (%):";
            // 
            // Focal
            // 
            this.Focal.Location = new System.Drawing.Point(93, 93);
            this.Focal.Name = "Focal";
            this.Focal.Size = new System.Drawing.Size(79, 20);
            this.Focal.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Focal (mm):";
            // 
            // Apperture
            // 
            this.Apperture.Location = new System.Drawing.Point(93, 67);
            this.Apperture.Name = "Apperture";
            this.Apperture.Size = new System.Drawing.Size(79, 20);
            this.Apperture.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 70);
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
            this.ScopeSelection.Location = new System.Drawing.Point(7, 15);
            this.ScopeSelection.Name = "ScopeSelection";
            this.ScopeSelection.Size = new System.Drawing.Size(234, 21);
            this.ScopeSelection.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Latitude);
            this.groupBox1.Controls.Add(this.Longitude);
            this.groupBox1.Controls.Add(this.Altitude);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.LatSuff);
            this.groupBox1.Controls.Add(this.LonSuff);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(4, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 104);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Location";
            // 
            // Latitude
            // 
            this.Latitude.Location = new System.Drawing.Point(69, 19);
            this.Latitude.Name = "Latitude";
            this.Latitude.Size = new System.Drawing.Size(79, 20);
            this.Latitude.TabIndex = 8;
            this.Latitude.Validated += new System.EventHandler(this.Latitude_Validated);
            // 
            // Longitude
            // 
            this.Longitude.Location = new System.Drawing.Point(69, 45);
            this.Longitude.Name = "Longitude";
            this.Longitude.Size = new System.Drawing.Size(79, 20);
            this.Longitude.TabIndex = 7;
            this.Longitude.Validated += new System.EventHandler(this.Latitude_Validated);
            // 
            // Altitude
            // 
            this.Altitude.Location = new System.Drawing.Point(69, 71);
            this.Altitude.Name = "Altitude";
            this.Altitude.Size = new System.Drawing.Size(79, 20);
            this.Altitude.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Altitude (m):";
            // 
            // LatSuff
            // 
            this.LatSuff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LatSuff.FormattingEnabled = true;
            this.LatSuff.Items.AddRange(new object[] {
            "N",
            "S"});
            this.LatSuff.Location = new System.Drawing.Point(154, 19);
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
            this.LonSuff.Location = new System.Drawing.Point(154, 45);
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
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 307);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TextBox Altitude;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label Obstruction;
        private System.Windows.Forms.TextBox Focal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox Apperture;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox TrackingMode;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox HasGPS;
        private System.Windows.Forms.TextBox Latitude;
        private System.Windows.Forms.TextBox Longitude;
    }
}