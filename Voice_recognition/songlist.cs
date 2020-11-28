using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace Voice_recognition
{
    public partial class songlist : Form
    {
        public songlist()
        {
            InitializeComponent();
        }
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader read;
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        String user = Login.Username;
        public void say(String s)
        {
            syn.Speak(s);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            cmd = new SqlCommand("insert into songlist values(@user,@counts,@names)", con);
            
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@counts", comboBox1.Text);
            cmd.Parameters.AddWithValue("@names", richTextBox1.Text);
            cmd.ExecuteNonQuery();
            say("The song list has been added successfully");
            DialogResult dr =  MessageBox.Show("success","error",MessageBoxButtons.OKCancel);
            if(dr == DialogResult.OK)
            {
                media m = new media();
                m.Show();
            }
            else
            {

            }
            con.Close();
        }

        private void songlist_Load(object sender, EventArgs e)
        {
           // say("Please enter the name of the songs you are going to add to the media player in textbox");
           // say("Enter names one by one by clicking the add button");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = textBox1.Text;
            textBox1.Clear();
        }
    }
}
