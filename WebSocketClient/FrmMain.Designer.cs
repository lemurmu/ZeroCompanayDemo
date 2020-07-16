namespace WebSocketClient
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.url_txt = new System.Windows.Forms.TextBox();
            this.connect_btn = new System.Windows.Forms.Button();
            this.msg_txt = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.recvmsg_txt = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // url_txt
            // 
            this.url_txt.Location = new System.Drawing.Point(32, 36);
            this.url_txt.Name = "url_txt";
            this.url_txt.Size = new System.Drawing.Size(244, 21);
            this.url_txt.TabIndex = 0;
            this.url_txt.Text = "ws://127.0.0.1:8080";
            // 
            // connect_btn
            // 
            this.connect_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connect_btn.Location = new System.Drawing.Point(320, 36);
            this.connect_btn.Name = "connect_btn";
            this.connect_btn.Size = new System.Drawing.Size(75, 23);
            this.connect_btn.TabIndex = 1;
            this.connect_btn.Text = "连接";
            this.connect_btn.UseVisualStyleBackColor = true;
            this.connect_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // msg_txt
            // 
            this.msg_txt.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.msg_txt.Location = new System.Drawing.Point(32, 76);
            this.msg_txt.Name = "msg_txt";
            this.msg_txt.Size = new System.Drawing.Size(476, 143);
            this.msg_txt.TabIndex = 2;
            this.msg_txt.Text = "hello world\nwhat the fuck are you doing?";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(553, 181);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "发送";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // recvmsg_txt
            // 
            this.recvmsg_txt.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.recvmsg_txt.Location = new System.Drawing.Point(32, 239);
            this.recvmsg_txt.Name = "recvmsg_txt";
            this.recvmsg_txt.Size = new System.Drawing.Size(476, 158);
            this.recvmsg_txt.TabIndex = 4;
            this.recvmsg_txt.Text = "";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(553, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "清空";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 420);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.recvmsg_txt);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.msg_txt);
            this.Controls.Add(this.connect_btn);
            this.Controls.Add(this.url_txt);
            this.Name = "FrmMain";
            this.Text = "WebSocketClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox url_txt;
        private System.Windows.Forms.Button connect_btn;
        private System.Windows.Forms.RichTextBox msg_txt;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox recvmsg_txt;
        private System.Windows.Forms.Button button1;
    }
}

