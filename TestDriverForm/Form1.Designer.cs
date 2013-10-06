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
            this.Ra_p = new System.Windows.Forms.Button();
            this.Ra_n = new System.Windows.Forms.Button();
            this.Dec_n = new System.Windows.Forms.Button();
            this.Dec_p = new System.Windows.Forms.Button();
            this.Sidereal = new System.Windows.Forms.Button();
            this.Solar = new System.Windows.Forms.Button();
            this.Lunar = new System.Windows.Forms.Button();
            this.TrMode = new System.Windows.Forms.ComboBox();
            this.Coordinates.SuspendLayout();
            this.ControlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonChoose
            // 
            this.buttonChoose.Location = new System.Drawing.Point(309, 10);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(72, 23);
            this.buttonChoose.TabIndex = 0;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(309, 39);
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
            this.labelDriverId.Location = new System.Drawing.Point(12, 40);
            this.labelDriverId.Name = "labelDriverId";
            this.labelDriverId.Size = new System.Drawing.Size(291, 21);
            this.labelDriverId.TabIndex = 2;
            this.labelDriverId.Text = global::ASCOM.CelestronAdvancedBluetooth.Properties.Settings.Default.DriverId;
            this.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Coordinates
            // 
            this.Coordinates.Controls.Add(this.Azm);
            this.Coordinates.Controls.Add(this.label6);
            this.Coordinates.Controls.Add(this.Alt);
            this.Coordinates.Controls.Add(this.label8);
            this.Coordinates.Controls.Add(this.Dec);
            this.Coordinates.Controls.Add(this.label4);
            this.Coordinates.Controls.Add(this.Ra);
            this.Coordinates.Controls.Add(this.label1);
            this.Coordinates.Location = new System.Drawing.Point(216, 73);
            this.Coordinates.Name = "Coordinates";
            this.Coordinates.Size = new System.Drawing.Size(181, 134);
            this.Coordinates.TabIndex = 3;
            this.Coordinates.TabStop = false;
            this.Coordinates.Text = "Coordinates";
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
            this.ControlButtons.Enabled = false;
            this.ControlButtons.Location = new System.Drawing.Point(12, 73);
            this.ControlButtons.Name = "ControlButtons";
            this.ControlButtons.Size = new System.Drawing.Size(198, 214);
            this.ControlButtons.TabIndex = 4;
            this.ControlButtons.TabStop = false;
            this.ControlButtons.Text = "Control";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 452);
            this.Controls.Add(this.ControlButtons);
            this.Controls.Add(this.Coordinates);
            this.Controls.Add(this.labelDriverId);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonChoose);
            this.Name = "Form1";
            this.Text = "TEMPLATEDEVICETYPE Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Coordinates.ResumeLayout(false);
            this.Coordinates.PerformLayout();
            this.ControlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}

