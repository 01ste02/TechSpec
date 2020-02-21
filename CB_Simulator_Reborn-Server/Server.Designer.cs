namespace CB_Simulator_Reborn_Server
{
    partial class CB_Simulator_Reborn_Server
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
            this.gbxControls = new System.Windows.Forms.GroupBox();
            this.btnSaveLog = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnClearConsole = new System.Windows.Forms.Button();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.btnKick = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbxUsers = new System.Windows.Forms.ListBox();
            this.lblUsers = new System.Windows.Forms.Label();
            this.lbxConsole = new System.Windows.Forms.ListBox();
            this.tbxMessage = new System.Windows.Forms.TextBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.saveConsoleDialog = new System.Windows.Forms.SaveFileDialog();
            this.stopBroadcastToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbxControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxControls
            // 
            this.gbxControls.Controls.Add(this.btnSaveLog);
            this.gbxControls.Controls.Add(this.btnClearAll);
            this.gbxControls.Controls.Add(this.btnClearConsole);
            this.gbxControls.Controls.Add(this.btnStopServer);
            this.gbxControls.Controls.Add(this.btnKick);
            this.gbxControls.Controls.Add(this.btnStop);
            this.gbxControls.Controls.Add(this.btnStart);
            this.gbxControls.Location = new System.Drawing.Point(12, 12);
            this.gbxControls.Name = "gbxControls";
            this.gbxControls.Size = new System.Drawing.Size(121, 227);
            this.gbxControls.TabIndex = 0;
            this.gbxControls.TabStop = false;
            this.gbxControls.Text = "Server Controls";
            // 
            // btnSaveLog
            // 
            this.btnSaveLog.Location = new System.Drawing.Point(8, 194);
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(107, 23);
            this.btnSaveLog.TabIndex = 6;
            this.btnSaveLog.Text = "Save log";
            this.btnSaveLog.UseVisualStyleBackColor = true;
            this.btnSaveLog.Click += new System.EventHandler(this.BtnSaveLog_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Enabled = false;
            this.btnClearAll.Location = new System.Drawing.Point(8, 165);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(107, 23);
            this.btnClearAll.TabIndex = 5;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.BtnClearAll_Click);
            // 
            // btnClearConsole
            // 
            this.btnClearConsole.Enabled = false;
            this.btnClearConsole.Location = new System.Drawing.Point(8, 136);
            this.btnClearConsole.Name = "btnClearConsole";
            this.btnClearConsole.Size = new System.Drawing.Size(107, 23);
            this.btnClearConsole.TabIndex = 4;
            this.btnClearConsole.Text = "Clear Console";
            this.btnClearConsole.UseVisualStyleBackColor = true;
            this.btnClearConsole.Click += new System.EventHandler(this.BtnClearConsole_Click);
            // 
            // btnStopServer
            // 
            this.btnStopServer.Enabled = false;
            this.btnStopServer.Location = new System.Drawing.Point(7, 77);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(108, 23);
            this.btnStopServer.TabIndex = 3;
            this.btnStopServer.Text = "Stop All";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.BtnStopServer_Click);
            // 
            // btnKick
            // 
            this.btnKick.Enabled = false;
            this.btnKick.Location = new System.Drawing.Point(8, 106);
            this.btnKick.Name = "btnKick";
            this.btnKick.Size = new System.Drawing.Size(108, 23);
            this.btnKick.TabIndex = 2;
            this.btnKick.Text = "Kick";
            this.btnKick.UseVisualStyleBackColor = true;
            this.btnKick.Click += new System.EventHandler(this.BtnKick_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(7, 48);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(107, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop Broadcast";
            this.stopBroadcastToolTip.SetToolTip(this.btnStop, "Stops broadcasting the server port over the network - No new clients can join. Re" +
        "verse with \"Start All\".");
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(6, 19);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(108, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start All";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // lbxUsers
            // 
            this.lbxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxUsers.FormattingEnabled = true;
            this.lbxUsers.HorizontalScrollbar = true;
            this.lbxUsers.Location = new System.Drawing.Point(12, 263);
            this.lbxUsers.Name = "lbxUsers";
            this.lbxUsers.Size = new System.Drawing.Size(121, 303);
            this.lbxUsers.TabIndex = 1;
            // 
            // lblUsers
            // 
            this.lblUsers.AutoSize = true;
            this.lblUsers.Location = new System.Drawing.Point(12, 242);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(37, 13);
            this.lblUsers.TabIndex = 2;
            this.lblUsers.Text = "Users:";
            // 
            // lbxConsole
            // 
            this.lbxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxConsole.FormattingEnabled = true;
            this.lbxConsole.HorizontalScrollbar = true;
            this.lbxConsole.Location = new System.Drawing.Point(158, 12);
            this.lbxConsole.Name = "lbxConsole";
            this.lbxConsole.Size = new System.Drawing.Size(551, 511);
            this.lbxConsole.TabIndex = 3;
            // 
            // tbxMessage
            // 
            this.tbxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxMessage.Location = new System.Drawing.Point(158, 529);
            this.tbxMessage.Multiline = true;
            this.tbxMessage.Name = "tbxMessage";
            this.tbxMessage.Size = new System.Drawing.Size(493, 37);
            this.tbxMessage.TabIndex = 4;
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendMessage.Enabled = false;
            this.btnSendMessage.Location = new System.Drawing.Point(657, 529);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(52, 37);
            this.btnSendMessage.TabIndex = 5;
            this.btnSendMessage.Text = "Send";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.BtnSendMessage_Click);
            // 
            // saveConsoleDialog
            // 
            this.saveConsoleDialog.DefaultExt = "txt";
            this.saveConsoleDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            // 
            // stopBroadcastToolTip
            // 
            this.stopBroadcastToolTip.IsBalloon = true;
            this.stopBroadcastToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.stopBroadcastToolTip.ToolTipTitle = "Stops broadcasting the server port over the network - No new clients can join. Re" +
    "verse with \"Start All\".";
            // 
            // CB_Simulator_Reborn_Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 578);
            this.Controls.Add(this.btnSendMessage);
            this.Controls.Add(this.tbxMessage);
            this.Controls.Add(this.lbxConsole);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.lbxUsers);
            this.Controls.Add(this.gbxControls);
            this.Name = "CB_Simulator_Reborn_Server";
            this.Text = "CB Simulator Reborn - Server Console";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TriggerFormClosing);
            this.gbxControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxControls;
        private System.Windows.Forms.Button btnSaveLog;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnClearConsole;
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.Button btnKick;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox lbxUsers;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.ListBox lbxConsole;
        private System.Windows.Forms.TextBox tbxMessage;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.SaveFileDialog saveConsoleDialog;
        private System.Windows.Forms.ToolTip stopBroadcastToolTip;
    }
}

