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
    public partial class delete : Form
    {
        public delete()
        {
            InitializeComponent();
        }
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SqlCommand cmd,cmd2;
        SqlConnection con,con1;
        SqlDataReader data;
        String user = Login.Username;
        public void say(String s)
        {
            syn.Speak(s);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void delete_Load(object sender, EventArgs e)
        {
            syn.SelectVoiceByHints(VoiceGender.Female);
            syn.Speak("Please tell your character code loudly to delete your account ");
          //  syn.Speak("If you are telling wrong character code alexa will not reply anything");

            try
            {
                String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn);
                con.Open();
                cmd = new SqlCommand("select code from alexa where name='" + user + "'", con);
                data = cmd.ExecuteReader();
                if (data.Read())
                {
                    string c = data.GetValue(0).ToString();
                    Choices clist = new Choices();
                    clist.Add(new string[] { c, "Yes", "no" });
                    Grammar gr = new Grammar(new GrammarBuilder(clist));
                    rec.RequestRecognizerUpdate();
                    rec.LoadGrammar(gr);
                    rec.SpeechRecognized += Rec_SpeechRecognized;
                    rec.SetInputToDefaultAudioDevice();
                    rec.RecognizeAsync(RecognizeMode.Multiple);
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Asterisk);
            }

        }

        

        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
            con = new SqlConnection(conn);
            con.Open();
            cmd = new SqlCommand("select code from alexa where name='"+user+"'", con);
            data = cmd.ExecuteReader();
            while (data.Read())
            {
                string c = data.GetValue(0).ToString();

                var text = e.Result.Text;
                if (text == c)
                {
                    say("are you sure your account is going to be deleted");
                    DialogResult dr = MessageBox.Show("Are you sure ", "sure", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        string conn2 = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                        con1 = new SqlConnection(conn2);
                        con1.Open();
                        cmd2 = new SqlCommand("delete from alexa where name='" + user + "'", con1);
                        cmd2.ExecuteNonQuery();
                        say("Good bye Mr " + user + "Your account has been deleted successfully");
                        Login ll = new Login();
                        this.Close();
                        ll.Show();
                    }
                    else
                    {
                        say("ok be careful while deleting");
                    }
                }

                else
                {
                    say("Invalid character code");
                }
            }
            }
        }
    }
