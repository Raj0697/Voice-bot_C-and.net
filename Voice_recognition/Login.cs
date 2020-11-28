using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Voice_recognition
{
    public partial class Login : Form
    {
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SqlCommand cmd,cmd2;
        SqlConnection con,con2;
        SqlDataReader read,data,data2;
        //SqlDataAdapter adapter;
        Thread th;

        public static string Username="";
        public void say(String s)
        {
            syn.SelectVoiceByHints(VoiceGender.Female);
            syn.Speak(s);
        }
        public Login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
         //   Register r = new Register();
           // r.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Forgot_password fp = new Forgot_password();
            fp.Show();
        }
        
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                say("Username should not be empty");
                e.Cancel = true;
                textBox1.Focus();
                pictureBox2.Image = Properties.Resources.invalid;
                errorProvider1.SetError(textBox1, "please enter your username");
            }
            else
            {
                e.Cancel = false;
                pictureBox2.Image = Properties.Resources.valid;
                errorProvider1.SetError(textBox1, null);
            }
            /*
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                
                e.Cancel = true;
                textBox2.Focus();
                errorProvider2.SetError(textBox2, "please enter your password");
                //say("password should not be empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider2.SetError(textBox2, null);
            }
            */
        }
        
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // MessageBox.Show("test");
            textBox2.Clear();
            String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
            con = new SqlConnection(conn);
            con.Open();
            cmd = new SqlCommand("select name,pin from alexa where pin='" + textBox3.Text + "'", con);
            data = cmd.ExecuteReader();
            if(data.Read())
            {
                String pin = data.GetValue(1).ToString();
                String nn = data.GetValue(0).ToString();
                if(textBox3.Text == pin)
                {
                    pictureBox1.Image = Properties.Resources.valid;
                    textBox1.Text = nn;
                    say("Welcome " + nn + "Now enter your password  ");
                    pictureBox2.Image = Properties.Resources.valid;
                    textBox2.Focus();
                   // MessageBox.Show("test");
                }
                if(textBox3.Text != pin)
                {
                    pictureBox1.Image = Properties.Resources.invalid;
                    say("invalid pin number");
                }
            }
            else
            {
                pictureBox1.Image = Properties.Resources.invalid;
                errorProvider1.SetError(textBox3, "Invalid pin number");
                textBox3.Focus();
            }
        }

        /*
private void textBox1_Leave(object sender, EventArgs e)
{
   if (string.IsNullOrEmpty(textBox1.Text))
   {
       errorProvider1.SetError(textBox1, "please enter your username");
   }
   else
   {
       errorProvider1.Icon = Properties.Resources.ok;
       errorProvider1.SetError(textBox1, "ok");
   }
}
*/
        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Length == 0)
            {
                say("Username should not be empty");
               // MessageBox.Show("username should not be empty");
                textBox1.Focus();
            }
            else if (textBox2.Text.Length == 0)
            {
                say("Password should not be empty");
                textBox2.Focus();
            }
            else
            {
                try
                {
                    String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                    con = new SqlConnection(conn);
                    con.Open();
                    cmd = new SqlCommand("select password from alexa where name='" + textBox1.Text + "'", con);

                    read = cmd.ExecuteReader();
                    if (read.Read())
                    {
                        //String u = read.GetValue(0).ToString();
                        String p = read.GetValue(0).ToString();
                        //MessageBox.Show( " " + p);
                        if (p != textBox2.Text)
                        {
                            say("password doesn't match");
                            pictureBox3.Image = Properties.Resources.invalid;
                            errorProvider2.SetError(textBox2, "Invalid password");
                        }
                        else
                        {
                            pictureBox3.Image = Properties.Resources.valid;
                            errorProvider2.SetError(textBox2, null);
                            Username = textBox1.Text;
                            Alexa a = new Alexa();
                            a.Show();
                            this.WindowState = FormWindowState.Minimized;
                            this.ShowInTaskbar = false;
                       //     th = new Thread(openform());
                       //    th.SetApartmentState(ApartmentState.STA);
                       //     th.Start();
                        }
                    }
                    else
                    {
                        say("Enter the valid username");
                        pictureBox2.Image = Properties.Resources.invalid;
                        errorProvider1.SetError(textBox1, "Enter the valid username");
                       // MessageBox.Show("The username you have entered is invalid");
                    }
                    
                    con.Close();
                    /*
                    String conn3 = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                    con2 = new SqlConnection(conn3);
                    con2.Open();
                    cmd = new SqlCommand("select name from alexa", con2);
                    data2 = cmd.ExecuteReader();
                    while (data2.Read())
                    {
                        string[] user = { data2.GetValue(0).ToString() };

                        for(int i=0; i<user.Length; i++)
                        {
                            if(user[i].Contains(textBox1.Text))
                            {
                                pictureBox2.Image = Properties.Resources.valid;
                                errorProvider1.SetError(textBox1, "");
                            }
                            else
                            {
                              //  say("invalid username");
                                pictureBox2.Image = Properties.Resources.invalid;
                                errorProvider1.SetError(textBox1, "please enter the valid username");
                            }
                        }
                    }
                    con2.Close();
                    */                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("enter valid username");
                   // say("you are not a registered user");
                }
            }
        }

        private ThreadStart openform()
        {
        //    Application.Run(new Alexa());
            throw new NotImplementedException();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // say("For security reasons please enter your security pin number and then Enter your password to signin to alexa");
            
            try
            {
                /*
                var btn = new Button();
                btn.Size = new Size(25, textBox1.ClientSize.Height + 2);
                btn.Location = new Point(textBox1.ClientSize.Width - btn.Width, -1);
                btn.Cursor = Cursors.Default;
                btn.Image = Properties.Resources.valid;
                textBox1.Controls.Add(btn);
                base.OnLoad(e);
                */
                textBox1.Focus();
                Choices clist = new Choices();
                clist.Add(new string[] {"show password","hide password" });
                Grammar gr = new Grammar(new GrammarBuilder(clist));
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(gr);
                rec.SpeechRecognized += Rec_SpeechRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Asterisk);
            }
            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    pictureBox1.Image = Properties.Resources.invalid;
                    say("Security pin should not be empty");
                    e.Cancel = true;
                    textBox3.Focus();
                    errorProvider1.SetError(textBox3, "please enter your security pin number");
                }
                else
                {
                    String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                    con = new SqlConnection(conn);
                    con.Open();
                    cmd = new SqlCommand("select pin from alexa where pin='" + textBox3.Text + "'", con);
                    data = cmd.ExecuteReader();
                    if(data.Read())
                    {
                        String p = data.GetValue(0).ToString();
                        if(p == textBox3.Text)
                        {
                            pictureBox1.Image = Properties.Resources.valid;
                            errorProvider1.SetError(textBox3, "");
                        }
                        else
                        {
                            pictureBox1.Image = Properties.Resources.invalid;
                            errorProvider1.SetError(textBox3, "Invalid pin number");
                            textBox3.Focus();
                        }
                    }
                    else
                    {
                        say("The pin you entered is invalid");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                pictureBox3.Image = Properties.Resources.invalid;
                say("password should not be empty");
                e.Cancel = true;
                textBox2.Focus();
                errorProvider2.SetError(textBox2, "please enter the password");
            }
            else
            {
                pictureBox3.Image = Properties.Resources.valid;
                e.Cancel = false;
                errorProvider2.SetError(textBox2, null);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
                say("characters are not allowed");
                errorProvider1.SetError(textBox3, "Enter digit only");
                textBox3.Focus();
            }
            else
            {
                Regex num = new Regex(@"^([0-9]*|\d*)$");
                if (num.IsMatch(textBox3.Text))
                {
                    errorProvider1.SetError(textBox3, "");
                }
                else
                {
                    say("only numbers are allowed");
                    errorProvider1.SetError(textBox3, "Enter only integers");
                }
                e.Handled = false;
                errorProvider1.SetError(textBox1, null);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && !char.IsControl(ch) && !char.IsWhiteSpace(ch) && (ch != 8))
            {
                say("numbers and special characters are not allowed in username");
                textBox1.Focus();
                //   errorProvider1.SetError(textBox1, "Enter letters only");
                e.Handled = true;

            }
            else
            {
                pictureBox2.Image = Properties.Resources.valid;
                e.Handled = false;
                errorProvider1.SetError(textBox1, null);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            cmd = new SqlCommand("select name from alexa", con);

            data = cmd.ExecuteReader();
            while (data.Read())
            {
                string[] arr = { data.GetValue(0).ToString()};
                for (int i = 0; i < arr.Length; i++)
                {
                    if (textBox1.Text.Equals(arr[i]))
                    {
                        Choices clist = new Choices();
                        clist.Add(arr[i]);
                        Grammar gr = new Grammar(new GrammarBuilder(clist));
                        rec.RequestRecognizerUpdate();
                        rec.LoadGrammar(gr);
                        rec.SpeechRecognized += Rec_SpeechRecognized;
                        rec.SetInputToDefaultAudioDevice();
                        rec.RecognizeAsync(RecognizeMode.Multiple);
                        say("login success");
                        MessageBox.Show(arr[i]);
                    }
                    else
                    {
                        //say("invalid");
                    }
                }
            }
            con.Close();
            con2 = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con2.Open();
            cmd2 = new SqlCommand("select password from alexa where name='"+textBox1.Text+"'", con2);
            read = cmd2.ExecuteReader();
            while (read.Read())
            {
                string pa = read.GetValue(0).ToString();
                if(textBox2.Text == pa)
                {
                    Choices clist = new Choices();
                    clist.Add(pa);
                    Grammar gr = new Grammar(new GrammarBuilder(clist));
                    rec.RequestRecognizerUpdate();
                    rec.LoadGrammar(gr);
                    rec.SpeechRecognized += Rec_SpeechRecognized;
                    rec.SetInputToDefaultAudioDevice();
                    rec.RecognizeAsync(RecognizeMode.Multiple);
                    say("success");
                    MessageBox.Show(pa);
                }
                else
                {
                    MessageBox.Show("invalid");
                }
            }
            con2.Close();
        }

        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            try
            {

                var text = e.Result.Text;
                /*
                if (text == "forgot password")
                {
                    say("but don't forgot your security pin number dear user,then alexa cannot recognize you");
                    Forgot_password fp = new Forgot_password();
                    fp.Show();
                }
                con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
                con.Open();
                cmd = new SqlCommand("select name from alexa", con);

                data = cmd.ExecuteReader();
                while (data.Read())
                {
                    string[] arr = { data.GetValue(0).ToString() };
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (text.Equals(arr[i]))
                        {
                            say("okay okay");
                        }
                    }
                }
                con.Close();
                con2 = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
                con2.Open();
                cmd2 = new SqlCommand("select password from alexa where name='" + textBox1.Text + "'", con2);
                read = cmd2.ExecuteReader();
                while (read.Read())
                {
                    string pa = read.GetValue(0).ToString();
                    if(text == pa)
                    {
                        textBox2.Text = pa;
                    }
                }
                */
                if(text == "show password")
                {
                    if (textBox2.Text.Length != 0)
                    {
                        textBox2.PasswordChar = '\0';
                    }
                    else
                    {
                        say("There is nothing to show please enter the password first");
                    }
                }
                if(text == "hide password")
                {
                    if (textBox2.Text.Length != 0)
                    {
                        textBox2.PasswordChar = '*';
                    }
                    else
                    {
                        say("There is nothing to hide please enter the password first");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
