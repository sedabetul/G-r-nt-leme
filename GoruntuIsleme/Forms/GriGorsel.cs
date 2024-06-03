using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoruntuIsleme.Forms
{
    public partial class GriGorsel : Form
    {
        public GriGorsel()
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
                for(int j=0; j < bmp.Width -1; j++)
                {
                    int deger = (bmp.GetPixel(j,i).R + bmp.GetPixel(j, i).G + bmp.GetPixel(j, i).B) / 3;
                    Color renk;
                    renk=Color.FromArgb(deger,deger,deger);

                    bmp.SetPixel(j,i,renk);

                }
            }

            return bmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap gri=griYap(image);
            pictureBox2.Image = gri;

        }

        private void GriGorsel_Load(object sender, EventArgs e)
        {

        }
    }
}
