using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class BinaryForm : Form
    {
        public BinaryForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog sec = new OpenFileDialog();
            sec.Filter = "BMP dosyaları (*.bmp)|*.bmp";

            if (sec.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(sec.FileName);
            }

        }


        private Bitmap griYap(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Height - 1; i++)
            {
                for (int j = 0; j < bmp.Width - 1; j++)
                {
                    int deger = (bmp.GetPixel(j, i).R + bmp.GetPixel(j, i).G + bmp.GetPixel(j, i).B) / 3;
                    Color renk;
                    renk = Color.FromArgb(deger, deger, deger);

                    bmp.SetPixel(j, i, renk);

                }
            }

            return bmp;
        }

        private Bitmap binaryYap(Bitmap image)
        {
            Bitmap gri = griYap(image);
            int temp = 0;
            int esik=100;
            Color renk;
            for(int i = 0;i < image.Height - 1; i++)
            {
                for(int j=0;j< image.Width - 1; j++)
                {
                    temp=gri.GetPixel(j, i).R;
                    if (temp < esik)
                    {
                        renk=Color.FromArgb(0,0,0);
                        gri.SetPixel(j, i, renk);
                    }
                    else
                    {
                        renk=Color.FromArgb(255,255,255);
                        gri.SetPixel(j,i, renk);
                    }

                }
            }

            return gri;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap binary = binaryYap(image);
            pictureBox2.Image = binary;
        }



    }
}
