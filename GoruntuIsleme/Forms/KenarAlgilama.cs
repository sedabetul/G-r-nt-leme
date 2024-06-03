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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GoruntuIsleme.Forms
{
    public partial class KenarAlgilama : Form
    {
        public KenarAlgilama()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Show();
            this.Hide();
        }

        private void KenarAlgilama_Load(object sender, EventArgs e)
        {

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
                Bitmap edgeImage = ApplyCanny(image);
                pictureBox2.Image = edgeImage;
            }
        }

        private Bitmap ApplyCanny(Bitmap image)
        {
            Bitmap grayImage = Grayscale(image);
            Bitmap blurredImage = GaussianBlur(grayImage);
            (Bitmap gradientImage, double[,] direction) = SobelFilter(blurredImage);
            Bitmap suppressedImage = NonMaximumSuppression(gradientImage, direction);
            Bitmap edgeImage = DoubleThresholdAndHysteresis(suppressedImage, 20, 40);
            return edgeImage;
        }

        private Bitmap Grayscale(Bitmap image)
        {
            Bitmap grayImage = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixelColor = image.GetPixel(i, j);
                    int grayValue = (int)(pixelColor.R * 0.2125 + pixelColor.G * 0.7154 + pixelColor.B * 0.0721);
                    grayImage.SetPixel(i, j, Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }
            return grayImage;
        }
        private Bitmap GaussianBlur(Bitmap image)
        {
            // 5x5 Gaussian bulanıklık çekirdeği kullanarak görüntüyü bulanıklaştırır ve yeni bir bulanıklaştırılmış görüntü oluşturur.

            
            double[,] kernel = {
                { 1, 4, 7, 4, 1 },
                { 4, 16, 26, 16, 4 },
                { 7, 26, 41, 26, 7 },
                { 4, 16, 26, 16, 4 },
                { 1, 4, 7, 4, 1 }
            };

            int kernelSize = 5;
            double kernelSum = 273; 

            Bitmap blurredImage = new Bitmap(image.Width, image.Height);
            for (int x = 2; x < image.Width - 2; x++)
            {
                for (int y = 2; y < image.Height - 2; y++)
                {
                    double sum = 0;
                    for (int i = -2; i <= 2; i++)
                    {
                        for (int j = -2; j <= 2; j++)
                        {
                            Color pixel = image.GetPixel(x + i, y + j);
                            sum += pixel.R * kernel[i + 2, j + 2];
                        }
                    }
                    int newValue = (int)(sum / kernelSum);
                    blurredImage.SetPixel(x, y, Color.FromArgb(newValue, newValue, newValue));
                }
            }
            return blurredImage;
        }

        private (Bitmap, double[,]) SobelFilter(Bitmap image)
        {
            //önce görüntüyü gri tonlamalı hale getirir, sonra bulanıklaştırır
            int width = image.Width;
            int height = image.Height;

            int[,] gx = new int[,]
            {
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 1 }
            };

            int[,] gy = new int[,]
            {
                { 1, 2, 1 },
                { 0, 0, 0 },
                { -1, -2, -1 }
            };

            Bitmap gradientImage = new Bitmap(width, height);
            double[,] direction = new double[width, height];

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    double gxSum = 0;
                    double gySum = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int pixel = image.GetPixel(x + i, y + j).R;
                            gxSum += pixel * gx[i + 1, j + 1];
                            gySum += pixel * gy[i + 1, j + 1];
                        }
                    }

                    double magnitude = Math.Sqrt(gxSum * gxSum + gySum * gySum);
                    int value = (int)(magnitude > 255 ? 255 : magnitude);

                    gradientImage.SetPixel(x, y, Color.FromArgb(value, value, value));

                    direction[x, y] = Math.Atan2(gySum, gxSum);
                }
            }

            return (gradientImage, direction);
        }

        private Bitmap NonMaximumSuppression(Bitmap gradientImage, double[,] direction)
        {
            //sadece en güçlü kenar piksellerini koruyarak yeni bir görüntü oluşturur.
            int width = gradientImage.Width;
            int height = gradientImage.Height;
            Bitmap suppressedImage = new Bitmap(width, height);

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    double angle = direction[x, y] * (180.0 / Math.PI);
                    angle = angle < 0 ? angle + 180 : angle;

                    int pixelValue = gradientImage.GetPixel(x, y).R;
                    int q = 255;
                    int r = 255;

                    if ((angle >= 0 && angle < 22.5) || (angle >= 157.5 && angle <= 180))
                    {
                        q = gradientImage.GetPixel(x + 1, y).R;
                        r = gradientImage.GetPixel(x - 1, y).R;
                    }
                    else if (angle >= 22.5 && angle < 67.5)
                    {
                        q = gradientImage.GetPixel(x + 1, y - 1).R;
                        r = gradientImage.GetPixel(x - 1, y + 1).R;
                    }
                    else if (angle >= 67.5 && angle < 112.5)
                    {
                        q = gradientImage.GetPixel(x, y + 1).R;
                        r = gradientImage.GetPixel(x, y - 1).R;
                    }
                    else if (angle >= 112.5 && angle < 157.5)
                    {
                        q = gradientImage.GetPixel(x - 1, y - 1).R;
                        r = gradientImage.GetPixel(x + 1, y + 1).R;
                    }

                    if (pixelValue >= q && pixelValue >= r)
                    {
                        suppressedImage.SetPixel(x, y, Color.FromArgb(pixelValue, pixelValue, pixelValue));
                    }
                    else
                    {
                        suppressedImage.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return suppressedImage;
        }

        private Bitmap DoubleThresholdAndHysteresis(Bitmap suppressedImage, int lowThreshold, int highThreshold)
        {
            //görüntüyü yüksek ve düşük eşik değerlerine göre işleyerek kenar tespitini gerçekleştirir
            int width = suppressedImage.Width;
            int height = suppressedImage.Height;
            Bitmap edgeImage = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int pixelValue = suppressedImage.GetPixel(x, y).R;
                    if (pixelValue >= highThreshold)
                    {
                        edgeImage.SetPixel(x, y, Color.White);
                    }
                    else if (pixelValue >= lowThreshold)
                    {
                        edgeImage.SetPixel(x, y, Color.Gray);
                    }
                    else
                    {
                        edgeImage.SetPixel(x, y, Color.Black);
                    }
                }
            }
               
                for (int x = 1; x < width - 1; x++)
                {
                    for (int y = 1; y < height - 1; y++)
                    {
                        if (edgeImage.GetPixel(x, y).R == Color.Gray.R)
                        {
                            bool strongEdgeFound = false;
                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    if (edgeImage.GetPixel(x + i, y + j).R == Color.White.R)
                                    {
                                        strongEdgeFound = true;
                                        break;
                                    }
                                }
                                if (strongEdgeFound)
                                {
                                    break;
                                }
                            }
                            if (strongEdgeFound)
                            {
                                edgeImage.SetPixel(x, y, Color.White);
                            }
                            else
                            {
                                edgeImage.SetPixel(x, y, Color.Black);
                            }
                        }
                    }
                }

                return edgeImage;
            }
        }
    }






        
               
