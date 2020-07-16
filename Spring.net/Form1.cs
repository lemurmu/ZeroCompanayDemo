using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpringNet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            //获取object的程序集类的对象，并且和对应的接口绑定，然后用接口接收
            IUserInfoService lister = (IUserInfoService)ctx.GetObject("UserInfoService");

            MessageBox.Show(lister.MsgShow());
        }
    }
}
