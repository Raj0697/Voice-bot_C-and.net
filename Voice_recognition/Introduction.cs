using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace Voice_recognition
{
    public partial class Introduction : Form
    {
        public Introduction()
        {
            InitializeComponent();
            mainmenu();
        }
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        private void mainmenu()
        {
            panel1.Visible = false;
        }

        private void hidesubmenu()
        {
            if(panel1.Visible == true)
            {
                panel1.Visible = false;
            }
        }

        private void showsidemenu(Panel submenu)
        {
            if(submenu.Visible == false)
            {
                hidesubmenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show("sure", "hello", MessageBoxButtons.YesNo);
            //if(dr == DialogResult.Yes)
            //{
                showsidemenu(panel1);
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            open(new admin());
            hidesubmenu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            open(new Login());
            hidesubmenu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            open(new Register());
            hidesubmenu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            open(new help());
            hidesubmenu();
        }

        private Form activeform = null;
        private void open(Form childform)
        {
            if(activeform != null)
                activeform.Close();
            activeform = childform;
            childform.TopLevel = false;
            childform.FormBorderStyle = FormBorderStyle.None;
            childform.Dock = DockStyle.Fill;
            panel3.Controls.Add(childform);
            panel3.Tag = childform;
            childform.BringToFront();
            childform.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Introduction_Load(object sender, EventArgs e)
        {
            syn.SelectVoiceByHints(VoiceGender.Female);
            syn.Speak("Please listen to the alexa carefully before accessing the application by clicking help");
        }
    }
}
