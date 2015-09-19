namespace MapleShark
{
    partial class frmImportProps
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
            this.txtPropFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudLocale = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudVersion = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.ofdPropFile = new System.Windows.Forms.OpenFileDialog();
            this.btnImport = new System.Windows.Forms.Button();
            this.chkIsSend = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudLocale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVersion)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPropFile
            // 
            this.txtPropFile.Location = new System.Drawing.Point(141, 12);
            this.txtPropFile.Name = "txtPropFile";
            this.txtPropFile.Size = new System.Drawing.Size(164, 20);
            this.txtPropFile.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Properties file:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "MapleStory Locale ID:";
            // 
            // nudLocale
            // 
            this.nudLocale.Location = new System.Drawing.Point(141, 38);
            this.nudLocale.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudLocale.Name = "nudLocale";
            this.nudLocale.Size = new System.Drawing.Size(213, 20);
            this.nudLocale.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "MapleStory Version:";
            // 
            // nudVersion
            // 
            this.nudVersion.Location = new System.Drawing.Point(141, 64);
            this.nudVersion.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudVersion.Name = "nudVersion";
            this.nudVersion.Size = new System.Drawing.Size(213, 20);
            this.nudVersion.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(311, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 20);
            this.button1.TabIndex = 8;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ofdPropFile
            // 
            this.ofdPropFile.DefaultExt = "*.properties";
            this.ofdPropFile.FileName = "*.properties";
            this.ofdPropFile.Filter = "Java Properties File|*.properties|TXT file|*.txt";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(12, 111);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(344, 23);
            this.btnImport.TabIndex = 11;
            this.btnImport.Text = "Import File";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // chkIsSend
            // 
            this.chkIsSend.AutoSize = true;
            this.chkIsSend.Location = new System.Drawing.Point(141, 91);
            this.chkIsSend.Name = "chkIsSend";
            this.chkIsSend.Size = new System.Drawing.Size(15, 14);
            this.chkIsSend.TabIndex = 12;
            this.chkIsSend.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Is Send opcode file";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 182);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(6, 19);
            this.txtLog.MaxLength = 3276700;
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(330, 157);
            this.txtLog.TabIndex = 0;
            // 
            // frmImportProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 334);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkIsSend);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nudVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudLocale);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPropFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmImportProps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Properties File";
            ((System.ComponentModel.ISupportInitialize)(this.nudLocale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVersion)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPropFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudLocale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudVersion;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog ofdPropFile;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.CheckBox chkIsSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLog;
    }
}