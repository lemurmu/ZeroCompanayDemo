using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlDemo
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            SplashScreen screen = new SplashScreen();
            screen.Show();
            InitializeComponent();
            metroTileItem1.Image = Image.FromFile("日志.png");
            metroTileItem1.TitleText = "日志管理";
            metroTileItem1.TitleTextAlignment = ContentAlignment.TopCenter;
            metroTileItem1.TitleTextFont = new Font("楷体", 10);
            metroTileItem1.ImageTextAlignment = ContentAlignment.MiddleCenter;

            timer1.Enabled = true;
            textBoxItem1.WatermarkColor = Color.Cyan;
            screen.Close();
        }

        private async void Init()
        {
            string s = "sssdawrwegadasd";
            string d = s.ToLower();
            await Get();
        }

        private Task Get()
        {
            return Task.Run(() =>
            {

            });
        }

        private void tabControlPanel1_Click(object sender, EventArgs e)
        {

        }

        private void metroTileItem4_Click(object sender, EventArgs e)
        {

        }

        private void metroTileItem1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBoxItem1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void colorPickerButton1_SelectedColorChanged(object sender, EventArgs e)
        {

        }

        private void colorPickerButton1_SelectedColorChanged_1(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

        }

        //private string Get()
        //{
        //    return "";
        //}
    }
}
