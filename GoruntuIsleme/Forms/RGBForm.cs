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
    public partial class RGBForm : Form
    {
        public RGBForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Show();
            this.Hide();
        }

        private void RGBForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap image = new Bitmap(pictureBox1.Image);
                Bitmap processedImage = ProcessRGBChannels(image);
                pictureBox2.Image = processedImage;
            }
        }

        private Bitmap ProcessRGBChannels(Bitmap image)
        {
  //bir görüntünün kırmızı, yeşil ve mavi kanallarını ayrı ayrı çıkarıp işledikten sonra bu kanalları birleştirerek yeni bir görüntü oluşturuyoruz.
                        Bitmap redChannel = ExtractChannel(image, 0);
            Bitmap greenChannel = ExtractChannel(image, 1);
            Bitmap blueChannel = ExtractChannel(image, 2);

            

            Bitmap combinedImage = CombineChannels(redChannel, greenChannel, blueChannel);
            return combinedImage;
        }

        private Bitmap ExtractChannel(Bitmap image, int channel)
        {
            //görüntünün belirtilen kanalındaki(kırmızı, yeşil veya mavi) değerleri kullanarak gri tonlamalı yeni bir görüntü oluşturur.
            Bitmap channelImage = new Bitmap(image.Width, image.Height);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int value = (channel == 0) ? pixel.R : (channel == 1) ? pixel.G : pixel.B;
                    channelImage.SetPixel(x, y, Color.FromArgb(value, value, value));
                }
            }
            return channelImage;
        }

        private Bitmap CombineChannels(Bitmap redChannel, Bitmap greenChannel, Bitmap blueChannel)
        {
            //kırmızı, yeşil ve mavi kanalları içeren görüntüleri birleştirerek orijinal renkli görüntüyü yeniden oluşturur.
            Bitmap combinedImage = new Bitmap(redChannel.Width, redChannel.Height);
            for (int x = 0; x < combinedImage.Width; x++)
            {
                for (int y = 0; y < combinedImage.Height; y++)
                {
                    int r = redChannel.GetPixel(x, y).R;
                    int g = greenChannel.GetPixel(x, y).G;
                    int b = blueChannel.GetPixel(x, y).B;
                    combinedImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return combinedImage;
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
    }
}
