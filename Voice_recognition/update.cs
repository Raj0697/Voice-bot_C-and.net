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
    public partial class update : Form
    {
        public update()
        {
            InitializeComponent();
        }
        int flag = 0;
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader data;
        String user = Login.Username;

        private void update_Load(object sender, EventArgs e)
        {
            syn.Speak("Enter your old password to edit your profile");
            String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
            con = new SqlConnection(conn);
            con.Open();
            cmd = new SqlCommand("select * from alexa where name='"+user+"'", con);
            data = cmd.ExecuteReader();
            while (data.Read())
            {
                String n = data.GetValue(0).ToString();
                String p = data.GetValue(1).ToString();
                String em = data.GetValue(3).ToString();
                String dob = data.GetValue(4).ToString();
                textBox4.Text = em;
                dateTimePicker1.Text = dob;
            }
            textBox1.Text = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String conn = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
            con = new SqlConnection(conn);
            con.Open();
            cmd = new SqlCommand("select password from alexa where name='" + user + "'", con);
            data = cmd.ExecuteReader();
            while (data.Read())
            {
                String pass = data.GetValue(0).ToString();
                if (textBox2.Text == pass)
                {
                    //MessageBox.Show(pass);
                    flag = 0;
                }
                else
                {
                    flag = 1;
                }
            }
            con.Close();
            if(flag == 1)
            {
                syn.Speak("old password is wrong");
                errorProvider2.SetError(textBox2, "Enter your old password");
                pictureBox2.Image = Properties.Resources.invalid;
            }
            else if(textBox2.Text == textBox3.Text)
            {
                syn.Speak("new password shouldn't be same as old password");
                errorProvider3.SetError(textBox3, "Enter different password");
                pictureBox3.Image = Properties.Resources.invalid;
            }
            else if (textBox3.Text != textBox5.Text)
            {
                syn.Speak("new password and confirm password doesn't match");
                errorProvider4.SetError(textBox3, "Re-enter your confirm password");
                pictureBox4.Image = Properties.Resources.invalid;
            }
            else
            {
                String conn2 = @"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True";
                con = new SqlConnection(conn2);
                con.Open();
                cmd = new SqlCommand("update alexa set password='" + textBox3.Text + "',confirm='" + textBox5.Text + "',email='" + textBox4.Text + "',dob='" + dateTimePicker1.Text + "' where name='" + user + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                syn.Speak("Your profile has been updated successfully");
                MessageBox.Show("update success");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
