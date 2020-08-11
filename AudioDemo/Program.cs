using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioDemo
{
    class Program
    {

        const int byteSample = 2;

        const int dataPosition = 40;

        //0x16 2byte 0002  双声道

        //0x22 2byte 0010  16位

        //0x18 4byte 0000AC44   44100采样率



        static void Main(string[] args)
        {

            byte[] length = new byte[4];

            FileStream fs = new FileStream("test.wav", FileMode.Open, FileAccess.Read);

            fs.Position = dataPosition;

            fs.Read(length, 0, 4);

            byte[] content = new byte[getHexToInt(length)];

            string[] sample = new string[content.Length / byteSample];

            fs.Read(content, 0, content.Length);

            getHex(content);

            sample = getSample(content);

            StreamWriter sw = new StreamWriter("data.txt", true, Encoding.Default);

            foreach (string i in sample)

            {

                sw.Flush();

                sw.WriteLine(i);

            }

            sw.Close();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());

        }



        static int getHexToInt(byte[] x)

        {

            string retValue = "";

            for (int i = x.Length - 1; i >= 0; i--)

            {

                retValue += x[i].ToString("X");

            }

            return Convert.ToInt32(retValue, 16);

        }



        static void getHex(byte[] x)

        {

            byte tmp;

            for (int i = 0; i < x.Length; i++)

            {

                tmp = Convert.ToByte(x[i].ToString("X"), 16);

                x[i] = tmp;

            }

        }



        static string[] getSample(byte[] x)

        {

            string[] retValue = new string[x.Length / byteSample];



            for (int i = 0; i < retValue.Length; i++)

            {

                for (int j = (i + 1) * byteSample - 1; j >= i * byteSample; j--)

                {

                    retValue[i] += x[j].ToString("X");

                }

                retValue[i] = ((double)Convert.ToInt16(retValue[i], 16) / 32768).ToString("F4");

            }

            return retValue;

        }

    }
}
