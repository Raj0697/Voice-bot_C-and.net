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
using System.Net;
using System.Net.Mail;

namespace Voice_recognition
{
    public partial class Forgot_password : Form
    {
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader data;
        public Forgot_password()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn);
                con.Open();
                cmd = new SqlCommand("select code,password from alexa where name='"+textBox1.Text+"'", con);
                data = cmd.ExecuteReader();
                while (data.Read())
                {
                    String code = data.GetValue(0).ToString();
                    String pass = data.GetValue(1).ToString();
                    Choices clist = new Choices();
                    clist.Add(new String[] { code, pass });
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
            cmd = new SqlCommand("select code,password from alexa where name='"+textBox1.Text+"'", con);
            data = cmd.ExecuteReader();
            if (data.Read())
            {
                String c = data.GetValue(0).ToString();
                String p = data.GetValue(1).ToString();
               // MessageBox.Show(c + " " + p);
                Choices clist = new Choices();
                clist.Add(new String[] { c, p });
                Grammar gr = new Grammar(new GrammarBuilder(clist));
                String text = e.Result.Text;
                if(text==c)
                {
                    say("Don't get panic Mr." +textBox1.Text+ " ,your password will be shown in the messagebox");
                    MessageBox.Show("Your password is : " + p);
                }
                else
                {
                    say("Incorrect character code");
                }
                
            }
        }

        public void say(String s)
        {
            syn.Speak(s);
            // richTextBox2.AppendText(s + "\n");
        }
        private void Forgot_password_Load(object sender, EventArgs e)
        {
            // say("After entering the username, click submit button and  tell loudly your security pin number");
            
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MailMessage msg = new MailMessage("rajk45980@gmail.com","rajk45980@gmail.com","voice","hello world");
            msg.IsBodyHtml = true;
            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.UseDefaultCredentials = false;
            NetworkCredential nc = new NetworkCredential("rajk45980@gmail.com","superstarraj");
            sc.Credentials = nc;
            sc.Send(msg);
            MessageBox.Show("msg send");
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            try
            {

                String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn);
                con.Open();
                cmd = new SqlCommand("select code,password,name from alexa where name='" + textBox1.Text + "'", con);
                data = cmd.ExecuteReader();
                while (data.Read())
                {
                    String code = data.GetValue(0).ToString();
                    String pass = data.GetValue(1).ToString();
                    String na = data.GetValue(2).ToString();
                    if (textBox1.Text == na)
                    {
                        say("Now tell your security pin number " + na);
                    }
                    else
                    {
                        say("invalid username");
                    }
                    Choices clist = new Choices();
                    clist.Add(new String[] { code, pass });
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
    }
}
