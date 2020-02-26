using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
//using Accord.Controls;
//using Accord.IO;
//using Accord.Math;
//using Accord.Statistics.Distributions.Univariate;
//using Accord.MachineLearning.Bayes;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Bitmap OriginalImage;
        public Form1()
        {
            InitializeComponent();
        }

        public Bitmap OriginalImage1 { get => OriginalImage; set => OriginalImage = value; }
        int count,Total=0;
        int redt, greent, bluet;
        int[,] zone = new int[6, 9];
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenImage = new OpenFileDialog();
            var returnvalue = OpenImage.ShowDialog();
            if (returnvalue == DialogResult.OK)
            {
                OriginalImage1 = new Bitmap(OpenImage.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = OriginalImage1;
            }
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            GrayscaleBT709 GrayImage = new GrayscaleBT709();
            Bitmap graycolor = GrayImage.Apply(OriginalImage);
            pictureBox2.Image = graycolor;
        }

        private void binaryImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            GrayscaleBT709 GrayImage = new GrayscaleBT709();
            Bitmap graycolor = GrayImage.Apply(OriginalImage);
            BinaryDilatation3x3 BinaryImage = new BinaryDilatation3x3();
            Bitmap binaryimage = BinaryImage.Apply(graycolor);
            pictureBox3.Image = binaryimage;
        }

        private void binaryELUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            GrayscaleBT709 GrayImage = new GrayscaleBT709();
            Bitmap graycolor = GrayImage.Apply(OriginalImage);
            BinaryDilatation3x3 BinaryImageE = new BinaryDilatation3x3();
            Bitmap binaryimagee = BinaryImageE.Apply(graycolor);
            pictureBox4.Image = binaryimagee;
        }

      /*  private void dataSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Read the Excel worksheet into a DataTable
            DataTable table = new ExcelReader("examples.xls").GetWorksheet("Classification - Yin Yang");

            // Convert the DataTable to input and output vectors
            //double[][] inputs = table.ToArray<double>("X", "Y");
            int[] outputs = table.Columns["G"].ToArray<int>();

            // Plot the data
            ScatterplotBox.Show("Device", zone, outputs).Hold();
        }*/

        private void bayerFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            GrayscaleBT709 GrayImage = new GrayscaleBT709();
            Bitmap graycolor = GrayImage.Apply(OriginalImage);
            BayerFilter BayerImage = new BayerFilter();
            Bitmap bayerimage = BayerImage.Apply(graycolor);
            pictureBox5.Image = bayerimage;
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResizeBilinear ResizedImage = new ResizeBilinear(90, 60);
            Bitmap resizeimage = ResizedImage.Apply(OriginalImage);
            pictureBox6.Image = resizeimage;
        }

        private void divideZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            ResizeBilinear ResizedImage = new ResizeBilinear(60, 90);
            Bitmap resizeimage = ResizedImage.Apply(OriginalImage);
            GrayscaleBT709 GrayImage = new GrayscaleBT709();
            Bitmap graycolor = GrayImage.Apply(OriginalImage);
            //BinaryDilatation3x3 BinaryImage = new BinaryDilatation3x3();
            Threshold BinaryImage = new Threshold();
            Bitmap binaryimage = BinaryImage.Apply(graycolor);
            int a = 0;
            //List<int> zone = new List<int>();
            for (int k = 0; k < 6; k++)
            {
                for (int l = 0; l < 9; l++)
                {
                    count = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            Color pixelColor = binaryimage.GetPixel(i + (10*k), j + (10*l));
                            redt = pixelColor.R;
                            greent = pixelColor.G;
                            bluet = pixelColor.B;
                            if ((redt & greent & bluet) == 0)
                            {
                                count++;
                                Total++;
                            }
                            
                        }
                        
                    }

                    zone[k, l] = count;
                }
                
            }
            for (int k = 0; k < 6; k++)
                for(int l=0; l < 9; l++)
            {
                    Console.WriteLine("Zone[{0},{1}] = {2}", k,l, zone[k,l]);
            }
            Console.WriteLine(Total);
        }

    }
}
