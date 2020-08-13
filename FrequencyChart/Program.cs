using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.IO;
using System.Text;

namespace FrequencyChart
{
    static class Program
    {

        const int byteSample = 2;

        const int dataPosition = 40;

        //0x16 2byte 0002  双声道

        //0x22 2byte 0010  16位

        //0x18 4byte 0000AC44   44100采样率

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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


