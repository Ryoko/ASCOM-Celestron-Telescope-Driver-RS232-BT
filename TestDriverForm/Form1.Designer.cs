namespace ASCOM.CelestronAdvancedBluetooth
{
    partial class Form1
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
            this.buttonChoose = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelDriverId = new System.Windows.Forms.Label();
            this.Coordinates = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.slewState = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Park = new System.Windows.Forms.Button();
            this.setPark = new System.Windows.Forms.Button();
            this.positionAlt = new System.Windows.Forms.Label();
            this.positionAzm = new System.Windows.Forms.Label();
            this.goHome = new System.Windows.Forms.Button();
            this.Azm = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Alt = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Dec = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Ra = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ControlButtons = new System.Windows.Forms.GroupBox();
            this.TrMode = new System.Windows.Forms.ComboBox();
            this.Lunar = new System.Windows.Forms.Button();
            this.Ra_p = new System.Windows.Forms.Button();
            this.Solar = new System.Windows.Forms.Button();
            this.Ra_n = new System.Windows.Forms.Button();
            this.Sidereal = new System.Windows.Forms.Button();
            this.Dec_n = new System.Windows.Forms.Button();
            this.Dec_p = new System.Windows.Forms.Button();
            this.console = new System.Windows.Forms.TextBox();
            this.useGamePad = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gamepadX = new System.Windows.Forms.TextBox();
            this.gamepadY = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.processAction = new System.Windows.Forms.Button();
            this.actionList = new System.Windows.Forms.ComboBox();
            this.actions = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.Coordinates.SuspendLayout();
            this.ControlButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.actions.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonChoose
            // 
            this.buttonChoose.Location = new System.Drawing.Point(247, 4);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(72, 23);
            this.buttonChoose.TabIndex = 0;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(325, 4);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(72, 23);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelDriverId
            // 
            this.labelDriverId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDriverId.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.CelestronAdvancedBluetooth.Properties.Settings.Default, "DriverId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.labelDriverId.Location = new System.Drawing.Point(12, 5);
            this.labelDriverId.Name = "labelDriverId";
            this.labelDriverId.Size = new System.Drawing.Size(229, 21);
            this.labelDriverId.TabIndex = 2;
            this.labelDriverId.Text = global::ASCOM.CelestronAdvancedBluetooth.Properties.Settings.Default.DriverId;
            this.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Coordinates
            // 
            this.Coordinates.Controls.Add(this.label9);
            this.Coordinates.Controls.Add(this.slewState);
            this.Coordinates.Controls.Add(this.label2);
            this.Coordinates.Controls.Add(this.label3);
            this.Coordinates.Controls.Add(this.Park);
            this.Coordinates.Controls.Add(this.setPark);
            this.Coordinates.Controls.Add(this.positionAlt);
            this.Coordinates.Controls.Add(this.positionAzm);
            this.Coordinates.Controls.Add(this.goHome);
            this.Coordinates.Controls.Add(this.Azm);
            this.Coordinates.Controls.Add(this.label6);
            this.Coordinates.Controls.Add(this.Alt);
            this.Coordinates.Controls.Add(this.label8);
            this.Coordinates.Controls.Add(this.Dec);
            this.Coordinates.Controls.Add(this.label4);
            this.Coordinates.Controls.Add(this.Ra);
            this.Coordinates.Controls.Add(this.label1);
            this.Coordinates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Coordinates.Location = new System.Drawing.Point(198, 0);
            this.Coordinates.Name = "Coordinates";
            this.Coordinates.Size = new System.Drawing.Size(211, 214);
            this.Coordinates.TabIndex = 3;
            this.Coordinates.TabStop = false;
            this.Coordinates.Text = "Coordinates";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Slew State:";
            // 
            // slewState
            // 
            this.slewState.AutoSize = true;
            this.slewState.Location = new System.Drawing.Point(99, 124);
            this.slewState.Name = "slewState";
            this.slewState.Size = new System.Drawing.Size(29, 13);
            this.slewState.TabIndex = 15;
            this.slewState.Text = "false";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Position Azimuth:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Position Altitude:";
            // 
            // Park
            // 
            this.Park.Location = new System.Drawing.Point(75, 171);
            this.Park.Name = "Park";
            this.Park.Size = new System.Drawing.Size(57, 23);
            this.Park.TabIndex = 12;
            this.Park.Text = "Park";
            this.Park.UseVisualStyleBackColor = true;
            this.Park.Click += new System.EventHandler(this.Park_Click);
            // 
            // setPark
            // 
            this.setPark.Location = new System.Drawing.Point(9, 171);
            this.setPark.Name = "setPark";
            this.setPark.Size = new System.Drawing.Size(60, 23);
            this.setPark.TabIndex = 11;
            this.setPark.Text = "SetPark";
            this.setPark.UseVisualStyleBackColor = true;
            this.setPark.Click += new System.EventHandler(this.setPark_Click);
            // 
            // positionAlt
            // 
            this.positionAlt.AutoSize = true;
            this.positionAlt.Location = new System.Drawing.Point(99, 89);
            this.positionAlt.Name = "positionAlt";
            this.positionAlt.Size = new System.Drawing.Size(58, 13);
            this.positionAlt.TabIndex = 10;
            this.positionAlt.Text = "00:00:00.0";
            // 
            // positionAzm
            // 
            this.positionAzm.AutoSize = true;
            this.positionAzm.Location = new System.Drawing.Point(99, 102);
            this.positionAzm.Name = "positionAzm";
            this.positionAzm.Size = new System.Drawing.Size(58, 13);
            this.positionAzm.TabIndex = 9;
            this.positionAzm.Text = "00:00:00.0";
            // 
            // goHome
            // 
            this.goHome.Location = new System.Drawing.Point(9, 142);
            this.goHome.Name = "goHome";
            this.goHome.Size = new System.Drawing.Size(57, 23);
            this.goHome.TabIndex = 8;
            this.goHome.Text = "GoHome";
            this.goHome.UseVisualStyleBackColor = true;
            this.goHome.Click += new System.EventHandler(this.goHome_Click);
            // 
            // Azm
            // 
            this.Azm.AutoSize = true;
            this.Azm.Location = new System.Drawing.Point(99, 70);
            this.Azm.Name = "Azm";
            this.Azm.Size = new System.Drawing.Size(58, 13);
            this.Azm.TabIndex = 7;
            this.Azm.Text = "00:00:00.0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Azimuth:";
            // 
            // Alt
            // 
            this.Alt.AutoSize = true;
            this.Alt.Location = new System.Drawing.Point(99, 57);
            this.Alt.Name = "Alt";
            this.Alt.Size = new System.Drawing.Size(58, 13);
            this.Alt.TabIndex = 5;
            this.Alt.Text = "00:00:00.0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 57);
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
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // ControlButtons
            // 
            this.ControlButtons.Controls.Add(this.TrMode);
            this.ControlButtons.Controls.Add(this.Lunar);
            this.ControlButtons.Controls.Add(this.Ra_p);
            this.ControlButtons.Controls.Add(this.Solar);
            this.ControlButtons.Controls.Add(this.Ra_n);
            this.ControlButtons.Controls.Add(this.Sidereal);
            this.ControlButtons.Controls.Add(this.Dec_n);
            this.ControlButtons.Controls.Add(this.Dec_p);
            this.ControlButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.ControlButtons.Enabled = false;
            this.ControlButtons.Location = new System.Drawing.Point(0, 0);
            this.ControlButtons.Name = "ControlButtons";
            this.ControlButtons.Size = new System.Drawing.Size(198, 214);
            this.ControlButtons.TabIndex = 4;
            this.ControlButtons.TabStop = false;
            this.ControlButtons.Text = "Control";
            // 
            // TrMode
            // 
            this.TrMode.FormattingEnabled = true;
            this.TrMode.Items.AddRange(new object[] {
            "Off",
            "AltAzm",
            "EQ-N",
            "EQ-S"});
            this.TrMode.Location = new System.Drawing.Point(6, 182);
            this.TrMode.Name = "TrMode";
            this.TrMode.Size = new System.Drawing.Size(120, 21);
            this.TrMode.TabIndex = 7;
            this.TrMode.SelectedIndexChanged += new System.EventHandler(this.TrackinMode_SelectedIndexChanged);
            // 
            // Lunar
            // 
            this.Lunar.Location = new System.Drawing.Point(132, 142);
            this.Lunar.Name = "Lunar";
            this.Lunar.Size = new System.Drawing.Size(57, 23);
            this.Lunar.TabIndex = 6;
            this.Lunar.Text = "Lunar";
            this.Lunar.UseVisualStyleBackColor = true;
            this.Lunar.Click += new System.EventHandler(this.Rate_Click);
            // 
            // Ra_p
            // 
            this.Ra_p.Location = new System.Drawing.Point(114, 56);
            this.Ra_p.Name = "Ra_p";
            this.Ra_p.Size = new System.Drawing.Size(48, 31);
            this.Ra_p.TabIndex = 3;
            this.Ra_p.Text = "Ra+";
            this.Ra_p.UseVisualStyleBackColor = true;
            this.Ra_p.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.Ra_p.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // Solar
            // 
            this.Solar.Location = new System.Drawing.Point(69, 142);
            this.Solar.Name = "Solar";
            this.Solar.Size = new System.Drawing.Size(57, 23);
            this.Solar.TabIndex = 5;
            this.Solar.Text = "Solar";
            this.Solar.UseVisualStyleBackColor = true;
            this.Solar.Click += new System.EventHandler(this.Rate_Click);
            // 
            // Ra_n
            // 
            this.Ra_n.Location = new System.Drawing.Point(6, 56);
            this.Ra_n.Name = "Ra_n";
            this.Ra_n.Size = new System.Drawing.Size(48, 31);
            this.Ra_n.TabIndex = 2;
            this.Ra_n.Text = "Ra-";
            this.Ra_n.UseVisualStyleBackColor = true;
            this.Ra_n.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.Ra_n.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // Sidereal
            // 
            this.Sidereal.Location = new System.Drawing.Point(6, 142);
            this.Sidereal.Name = "Sidereal";
            this.Sidereal.Size = new System.Drawing.Size(57, 23);
            this.Sidereal.TabIndex = 4;
            this.Sidereal.Text = "Sidereal";
            this.Sidereal.UseVisualStyleBackColor = true;
            this.Sidereal.Click += new System.EventHandler(this.Rate_Click);
            // 
            // Dec_n
            // 
            this.Dec_n.Location = new System.Drawing.Point(60, 93);
            this.Dec_n.Name = "Dec_n";
            this.Dec_n.Size = new System.Drawing.Size(48, 31);
            this.Dec_n.TabIndex = 1;
            this.Dec_n.Text = "Dec-";
            this.Dec_n.UseVisualStyleBackColor = true;
            this.Dec_n.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.Dec_n.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // Dec_p
            // 
            this.Dec_p.Location = new System.Drawing.Point(60, 19);
            this.Dec_p.Name = "Dec_p";
            this.Dec_p.Size = new System.Drawing.Size(48, 31);
            this.Dec_p.TabIndex = 0;
            this.Dec_p.Text = "Dec+";
            this.Dec_p.UseVisualStyleBackColor = true;
            this.Dec_p.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
            this.Dec_p.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
            // 
            // console
            // 
            this.console.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.console.Location = new System.Drawing.Point(0, 316);
            this.console.Multiline = true;
            this.console.Name = "console";
            this.console.Size = new System.Drawing.Size(409, 136);
            this.console.TabIndex = 5;
            // 
            // useGamePad
            // 
            this.useGamePad.AutoSize = true;
            this.useGamePad.Location = new System.Drawing.Point(10, 7);
            this.useGamePad.Name = "useGamePad";
            this.useGamePad.Size = new System.Drawing.Size(92, 17);
            this.useGamePad.TabIndex = 6;
            this.useGamePad.Text = "UseGamePad";
            this.useGamePad.UseVisualStyleBackColor = true;
            this.useGamePad.CheckedChanged += new System.EventHandler(this.useGamePad_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(108, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "X:";
            // 
            // gamepadX
            // 
            this.gamepadX.Location = new System.Drawing.Point(131, 5);
            this.gamepadX.Name = "gamepadX";
            this.gamepadX.ReadOnly = true;
            this.gamepadX.Size = new System.Drawing.Size(41, 20);
            this.gamepadX.TabIndex = 8;
            // 
            // gamepadY
            // 
            this.gamepadY.Location = new System.Drawing.Point(203, 5);
            this.gamepadY.Name = "gamepadY";
            this.gamepadY.ReadOnly = true;
            this.gamepadY.Size = new System.Drawing.Size(41, 20);
            this.gamepadY.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(180, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Y:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.useGamePad);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.gamepadY);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.gamepadX);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 249);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 29);
            this.panel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelDriverId);
            this.panel2.Controls.Add(this.buttonConnect);
            this.panel2.Controls.Add(this.buttonChoose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(409, 35);
            this.panel2.TabIndex = 12;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Coordinates);
            this.panel3.Controls.Add(this.ControlButtons);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 35);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(409, 214);
            this.panel3.TabIndex = 13;
            // 
            // processAction
            // 
            this.processAction.Location = new System.Drawing.Point(175, 4);
            this.processAction.Name = "processAction";
            this.processAction.Size = new System.Drawing.Size(66, 23);
            this.processAction.TabIndex = 14;
            this.processAction.Text = "Process";
            this.processAction.UseVisualStyleBackColor = true;
            this.processAction.Click += new System.EventHandler(this.slewTo_Click);
            // 
            // actionList
            // 
            this.actionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.actionList.FormattingEnabled = true;
            this.actionList.Items.AddRange(new object[] {
            "SlewTo",
            "SiteOfPier",
            "Destination SiteOfPier"});
            this.actionList.Location = new System.Drawing.Point(49, 6);
            this.actionList.Name = "actionList";
            this.actionList.Size = new System.Drawing.Size(120, 21);
            this.actionList.TabIndex = 8;
            // 
            // actions
            // 
            this.actions.Controls.Add(this.label10);
            this.actions.Controls.Add(this.actionList);
            this.actions.Controls.Add(this.processAction);
            this.actions.Dock = System.Windows.Forms.DockStyle.Top;
            this.actions.Location = new System.Drawing.Point(0, 278);
            this.actions.Name = "actions";
            this.actions.Size = new System.Drawing.Size(409, 32);
            this.actions.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Action:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 452);
            this.Controls.Add(this.actions);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.console);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TEMPLATEDEVICETYPE Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Coordinates.ResumeLayout(false);
            this.Coordinates.PerformLayout();
            this.ControlButtons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.actions.ResumeLayout(false);
            this.actions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelDriverId;
        private System.Windows.Forms.GroupBox Coordinates;
        private System.Windows.Forms.Label Azm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label Alt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label Dec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Ra;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox ControlButtons;
        private System.Windows.Forms.Button Ra_p;
        private System.Windows.Forms.Button Ra_n;
        private System.Windows.Forms.Button Dec_n;
        private System.Windows.Forms.Button Dec_p;
        private System.Windows.Forms.Button Sidereal;
        private System.Windows.Forms.Button Solar;
        private System.Windows.Forms.Button Lunar;
        private System.Windows.Forms.ComboBox TrMode;
        private System.Windows.Forms.Button goHome;
        private System.Windows.Forms.Label positionAzm;
        private System.Windows.Forms.Label positionAlt;
        private System.Windows.Forms.Button setPark;
        private System.Windows.Forms.TextBox console;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Park;
        private System.Windows.Forms.CheckBox useGamePad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox gamepadX;
        private System.Windows.Forms.TextBox gamepadY;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button processAction;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label slewState;
        private System.Windows.Forms.ComboBox actionList;
        private System.Windows.Forms.Panel actions;
        private System.Windows.Forms.Label label10;
    }
}

