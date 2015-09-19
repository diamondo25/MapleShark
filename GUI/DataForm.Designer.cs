namespace MapleShark
{
    partial class DataForm
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
            this.mHex = new System.Windows.Forms.HexBox();
            this.SuspendLayout();
            // 
            // mHex
            // 
            this.mHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mHex.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mHex.LineInfoForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.mHex.LineInfoVisible = true;
            this.mHex.Location = new System.Drawing.Point(0, 0);
            this.mHex.Name = "mHex";
            this.mHex.ReadOnly = true;
            this.mHex.SelectionBackColor = System.Drawing.Color.Black;
            this.mHex.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.mHex.Size = new System.Drawing.Size(613, 147);
            this.mHex.StringViewVisible = true;
            this.mHex.TabIndex = 2;
            this.mHex.UseFixedBytesPerLine = true;
            this.mHex.VScrollBarVisible = true;
            this.mHex.SelectionLengthChanged += new System.EventHandler(this.mHex_SelectionLengthChanged);
            this.mHex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mHex_KeyDown);
            // 
            // DataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 147);
            this.Controls.Add(this.mHex);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "DataForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
            this.Text = "Data";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HexBox mHex;
    }
}