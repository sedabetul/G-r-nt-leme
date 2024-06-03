using Accord.Imaging.Filters;
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
using System.Drawing.Imaging;

namespace GoruntuIsleme.Forms
{
    public partial class Bulaniklastir : Form
    {
        public Bulaniklastir()
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
                Bitmap bulanikresimm = GaussFiltre(image);
                pictureBox2.Image = bulanikresimm;
            }
        }



        private Bitmap GaussFiltre(Bitmap image)
        {
            //belirtilen sigma ve kernel boyutuna göre bir Gaussian kernel oluşturur
            double[,] kernel = GenerateGaussianKernel(4.0, 11); // sigma ve kernel boyutu
            return ApplyConvolution(image, kernel);
        }

        private double[,] GenerateGaussianKernel(double sigma, int size)
        {
            //verilen sigma ve boyuta göre bir Gaussian kernel oluşturur.
            double[,] kernel = new double[size, size];
            double sum = 0.0;
            int halfSize = size / 2;
            double twoSigmaSquare = 2.0 * sigma * sigma;
            double piSigma = 2.0 * Math.PI * sigma * sigma;

            for (int x = -halfSize; x <= halfSize; x++)
            {
                for (int y = -halfSize; y <= halfSize; y++)
                {
                    double r = x * x + y * y;
                    kernel[x + halfSize, y + halfSize] = Math.Exp(-r / twoSigmaSquare) / piSigma;
                    sum += kernel[x + halfSize, y + halfSize];
                }
            }

            
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    kernel[x, y] /= sum;
                }
            }

            return kernel;
        }

        private Bitmap ApplyConvolution(Bitmap image, double[,] kernel)
        {
            //görüntü pikselleri üzerinde kernel'i kaydırarak her piksel için yeni değeri hesaplar ve sonuç görüntüsünü oluşturulur.
            int width = image.Width;
            int height = image.Height;
            int kernelSize = kernel.GetLength(0);
            int halfKernelSize = kernelSize / 2;

            Bitmap result = new Bitmap(width, height);

            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            IntPtr srcScan0 = srcData.Scan0;
            IntPtr dstScan0 = dstData.Scan0;

            int bytesPerPixel = 3;
            byte[] pixelBuffer = new byte[srcData.Stride * srcData.Height];
            byte[] resultBuffer = new byte[dstData.Stride * dstData.Height];

            System.Runtime.InteropServices.Marshal.Copy(srcScan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = halfKernelSize; y < height - halfKernelSize; y++)
            {
                for (int x = halfKernelSize; x < width - halfKernelSize; x++)
                {
                    double r = 0.0, g = 0.0, b = 0.0;

                    for (int ky = -halfKernelSize; ky <= halfKernelSize; ky++)
                    {
                        for (int kx = -halfKernelSize; kx <= halfKernelSize; kx++)
                        {
                            int pixelX = x + kx;
                            int pixelY = y + ky;

                            int pixelIndex = (pixelY * stride) + (pixelX * bytesPerPixel);
                            double kernelValue = kernel[kx + halfKernelSize, ky + halfKernelSize];

                            b += pixelBuffer[pixelIndex] * kernelValue;
                            g += pixelBuffer[pixelIndex + 1] * kernelValue;
                            r += pixelBuffer[pixelIndex + 2] * kernelValue;
                        }
                    }

                    int resultIndex = (y * stride) + (x * bytesPerPixel);

                    resultBuffer[resultIndex] = (byte)Math.Min(Math.Max(b, 0), 255);
                    resultBuffer[resultIndex + 1] = (byte)Math.Min(Math.Max(g, 0), 255);
                    resultBuffer[resultIndex + 2] = (byte)Math.Min(Math.Max(r, 0), 255);
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstScan0, resultBuffer.Length);

            image.UnlockBits(srcData);
            result.UnlockBits(dstData);

            return result;
        }

        private void Bulaniklastir_Load(object sender, EventArgs e)
        {

        }
    }
}
