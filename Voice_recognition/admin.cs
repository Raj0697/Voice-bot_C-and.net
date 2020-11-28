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
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace Voice_recognition
{
    public partial class admin : Form
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader data;
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        public admin()
        {
            InitializeComponent();
        }
        public void say(String s)
        {
            syn.Speak(s);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {
                con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
                con.Open();
                cmd = new SqlCommand("select password from Admin where username='" + textBox1.Text + "'", con);
                data = cmd.ExecuteReader();
                if (data.Read())
                {
                    String pass = data.GetValue(0).ToString();
                    if (pass == textBox2.Text)
                    {
                        display d = new display();
                        d.Show();
                    }
                    else
                    {
                        say("Password doesn't match");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void admin_Load(object sender, EventArgs e)
        {
            try
            {
                //say("Please tell your username");
                say("Please tell the security code to access the application");
                    
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            try
            {
                
                var t = e.Result.Text;
                if(t == "abcd")
                {
                    say("hello admin access granted");
                    display d = new display();
                    d.Show();
                }
                else
                {
                    say("invalid code please try again");

                }
                /*
            if (t == "raj")
            {
               // say("okay now enter your password, for security reasons alexa will not allow you to tell your password");
                textBox1.Text = t;
                    display d = new display();
                    d.Show();
                }
                else
                {
                    say("invalid username");
                }
            
            if(textBox2.Text == "rajkumar")
                {
                    if (t == "login")
                    {
                        display d = new display();
                        d.Show();
                    }
                }
                else
                {
                    say("invalid password");
                }
            */

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Choices clist = new Choices();
            clist.Add(new string[] { "raj", "login", "rajkumar", "abcd" });
            Grammar gr = new Grammar(new GrammarBuilder(clist));
            rec.RequestRecognizerUpdate();
            rec.LoadGrammar(gr);
            rec.SpeechRecognized += Rec_SpeechRecognized;
            rec.SetInputToDefaultAudioDevice();
            rec.RecognizeAsync(RecognizeMode.Multiple);
        }
    }
}
