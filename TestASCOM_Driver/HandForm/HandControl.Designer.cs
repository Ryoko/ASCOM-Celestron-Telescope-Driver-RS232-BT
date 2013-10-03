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
            this.Ra_p = new System.Windows.Forms.Button();
            this.Ra_n = new System.Windows.Forms.Button();
            this.Dec_n = new System.Windows.Forms.Button();
            this.Dec_p = new System.Windows.Forms.Button();
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
            this.Coordinates.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlButtons
            // 
            this.ControlButtons.Controls.Add(this.Ra_p);
            this.ControlButtons.Controls.Add(this.Ra_n);
            this.ControlButtons.Controls.Add(this.Dec_n);
            this.ControlButtons.Controls.Add(this.Dec_p);
            this.ControlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlButtons.Enabled = false;
            this.ControlButtons.Location = new System.Drawing.Point(0, 122);
            this.ControlButtons.Name = "ControlButtons";
            this.ControlButtons.Size = new System.Drawing.Size(174, 134);
            this.ControlButtons.TabIndex = 6;
            this.ControlButtons.TabStop = false;
            this.ControlButtons.Text = "Controls";
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
            this.ClientSize = new System.Drawing.Size(174, 254);
            this.Controls.Add(this.ControlButtons);
            this.Controls.Add(this.Coordinates);
            this.Name = "HandControl";
            this.Text = "HandControl";
            this.ControlButtons.ResumeLayout(false);
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
    }
}