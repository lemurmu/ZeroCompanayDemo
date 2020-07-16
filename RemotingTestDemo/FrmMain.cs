using ITestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestModel;

namespace RemotingTestDemo
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        Model model;
        IModel model2;
        int count = 0;
        Random random = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //  string msg = model.Hello().Trim().ToString();
                string msg = model2.Hello().Trim().ToString();
                MessageBox.Show(msg);
            }
            catch (Exception)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //  model.SetName($"fuck you{count++}次!!!");
            model2.SetName($"People{random.Next(0, 100)}");
            MessageBox.Show("修改远程服务成功！");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //People p = model.GetPeople;
            People p = model2.GetPeople;
            MessageBox.Show("远程对象：" + p.Name);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            model2.GetPeople.Name = $"我修改了{count++}次对象....";
            MessageBox.Show("修改远程对象成功！");
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            model = (Model)Activator.GetObject(typeof(Model), "Tcp://127.0.0.1:4008/testModel");

            //使用接口抽象封装 服务端是具体的实现 服务端客户端通信使用服务契约（接口）
            model2 = (IModel)Activator.GetObject(typeof(IModel), "Tcp://127.0.0.1:4008/ModelImple");
        }
    }
}
