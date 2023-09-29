using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Pexeso
{
    
    public partial class Form1 : Form
    {
        
        //global variables
        private List<Button> cards;
        private DomainUpDown domainUpDownX;
        private DomainUpDown domainUpDownY;
        private Button button;
        public List<Color> colors = new List<Color>();
        private int x, y;
        private Button firstButton = null; // the first button clicked
        private Button secondButton = null; // the second button clicked
        private List<Button> matchedButtons = new List<Button>();
        private int currCount;
        private int count;



        public Form1()
        {
            InitializeComponent();
            StartPrep();
            /*
             * this.Height = 600;
             * this.Width = 600;
            */
        }

        private void StartPrep()
        {
            //reset variables
            count = 0;
            currCount = 0;
            
            // Create DomainUpDown controls
            this.domainUpDownX = new DomainUpDown();
            this.domainUpDownX.Location = new Point(10, 10);
            this.Controls.Add(this.domainUpDownX);
            this.domainUpDownX.Text = "1";
            this.domainUpDownX.Items.AddRange(new char[] { '6', '5', '4', '3', '2', '1' });

            this.domainUpDownY = new DomainUpDown();
            this.domainUpDownY.Location = new Point(150, 10);
            this.Controls.Add(this.domainUpDownY);
            this.domainUpDownY.Text = "1";
            this.domainUpDownY.Items.AddRange(new char[] { '6','5','4','3','2','1' });

            //start button
            this.button = new Button();
            this.button.Location = new Point(290, 10);
            this.button.Text = "Get Values";
            this.button.Click += new EventHandler(buttonStart_Click);
            this.Controls.Add(this.button);

            //set size of window
            
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
           
            this.x = int.Parse(this.domainUpDownX.Text);
            this.y = int.Parse(this.domainUpDownY.Text);

            
            
            // Do something with the input values...
            MessageBox.Show($"The values you entered are X={this.x} and Y={this.y}");

            if ((this.x * this.y) % 2 == 0) { StartGame(this.x, this.y); }
            else { MessageBox.Show($"Invalid input"); }
            
        }

        private List<Color> Shuffle(int x,int y)
        {
            Random rand = new Random();
            int count = x * y;
            int numPairs = count / 2;
            List<Color> colors = new List<Color>();
            List<Color> diffColour = new List<Color>();

            for (int i = 0; i < numPairs; i++)
            {
                Color color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                colors.Add(color);
                colors.Add(color);
            }

            // shuffle the list of colors randomly
            for (int i = 0; i < numPairs * 2; i++)
            {
                int j = rand.Next(i, numPairs * 2);
                Color temp = colors[i];
                colors[i] = colors[j];
                colors[j] = temp;
            }
            return colors;
        }  

        private void StartGame(int X, int Y)
        {
            //make start screen disappear
            domainUpDownX.Visible = false;
            domainUpDownX.Enabled = false;
            domainUpDownY.Visible = false;
            domainUpDownY.Enabled = false;

            button.Visible = false;
            button.Enabled = false;

            //some prep before the loops
            this.colors = Shuffle(X,Y);
            
            
            //calculate the size of each button based on size of the window
            List<int> windowSize= new List<int>();
            List<int> buttonSize= new List<int>();

            windowSize.Add(this.Width);
            windowSize.Add(this.Height);

            buttonSize.Add((windowSize[0]-100) / X);
            buttonSize.Add((windowSize[1]-100) / Y);




            for (int i = 0; i < X; i++) 
            { 
                for (int j = 0; j < Y; j++)
                {
                    /* creating button (cards)*/
                    Button card = new Button();
                    //colour needs to be changed only after clicking on the button
                    //card.BackColor = colors[count];
                    card.Location = new Point(i * buttonSize[0], j* buttonSize[1]);
                    card.Text = "Pexeso";
                    card.Size = new Size(100, 100);
                    card.Click += CardClick;


                    this.Controls.Add(card);
                    
                    //counting every card 
                    count++;
                }
                
            }
        }
        


        private async void CardClick(object sender, EventArgs e)
        {
            //this fuction handles button click
            Button clickedButton = sender as Button;
            //button.BackColor = Color.Red;
            int index = this.Controls.IndexOf(clickedButton)-3;
            clickedButton.BackColor = colors[index];
            

            if (firstButton == null)
            {
                firstButton = clickedButton;
            }
            else if (secondButton == null && firstButton != clickedButton)
            {
                secondButton = clickedButton;
                if (colors[this.Controls.IndexOf(firstButton) -3] == colors[this.Controls.IndexOf(secondButton) -3])
                {
                    // the colors match, add the buttons to the matched list and hide them
                    matchedButtons.Add(firstButton);
                    matchedButtons.Add(secondButton);
                    
                    
                    switch ((this.Controls.IndexOf(firstButton) - 3) %2)
                    {
                        case 0:
                            firstButton.BackColor = Color.Black;
                            
                            break;

                        default:
                            firstButton.BackColor = Color.White;
                            firstButton.Text = null;
                           
                            break;
                    }
                    
                    switch ((this.Controls.IndexOf(secondButton) - 3) % 2)
                    {
                        case 0:
                            secondButton.BackColor = Color.Black;
                        
                            break;

                        default:
                            secondButton.BackColor = Color.White;
                            secondButton.Text = null;
                            break;
                    }
                    
                    currCount++;
                    
                    if (currCount == count/2)
                    {
                        MessageBox.Show("Winner winner chicken dinner");
                    }
                    
                    //reset buttons
                    firstButton.Enabled = false;
                    secondButton.Enabled = false;
                    firstButton = null;
                    secondButton = null;
                }
                else
                {
                    // the colors do not match, flip the buttons back after a delay
                    await Task.Delay(500).ContinueWith(_ =>
                    {
                        firstButton.BackColor = SystemColors.Control;
                        secondButton.BackColor = SystemColors.Control;
                        firstButton = null;
                        secondButton = null;
                    });
                }
            }

        }


        private void domainUpDownX_SelectedItemChanged(object sender, EventArgs e)
        {
            domainUpDownX.SelectedItemChanged -= domainUpDownX_SelectedItemChanged;
        }
    }

       
    
}
