namespace MapleShark
{
    partial class ScriptForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptForm));
            this.mSaveButton = new System.Windows.Forms.Button();
            this.mScriptEditor = new Alsing.Windows.Forms.SyntaxBoxControl();
            this.mScriptSyntax = new Alsing.SourceCode.SyntaxDocument(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mImportButton = new System.Windows.Forms.Button();
            this.FileImporter = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mSaveButton
            // 
            this.mSaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mSaveButton.Enabled = false;
            this.mSaveButton.Location = new System.Drawing.Point(0, 0);
            this.mSaveButton.Name = "mSaveButton";
            this.mSaveButton.Size = new System.Drawing.Size(392, 25);
            this.mSaveButton.TabIndex = 5;
            this.mSaveButton.Text = "&Save script";
            this.mSaveButton.UseVisualStyleBackColor = true;
            this.mSaveButton.Click += new System.EventHandler(this.mSaveButton_Click);
            // 
            // mScriptEditor
            // 
            this.mScriptEditor.ActiveView = Alsing.Windows.Forms.ActiveView.BottomRight;
            this.mScriptEditor.AutoListPosition = null;
            this.mScriptEditor.AutoListSelectedText = "a123";
            this.mScriptEditor.AutoListVisible = false;
            this.mScriptEditor.BackColor = System.Drawing.Color.White;
            this.mScriptEditor.BorderStyle = Alsing.Windows.Forms.BorderStyle.None;
            this.mScriptEditor.CopyAsRTF = false;
            this.mScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mScriptEditor.Document = this.mScriptSyntax;
            this.mScriptEditor.FontName = "Courier new";
            this.mScriptEditor.HighLightActiveLine = true;
            this.mScriptEditor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.mScriptEditor.Indent = Alsing.Windows.Forms.SyntaxBox.IndentStyle.Scope;
            this.mScriptEditor.InfoTipCount = 1;
            this.mScriptEditor.InfoTipPosition = null;
            this.mScriptEditor.InfoTipSelectedIndex = 1;
            this.mScriptEditor.InfoTipVisible = false;
            this.mScriptEditor.Location = new System.Drawing.Point(0, 0);
            this.mScriptEditor.LockCursorUpdate = false;
            this.mScriptEditor.Name = "mScriptEditor";
            this.mScriptEditor.ParseOnPaste = true;
            this.mScriptEditor.ShowScopeIndicator = false;
            this.mScriptEditor.Size = new System.Drawing.Size(598, 370);
            this.mScriptEditor.SmoothScroll = false;
            this.mScriptEditor.SplitviewH = -4;
            this.mScriptEditor.SplitviewV = -4;
            this.mScriptEditor.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(219)))), ((int)(((byte)(214)))));
            this.mScriptEditor.TabIndex = 0;
            this.mScriptEditor.TabsToSpaces = true;
            this.mScriptEditor.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
            this.mScriptEditor.TextChanged += new System.EventHandler(this.mScriptEditor_TextChanged);
            // 
            // mScriptSyntax
            // 
            this.mScriptSyntax.Lines = new string[] {
        ""};
            this.mScriptSyntax.MaxUndoBufferSize = 1000;
            this.mScriptSyntax.Modified = false;
            this.mScriptSyntax.UndoStep = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 370);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mSaveButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mImportButton);
            this.splitContainer1.Size = new System.Drawing.Size(598, 25);
            this.splitContainer1.SplitterDistance = 392;
            this.splitContainer1.TabIndex = 6;
            // 
            // mImportButton
            // 
            this.mImportButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mImportButton.Location = new System.Drawing.Point(0, 0);
            this.mImportButton.Name = "mImportButton";
            this.mImportButton.Size = new System.Drawing.Size(202, 25);
            this.mImportButton.TabIndex = 0;
            this.mImportButton.Text = "Import script...";
            this.mImportButton.UseVisualStyleBackColor = true;
            this.mImportButton.Click += new System.EventHandler(this.mImportButton_Click);
            // 
            // FileImporter
            // 
            this.FileImporter.FileName = "*.*";
            // 
            // ScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 395);
            this.Controls.Add(this.mScriptEditor);
            this.Controls.Add(this.splitContainer1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScriptForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Float;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Script";
            this.Load += new System.EventHandler(this.ScriptForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Alsing.Windows.Forms.SyntaxBoxControl mScriptEditor;
        private Alsing.SourceCode.SyntaxDocument mScriptSyntax;
        private System.Windows.Forms.Button mSaveButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button mImportButton;
        private System.Windows.Forms.OpenFileDialog FileImporter;

    }
}