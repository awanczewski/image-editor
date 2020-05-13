using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor
{
    public partial class Form1 : Form
    {
        double gamma = 1;
        private int width = 0;
        private int height = 0;
        Bitmap oldBitmap;
        Bitmap newBitmap;
        Color k;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                width = pictureBox1.Image.Width;
                height = pictureBox1.Image.Height;
                oldBitmap = (Bitmap)pictureBox1.Image;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Bitmap newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            gamma = trackBar1.Value / 100.0;
            label1.Text = (trackBar1.Value / 100.0).ToString();
            byte[] LUT = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                if ((255 * Math.Pow(i / 255.0, 1 / gamma)) > 255)
                {
                    LUT[i] = 255;
                }
                else
                {
                    LUT[i] = (byte)(255 * Math.Pow(i / 255.0, 1 / gamma));
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    k = oldBitmap.GetPixel(x, y);
                    newBitmap.SetPixel(x, y, Color.FromArgb((int)LUT[k.R], (int)LUT[k.G], (int)LUT[k.B]));
                }
            }
            pictureBox1.Image = newBitmap;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            byte[] LUT = new byte[256];
            double a;
            label2.Text = trackBar2.Value.ToString();
            if (trackBar2.Value <= 0)
            {
                a = 1.0 + (trackBar2.Value / 256.0);
            }
            else
            {
                a = 256.0 / Math.Pow(2, Math.Log(257 - trackBar2.Value, 2));
            }
            for (int i = 0; i < 256; i++)
            {
                if ((a * (i - 127) + 127) > 255)
                {
                    LUT[i] = 255;
                }
                else if ((a * (i - 127) + 127) < 0)
                {
                    LUT[i] = 0;
                }
                else
                {
                    LUT[i] = (byte)(a * (i - 127) + 127);
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    k = oldBitmap.GetPixel(x, y);
                    newBitmap.SetPixel(x, y, Color.FromArgb((int)LUT[k.R], (int)LUT[k.G], (int)LUT[k.B]));
                }
            }
            pictureBox1.Image = newBitmap;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            drawHistogramRed();
            drawHistogramGreen();
            drawHistogramBlue();
        }

        void drawHistogramRed()
        {
            int n;
            int h = pictureBox2.Height;
            int[] tab = new int[256];
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = 0;
            }
            Color pixel;
            int value;
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                    value = (pixel.R);
                    tab[value]++;

                }
            }
            int max = tab[0];
            for (int x = 1; x < 256; x++)
            {
                if (tab[x] > max)
                {
                    max = tab[x];
                }
            }
            Bitmap hist = new Bitmap(256, h);
            for (int x = 0; x < 256; x++)
            {
                n = (h * tab[x]) / max;
                for (int y = n - 1; y >= 0; y--)
                {
                    hist.SetPixel(x, y, Color.Red);

                }
            }
            hist.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureBox2.Image = hist;
        }

        void drawHistogramGreen()
        {
            int n;
            int h = pictureBox3.Height;
            int[] tab = new int[256];
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = 0;
            }
            Color pixel;
            int value;
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                    value = (pixel.G);
                    tab[value]++;

                }
            }
            int max = tab[0];
            for (int x = 1; x < 256; x++)
            {
                if (tab[x] > max)
                {
                    max = tab[x];
                }
            }
            Bitmap hist = new Bitmap(256, h);
            for (int x = 0; x < 256; x++)
            {
                n = (h * tab[x]) / max;
                for (int y = n - 1; y >= 0; y--)
                {
                    hist.SetPixel(x, y, Color.Green);

                }
            }
            hist.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureBox3.Image = hist;
        }

        void drawHistogramBlue()
        {
            int n;
            int h = pictureBox4.Height;
            int[] tab = new int[256];
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = 0;
            }
            Color pixel;
            int value;
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                    value = (pixel.B);
                    tab[value]++;

                }
            }
            int max = tab[0];
            for (int x = 1; x < 256; x++)
            {
                if (tab[x] > max)
                {
                    max = tab[x];
                }
            }
            Bitmap hist = new Bitmap(256, h);
            for (int x = 0; x < 256; x++)
            {
                n = (h * tab[x]) / max;
                for (int y = n - 1; y >= 0; y--)
                {
                    hist.SetPixel(x, y, Color.Blue);

                }
            }
            hist.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureBox4.Image = hist;
        }

        void filter(int[] masks)
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            int r, g, b;
            Color[] colors = new Color[9];
            int sum = masks[0] + masks[1] + masks[2] + masks[3] + masks[4] + masks[5] + masks[6] + masks[7] + masks[8];
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    r = 0;
                    g = 0;
                    b = 0;
                    colors[0] = oldBitmap.GetPixel(x - 1, y - 1);
                    colors[1] = oldBitmap.GetPixel(x, y - 1);
                    colors[2] = oldBitmap.GetPixel(x + 1, y - 1);
                    colors[3] = oldBitmap.GetPixel(x - 1, y);
                    colors[4] = oldBitmap.GetPixel(x, y);
                    colors[5] = oldBitmap.GetPixel(x + 1, y);
                    colors[6] = oldBitmap.GetPixel(x - 1, y + 1);
                    colors[7] = oldBitmap.GetPixel(x, y + 1);
                    colors[8] = oldBitmap.GetPixel(x + 1, y + 1);
                    for (int i = 0; i < 9; i++)
                    {
                        r += (colors[i].R * masks[i]);
                        g += (colors[i].G * masks[i]);
                        b += (colors[i].B * masks[i]);
                    }
                    if (sum != 0)
                    {
                        r /= sum;
                        g /= sum;
                        b /= sum;
                    }
                    if (r > 255)
                    {
                        r = 255;
                    }
                    else if (r < 0)
                    {
                        r = 0;
                    }
                    if (g > 255)
                    {
                        g = 255;
                    }
                    else if (g < 0)
                    {
                        g = 0;
                    }
                    if (b > 255)
                    {
                        b = 255;
                    }
                    else if (b < 0)
                    {
                        b = 0;
                    }
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            pictureBox1.Image = newBitmap;
        }

        private void button6_Click(object sender, EventArgs e)     //UŚREDNIENIE
        {
            int[] masks = new int[]
            {
                1, 1, 1,
                1, 1, 1,
                1, 1, 1
            };
            filter(masks);
        }

        private void button7_Click(object sender, EventArgs e)  //GAUSS
        {
            int[] masks = new int[]
            {
                1, 2, 1,
                2, 4, 2,
                1, 2, 1
            };
            filter(masks);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int[] masks = new int[]
            {
                 1,  2,  1,
                 0,  0,  0,
                -1, -2, -1
            };
            filter(masks);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int[] masks = new int[]
            {
                -1, -1, -1,
                 0,  0,  0,
                 1,  1,  1
            };
            filter(masks);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int[] masks = new int[]
            {
                 0,  0, 0,
                 0,  1, 0,
                 0, -1, 0
            };
            filter(masks);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] masks = new int[]
            {
            (int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value,
            (int)numericUpDown6.Value, (int)numericUpDown7.Value, (int)numericUpDown8.Value, (int)numericUpDown9.Value
            };
            filter(masks);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            numericUpDown9.Value = 0;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            byte[] LUTR = new byte[256];
            byte[] LUTG = new byte[256];
            byte[] LUTB = new byte[256];
            LUTR = stretchHistogramRed();
            LUTG = stretchHistogramGreen();
            LUTB = stretchHistogramBlue();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    k = oldBitmap.GetPixel(x, y);
                    newBitmap.SetPixel(x, y, Color.FromArgb((int)LUTR[k.R], (int)LUTG[k.G], (int)LUTB[k.B]));
                }
            }
            pictureBox1.Image = newBitmap;

            drawHistogramRed();
            drawHistogramGreen();
            drawHistogramBlue();
        }

        byte[] stretchHistogramRed()
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            byte[] LUT = new byte[256];
            Color pixel;
            int value;
            int[] tab = new int[256];
            int vMax = ((Bitmap)pictureBox1.Image).GetPixel(0, 0).R;
            int vMin = ((Bitmap)pictureBox1.Image).GetPixel(0, 0).R;
            int iMax = 255;
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = 0;
            }
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                    value = (pixel.R);
                    tab[value]++;
                    if (value > vMax)
                    {
                        vMax = value;
                    }
                    if (value < vMin)
                    {
                        vMin = value;
                    }
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if ( ((iMax) / (vMax - vMin)) * (i - vMin) > 255)
                {
                    LUT[i] = 255;
                }
                else if ( ((iMax) / (vMax - vMin)) * (i - vMin) < 0)
                {
                    LUT[i] = 0;
                }
                else
                {
                    LUT[i] = (byte)(((iMax) / (vMax - vMin)) * (i - vMin));
                }
            }
            return LUT;
        }

        byte[] stretchHistogramGreen()
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            byte[] LUT = new byte[256];
            Color pixel;
            int value;
            int[] tab = new int[256];
            int vMax = ((Bitmap)pictureBox1.Image).GetPixel(0, 0).G;
            int vMin = ((Bitmap)pictureBox1.Image).GetPixel(0, 0).G;
            int iMax = 255;
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = 0;
            }
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                    value = (pixel.G);
                    tab[value]++;
                    if (value > vMax)
                    {
                        vMax = value;
                    }
                    if (value < vMin)
                    {
                        vMin = value;
                    }
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (((iMax) / (vMax - vMin)) * (i - vMin) > 255)
                {
                    LUT[i] = 255;
                }
                else if (((iMax) / (vMax - vMin)) * (i - vMin) < 0)
                {
                    LUT[i] = 0;
                }
                else
                {
                    LUT[i] = (byte)(((iMax) / (vMax - vMin)) * (i - vMin));
                }
            }
            return LUT;
        }

        byte[] stretchHistogramBlue()
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            byte[] LUT = new byte[256];
            Color pixel;
            int value;
            int[] tab = new int[256];
            int vMax = ((Bitmap)pictureBox1.Image).GetPixel(0, 0).B;
            int vMin = ((Bitmap)pictureBox1.Image).GetPixel(0, 0).B;
            int iMax = 255;
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = 0;
            }
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                    value = (pixel.B);
                    tab[value]++;
                    if (value > vMax)
                    {
                        vMax = value;
                    }
                    if (value < vMin)
                    {
                        vMin = value;
                    }
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (((iMax) / (vMax - vMin)) * (i - vMin) > 255)
                {
                    LUT[i] = 255;
                }
                else if (((iMax) / (vMax - vMin)) * (i - vMin) < 0)
                {
                    LUT[i] = 0;
                }
                else
                {
                    LUT[i] = (byte)(((iMax) / (vMax - vMin)) * (i - vMin));
                }
            }
            return LUT;
        }
        byte[] alignHistogramRed()
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            byte[] LUT = new byte[256];
            Color pixel;
            int value;
            int[] tab = new int[256];
            double d0 = 0;
            double sum = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = 0;
            }
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                    value = (pixel.R);
                    tab[value]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (tab[i] != 0)
                {
                    d0 = tab[i];
                    break;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                sum += tab[i];
                LUT[i] = (byte)(((sum - d0) / ((width * height) - d0)) * 255);
            }
            return LUT;
        }

        byte[] alignHistogramGreen()
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            byte[] LUT = new byte[256];
            Color pixel;
            int value;
            int[] tab = new int[256];
            double d0 = 0;
            double sum = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = 0;
            }
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                    value = (pixel.G);
                    tab[value]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (tab[i] != 0)
                {
                    d0 = tab[i];
                    break;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                sum += tab[i];
                LUT[i] = (byte)(((sum - d0) / ((width * height) - d0)) * 255);
            }
            return LUT;
        }

        byte[] alignHistogramBlue()
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            byte[] LUT = new byte[256];
            Color pixel;
            int value;
            int[] tab = new int[256];
            double d0 = 0;
            double sum = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = 0;
            }
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                    value = (pixel.B);
                    tab[value]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (tab[i] != 0)
                {
                    d0 = tab[i];
                    break;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                sum += tab[i];
                LUT[i] = (byte)(((sum - d0) / ((width * height) - d0)) * 255);
            }
            return LUT;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            byte[] LUTR = alignHistogramRed();
            byte[] LUTG = alignHistogramGreen();
            byte[] LUTB = alignHistogramBlue();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    k = oldBitmap.GetPixel(x, y);
                    newBitmap.SetPixel(x, y, Color.FromArgb((int)LUTR[k.R], (int)LUTG[k.G], (int)LUTB[k.B]));
                }
            }
            pictureBox1.Image = newBitmap;

            drawHistogramRed();
            drawHistogramGreen();
            drawHistogramBlue();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            int[] masks = new int[]
            {
                 0, -1,  0,
                -1,  5, -1,
                 0, -1,  0
            };
            filter(masks);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            Color temp;
            for (int x = 1; x < pictureBox1.Image.Width -1; x++)
            {
                for (int y = 1; y < pictureBox1.Image.Height -1; y++)
                {
                    List<int> r = new List<int>();
                    List<int> g = new List<int>();
                    List<int> b = new List<int>();
                    for (int j = x - 1; j < x + 1; j++)
                    {
                        for(int k = y - 1; k < y + 1; k++)
                        {
                            temp = oldBitmap.GetPixel(j, k);
                            r.Add(temp.R);
                            g.Add(temp.G);
                            b.Add(temp.B);
                        }
                    }
                    r.Sort();
                    g.Sort();
                    b.Sort();
                    newBitmap.SetPixel(x, y, Color.FromArgb(r[0], g[0], b[0]));
                }
            }
            pictureBox1.Image = newBitmap;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            Color temp;
            for (int x = 1; x < pictureBox1.Image.Width - 1; x++)
            {
                for (int y = 1; y < pictureBox1.Image.Height - 1; y++)
                {
                    List<int> r = new List<int>();
                    List<int> g = new List<int>();
                    List<int> b = new List<int>();
                    for (int j = x - 1; j < x + 1; j++)
                    {
                        for (int k = y - 1; k < y + 1; k++)
                        {
                            temp = oldBitmap.GetPixel(j, k);
                            r.Add(temp.R);
                            g.Add(temp.G);
                            b.Add(temp.B);
                        }
                    }
                    r.Sort();
                    g.Sort();
                    b.Sort();         
                    newBitmap.SetPixel(x, y, Color.FromArgb(r[r.Count -1], g[g.Count - 1], b[b.Count -1]));
                }
            }
            pictureBox1.Image = newBitmap;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            Color temp;
            for (int x = 1; x < pictureBox1.Image.Width - 1; x++)
            {
                for (int y = 1; y < pictureBox1.Image.Height - 1; y++)
                {
                    List<int> r = new List<int>();
                    List<int> g = new List<int>();
                    List<int> b = new List<int>();
                    for (int j = x - 1; j < x + 1; j++)
                    {
                        for (int k = y - 1; k < y + 1; k++)
                        {
                            temp = oldBitmap.GetPixel(j, k);
                            r.Add(temp.R);
                            g.Add(temp.G);
                            b.Add(temp.B);
                        }
                    }
                    r.Sort();
                    g.Sort();
                    b.Sort();
                    newBitmap.SetPixel(x, y, Color.FromArgb(r[r.Count / 2], g[g.Count / 2], b[b.Count / 2]));
                }
            }
            pictureBox1.Image = newBitmap;
        }
    }
}
