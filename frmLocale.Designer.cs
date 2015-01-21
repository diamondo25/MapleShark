namespace MapleShark
{
    partial class frmLocale
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
            this.label1 = new System.Windows.Forms.Label();
            this.nudLocale = new System.Windows.Forms.NumericUpDown();
            this.btnContinue = new System.Windows.Forms.Button();
            this.cbLocale = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudLocale)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "This file seems to be from an old version of MapleShark.\r\nPlease identify which l" +
    "ocale this MSB file is from:";
            // 
            // nudLocale
            // 
            this.nudLocale.Location = new System.Drawing.Point(230, 57);
            this.nudLocale.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudLocale.Name = "nudLocale";
            this.nudLocale.Size = new System.Drawing.Size(52, 20);
            this.nudLocale.TabIndex = 1;
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(12, 83);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(274, 23);
            this.btnContinue.TabIndex = 2;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // cbLocale
            // 
            this.cbLocale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocale.FormattingEnabled = true;
            this.cbLocale.Location = new System.Drawing.Point(12, 56);
            this.cbLocale.Name = "cbLocale";
            this.cbLocale.Size = new System.Drawing.Size(157, 21);
            this.cbLocale.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(188, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Other:";
            // 
            // frmLocale
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 118);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbLocale);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.nudLocale);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLocale";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MapleShark";
            this.Load += new System.EventHandler(this.frmLocale_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudLocale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudLocale;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.ComboBox cbLocale;
        private System.Windows.Forms.Label label3;
    }
}