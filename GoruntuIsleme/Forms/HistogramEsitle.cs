using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Imaging.Filters;

namespace GoruntuIsleme.Forms
{
    public partial class HistogramEsitle : Form
    {
        public HistogramEsitle()
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap image = new Bitmap(pictureBox1.Image);
                Bitmap histogramEsitle = HistogramBul(image);
                pictureBox2.Image = histogramEsitle;
            }
        }


        private Bitmap HistogramBul(Bitmap image)
        {
            //verilen görüntüyü gri tonlamaya dönüştürür, histogramını hesaplar
            Bitmap griresim = Grayscale(image);

           
            int[] histogram = new int[256];
            for (int i = 0; i < griresim.Width; i++)
            {
                for (int j = 0; j < griresim.Height; j++)
                {
                    int pixelValue = griresim.GetPixel(i, j).R;
                    histogram[pixelValue]++;
                }
            }

            // Kümülatif dağılım fonksiyonu  hesapla
            int[] cdf = new int[256];
            cdf[0] = histogram[0];
            for (int i = 1; i < 256; i++)
            {
                cdf[i] = cdf[i - 1] + histogram[i];
            }

            // En düşük ve en yüksek CDF değerlerini bul
            int cdfMin = cdf.First(x => x > 0);
            int cdfMax = cdf.Last();

            // Yeni piksel değerlerini hesapla
            byte[] equalizedValues = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                equalizedValues[i] = (byte)((cdf[i] - cdfMin) * 255 / (cdfMax - cdfMin));
            }

            // Görüntüyü güncelle
            Bitmap resultImage = new Bitmap(griresim.Width, griresim.Height);
            for (int i = 0; i < griresim.Width; i++)
            {
                for (int j = 0; j < griresim.Height; j++)
                {
                    int pixelValue = griresim.GetPixel(i, j).R;
                    int newValue = equalizedValues[pixelValue];
                    resultImage.SetPixel(i, j, Color.FromArgb(newValue, newValue, newValue));
                }
            }

            return resultImage;
        }




        private Bitmap Grayscale(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    Color originalColor = original.GetPixel(i, j);
                    int grayValue = (int)(originalColor.R * 0.2125 + originalColor.G * 0.7154 + originalColor.B * 0.0721);
                    Color grayColor = Color.FromArgb(grayValue, grayValue, grayValue);
                    newBitmap.SetPixel(i, j, grayColor);
                }
            }
            return newBitmap;
        }

        private void HistogramEsitle_Load(object sender, EventArgs e)
        {

        }
    }
}
