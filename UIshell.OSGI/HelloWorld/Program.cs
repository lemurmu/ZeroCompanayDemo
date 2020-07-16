using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIShell.OSGi;
using UIShell.OSGi.Core;
using UIShell.OSGi.Core.Bundle;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            //FrameworkFactory factory = new FrameworkFactory();
            //IFramework framework = factory.CreateFramework();
            //Console.WriteLine("Start bundle...");
            //framework.Init();
            //framework.Start();
            //framework.Stop();

            using (BundleRuntime bdrt = new BundleRuntime())
            {
                Console.WriteLine("Start bundle...");
                bdrt.Start();

                IFramework framework = bdrt.Framework;
                BundleRepository bundleRepository = framework.Bundles;
                //framework.Start();
                

                foreach (var bundle in bundleRepository)
                {
                    Console.WriteLine("name:" + bundle.Name + " State:" + bundle.State + "\r\n" +
                                        "Location:" + bundle.Location + " StartLevel:" + bundle.StartLevel);
                    IBundleContext context = bundle.Context;
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
                bdrt.Stop();


                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
