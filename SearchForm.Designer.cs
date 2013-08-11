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
            this.mSplitter = new System.Windows.Forms.SplitContainer();
            this.mOpcodeSplitter = new System.Windows.Forms.SplitContainer();
            this.mOpcodeCombo = new System.Windows.Forms.ComboBox();
            this.mNextOpcodeButton = new System.Windows.Forms.Button();
            this.mPrevOpcodeButton = new System.Windows.Forms.Button();
            this.mSequenceSplitter = new System.Windows.Forms.SplitContainer();
            this.mSequenceHex = new System.Windows.Forms.HexBox();
            this.mPrevSequenceButton = new System.Windows.Forms.Button();
            this.mNextSequenceButton = new System.Windows.Forms.Button();
            this.mSplitter.Panel1.SuspendLayout();
            this.mSplitter.Panel2.SuspendLayout();
            this.mSplitter.SuspendLayout();
            this.mOpcodeSplitter.Panel1.SuspendLayout();
            this.mOpcodeSplitter.Panel2.SuspendLayout();
            this.mOpcodeSplitter.SuspendLayout();
            this.mSequenceSplitter.Panel1.SuspendLayout();
            this.mSequenceSplitter.Panel2.SuspendLayout();
            this.mSequenceSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // mSplitter
            // 
            this.mSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mSplitter.IsSplitterFixed = true;
            this.mSplitter.Location = new System.Drawing.Point(3, 3);
            this.mSplitter.Margin = new System.Windows.Forms.Padding(0);
            this.mSplitter.Name = "mSplitter";
            this.mSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mSplitter.Panel1
            // 
            this.mSplitter.Panel1.Controls.Add(this.mOpcodeSplitter);
            // 
            // mSplitter.Panel2
            // 
            this.mSplitter.Panel2.Controls.Add(this.mSequenceSplitter);
            this.mSplitter.Size = new System.Drawing.Size(514, 54);
            this.mSplitter.SplitterDistance = 25;
            this.mSplitter.TabIndex = 9;
            this.mSplitter.TabStop = false;
            // 
            // mOpcodeSplitter
            // 
            this.mOpcodeSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mOpcodeSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mOpcodeSplitter.IsSplitterFixed = true;
            this.mOpcodeSplitter.Location = new System.Drawing.Point(0, 0);
            this.mOpcodeSplitter.Name = "mOpcodeSplitter";
            // 
            // mOpcodeSplitter.Panel1
            // 
            this.mOpcodeSplitter.Panel1.Controls.Add(this.mOpcodeCombo);
            // 
            // mOpcodeSplitter.Panel2
            // 
            this.mOpcodeSplitter.Panel2.Controls.Add(this.mNextOpcodeButton);
            this.mOpcodeSplitter.Panel2.Controls.Add(this.mPrevOpcodeButton);
            this.mOpcodeSplitter.Size = new System.Drawing.Size(514, 25);
            this.mOpcodeSplitter.SplitterDistance = 380;
            this.mOpcodeSplitter.TabIndex = 5;
            this.mOpcodeSplitter.TabStop = false;
            // 
            // mOpcodeCombo
            // 
            this.mOpcodeCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mOpcodeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mOpcodeCombo.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mOpcodeCombo.FormattingEnabled = true;
            this.mOpcodeCombo.Location = new System.Drawing.Point(0, 0);
            this.mOpcodeCombo.Name = "mOpcodeCombo";
            this.mOpcodeCombo.Size = new System.Drawing.Size(380, 23);
            this.mOpcodeCombo.TabIndex = 4;
            this.mOpcodeCombo.SelectedIndexChanged += new System.EventHandler(this.mOpcodeCombo_SelectedIndexChanged);
            // 
            // mNextOpcodeButton
            // 
            this.mNextOpcodeButton.Enabled = false;
            this.mNextOpcodeButton.Location = new System.Drawing.Point(65, 0);
            this.mNextOpcodeButton.Name = "mNextOpcodeButton";
            this.mNextOpcodeButton.Size = new System.Drawing.Size(65, 25);
            this.mNextOpcodeButton.TabIndex = 5;
            this.mNextOpcodeButton.Text = "Next";
            this.mNextOpcodeButton.UseVisualStyleBackColor = true;
            this.mNextOpcodeButton.Click += new System.EventHandler(this.mNextOpcodeButton_Click);
            // 
            // mPrevOpcodeButton
            // 
            this.mPrevOpcodeButton.Enabled = false;
            this.mPrevOpcodeButton.Location = new System.Drawing.Point(0, 0);
            this.mPrevOpcodeButton.Name = "mPrevOpcodeButton";
            this.mPrevOpcodeButton.Size = new System.Drawing.Size(65, 25);
            this.mPrevOpcodeButton.TabIndex = 9;
            this.mPrevOpcodeButton.Text = "Prev";
            this.mPrevOpcodeButton.UseVisualStyleBackColor = true;
            this.mPrevOpcodeButton.Click += new System.EventHandler(this.mPrevOpcodeButton_Click);
            // 
            // mSequenceSplitter
            // 
            this.mSequenceSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mSequenceSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mSequenceSplitter.IsSplitterFixed = true;
            this.mSequenceSplitter.Location = new System.Drawing.Point(0, 0);
            this.mSequenceSplitter.Name = "mSequenceSplitter";
            // 
            // mSequenceSplitter.Panel1
            // 
            this.mSequenceSplitter.Panel1.Controls.Add(this.mSequenceHex);
            // 
            // mSequenceSplitter.Panel2
            // 
            this.mSequenceSplitter.Panel2.Controls.Add(this.mPrevSequenceButton);
            this.mSequenceSplitter.Panel2.Controls.Add(this.mNextSequenceButton);
            this.mSequenceSplitter.Size = new System.Drawing.Size(514, 25);
            this.mSequenceSplitter.SplitterDistance = 380;
            this.mSequenceSplitter.TabIndex = 7;
            this.mSequenceSplitter.TabStop = false;
            // 
            // mSequenceHex
            // 
            this.mSequenceHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mSequenceHex.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mSequenceHex.LineInfoForeColor = System.Drawing.Color.Empty;
            this.mSequenceHex.Location = new System.Drawing.Point(0, 0);
            this.mSequenceHex.Name = "mSequenceHex";
            this.mSequenceHex.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.mSequenceHex.Size = new System.Drawing.Size(380, 25);
            this.mSequenceHex.TabIndex = 6;
            this.mSequenceHex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mSequenceHex_KeyPress);
            // 
            // mPrevSequenceButton
            // 
            this.mPrevSequenceButton.Enabled = false;
            this.mPrevSequenceButton.Location = new System.Drawing.Point(0, 0);
            this.mPrevSequenceButton.Name = "mPrevSequenceButton";
            this.mPrevSequenceButton.Size = new System.Drawing.Size(65, 25);
            this.mPrevSequenceButton.TabIndex = 8;
            this.mPrevSequenceButton.Text = "Prev";
            this.mPrevSequenceButton.UseVisualStyleBackColor = true;
            this.mPrevSequenceButton.Click += new System.EventHandler(this.mPrevSequenceButton_Click);
            // 
            // mNextSequenceButton
            // 
            this.mNextSequenceButton.Enabled = false;
            this.mNextSequenceButton.Location = new System.Drawing.Point(65, 0);
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
            this.ClientSize = new System.Drawing.Size(529, 72);
            this.Controls.Add(this.mSplitter);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.MinimumSize = new System.Drawing.Size(537, 99);
            this.Name = "SearchForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockTop;
            this.Text = "Search";
            this.mSplitter.Panel1.ResumeLayout(false);
            this.mSplitter.Panel2.ResumeLayout(false);
            this.mSplitter.ResumeLayout(false);
            this.mOpcodeSplitter.Panel1.ResumeLayout(false);
            this.mOpcodeSplitter.Panel2.ResumeLayout(false);
            this.mOpcodeSplitter.ResumeLayout(false);
            this.mSequenceSplitter.Panel1.ResumeLayout(false);
            this.mSequenceSplitter.Panel2.ResumeLayout(false);
            this.mSequenceSplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mSplitter;
        private System.Windows.Forms.SplitContainer mOpcodeSplitter;
        private System.Windows.Forms.ComboBox mOpcodeCombo;
        private System.Windows.Forms.Button mNextOpcodeButton;
        private System.Windows.Forms.SplitContainer mSequenceSplitter;
        private System.Windows.Forms.HexBox mSequenceHex;
        private System.Windows.Forms.Button mNextSequenceButton;
        private System.Windows.Forms.Button mPrevOpcodeButton;
        private System.Windows.Forms.Button mPrevSequenceButton;
    }
}