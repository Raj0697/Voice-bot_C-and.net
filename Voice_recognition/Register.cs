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
using System.Text.RegularExpressions;

namespace Voice_recognition
{
    public partial class Register : Form
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader data;

        public Register()
        {
            InitializeComponent();
        }
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        DateTimePicker dtp = new DateTimePicker();
        

        public void say(String s)
        {
            syn.Speak(s);
            // richTextBox2.AppendText(s + "\n");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int flag = 0;
            try
            {
                int year = dtp.Value.Year;
                int month = dtp.Value.Month;
                int day = dtp.Value.Day;
                if(textBox1.Text.Length != 0 && textBox2.Text.Length != 0 && textBox3.Text.Length != 0 && textBox4.Text.Length !=0 && textBox5.Text.Length != 0 && textBox6.Text.Length != 0 && dateTimePicker1.Text != " ")
                {
                    String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                    con = new SqlConnection(conn);
                    con.Open();
                    String sql = "select * from alexa where name='" + textBox1.Text + "'";
                    cmd = new SqlCommand(sql, con);
                    data = cmd.ExecuteReader();
                    if (data.Read())
                    {
                        String un = data.GetValue(0).ToString();
                        //String em = data.GetValue(1).ToString();
                        String p = data.GetValue(5).ToString();
                        // int spin = int.Parse(data.GetValue(1).ToString());
                        flag = 1;

                    }
                    con.Close();

                    if (flag == 1)
                    {
                        errorProvider1.SetError(textBox1, "Enter different username!!");
                        say("The username " + textBox1.Text + " is already available,so please enter different username");
                    }
                    else
                    {
                        String conn3 = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                        con = new SqlConnection(conn3);
                        con.Open();
                        String sql3 = "insert into alexa values (@name,@pass,@conf,@email,@dob,@code,@pin)";
                        cmd = new SqlCommand(sql3, con);
                        cmd.Parameters.AddWithValue("@name", textBox1.Text);
                        cmd.Parameters.AddWithValue("@pass", textBox2.Text);
                        cmd.Parameters.AddWithValue("@conf", textBox3.Text);
                        cmd.Parameters.AddWithValue("@email", textBox4.Text);
                        cmd.Parameters.AddWithValue("@dob", dateTimePicker1.Text);
                        cmd.Parameters.AddWithValue("@code", textBox6.Text);
                        cmd.Parameters.AddWithValue("@pin", textBox5.Text);
                        cmd.ExecuteNonQuery();
                        say("The user " + textBox1.Text + " has been registered successfully");
                        MessageBox.Show("success");
                        con.Close();
                    }
                }
                else
                {
                    say("some fields are empty");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Register_Load(object sender, EventArgs e)
        {
            syn.SelectVoiceByHints(VoiceGender.Female);
            //  syn.Speak("hello, my name is voice robot.");
            //syn.Speak("Fill all the details to register into alexa");
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = " ";

            Choices clist = new Choices();
            clist.Add(new string[] { "show password", "hide password","show confirm password","hide confirm password" });
            Grammar gr = new Grammar(new GrammarBuilder(clist));
            rec.RequestRecognizerUpdate();
            rec.LoadGrammar(gr);
            rec.SpeechRecognized += Rec_SpeechRecognized;
            rec.SetInputToDefaultAudioDevice();
            rec.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var t = e.Result.Text;

            if(t == "show password")
            {
                if(textBox2.Text.Length != 0)
                {
                    textBox2.PasswordChar = '\0';
                }
                else
                {
                    say("There is nothing to show please enter the password first");
                }
            }
            if(t == "hide password")
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
            if(t == "show confirm password")
            {
                if (textBox3.Text.Length != 0)
                {
                    textBox3.PasswordChar = '\0';
                }
                else
                {
                    say("There is nothing to show please enter the password first");
                }
            }
            if (t == "hide confirm password")
            {
                if (textBox3.Text.Length != 0)
                {
                    textBox3.PasswordChar = '*';
                }
                else
                {
                    say("There is nothing to hide please enter the password first");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
         //   Login l = new Login();
           // l.Show();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String conn2 = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn2);
                con.Open();
                String sql2 = "select pin from alexa where pin='" + textBox5.Text + "'";
                cmd = new SqlCommand(sql2, con);
                data = cmd.ExecuteReader();
                while (data.Read())
                {
                    String p = data.GetValue(0).ToString();

                    if (p == textBox5.Text)
                    {
                        pictureBox6.Image = Properties.Resources.invalid;
                        errorProvider5.SetError(textBox5, "Enter different pin number");
                        MessageBox.Show(p + " is already available");
                        textBox5.Focus();
                        say("Enter different pin number");   
                    }
                    else
                    {
                        pictureBox6.Image = Properties.Resources.valid;
                        errorProvider5.SetError(textBox5, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
            if (dateTimePicker1.Text == " ")
            {
                dateTimePicker1.CustomFormat = "yyyy-MM-dd";
                say("select your date of birth");
                dateTimePicker1.Focus();
                errorProvider5.SetError(dateTimePicker1, "please select the date of birth");
            }
            else
            {
               // dateTimePicker1.Value = DateTime.Today;
                errorProvider5.SetError(dateTimePicker1, "");
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
           // say("If you forgot your password ,it will be used to recover your password");
            try
            {
                String conn2 = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn2);
                con.Open();
                String sql2 = "select code from alexa where code='"+ textBox6.Text +"'";
                cmd = new SqlCommand(sql2, con);
                data = cmd.ExecuteReader();
                if(data.Read())
                {
                    String c = data.GetValue(0).ToString();

                    if (c == textBox6.Text)
                    {
                        pictureBox7.Image = Properties.Resources.invalid;
                        errorProvider6.SetError(textBox6, "Enter different character code");
                        MessageBox.Show(c + " is already available");
                        textBox6.Focus();
                        say("Enter different character code");
                    }
                }
                else
                {
                    pictureBox7.Image = Properties.Resources.valid;
                    errorProvider6.SetError(textBox6, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                say("Username is mandatory");
                e.Cancel = true;
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Enter the username");
                pictureBox1.Image = Properties.Resources.invalid;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox1, null);

                String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn);
                con.Open();
                String sql = "select name from alexa where name='" + textBox1.Text + "'";
                cmd = new SqlCommand(sql, con);
                data = cmd.ExecuteReader();
                if (data.Read())
                {
                    String us = data.GetValue(0).ToString();
                    if (us == textBox1.Text)
                    {
                        pictureBox1.Image = Properties.Resources.invalid;
                        say("Enter different Username");
                        errorProvider1.SetError(textBox1, textBox1.Text + " is already available");
                        textBox1.Focus();
                    }
                    else
                    {
                        pictureBox1.Image = Properties.Resources.valid;
                        errorProvider1.SetError(textBox1, null);
                    }
                }
                /*
                Regex num = new Regex(@"^([0-9]*|\d*)$");
                if (num.IsMatch(textBox1.Text))
                {
                    say("Alexa doesn't accept numbers and special characters in username");
                    e.Cancel = true;
                    textBox1.Focus();
                    errorProvider1.SetError(textBox1, "Username should not contain numbers and special characters");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(textBox1, null);
                }
                */
            }
        }

        private void dateTimePicker1_Validating(object sender, CancelEventArgs e)
        {
            //  dateTimePicker1.CustomFormat = " ";
            
            if (dateTimePicker1.Text == " ")
            {
                dateTimePicker1.CustomFormat = "yyyy-MM-dd";
                say("select your date of birth");
                e.Cancel = true;
                dateTimePicker1.Focus();
                errorProvider5.SetError(dateTimePicker1, "please select the date of birth");
            }
            else
            {
                
                e.Cancel = false;
                errorProvider5.SetError(dateTimePicker1, null);
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                pictureBox2.Image = Properties.Resources.invalid;
                say("password should not be empty");
                e.Cancel = true;
                textBox2.Focus();
                errorProvider2.SetError(textBox2, "set the password");
            }
            else
            {
                pictureBox2.Image = Properties.Resources.valid;
                e.Cancel = false;
                errorProvider2.SetError(textBox2, null);
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                pictureBox3.Image = Properties.Resources.invalid;
                say("confirm password should not be empty");
                e.Cancel = true;
                textBox3.Focus();
                errorProvider3.SetError(textBox3, "re-enter the password");
            }
            else
            {
                if(textBox2.Text != textBox3.Text)
                {
                    pictureBox3.Image = Properties.Resources.invalid;
                    say("password and confirm password doesn't match");
                    e.Cancel = true;
                    textBox3.Focus();
                    errorProvider3.SetError(textBox3, "Re-Enter your password");
                }
                else
                {
                    pictureBox3.Image = Properties.Resources.valid;
                    e.Cancel = false;
                    errorProvider3.SetError(textBox3, null);
                }
                
            }
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            // alphanum@alphanum.com
            Regex reg = new Regex(@"^[a-zA-z0-9]{1,20}@[a-zA-z0-9]{1,20}.[a-zA-Z]{2,3}$");
            Regex r = new Regex(@"^([\w]+)@([\w]+)\.([\w]+)$");
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                pictureBox4.Image = Properties.Resources.invalid;
                say("Email should not be empty " + textBox1.Text);
                e.Cancel = true;
                textBox4.Focus();
                errorProvider4.SetError(textBox4, "Enter your Email Id");
            }
            else
            {
                if (r.IsMatch(textBox4.Text))
                {
                    pictureBox4.Image = Properties.Resources.valid;
                    e.Cancel = false;
                    errorProvider4.SetError(textBox4, null);
                }
                else
                {
                    pictureBox4.Image = Properties.Resources.invalid;
                    say("Email Id is not in correct format");
                    e.Cancel = true;
                    textBox4.Focus();
                    errorProvider4.SetError(textBox4, "Enter the valid Email Id");
                }
                con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
                con.Open();
                String sql = "select email,name from alexa where email='" + textBox4.Text + "'";
                cmd = new SqlCommand(sql, con);
                data = cmd.ExecuteReader();
                if (data.Read())
                {
                    String em = data.GetValue(0).ToString();
                    String n = data.GetValue(1).ToString();
                    if (textBox4.Text == em)
                    {
                        pictureBox4.Image = Properties.Resources.invalid;
                        say("This email Id is already registered by " + n);
                        errorProvider4.SetError(textBox4, "This id is already registered");
                        textBox4.Focus();
                    }
                    else
                    {
                        errorProvider4.SetError(textBox4, "");
                    }
                }
            }
        }
        
        private void textBox5_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(textBox5.Text))
            {
                pictureBox6.Image = Properties.Resources.invalid;
                say("Security pin should not be empty");
                e.Cancel = true;
                textBox5.Focus();
                errorProvider5.SetError(textBox5, "please enter your security pin number");
                
            }
            else
            {
                String conn2 = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn2);
                con.Open();
                String sql2 = "select pin from alexa where pin='" + textBox5.Text + "'";
                cmd = new SqlCommand(sql2, con);
                data = cmd.ExecuteReader();
                if(data.Read())
                {
                    String p = data.GetValue(0).ToString();

                    if (p == textBox5.Text)
                    {
                        pictureBox6.Image = Properties.Resources.invalid;
                        errorProvider5.SetError(textBox5, "Enter different pin number");
                        MessageBox.Show(p + " is already available");
                        textBox5.Focus();
                        say("Enter different pin number");
                    }
                    else
                    {
                        pictureBox6.Image = Properties.Resources.valid;
                        errorProvider5.SetError(textBox5, null);
                    }
                }
                
            }
                /*
                Regex num = new Regex(@"^([0-9]*|\d*)$");
                if (num.IsMatch(textBox5.Text))
                {
                    e.Cancel = false;
                    errorProvider5.SetError(textBox5, "");
                }
                else
                {
                    say("only numbers are allowed");
                    errorProvider5.SetError(textBox5, "Enter only integers");
                }

            }
            */
        }

        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                pictureBox7.Image = Properties.Resources.invalid;
                say("Recovery character should not be empty");
                e.Cancel = true;
                textBox6.Focus();
                errorProvider6.SetError(textBox6, "Enter the recovery character");
                
            }
            else
            {
                String conn2 = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn2);
                con.Open();
                String sql2 = "select code from alexa where code='" + textBox6.Text + "'";
                cmd = new SqlCommand(sql2, con);
                data = cmd.ExecuteReader();
                if (data.Read())
                {
                    String c = data.GetValue(0).ToString();

                    if (c == textBox6.Text)
                    {
                        pictureBox7.Image = Properties.Resources.invalid;
                        errorProvider6.SetError(textBox6, "Enter different character code");
                        MessageBox.Show(c + " is already available");
                        textBox6.Focus();
                        say("Enter different character code");
                    }
                }
                else
                {
                    pictureBox7.Image = Properties.Resources.valid;
                    errorProvider6.SetError(textBox6, null);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox3.Clear();
          //  textBox2.PasswordChar = default(char);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            // e.keyChar != 8 -> here 8 denotes backspace key in key enumeration for c#
            if((!char.IsDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                say("Characters are not allowed ");
                textBox5.Focus();
              //  errorProvider5.SetError(textBox5, "enter digits only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                errorProvider5.SetError(textBox5, null);
                pictureBox6.Image = Properties.Resources.valid;
            }
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
                pictureBox1.Image = Properties.Resources.valid;
                e.Handled = false;
                errorProvider1.SetError(textBox1, null);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
            con = new SqlConnection(conn);
            con.Open();
            String sql = "select name from alexa where name='" + textBox1.Text + "'";
            cmd = new SqlCommand(sql, con);
            data = cmd.ExecuteReader();
            if (data.Read())
            {
                String us = data.GetValue(0).ToString();
                if (us == textBox1.Text)
                {
                    pictureBox1.Image = Properties.Resources.invalid;
                    say("Enter different Username");
                    errorProvider1.SetError(textBox1, textBox1.Text + " is already available");
                    textBox1.Focus();
                }
                else
                {
                    pictureBox1.Image = Properties.Resources.valid;
                    errorProvider1.SetError(textBox1, null);
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            String sql = "select email,name from alexa where email='"+textBox4.Text+"'";
            cmd = new SqlCommand(sql,con);
            data = cmd.ExecuteReader();
            if (data.Read())
            {
                String em = data.GetValue(0).ToString();
                String n = data.GetValue(1).ToString();
                if(textBox4.Text == em)
                {
                    pictureBox4.Image = Properties.Resources.invalid;
                    say("This email Id is already registered by " + n);
                    errorProvider4.SetError(textBox4, "This id is already registered");
                    textBox4.Focus();
                }
                else
                {
                    
                    errorProvider4.SetError(textBox4, "");
                }
            }
            else
            {
                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox3.PasswordChar = '\0';
            }
            else
            {
                textBox3.PasswordChar = '*';
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker1.CustomFormat = " ";
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
        }
    }
}

