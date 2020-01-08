namespace TCPAsyncServer
{
    partial class Form1
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
            this.gbxSender = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbxMessage = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.tbxPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.gbxSender.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxSender
            // 
            this.gbxSender.Controls.Add(this.btnStart);
            this.gbxSender.Controls.Add(this.tbxMessage);
            this.gbxSender.Controls.Add(this.lblMessage);
            this.gbxSender.Controls.Add(this.tbxPort);
            this.gbxSender.Controls.Add(this.lblPort);
            this.gbxSender.Location = new System.Drawing.Point(12, 12);
            this.gbxSender.Name = "gbxSender";
            this.gbxSender.Size = new System.Drawing.Size(299, 628);
            this.gbxSender.TabIndex = 2;
            this.gbxSender.TabStop = false;
            this.gbxSender.Text = "Server";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(196, 17);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbxMessage
            // 
            this.tbxMessage.Location = new System.Drawing.Point(6, 66);
            this.tbxMessage.Multiline = true;
            this.tbxMessage.Name = "tbxMessage";
            this.tbxMessage.Size = new System.Drawing.Size(287, 556);
            this.tbxMessage.TabIndex = 7;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(6, 50);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 4;
            this.lblMessage.Text = "Message";
            // 
            // tbxPort
            // 
            this.tbxPort.Location = new System.Drawing.Point(91, 19);
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(81, 20);
            this.tbxPort.TabIndex = 3;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(17, 22);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 646);
            this.Controls.Add(this.gbxSender);
            this.Name = "Form1";
            this.Text = "TCP Async Server";
            this.gbxSender.ResumeLayout(false);
            this.gbxSender.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxSender;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbxMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox tbxPort;
        private System.Windows.Forms.Label lblPort;
    }
}

