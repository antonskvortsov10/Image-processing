﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap bmp = null;
        byte i1, i2, i3;
        Image MemForImage;



        private void LoadImage(bool Bmp)
        {
            openFileDialog1.InitialDirectory = "";

            if (Bmp)
                openFileDialog1.Filter = "image (BMP) files (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    MemForImage = Image.FromFile(openFileDialog1.FileName);
                    LoadedPictureBox.Image = MemForImage;
                    bmp = (Bitmap)LoadedPictureBox.Image;
                    GrayscaleProcessButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось загрузить файл: " + ex.Message);
                }
            }
        }

        private void GrayscaleProcessButton_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)LoadedPictureBox.Image;
            double N1, N2, N3;
            N1 = RTrackBar.Value;
            N2 = GTrackBar.Value;
            N3 = BTrackBar.Value;
            bool ok = true;
            if (N1 == 0 && N2 == 0 && N3 == 0)
                ok = false;
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color col = bmp.GetPixel(x, y);
                    byte r, g, b;
                    r = col.R;
                    g = col.G;
                    b = col.B;
                    byte gray;

                    if (ok)
                        // Formula using input coefficients:
                        gray = (byte)((N1 / (N1 + N2 + N3)) * r + (N2 / (N1 + N2 + N3)) * g + (N3 / (N1 + N2 + N3)) * b);
                    else
                        // Standart formula for conversion:
                        // byte gray = (byte)(0.3 * r + 0.59 * g + 0.11 * b);
                        gray = (byte)(0.3 * r + 0.59 * g + 0.11 * b);
                    bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            ShowResultImageButton.Enabled = true;
        }

        private void ShowResultImageButton_Click(object sender, EventArgs e)
        {
            ProcessedPictureBox.Image = bmp;
            ChartButton.Enabled = true;
        }

        private void RButton_Click(object sender, EventArgs e)
        {
            i1 = (byte)RTrackBar.Value;
            textBox1.Text = Convert.ToString(i1);
        }

        private void GButton_Click(object sender, EventArgs e)
        {
            i2 = (byte)GTrackBar.Value;
            textBox2.Text = Convert.ToString(i2);
        }

        private void BButton_Click(object sender, EventArgs e)
        {
            i3 = (byte)BTrackBar.Value;
            textBox3.Text = Convert.ToString(i3);
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            LoadImage(true);
        }

        private void ChartButton_Click(object sender, EventArgs e)
        {
            Bitmap I = (Bitmap)ProcessedPictureBox.Image;
            byte gray;
            int[] X = new int[256];
            for (int i = 0; i < 256; i++)
                X[i] = i;
            int[] H = new int[256];
            for (int i = 0; i < 256; i++)
                H[i] = 0;
            for (int x = 0; x < I.Width; x++ )
            {
                for (int y = 0; y < I.Height; y++)
                {
                    gray = I.GetPixel(x, y).R;
                    H[gray]++;
                }
            }

            this.GrayscaleChart.Series["Grayscale"].Points.DataBindXY(X, H);
            for (int i = 0; i < 256; i++)
            {
                this.GrayscaleChart.Series["Grayscale"].Points[i].Color = Color.FromArgb(i, i, i);
            }
        }
    }
}
