using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebSocketClient
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            //msg_txt.ForeColor = Color.DarkTurquoise;
            recvmsg_txt.ForeColor = Color.Green;
        }

        ClientWebSocket client;
        Thread r_thread;

        private async void button1_Click(object sender, EventArgs e)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            try
            {
                if (connect_btn.Text == "连接")
                {
                    string url = url_txt.Text;
                    client = new ClientWebSocket();
                    // await client.ConnectAsync(new Uri(url), new System.Threading.CancellationToken(false));
                    await client.ConnectAsync(new Uri(url), token);
                    MessageBox.Show("连接成功!");
                    connect_btn.Text = "断开";
                    RecievMessage(client);
                }
                else
                {
                    // await client?.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", new System.Threading.CancellationToken(false));
                    await client?.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", token);
                    MessageBox.Show("关闭成功!");
                    connect_btn.Text = "连接";
                    r_thread?.Abort();
                    r_thread = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}!");
                source.Cancel();
            }

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string[] lines = msg_txt.Lines;
                for (int i = 0; i < lines.Length; i++)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(lines[i]);
                    ArraySegment<byte> buffer = new ArraySegment<byte>(bytes);
                    await client.SendAsync(buffer, WebSocketMessageType.Text, true, new System.Threading.CancellationToken(false));
                    recvmsg_txt.AppendText($"\n发送：{lines[i]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}!");
            }


        }

        private void RecievMessage(ClientWebSocket _client)
        {
            r_thread = new Thread(new ThreadStart(async () =>
            {
                try
                {
                    while (client.State == WebSocketState.Open)
                    {
                        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                        WebSocketReceiveResult result = await _client.ReceiveAsync(buffer, new CancellationToken(false));
                        if (result.Count > 0)
                        {
                            byte[] recnBytes = new byte[result.Count];
                            Array.Copy(buffer.Array, 0, recnBytes, 0, recnBytes.Length);
                            string msg = Encoding.UTF8.GetString(recnBytes);
                            BeginInvoke(new Action(() =>
                            {
                                recvmsg_txt.AppendText($"\n接收:{msg}");
                            }));
                        }
                    }
                }
                catch (Exception)
                {

                }

            }));
            r_thread.IsBackground = true;
            r_thread.Start();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            msg_txt.Clear();
        }
    }
}
