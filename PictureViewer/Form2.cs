using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureViewer
{
    public partial class Form2 : Form
    {
        int height, width;
        public List<List<int>> outputKernel;
        public List<List<TextBox>> boxList;

        TextBox[,] textbox = new TextBox[100, 100];

        private void button1_Click(object sender, EventArgs e)
        {
            outputKernel = new List<List<int>>();
            for(int i = 0;i< height;i++)
            {
                List<int> sublist = new List<int>();
                for (int j = 0; j < width; j++)
                {
                    int currentCell = Convert.ToInt32(boxList[i][j].Text);
                    if (currentCell!=0)
                    {
                        sublist.Add(currentCell);
                    }
                    else
                    {
                        currentCell = 1;
                    }

                }
                outputKernel.Add(sublist);
            }

            Form1.kernel = outputKernel;
            this.Close();

            
        }

        public Form2(int height , int width)
        {
            InitializeComponent();
            this.height = height;
            this.width = width;
            this.outputKernel = new List<List<int>>();
            this.boxList = new List<List<TextBox>>();
            
        

            Point initLocation = this.theLabel.Location;

            int a = initLocation.X + 80;
            int b = initLocation.Y;

            for (int i = 0; i < height; i++)
            {
                List<TextBox> sublist = new List<TextBox>();
                a = initLocation.X + 10;
                b = b + 30;
                for (int j = 0; j<width;j++)
                {

                    
     
                    string correspondingTextbox = 'A' + Convert.ToString(i);
                    
                    a = a + 50;
                    TextBox tempBox = new TextBox();
                    tempBox.Name = correspondingTextbox;
                    
                    tempBox.Width = 35;
                    tempBox.Height = 35;
                    tempBox.Location = new Point(a, b + 15);
                    this.Controls.Add(tempBox);
                    sublist.Add(tempBox);

                    

                }
                boxList.Add(sublist);
            }


        }
    }
}
