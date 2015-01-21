namespace MapleShark
{
    partial class SearchForm
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
            this.mOpcodeCombo = new System.Windows.Forms.ComboBox();
            this.mNextOpcodeButton = new System.Windows.Forms.Button();
            this.mPrevOpcodeButton = new System.Windows.Forms.Button();
            this.mSequenceHex = new System.Windows.Forms.HexBox();
            this.mPrevSequenceButton = new System.Windows.Forms.Button();
            this.mNextSequenceButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mOpcodeCombo
            // 
            this.mOpcodeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mOpcodeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mOpcodeCombo.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mOpcodeCombo.FormattingEnabled = true;
            this.mOpcodeCombo.Location = new System.Drawing.Point(3, 3);
            this.mOpcodeCombo.Name = "mOpcodeCombo";
            this.mOpcodeCombo.Size = new System.Drawing.Size(152, 23);
            this.mOpcodeCombo.TabIndex = 4;
            this.mOpcodeCombo.SelectedIndexChanged += new System.EventHandler(this.mOpcodeCombo_SelectedIndexChanged);
            // 
            // mNextOpcodeButton
            // 
            this.mNextOpcodeButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.mNextOpcodeButton.Enabled = false;
            this.mNextOpcodeButton.Location = new System.Drawing.Point(232, 3);
            this.mNextOpcodeButton.Name = "mNextOpcodeButton";
            this.mNextOpcodeButton.Size = new System.Drawing.Size(65, 25);
            this.mNextOpcodeButton.TabIndex = 5;
            this.mNextOpcodeButton.Text = "Next";
            this.mNextOpcodeButton.UseVisualStyleBackColor = true;
            this.mNextOpcodeButton.Click += new System.EventHandler(this.mNextOpcodeButton_Click);
            // 
            // mPrevOpcodeButton
            // 
            this.mPrevOpcodeButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.mPrevOpcodeButton.Enabled = false;
            this.mPrevOpcodeButton.Location = new System.Drawing.Point(161, 3);
            this.mPrevOpcodeButton.Name = "mPrevOpcodeButton";
            this.mPrevOpcodeButton.Size = new System.Drawing.Size(65, 25);
            this.mPrevOpcodeButton.TabIndex = 9;
            this.mPrevOpcodeButton.Text = "Prev";
            this.mPrevOpcodeButton.UseVisualStyleBackColor = true;
            this.mPrevOpcodeButton.Click += new System.EventHandler(this.mPrevOpcodeButton_Click);
            // 
            // mSequenceHex
            // 
            this.mSequenceHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mSequenceHex.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mSequenceHex.LineInfoForeColor = System.Drawing.Color.Empty;
            this.mSequenceHex.Location = new System.Drawing.Point(3, 32);
            this.mSequenceHex.Name = "mSequenceHex";
            this.mSequenceHex.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.mSequenceHex.Size = new System.Drawing.Size(152, 25);
            this.mSequenceHex.TabIndex = 6;
            this.mSequenceHex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mSequenceHex_KeyPress);
            // 
            // mPrevSequenceButton
            // 
            this.mPrevSequenceButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.mPrevSequenceButton.Enabled = false;
            this.mPrevSequenceButton.Location = new System.Drawing.Point(161, 32);
            this.mPrevSequenceButton.Name = "mPrevSequenceButton";
            this.mPrevSequenceButton.Size = new System.Drawing.Size(65, 25);
            this.mPrevSequenceButton.TabIndex = 8;
            this.mPrevSequenceButton.Text = "Prev";
            this.mPrevSequenceButton.UseVisualStyleBackColor = true;
            // 
            // mNextSequenceButton
            // 
            this.mNextSequenceButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.mNextSequenceButton.Enabled = false;
            this.mNextSequenceButton.Location = new System.Drawing.Point(232, 32);
            this.mNextSequenceButton.Name = "mNextSequenceButton";
            this.mNextSequenceButton.Size = new System.Drawing.Size(65, 25);
            this.mNextSequenceButton.TabIndex = 7;
            this.mNextSequenceButton.Text = "Next";
            this.mNextSequenceButton.UseVisualStyleBackColor = true;
            this.mNextSequenceButton.Click += new System.EventHandler(this.mNextSequenceButton_Click);
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 60);
            this.Controls.Add(this.mNextSequenceButton);
            this.Controls.Add(this.mNextOpcodeButton);
            this.Controls.Add(this.mPrevSequenceButton);
            this.Controls.Add(this.mPrevOpcodeButton);
            this.Controls.Add(this.mSequenceHex);
            this.Controls.Add(this.mOpcodeCombo);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.MinimumSize = new System.Drawing.Size(300, 60);
            this.Name = "SearchForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockTop;
            this.Text = "Search";
            this.Load += new System.EventHandler(this.SearchForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox mOpcodeCombo;
        private System.Windows.Forms.Button mNextOpcodeButton;
        private System.Windows.Forms.HexBox mSequenceHex;
        private System.Windows.Forms.Button mNextSequenceButton;
        private System.Windows.Forms.Button mPrevOpcodeButton;
        private System.Windows.Forms.Button mPrevSequenceButton;
    }
}