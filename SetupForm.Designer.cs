namespace MapleShark
{
    partial class SetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
            this.mCancelButton = new System.Windows.Forms.Button();
            this.mOKButton = new System.Windows.Forms.Button();
            this.mHighPortNumeric = new System.Windows.Forms.NumericUpDown();
            this.mLowPortNumeric = new System.Windows.Forms.NumericUpDown();
            this.mPortsLabel = new System.Windows.Forms.Label();
            this.mInterfaceCombo = new System.Windows.Forms.ComboBox();
            this.mInterfaceLabel = new System.Windows.Forms.Label();
            this.mMainPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mHighPortNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mLowPortNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mMainPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // mCancelButton
            // 
            this.mCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mCancelButton.Location = new System.Drawing.Point(341, 84);
            this.mCancelButton.Name = "mCancelButton";
            this.mCancelButton.Size = new System.Drawing.Size(75, 23);
            this.mCancelButton.TabIndex = 14;
            this.mCancelButton.Text = "&Cancel";
            this.mCancelButton.UseVisualStyleBackColor = true;
            // 
            // mOKButton
            // 
            this.mOKButton.Enabled = false;
            this.mOKButton.Location = new System.Drawing.Point(201, 84);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(75, 23);
            this.mOKButton.TabIndex = 13;
            this.mOKButton.Text = "&Ok";
            this.mOKButton.UseVisualStyleBackColor = true;
            this.mOKButton.Click += new System.EventHandler(this.mOKButton_Click);
            // 
            // mHighPortNumeric
            // 
            this.mHighPortNumeric.Location = new System.Drawing.Point(316, 39);
            this.mHighPortNumeric.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.mHighPortNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mHighPortNumeric.Name = "mHighPortNumeric";
            this.mHighPortNumeric.Size = new System.Drawing.Size(100, 20);
            this.mHighPortNumeric.TabIndex = 12;
            this.mHighPortNumeric.Value = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.mHighPortNumeric.ValueChanged += new System.EventHandler(this.mHighPortNumeric_ValueChanged);
            // 
            // mLowPortNumeric
            // 
            this.mLowPortNumeric.Location = new System.Drawing.Point(201, 39);
            this.mLowPortNumeric.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.mLowPortNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mLowPortNumeric.Name = "mLowPortNumeric";
            this.mLowPortNumeric.Size = new System.Drawing.Size(100, 20);
            this.mLowPortNumeric.TabIndex = 11;
            this.mLowPortNumeric.Value = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.mLowPortNumeric.ValueChanged += new System.EventHandler(this.mLowPortNumeric_ValueChanged);
            // 
            // mPortsLabel
            // 
            this.mPortsLabel.AutoSize = true;
            this.mPortsLabel.Location = new System.Drawing.Point(161, 41);
            this.mPortsLabel.Name = "mPortsLabel";
            this.mPortsLabel.Size = new System.Drawing.Size(34, 13);
            this.mPortsLabel.TabIndex = 10;
            this.mPortsLabel.Text = "&Ports:";
            // 
            // mInterfaceCombo
            // 
            this.mInterfaceCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mInterfaceCombo.FormattingEnabled = true;
            this.mInterfaceCombo.Location = new System.Drawing.Point(201, 12);
            this.mInterfaceCombo.Name = "mInterfaceCombo";
            this.mInterfaceCombo.Size = new System.Drawing.Size(215, 21);
            this.mInterfaceCombo.TabIndex = 9;
            this.mInterfaceCombo.SelectedIndexChanged += new System.EventHandler(this.mInterfaceCombo_SelectedIndexChanged);
            // 
            // mInterfaceLabel
            // 
            this.mInterfaceLabel.AutoSize = true;
            this.mInterfaceLabel.Location = new System.Drawing.Point(143, 15);
            this.mInterfaceLabel.Name = "mInterfaceLabel";
            this.mInterfaceLabel.Size = new System.Drawing.Size(52, 13);
            this.mInterfaceLabel.TabIndex = 8;
            this.mInterfaceLabel.Text = "&Interface:";
            // 
            // mMainPicture
            // 
            this.mMainPicture.Image = ((System.Drawing.Image)(resources.GetObject("mMainPicture.Image")));
            this.mMainPicture.Location = new System.Drawing.Point(12, 12);
            this.mMainPicture.Name = "mMainPicture";
            this.mMainPicture.Size = new System.Drawing.Size(121, 92);
            this.mMainPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mMainPicture.TabIndex = 15;
            this.mMainPicture.TabStop = false;
            // 
            // SetupForm
            // 
            this.AcceptButton = this.mOKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.mCancelButton;
            this.ClientSize = new System.Drawing.Size(428, 119);
            this.Controls.Add(this.mMainPicture);
            this.Controls.Add(this.mCancelButton);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.mHighPortNumeric);
            this.Controls.Add(this.mLowPortNumeric);
            this.Controls.Add(this.mPortsLabel);
            this.Controls.Add(this.mInterfaceCombo);
            this.Controls.Add(this.mInterfaceLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup";
            this.Load += new System.EventHandler(this.SetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mHighPortNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mLowPortNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mMainPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button mCancelButton;
        private System.Windows.Forms.Button mOKButton;
        private System.Windows.Forms.NumericUpDown mHighPortNumeric;
        private System.Windows.Forms.NumericUpDown mLowPortNumeric;
        private System.Windows.Forms.Label mPortsLabel;
        private System.Windows.Forms.ComboBox mInterfaceCombo;
        private System.Windows.Forms.Label mInterfaceLabel;
        private System.Windows.Forms.PictureBox mMainPicture;
    }
}