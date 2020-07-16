using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIShell.OSGi;

namespace PopupMsgBoxPlugin
{
    public class PopupMsgBox : IBundleActivator
    {
        public void Start(IBundleContext context)
        {
            // MessageBox.Show("<Start> Hello World!", "MsgBox");
            //context.ExtensionChanged += (s, e) =>
            //{
            //    Extension extension = e.Extension;
            //    var list = extension.Data;
            //    foreach (var node in list)
            //    {
            //        string value = node.Attributes["Name"].Value;
            //        Console.WriteLine(value);
            //    }
            //};
        }

        public void Stop(IBundleContext context)
        {
           // MessageBox.Show("<Stop> See you.", "MsgBox");
           
        }

    }
}
