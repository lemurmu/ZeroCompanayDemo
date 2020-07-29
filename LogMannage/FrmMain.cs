using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;

namespace LogMannage
{
    public partial class FrmMain : XtraForm
    {
        public FrmMain()
        {
            InitializeComponent();
            level_cmb.SelectedIndex = 2;
            startTime.Text = "2020/1/1";
            endTime.Text = "2020/7/1";
            List<LogData> list = new List<LogData>();
            string[] levelArr = { "ALL", "Info", "Error", "Trace", "Falut" };
            for (int i = 0; i < 100; i++)
            {
                int index = rd.Next(0, 5);
                list.Add(new LogData { Time = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss"), Level = levelArr[index], Function = "fuc", Module = "model", Action = "get", Message = "u successful !" });
            }
            this.gridControl.DataSource = list;
        }

        Random rd = new Random();

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //this.Controls.Clear();
            //this.Controls.Add(new XtraUserControl1());
        }
    }

    public class LogData : MarshalByRefObject
    {

        #region "MarshalByRefObject 重载"

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion

        public string Time { get; set; }
        public string Level { get; set; }
        public string Function { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
    }
}
