using GoruntuIsleme.Forms;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog sec = new OpenFileDialog();
            if (sec.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(sec.FileName);

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BinaryForm bf= new BinaryForm();
            bf.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RGBForm rg=new RGBForm();
            rg.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HistogramEsitle he = new HistogramEsitle();
            he.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bulaniklastir b=new Bulaniklastir();
            b.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            KenarAlgilama ka=new KenarAlgilama();
            ka.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GriGorsel gg=new GriGorsel();
            gg.Show();
            this.Hide(); 
        }

        int sayac = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;
            if(sayac%3==0 )
            {
                pictureBox1.Image = Image.FromFile("C:\\Users\\Lenova\\source\\repos\\GoruntuIsleme\\GoruntuIsleme\\i3.jpg");
            }
            else if(sayac % 3 == 1)
            {
                pictureBox1.Image = Image.FromFile("C:\\Users\\Lenova\\source\\repos\\GoruntuIsleme\\GoruntuIsleme\\i2.jpg");
            }
            else if (sayac % 3 == 2)
            {
                pictureBox1.Image = Image.FromFile("C:\\Users\\Lenova\\source\\repos\\GoruntuIsleme\\GoruntuIsleme\\i1.jpg");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
