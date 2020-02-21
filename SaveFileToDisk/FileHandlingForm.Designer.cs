namespace SaveFileToDisk
{
    partial class frmEdit
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
            this.btnReplace = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.tbxReplace = new System.Windows.Forms.TextBox();
            this.lblReplaceWIth = new System.Windows.Forms.Label();
            this.tbxReplaceWith = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.rtbText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(13, 13);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(75, 23);
            this.btnReplace.TabIndex = 0;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.BtnReplace_Click);
            // 
            // tbxReplace
            // 
            this.tbxReplace.Location = new System.Drawing.Point(94, 15);
            this.tbxReplace.Name = "tbxReplace";
            this.tbxReplace.Size = new System.Drawing.Size(100, 20);
            this.tbxReplace.TabIndex = 1;
            // 
            // lblReplaceWIth
            // 
            this.lblReplaceWIth.AutoSize = true;
            this.lblReplaceWIth.Location = new System.Drawing.Point(200, 18);
            this.lblReplaceWIth.Name = "lblReplaceWIth";
            this.lblReplaceWIth.Size = new System.Drawing.Size(32, 13);
            this.lblReplaceWIth.TabIndex = 3;
            this.lblReplaceWIth.Text = "With:";
            // 
            // tbxReplaceWith
            // 
            this.tbxReplaceWith.Location = new System.Drawing.Point(238, 15);
            this.tbxReplaceWith.Name = "tbxReplaceWith";
            this.tbxReplaceWith.Size = new System.Drawing.Size(100, 20);
            this.tbxReplaceWith.TabIndex = 4;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(632, 13);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 5;
            this.btnOpen.Text = "Open File";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(713, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save File";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // rtbText
            // 
            this.rtbText.Location = new System.Drawing.Point(12, 42);
            this.rtbText.Name = "rtbText";
            this.rtbText.Size = new System.Drawing.Size(776, 396);
            this.rtbText.TabIndex = 7;
            this.rtbText.Text = "";
            // 
            // frmEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rtbText);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.tbxReplaceWith);
            this.Controls.Add(this.lblReplaceWIth);
            this.Controls.Add(this.tbxReplace);
            this.Controls.Add(this.btnReplace);
            this.Name = "frmEdit";
            this.Text = "TextEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.TextBox tbxReplace;
        private System.Windows.Forms.Label lblReplaceWIth;
        private System.Windows.Forms.TextBox tbxReplaceWith;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.RichTextBox rtbText;
    }
}

