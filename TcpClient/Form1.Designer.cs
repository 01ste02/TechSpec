namespace TcpClient
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
            this.tbxMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.tbxPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.tbxIP = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.gbxSender.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxSender
            // 
            this.gbxSender.Controls.Add(this.tbxMessage);
            this.gbxSender.Controls.Add(this.btnSend);
            this.gbxSender.Controls.Add(this.lblMessage);
            this.gbxSender.Controls.Add(this.tbxPort);
            this.gbxSender.Controls.Add(this.lblPort);
            this.gbxSender.Controls.Add(this.tbxIP);
            this.gbxSender.Controls.Add(this.lblIP);
            this.gbxSender.Location = new System.Drawing.Point(12, 12);
            this.gbxSender.Name = "gbxSender";
            this.gbxSender.Size = new System.Drawing.Size(299, 628);
            this.gbxSender.TabIndex = 1;
            this.gbxSender.TabStop = false;
            this.gbxSender.Text = "Sender";
            // 
            // tbxMessage
            // 
            this.tbxMessage.Location = new System.Drawing.Point(6, 138);
            this.tbxMessage.Multiline = true;
            this.tbxMessage.Name = "tbxMessage";
            this.tbxMessage.Size = new System.Drawing.Size(287, 455);
            this.tbxMessage.TabIndex = 7;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(6, 599);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(287, 23);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(6, 122);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 4;
            this.lblMessage.Text = "Message";
            // 
            // tbxPort
            // 
            this.tbxPort.Location = new System.Drawing.Point(80, 74);
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(81, 20);
            this.tbxPort.TabIndex = 3;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 77);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port";
            // 
            // tbxIP
            // 
            this.tbxIP.Location = new System.Drawing.Point(80, 37);
            this.tbxIP.Name = "tbxIP";
            this.tbxIP.Size = new System.Drawing.Size(180, 20);
            this.tbxIP.TabIndex = 1;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(6, 40);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(51, 13);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "IP-adress";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 649);
            this.Controls.Add(this.gbxSender);
            this.Name = "Form1";
            this.Text = "Sender";
            this.gbxSender.ResumeLayout(false);
            this.gbxSender.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxSender;
        private System.Windows.Forms.TextBox tbxMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox tbxPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox tbxIP;
        private System.Windows.Forms.Label lblIP;
    }
}

