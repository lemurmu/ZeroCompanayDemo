using NetDimension.NanUI;
using NetDimension.NanUI.Browser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NanUI.Demo
{
    class MainWindow : Formium
    {
        public override string StartUrl => "https://www.iconfont.cn/";

        public override HostWindowType WindowType => HostWindowType.Standard;

        protected override Control LaunchScreen => null;

        public MainWindow()
        {
            Title = "IconFont";
        }

        protected override void OnWindowReady(IWebBrowserHandler browserClient)
        {

        }

        protected override void OnRegisterGlobalObject(JSObject global)
        {

        }

        /// <summary>
        /// 重载设置窗体的样式
        /// </summary>
        /// <param name="style"></param>
        protected override void OnStandardFormStyle(IStandardHostWindowStyle style)
        {
            base.OnStandardFormStyle(style);

            style.Width = 1280;
            style.Height = 720;
            style.Icon =new Icon("Apple.ico");
            style.StartPosition = FormStartPosition.CenterScreen;
            style.WindowState = FormWindowState.Maximized;
        }
    }
}
