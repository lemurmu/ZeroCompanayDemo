using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpSelfHost.WebApi
{
    class WebServer
    {

        static IDisposable server = null;
        public static event EventHandles.MessageEventHandle MessageEvent = null;

       
        public static void Start()
        {
            StartOptions opt = new StartOptions();
            opt.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Port"));
            string baseAddress = string.Format("http://{0}:{1}/",
                   "*",
                   System.Configuration.ConfigurationManager.AppSettings.Get("Port"));
            server = WebApp.Start<Startup>(url: baseAddress);
            MessageEvent?.Invoke(String.Format("host 已启动：{0}", baseAddress));
        }

    }
}
