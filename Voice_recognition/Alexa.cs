using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Voice_recognition
{
    public partial class Alexa : Form
    {
        public Alexa()
        {
            InitializeComponent();
            mainmenu();
        }

        private void mainmenu()
        {
            panel2.Visible = false;
        }
        private void hidesubmenu()
        {
            if (panel2.Visible == true)
            {
                panel2.Visible = false;
            }
        }

        private void showsidemenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hidesubmenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }
        private void Alexa_Load(object sender, EventArgs e)
        {

        }

        private void HOME_Click(object sender, EventArgs e)
        {
            showsidemenu(panel2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            open(new Form1());
            hidesubmenu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            open(new profile());
            hidesubmenu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            open(new Login());
            hidesubmenu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            open(new delete());
            hidesubmenu();
        }
        private Form activeform = null;
        private void open(Form childform)
        {
            if (activeform != null)
                activeform.Close();
            activeform = childform;
            childform.TopLevel = false;
            childform.FormBorderStyle = FormBorderStyle.None;
            childform.Dock = DockStyle.Fill;
            panel4.Controls.Add(childform);
            panel4.Tag = childform;
            childform.BringToFront();
            childform.Show();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            open(new update()); 
            hidesubmenu();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            open(new feedback());
            hidesubmenu();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            open(new songlist());
            hidesubmenu();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            open(new Alexa());
            hidesubmenu();
        }
    }
}
