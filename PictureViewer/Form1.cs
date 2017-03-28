using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PictureViewer
{
    public partial class Form1 : Form
    {
        Image originalImage = null;
        const int brightnessConstant = 10;
        const int contrastConstant = 5;
        const double gammaConstant = 0.9;
        public static List<List<int>> kernel = new List<List<int>> { new List<int> {1,1,1},
                                                                     new List<int> {1,1,1},
                                                                     new List<int> {1,1,1}
                                                                    };
        public List<List<float>> diffusionFilter = new List<List<float>>
        {
            new List<float> {0 , 0 , 0 },
            new List<float> {0 , 0 , 7/16f },
            new List<float> { 3/16f , 5/16f, 1/16f }
        };


        public int anchorX = 1;
        public int anchorY = 1;
        public int offSet = 0;
        public int divisor = 1;
        public Form1()
        {
            InitializeComponent();
            List<int> widths = new List<int> { 3, 4, 5, 6, 7, 8, 9 };
            List<int> heights = new List<int> { 3, 4, 5, 6, 7, 8, 9 };
            comboBox1.DataSource = widths;
            comboBox2.DataSource = heights;
            List<string> convolutionTypes = new List<string> { "blur", "gaussian", "sharpen", "edge detection", "emboss" };
            List<string> diffusionTypes = new List<string> { "Floyd", "Burkes", "Stucky", "Sierra", "Atkinson" };
            comboBox3.DataSource = convolutionTypes;
            comboBox4.DataSource = diffusionTypes;

            textBox1.Text = offSet.ToString();
            textBox2.Text = anchorX.ToString();
            textBox3.Text = anchorY.ToString();
            textBox4.Text = divisor.ToString();

        }

        public string getcurrentConvolution()
        {
            return (string)comboBox3.SelectedItem;
        }

        public string getcurrentDiffusion()
        {
            return (string)comboBox4.SelectedItem;
        }

        public static int wrap(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }



        private void showButton_Click(object sender, EventArgs e)
        {
            // Show the Open File dialog. If the user chooses OK, load the 
            // picture that the user chose.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                pictureBox2.Load(openFileDialog1.FileName);
                originalImage = Image.FromFile(openFileDialog1.FileName);

            }

        }



        private void clearButton_Click(object sender, EventArgs e)
        {
            // Clear the picture.
            pictureBox1.Image = null;
            pictureBox2.Image = null;
        }

        private void backgroundButton_Click(object sender, EventArgs e)
        {
            // Show the color picker dialog box. If the user chooses OK, change the 
            // PictureBox control's background to the color the user chose. 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                pictureBox1.BackColor = colorDialog1.Color;
        }






        public static Bitmap AdjustBrightness(Image image, int Value)
        {
            Bitmap bmp = (Bitmap)image;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    Color newPixel = Color.FromArgb(wrap(pixel.R + Value, 0, 255),
                                                    wrap(pixel.G + Value, 0, 255),
                                                    wrap(pixel.B + Value, 0, 255));
                    bmp.SetPixel(i, j, newPixel);
                }
            }
            return bmp;

        }

        public static Bitmap AdjustContrast(Image image, int Value)
        {
            Bitmap bmp = (Bitmap)image;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    Color newPixel = Color.FromArgb(wrap((int)(contrastConstant * (pixel.R - 0.5) + Value), 0, 255),
                                                    wrap((int)(contrastConstant * (pixel.G - 0.5) + Value), 0, 255),
                                                    wrap((int)(contrastConstant * (pixel.B - 0.5) + Value), 0, 255));
                    bmp.SetPixel(i, j, newPixel);
                }
            }

            return bmp;

        }

        public static Bitmap AdjustGamma(Image image, double Value)
        {
            Bitmap bmp = (Bitmap)image;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    Color newPixel = Color.FromArgb(wrap((int)Math.Pow(pixel.R, Value), 0, 255),
                                                    wrap((int)Math.Pow(pixel.R, Value), 0, 255),
                                                    wrap((int)Math.Pow(pixel.R, Value), 0, 255));
                    bmp.SetPixel(i, j, newPixel);
                }
            }

            return bmp;

        }


        public static Bitmap Negation(Image image)
        {
            Bitmap bmp = (Bitmap)image;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    Color newPixel = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    bmp.SetPixel(i, j, newPixel);
                }
            }

            return bmp;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = AdjustBrightness(originalImage, brightnessConstant);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = AdjustBrightness(originalImage, -1 * brightnessConstant);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = AdjustContrast(originalImage, contrastConstant);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = AdjustGamma(originalImage, gammaConstant);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Negation(originalImage);
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            offSet = (textBox1.Text == null ? Convert.ToInt32(textBox1.Text) : 0);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            anchorX = (textBox2.Text == null ? Convert.ToInt32(textBox2.Text) : 1);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            anchorY = (textBox3.Text == null ? Convert.ToInt32(textBox3.Text) : 1);
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedItem.ToString())
            {
                case "blur":
                    kernel.Clear();
                    kernel = new List<List<int>> { new List<int> { 1, 1, 1 },
                                                   new List<int> { 1, 1, 1 },
                                                   new List<int> { 1, 1, 1 } };
                    break;
                case "gaussian":
                    kernel.Clear();
                    kernel = new List<List<int>> { new List<int> { 0, 1, 0 },
                                                   new List<int> { 1, 4, 1 },
                                                   new List<int> { 0, 1, 0 } };
                    break;
                case "sharpen":
                    kernel.Clear();
                    kernel = new List<List<int>> { new List<int> { 0, -1, 0 },
                                                   new List<int> { -1, 5, -1 },
                                                   new List<int> { 0, -1, 0 }};
                    break;
                case "edge detection":

                    kernel.Clear();
                    kernel = new List<List<int>> { new List<int> { 0, 0, 0 },
                                                   new List<int> { -1, 1, 0 },
                                                   new List<int> { 0, 0, 0 } };
                    break;
                case "emboss":

                    kernel.Clear();
                    kernel = new List<List<int>> { new List<int> { -1, -1, 0 },
                                                   new List<int> { -1, 1, 1 },
                                                   new List<int> { 0, 1, 1 } };
                    break;





                default:
                    break;
            }
            comboBox1.SelectedItem = 3;
            comboBox2.SelectedItem = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            int divisor = 0; 
            for (int i = 0; i < kernel.Count(); i++)
            {
                for (int j = 0; j < kernel[0].Count; j++)
                {
                    divisor += kernel[i][j];
                }
            }

            textBox4.Text = divisor.ToString();

            Bitmap bmp = (Bitmap)originalImage;
            Bitmap temp = new Bitmap(bmp.Width, bmp.Height);

            for (int x = 1; x < bmp.Width - 1; x++)
            {
                for (int y = 1; y < bmp.Height - 1; y++)
                {


                    int rAccumulator = 0, gAccumulator = 0, bAccumulator = 0;
                    for (int i = 0; i < kernel.Count; i++)
                    {
                        for (int j = 0; j < kernel[i].Count; j++)
                        {

                            int correspondingXIndex = x - anchorX + j;
                            int correspondingYIndex = y - anchorY + i;
                            int rPixelVal = 0, gPixelVal = 0, bPixelVal = 0;

                            if (correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(correspondingXIndex, correspondingYIndex);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }


                            else if (correspondingXIndex < 0 && correspondingYIndex < 0)
                            {
                                Color tempPixel = bmp.GetPixel(0, 0);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex < 0)
                            {
                                Color tempPixel = bmp.GetPixel(bmp.Width - 1, 0);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex < 0 && correspondingYIndex > bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(0, bmp.Height - 1);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex > bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(bmp.Width - 1, bmp.Height - 1);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex < 0 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(0, correspondingYIndex);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(bmp.Width - 1, correspondingYIndex);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingYIndex < 0 && correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1)
                            {
                                Color tempPixel = bmp.GetPixel(correspondingXIndex, 0);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingYIndex > bmp.Height - 1 && correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1)
                            {
                                Color tempPixel = bmp.GetPixel(correspondingXIndex, bmp.Height - 1);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            rAccumulator += (rPixelVal *= kernel[i][j]);
                            gAccumulator += (gPixelVal *= kernel[i][j]);
                            bAccumulator += (bPixelVal *= kernel[i][j]);


                        }
                    }
                   
                    if (divisor != 0)
                    {
                        rAccumulator = wrap((rAccumulator / divisor) + offSet, 0, 255);
                        gAccumulator = wrap((gAccumulator / divisor) + offSet, 0, 255);
                        bAccumulator = wrap((bAccumulator / divisor) + offSet, 0, 255);
                        temp.SetPixel(x, y, Color.FromArgb(rAccumulator, gAccumulator, bAccumulator));
                    }

                }
            }


            pictureBox1.Image = temp;

        }



        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            divisor = (textBox4.Text == null ? Convert.ToInt32(textBox4.Text) : 0);
        }

        private void button6_Click(object sender, EventArgs e)
        {

            int height;
            int width;


            bool parseheightOK = Int32.TryParse(comboBox2.SelectedValue.ToString(), out height);
            bool parsewidthOK = Int32.TryParse(comboBox1.SelectedValue.ToString(), out width);

            if (parseheightOK || parsewidthOK)
            {
                Form editKernel = new Form2(height, width);
                editKernel.Show();
            }


        }

        private void button8_Click(object sender, EventArgs e)
        {

            Bitmap bmp = (Bitmap)originalImage;
            Bitmap temp = new Bitmap(bmp.Width, bmp.Height);

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {

                    List<int> RValues = new List<int>();


                    for (int i = 1; i < kernel.Count - 1; i++)
                    {
                        for (int j = 1; j < kernel[i].Count - 1; j++)
                        {
                            int correspondingXIndex = x - anchorX + j;
                            int correspondingYIndex = y - anchorY + i;
                            int rPixelVal = 0, gPixelVal = 0, bPixelVal = 0;

                            if (correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(correspondingXIndex, correspondingYIndex);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }


                            else if (correspondingXIndex < 0 && correspondingYIndex < 0)
                            {
                                Color tempPixel = bmp.GetPixel(0, 0);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex < 0)
                            {
                                Color tempPixel = bmp.GetPixel(bmp.Width - 1, 0);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex < 0 && correspondingYIndex > bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(0, bmp.Height - 1);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex > bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(bmp.Width - 1, bmp.Height - 1);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex < 0 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(0, correspondingYIndex);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                            {
                                Color tempPixel = bmp.GetPixel(bmp.Width - 1, correspondingYIndex);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingYIndex < 0 && correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1)
                            {
                                Color tempPixel = bmp.GetPixel(correspondingXIndex, 0);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            else if (correspondingYIndex > bmp.Height - 1 && correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1)
                            {
                                Color tempPixel = bmp.GetPixel(correspondingXIndex, bmp.Height - 1);
                                rPixelVal = tempPixel.R;
                                gPixelVal = tempPixel.G;
                                bPixelVal = tempPixel.B;
                            }

                            RValues.Add(rPixelVal);

                        }
                    }
                    RValues.Sort();

                    Color MedianPixel = Color.FromArgb(RValues[RValues.Count / 2]);

                    temp.SetPixel(x, y, MedianPixel);
                }
            }
            pictureBox1.Image = temp;

        }

       


        public Bitmap medianfilter()
        {
            Bitmap bmp = (Bitmap)originalImage;
            Bitmap temp = new Bitmap(bmp.Width, bmp.Height);

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {

                    List<int> RValues = new List<int>();
                    List<int> GValues = new List<int>();
                    List<int> BValues = new List<int>();

                    int correspondingXIndex = x - anchorX;
                    int correspondingYIndex = y - anchorY;
                    int rPixelVal = 0, gPixelVal = 0, bPixelVal = 0;

                    if (correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                    {
                        Color tempPixel = bmp.GetPixel(correspondingXIndex, correspondingYIndex);
                        rPixelVal = tempPixel.R;
                        gPixelVal = tempPixel.G;
                        bPixelVal = tempPixel.B;
                    }


                    else if (correspondingXIndex < 0 && correspondingYIndex < 0)
                    {
                        Color tempPixel = bmp.GetPixel(0, 0);
                        rPixelVal = tempPixel.R;
                        gPixelVal = tempPixel.G;
                        bPixelVal = tempPixel.B;
                    }

                    else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex < 0)
                    {
                        Color tempPixel = bmp.GetPixel(bmp.Width - 1, 0);
                        rPixelVal = tempPixel.R;
                        gPixelVal = tempPixel.G;
                        bPixelVal = tempPixel.B;
                    }

                    else if (correspondingXIndex < 0 && correspondingYIndex > bmp.Height - 1)
                    {
                        Color tempPixel = bmp.GetPixel(0, bmp.Height - 1);
                        rPixelVal = tempPixel.R;
                        gPixelVal = tempPixel.G;
                        bPixelVal = tempPixel.B;
                    }

                    else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex > bmp.Height - 1)
                    {
                        Color tempPixel = bmp.GetPixel(bmp.Width - 1, bmp.Height - 1);
                        rPixelVal = tempPixel.R;
                        gPixelVal = tempPixel.G;
                        bPixelVal = tempPixel.B;
                    }

                    else if (correspondingXIndex < 0 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                    {
                        Color tempPixel = bmp.GetPixel(0, correspondingYIndex);
                        rPixelVal = tempPixel.R;
                        gPixelVal = tempPixel.G;
                        bPixelVal = tempPixel.B;
                    }

                    else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                    {
                        Color tempPixel = bmp.GetPixel(bmp.Width - 1, correspondingYIndex);
                        rPixelVal = tempPixel.R;
                        gPixelVal = tempPixel.G;
                        bPixelVal = tempPixel.B;
                    }

                    else if (correspondingYIndex < 0 && correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1)
                    {
                        Color tempPixel = bmp.GetPixel(correspondingXIndex, 0);
                        rPixelVal = tempPixel.R;
                        gPixelVal = tempPixel.G;
                        bPixelVal = tempPixel.B;
                    }

                    else if (correspondingYIndex > bmp.Height - 1 && correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1)
                    {
                        Color tempPixel = bmp.GetPixel(correspondingXIndex, bmp.Height - 1);
                        rPixelVal = tempPixel.R;
                        gPixelVal = tempPixel.G;
                        bPixelVal = tempPixel.B;
                    }

                    RValues.Add(rPixelVal);
                    GValues.Add(gPixelVal);
                    BValues.Add(bPixelVal);

                    RValues.Sort();
                    GValues.Sort();
                    BValues.Sort();

                    Color MedianPixel = Color.FromArgb(RValues[RValues.Count / 2],
                                                       GValues[GValues.Count / 2],
                                                       BValues[BValues.Count / 2]);
                    temp.SetPixel(x, y, MedianPixel);


                }
            }



            return temp;



        }

        private void button9_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)originalImage;
            int m = 4;
            int t = 1 / 2;
            Bitmap temp = grayScale(bmp);
            Bitmap thresholded = threshold(temp, m, t);



            pictureBox1.Image = thresholded;

        }

        

        private void button10_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)originalImage;
            Bitmap temp = grayScale(bmp);
            Bitmap diffused = errorDiffusion(temp, diffusionFilter);

            pictureBox1.Image = diffused;
        }

        public List<double> findNewRange(int  K)
        {
            int group = 255 / (K-1);
            List<double> diffuseLevels = new List<double>();
            int value = 0;

            if (K == 2)
            {
                diffuseLevels.Add(0);
                
                diffuseLevels.Add(255);
                return diffuseLevels;
            }

            while (value <= 255)
            {
                diffuseLevels.Add(value);
                value += group;
                if(value >=255)
                {
                    value = 255;
                    diffuseLevels.Add(value);
                    break;
                }
            }

            return diffuseLevels;
            
        }

        public float findClosestValue(float Value, List<double> Range)
        {
        
            int idx = 0 ;
            int temp = 0 ;
            for ( int i= 0;i < Range.Count(); i++)
            {
                temp =(int) Math.Abs(Range[i] - Value);
                if (temp < Math.Abs(Range[idx] - Value))
                {
                    idx = i;
                }
            }

            return (float)Range[idx];

        }

        public int findClosestValueIndex(int Value, List<double> Range)
        {

            int idx = 0;
            int temp = 0;
            for (int i = 0; i < Range.Count(); i++)
            {
                temp = (int)Math.Abs(Range[i] - Value);
                if (temp < Math.Abs(Range[idx] - Value))
                {
                    idx = i;
                }
            }

            return idx;

        }


        public int findSecondClosestValueIndex(int closestindex,int Value, List<double> Range)
        {
            int closestvalueDistance = (int)Math.Abs(Range[closestindex] - Value);
            
            int minusOne = (int)Math.Abs(Range[closestindex - 1] - Value);
            int plusOne = (int)Math.Abs(Range[closestindex + 1] - Value);

            if(minusOne < plusOne)
            {
                return (closestindex - 1);
            }
            else
                return (closestindex + 1);



        }
        

        public int findCubeColorMidValue(int Value, List<double> Range)
        {

            int closestcolorindex = findClosestValueIndex(Value, Range);
            double result = 0;
            int firstPoint = 0;
            int secondPoint = 255;

           
                if (closestcolorindex == 0)
                {
                    result = (Range[0] + Range[1]) / 2;
                    return (int)result;
                }
                if (closestcolorindex == Range.Count() - 1)
                {
                    result = (Range[Range.Count - 1] + Range[Range.Count - 2]) / 2;
                    return (int)result;
                }
                else
                {
                    firstPoint = closestcolorindex;
                    secondPoint = findSecondClosestValueIndex(closestcolorindex, Value , Range);
                    result = (Range[firstPoint] + Range[secondPoint]) / 2;
                }
            



            return (int)result;

        }




        public Bitmap quantizeImage(Image image , int RK,int GK,int BK)
        {
            Bitmap bmp = (Bitmap)image;
        
            List<double> Rpallet = findNewRange(RK);
            List<double> Gpallet = findNewRange(GK);
            List<double> Bpallet = findNewRange(BK);
            
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color temppixel = bmp.GetPixel(x, y);

                    int Rvalue = bmp.GetPixel(x, y).R;
                    int Gvalue = bmp.GetPixel(x, y).G;
                    int Bvalue = bmp.GetPixel(x, y).B;

                    int newRvalue = (int)findCubeColorMidValue(Rvalue, Rpallet);
                    int newGvalue = (int)findCubeColorMidValue(Gvalue, Gpallet);
                    int newBvalue = (int)findCubeColorMidValue(Bvalue, Bpallet);
                    


                    bmp.SetPixel(x,y,Color.FromArgb(temppixel.A,newRvalue,newGvalue,newBvalue));
                    
                }
            }

            return bmp;
            
        }

        //--------------------- Corrected Thresholding -----------------
        public float findThresholdedValue(int Value, int levels, double T)
        {
            int result = 0;
            List<int> levelValues = new List<int>();
            List<int> thresholds = new List<int>();



            for (int i = 0; i < levels; i++)
                levelValues.Add( (int)((255.0 / (levels - 1)) * i));
            
            for (int i = 0; i < levels - 1; i++)
                thresholds.Add( (int)(levelValues[i] + T * (levelValues[i + 1] - levelValues[i])));

            for (int i = 0; i < levels - 1; i++)
            {
                if (thresholds[i] >= Value)
                {
                    result =  levelValues[i];
                    
                }
                else if (levels == 2)
                    result =  255;
            }
            return result;
            
        }
        // -------------------------  error Diffusion ----------------
      
        public Bitmap errorDiffusion(Image image , List<List<float>> diffusionMatrix)
        {
            Bitmap bmp = (Bitmap)image; 
            Bitmap d = new Bitmap(image.Width, image.Height);
            int filterX = (int)Math.Floor((double)diffusionMatrix.Count()/2)  ;
            int filterY =(int)Math.Floor((double)diffusionMatrix[0].Count()/2) ;
            int levels = 2;//no of colours you want 
            
            List<double> thresholdingLevels = findNewRange(levels);
            float error = 0f;


            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0 ; y < bmp.Height ; y++)
                {
                    Color currentPixel = bmp.GetPixel(x, y);
                    float intensity = currentPixel.R;
                    float K = findThresholdedValue(currentPixel.R, levels ,0.5);
                    error = currentPixel.R - K ;

                    d.SetPixel(x, y, Color.FromArgb(currentPixel.A, wrap((int)K, 0, 255), wrap((int)K, 0, 255), wrap((int)K, 0, 255)));
                    for (int i = -filterX; i <= filterX; i++)
                    {

                        for (int j = -filterY; j <= filterY; j++)
                        {

                            int correspondingXIndex = x + i;
                            int correspondingXFilter = i + filterX;

                            int correspondingYIndex = y + j ;
                            int correspondingYFilter = j + filterY;

                            
                                if (correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                                {
                                    Color tempPixel = bmp.GetPixel(correspondingXIndex, correspondingYIndex);
                                    float currentValue = tempPixel.R;
                                    currentValue += error * (diffusionMatrix[correspondingXFilter][correspondingYFilter]);
                   
                                    Color diffusedColor = Color.FromArgb(tempPixel.A, wrap((int)currentValue,0,255), wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255));
                                    bmp.SetPixel(correspondingXIndex, correspondingYIndex, diffusedColor);
                                }
                                
                                else if (correspondingXIndex < 0 && correspondingYIndex < 0)
                                {
                                    Color tempPixel = bmp.GetPixel(0, 0);
                                    double currentValue = tempPixel.R;
                                    currentValue += error * (diffusionMatrix[correspondingXFilter][correspondingYFilter]);
                                    Color diffusedColor = Color.FromArgb(tempPixel.A, wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255));
                                    bmp.SetPixel(0, 0, diffusedColor);

                                }

                                else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex < 0)
                                {
                                    Color tempPixel = bmp.GetPixel(bmp.Width - 1, 0);
                                    double currentValue = tempPixel.R;
                                    currentValue += error * (diffusionMatrix[correspondingXFilter][correspondingYFilter]);
                                    Color diffusedColor = Color.FromArgb(tempPixel.A, wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255));
                                    bmp.SetPixel(bmp.Width - 1, 0, diffusedColor);

                                }

                                else if (correspondingXIndex < 0 && correspondingYIndex > bmp.Height - 1)
                                {
                                    Color tempPixel = bmp.GetPixel(0, bmp.Height - 1);
                                    double currentValue = tempPixel.R;
                                    currentValue += error * (diffusionMatrix[correspondingXFilter][correspondingYFilter]);
                                    Color diffusedColor = Color.FromArgb(tempPixel.A, wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255));
                                    bmp.SetPixel(0, bmp.Height - 1, diffusedColor);

                                }

                                else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex > bmp.Height - 1)
                                {
                                    Color tempPixel = bmp.GetPixel(bmp.Width - 1, bmp.Height - 1);
                                    double currentValue = tempPixel.R;
                                    currentValue += error * (diffusionMatrix[correspondingXFilter][correspondingYFilter]);
                                    Color diffusedColor = Color.FromArgb(tempPixel.A, wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255));
                                    bmp.SetPixel(bmp.Width - 1, bmp.Height - 1, diffusedColor);

                                }

                                else if (correspondingXIndex < 0 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                                {
                                    Color tempPixel = bmp.GetPixel(0, correspondingYIndex);
                                    double currentValue = tempPixel.R;
                                    currentValue += error * (diffusionMatrix[correspondingXFilter][correspondingYFilter]);
                                    Color diffusedColor = Color.FromArgb(tempPixel.A, wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255));
                                    bmp.SetPixel(0, correspondingYIndex, diffusedColor);

                                }

                                else if (correspondingXIndex > bmp.Width - 1 && correspondingYIndex >= 0 && correspondingYIndex <= bmp.Height - 1)
                                {
                                    Color tempPixel = bmp.GetPixel(bmp.Width - 1, correspondingYIndex);
                                    double currentValue = tempPixel.R;
                                    currentValue += error * (diffusionMatrix[correspondingXFilter][correspondingYFilter]);
                                    Color diffusedColor = Color.FromArgb(tempPixel.A, wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255));
                                    bmp.SetPixel(bmp.Width - 1, correspondingYIndex, diffusedColor);

                                }

                                else if (correspondingYIndex < 0 && correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1)
                                {
                                    Color tempPixel = bmp.GetPixel(correspondingXIndex, 0);
                                    double currentValue = tempPixel.R;
                                    currentValue += error * (diffusionMatrix[correspondingXFilter][correspondingYFilter]);
                                    Color diffusedColor = Color.FromArgb(tempPixel.A, wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255));
                                    bmp.SetPixel(correspondingXIndex, 0, diffusedColor);

                                }

                                else if (correspondingYIndex > bmp.Height - 1 && correspondingXIndex >= 0 && correspondingXIndex <= bmp.Width - 1)
                                {
                                    Color tempPixel = bmp.GetPixel(correspondingXIndex, bmp.Height - 1);
                                    double currentValue = tempPixel.R;
                                    currentValue += error * (diffusionMatrix[correspondingXFilter][correspondingYFilter]);
                                    Color diffusedColor = Color.FromArgb(tempPixel.A, wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255), wrap((int)currentValue, 0, 255));
                                    bmp.SetPixel(correspondingXIndex, bmp.Height - 1, diffusedColor);

                                }
                            
                            

                        }
                    }




                }

            }

            return d;

          }

        public Bitmap threshold(Image image, int m, int threshold)
        {
           
            Bitmap bmp = (Bitmap)image;
            Bitmap d = new Bitmap(image.Width, image.Height);


            
            List<double> Range = findNewRange(m);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int x = 0; x < bmp.Height; x++)
                {
                    Color oc = bmp.GetPixel(i, x);
                    int currentIntensity = (oc.R);
                    int grayscale = 0;

                    grayscale = (int)findThresholdedValue(currentIntensity,  m , threshold);


                    Color nc = Color.FromArgb(oc.A, wrap(grayscale, 0, 255), wrap(grayscale, 0, 255), wrap(grayscale, 0, 255));

                    d.SetPixel(i, x, nc);



                }
            }



            return d;
        }

        public static Bitmap grayScale(Image image)
        {
            Bitmap bmp = (Bitmap)image;

            Bitmap d = new Bitmap(bmp.Width, bmp.Height);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int x = 0; x < bmp.Height; x++)
                {
                    Color oc = bmp.GetPixel(i, x);
                    int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                    d.SetPixel(i, x, nc);
                }
            }

            return d;


        }




        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox4.SelectedItem.ToString())
            {
                case "Floyd":
                    diffusionFilter.Clear();
                    diffusionFilter = new List<List<float>> {
                                                   new List<float> { 0, 0, 0 },
                                                   new List<float> { 0, 0, 7/16f },
                                                   new List<float> { 3/16f, 5/16f, 1/16f } };
                    break;
                case "Burkes":
                    diffusionFilter.Clear();
                    diffusionFilter = new List<List<float>> {
                                                   new List<float> { 0 , 0 , 0 , 0 , 0 },
                                                   new List<float> { 0 , 0 , 0 , 8/32f , 4/32f },
                                                   new List<float> { 2/32f , 4/32f , 8/32f , 4/32f , 2/32f } };
                    break;
                case "Stucky":
                    diffusionFilter.Clear();
                    diffusionFilter = new List<List<float>> {
                                                   new List<float> { 0,  0 , 0 , 0 , 0 },
                                                   new List<float> { 0 , 0 , 0 , 0 , 0 },
                                                   new List<float> { 0 , 0 , 0 , 8 / 42f , 4/42f },
                                                   new List<float> { 2/42f , 4/42f , 8/42f , 4/42f , 2/42f },
                                                   new List<float> { 1 / 42f, 2 / 42f, 4 / 42f, 2 / 42f, 1 / 42f }};
                    break;
                case "Sierra":

                    diffusionFilter.Clear();
                    diffusionFilter = new List<List<float>> {
                                                            new List<float> { 0 , 0 , 0 , 0 , 0 },
                                                            new List<float> { 0 , 0 , 0 , 0 , 0 },
                                                            new List<float> { 0 , 0 , 0 , 5 / 32f , 3 / 32f },
                                                            new List<float> { 2 / 32f, 4 / 32f, 8 / 32f,4 / 32f,2 / 32f },
                                                            new List<float> { 1 / 42f, 2 / 42f, 4 / 42f, 2 / 42f, 1 / 42f }};
                    break;
                case "Atkinson":

                    diffusionFilter.Clear();
                    diffusionFilter = new List<List<float>> {
                                                            new List<float> { 0 , 0 , 0 , 0 , 0 },
                                                            new List<float> { 0 , 0 , 0 , 0 , 0 },
                                                            new List<float> { 0 , 0 , 0 , 1 / 8f , 1 / 8f },
                                                            new List<float> { 0 , 1 / 8f , 1 / 8f,1 / 8f, 0 },
                                                            new List<float> { 0, 0, 1 / 8f, 0, 0 }};
                    break;



                default:
                    break;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)originalImage;
            int RK = Convert.ToInt32(textBox5.Text);
            int BK = Convert.ToInt32(textBox7.Text);
            int GK = Convert.ToInt32(textBox6.Text);
            Bitmap temp = quantizeImage(bmp,RK,GK,BK);
            pictureBox1.Image = temp;
        }

       
    }
}
