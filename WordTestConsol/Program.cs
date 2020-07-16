using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordTestConsol
{
    class Program
    {
        static void Main(string[] args)
        {
            FrequencyBandTestReport report = new FrequencyBandTestReport();

            //List<FrequencyBandTestData> data = new List<FrequencyBandTestData>();
            //data.Add(new FrequencyBandTestData { Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now });
            //data.Add(new FrequencyBandTestData { Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now });
            //data.Add(new FrequencyBandTestData { Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now });
            //data.Add(new FrequencyBandTestData { Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now });

            string bookmaskName = "QueryParaTable";
            string imagePath = @"resource\test.png";
            string image1path = @"resource\1.png";
            string image2path = @"resource\2.png";
            Image image = Image.FromFile(imagePath);
            Image image1 = Image.FromFile(image1path);
            Image image2 = Image.FromFile(image2path);
            //  Form1 form = new Form1();
            // form.SetImages(image, image1, image2);
            //   form.Show();
            ////  report.WriteData(1, data);
            //report.TableWriteD(bookmaskName);

            //report.WriteData(bookmaskName);
            //report.InsertImage(imagePath, ImageFormat.Png, "pic2");

            Dictionary<WordLabel, List<FrequencyBandTestData>> tableDataDict = new Dictionary<WordLabel, List<FrequencyBandTestData>>
            {
                {
                    WordLabel.freq_12501565MHz,
                    new List<FrequencyBandTestData>
            {
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
            }
                },

                {
                    WordLabel.freq_22002300MHz,
                    new List<FrequencyBandTestData>
            {
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
            }
                },

                {
                    WordLabel.freq_36204200MHz,
                    new List<FrequencyBandTestData>
            {
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
            }
                },

                {
                    WordLabel.freq_52005350MHz,
                    new List<FrequencyBandTestData>
            {
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
            }
                },

                {
                    WordLabel.freq_55505650MHz,
                    new List<FrequencyBandTestData>
            {
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
            }
                },

                {
                    WordLabel.freq_58456425MHz,
                    new List<FrequencyBandTestData>
            {
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
                new FrequencyBandTestData{ Freq = 23.68, Bandwidth = 500.2, Directional = 45.2, StartTime = DateTime.Now, StopTime = DateTime.Now },
            }
                }
            };

            Dictionary<WordLabel, List<Image>> imageDict = new Dictionary<WordLabel, List<Image>>();

            imageDict.Add(WordLabel.freq_12501565MHz, new List<Image> {
                image,image1,image2
            });

            imageDict.Add(WordLabel.freq_22002300MHz, new List<Image> {
                image,image1,image2
            });
            imageDict.Add(WordLabel.freq_36204200MHz, new List<Image> {
                image,image1,image2
            });
            imageDict.Add(WordLabel.freq_52005350MHz, new List<Image> {
                image,image1,image2
            });
            imageDict.Add(WordLabel.freq_55505650MHz, new List<Image> {
                image,image1,image2
            });
            imageDict.Add(WordLabel.freq_58456425MHz, new List<Image> {
                image,image1,image2
            });

            report.WriteData(tableDataDict, imageDict);
            report.Close(AppDomain.CurrentDomain.BaseDirectory + "test.docx");


            Console.ReadKey();
        }
    }
}
