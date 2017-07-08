using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormswithSQL
{
    public partial class Form1 : Form
    {
        private int counter;
        public Form1()
        {
            InitializeComponent();
            counter = 0;
            timer1.Interval = 6000;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            

            if (counter >= 10)
            {
                // Exit loop code.
                timer1.Enabled = false;
                counter = 0;
            }
            else
            {
               //if (Access.edit_ctegory("ccoo1", "dddsa")) { 
               // counter = counter + 1;
               // label1.Text = "运行次数： " + counter.ToString();
               // }
            }
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (counter >= 10)
            {
                // Exit loop code.
                timer1.Enabled = false;
                counter = 0;
            }
            else
            {
                
                
                    counter = counter + 1;
                    label1.Text = "运行次数： " + Access.edit_ctegory("ccoo1","dddsa").ToString();
                
            }
        }
    }
}
