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
            this.mScriptEditor = new Alsing.Windows.Forms.SyntaxBoxControl();
            this.mScriptSyntax = new Alsing.SourceCode.SyntaxDocument(this.components);
            this.FileImporter = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SaveButton = new System.Windows.Forms.ToolStripButton();
            this.ImportButton = new System.Windows.Forms.ToolStripButton();
            this.ExportButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportScript = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mScriptEditor
            // 
            this.mScriptEditor.ActiveView = Alsing.Windows.Forms.ActiveView.BottomRight;
            this.mScriptEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mScriptEditor.AutoListPosition = null;
            this.mScriptEditor.AutoListSelectedText = "a123";
            this.mScriptEditor.AutoListVisible = false;
            this.mScriptEditor.BackColor = System.Drawing.Color.White;
            this.mScriptEditor.BorderStyle = Alsing.Windows.Forms.BorderStyle.None;
            this.mScriptEditor.CopyAsRTF = false;
            this.mScriptEditor.Document = this.mScriptSyntax;
            this.mScriptEditor.FontName = "Courier new";
            this.mScriptEditor.HighLightActiveLine = true;
            this.mScriptEditor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.mScriptEditor.Indent = Alsing.Windows.Forms.SyntaxBox.IndentStyle.Scope;
            this.mScriptEditor.InfoTipCount = 1;
            this.mScriptEditor.InfoTipPosition = null;
            this.mScriptEditor.InfoTipSelectedIndex = 1;
            this.mScriptEditor.InfoTipVisible = false;
            this.mScriptEditor.Location = new System.Drawing.Point(0, 52);
            this.mScriptEditor.LockCursorUpdate = false;
            this.mScriptEditor.Name = "mScriptEditor";
            this.mScriptEditor.ParseOnPaste = true;
            this.mScriptEditor.ShowScopeIndicator = false;
            this.mScriptEditor.Size = new System.Drawing.Size(658, 353);
            this.mScriptEditor.SmoothScroll = false;
            this.mScriptEditor.SplitviewH = -4;
            this.mScriptEditor.SplitviewV = -4;
            this.mScriptEditor.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(219)))), ((int)(((byte)(214)))));
            this.mScriptEditor.TabIndex = 0;
            this.mScriptEditor.TabsToSpaces = true;
            this.mScriptEditor.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
            // 
            // mScriptSyntax
            // 
            this.mScriptSyntax.Lines = new string[] {
        ""};
            this.mScriptSyntax.MaxUndoBufferSize = 1000;
            this.mScriptSyntax.Modified = false;
            this.mScriptSyntax.UndoStep = 0;
            // 
            // FileImporter
            // 
            this.FileImporter.FileName = "*.*";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveButton,
            this.ImportButton,
            this.ExportButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(658, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // SaveButton
            // 
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveButton.Image = global::MapleShark.Properties.Resources.Save;
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(23, 22);
            this.SaveButton.Text = "Save script";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ImportButton.Image = global::MapleShark.Properties.Resources.Login;
            this.ImportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(23, 22);
            this.ImportButton.Text = "Import script";
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ExportButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportButton.Image")));
            this.ExportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(23, 22);
            this.ExportButton.Text = "Export script";
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(658, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importScriptToolStripMenuItem,
            this.exportScriptToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importScriptToolStripMenuItem
            // 
            this.importScriptToolStripMenuItem.Image = global::MapleShark.Properties.Resources.Login;
            this.importScriptToolStripMenuItem.Name = "importScriptToolStripMenuItem";
            this.importScriptToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importScriptToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.importScriptToolStripMenuItem.Text = "Import script";
            this.importScriptToolStripMenuItem.Click += new System.EventHandler(this.importScriptToolStripMenuItem_Click);
            // 
            // exportScriptToolStripMenuItem
            // 
            this.exportScriptToolStripMenuItem.Image = global::MapleShark.Properties.Resources.Logout;
            this.exportScriptToolStripMenuItem.Name = "exportScriptToolStripMenuItem";
            this.exportScriptToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportScriptToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.exportScriptToolStripMenuItem.Text = "Export script";
            this.exportScriptToolStripMenuItem.Click += new System.EventHandler(this.exportScriptToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::MapleShark.Properties.Resources.Save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::MapleShark.Properties.Resources.Delete;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // ScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 406);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.mScriptEditor);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScriptForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Float;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Script";
            this.Load += new System.EventHandler(this.ScriptForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Alsing.Windows.Forms.SyntaxBoxControl mScriptEditor;
        private Alsing.SourceCode.SyntaxDocument mScriptSyntax;
        private System.Windows.Forms.OpenFileDialog FileImporter;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton SaveButton;
        private System.Windows.Forms.ToolStripButton ImportButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog ExportScript;
        private System.Windows.Forms.ToolStripButton ExportButton;
        private System.Windows.Forms.ToolStripMenuItem exportScriptToolStripMenuItem;

    }
}