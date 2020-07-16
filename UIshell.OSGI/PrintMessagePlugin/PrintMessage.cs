using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIShell.OSGi;

namespace PrintMessagePlugin
{
    public class PrintMessage : IBundleActivator
    {
        public void Start(IBundleContext context)
        {
            Console.WriteLine("<Start> Hello World  -- PrintMessage");
            context.ExtensionChanged += (s, e) =>
            {
                Extension extension = e.Extension;
                var list = extension.Data;
                foreach (var node in list)
                {
                    string value = node.Attributes["Name"].Value;
                    Console.WriteLine(value);
                }
            };

           
        }

        public void Stop(IBundleContext context)
        {
            Console.WriteLine("<Stop> See you next time. -- PrintMessage");
        }
    }
}
