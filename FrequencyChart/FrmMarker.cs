using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using System.IO;
using Itc.Library.Basic;

namespace FrequencyChart
{
    public partial class FrmMarker : DevExpress.XtraEditors.XtraForm
    {
        public FrmMarker()
        {
            InitializeComponent();
        }

        private void SaveConfig(object sender, EventArgs e)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ToolConfig.SetAppSetting("mark1visible", display_check1.Checked.ToString());
            ToolConfig.SetAppSetting("mark2visible", display_check2.Checked.ToString());
            ToolConfig.SetAppSetting("freq1", freq1_txt.Value.ToString());
            ToolConfig.SetAppSetting("freq2", freq2_txt.Value.ToString());
            ToolConfig.SetAppSetting("color1", $"{mark1_color.Color.R},{mark1_color.Color.G},{mark1_color.Color.B}");
            ToolConfig.SetAppSetting("color2", $"{mark2_color.Color.R},{mark2_color.Color.G},{mark2_color.Color.B}");

        }

        private void FrmMarker_Load(object sender, EventArgs e)
        {
            display_check1.CheckState = Convert.ToBoolean(ToolConfig.GetAppSetting("mark1visible")) ? CheckState.Checked : CheckState.Unchecked;
            display_check2.CheckState = Convert.ToBoolean(ToolConfig.GetAppSetting("mark2visible")) ? CheckState.Checked : CheckState.Unchecked;
            freq1_txt.Value = Convert.ToDecimal(ToolConfig.GetAppSetting("freq1"));
            freq2_txt.Value = Convert.ToDecimal(ToolConfig.GetAppSetting("freq2"));
            string[] colors1 = ToolConfig.GetAppSetting("color1").Split(',');
            string[] colors2 = ToolConfig.GetAppSetting("color2").Split(',');
            mark1_color.Color = Color.FromArgb(int.Parse(colors1[0]), int.Parse(colors1[1]), int.Parse(colors1[2]));
            mark2_color.Color = Color.FromArgb(int.Parse(colors2[0]), int.Parse(colors2[1]), int.Parse(colors2[2]));

            mark1_color.EditValueChanged += new EventHandler(SaveConfig);
            mark2_color.EditValueChanged += new EventHandler(SaveConfig);
            freq1_txt.EditValueChanged += new EventHandler(SaveConfig);
            freq2_txt.EditValueChanged += new EventHandler(SaveConfig);
            display_check1.CheckedChanged += new EventHandler(SaveConfig);
            display_check2.CheckedChanged += new EventHandler(SaveConfig);
            this.FormClosed += (s, ev) =>
            {
                SaveConfig(s, null);
            };
        }

        public double Value1
        {
            set
            {
                this.value_lab1.Text = "value:" + value.ToString("0.000");
            }
        }

        public double Value2
        {
            set
            {
                this.value_lab2.Text = "value:" + value.ToString("0.000");
            }
        }

    }
}