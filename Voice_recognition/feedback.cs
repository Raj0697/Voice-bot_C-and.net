using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Data.SqlClient;

namespace Voice_recognition
{
    public partial class feedback : Form
    {
        public feedback()
        {
            InitializeComponent();
        }
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SqlCommand cmd;
        SqlConnection con;
        String user = Login.Username;

        private void button1_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Text.Length != 0)
            {
                String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn);
                con.Open();
                cmd = new SqlCommand("insert into feedback values(@comments,@username)",con);
                cmd.Parameters.AddWithValue("@comments", richTextBox1.Text);
                cmd.Parameters.AddWithValue("@username", user);
                cmd.ExecuteNonQuery();
                con.Close();
                syn.Speak("Thank you for your feedback " + user);
            }
            else
            {
                syn.SelectVoiceByHints(VoiceGender.Female);
                syn.Speak("Please enter the feedback to proceed");
            }
        }

        private void feedback_Load(object sender, EventArgs e)
        {
            
            label4.Text = user;
        }
    }
}
