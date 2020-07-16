using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordTestConsol
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public void SetImages(Image image1,Image image2,Image image3)
        {
            this.pictureBox1.Image = image1;
            this.pictureBox2.Image = image2;
            this.pictureBox3.Image = image3;
        }
    }
}
