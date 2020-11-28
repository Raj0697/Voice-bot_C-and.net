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
using System.Collections;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace Voice_recognition
{
    public partial class display : Form
    {
        public display()
        {
            InitializeComponent();
        }
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SqlDataAdapter adapter;
        DataSet ds;
        DataTable dt;
        ArrayList ar = new ArrayList();

        private void display_Load(object sender, EventArgs e)
        {
            Choices clist = new Choices(new string[] { "alexa show the users table","alexa show the feedback table","Tell me the total number of users" });
            Grammar gr = new Grammar(new GrammarBuilder(clist));
            syn.SelectVoiceByHints(VoiceGender.Female);
            syn.Speak("Welcome admin");
            rec.RequestRecognizerUpdate();
            rec.LoadGrammar(gr);
            rec.SpeechRecognized += Rec_SpeechRecognized;
            rec.SetInputToDefaultAudioDevice();
            rec.RecognizeAsync(RecognizeMode.Multiple);
        }
        public void say(String s)
        {
            syn.Speak(s);
        }
        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var text = e.Result.Text;
            if(text == "alexa show the users table")
            {
                say("okay admin");
                // TODO: This line of code loads data into the 'rajDataSet.alexa' table. You can move, or remove it, as needed.
                this.alexaTableAdapter.Fill(this.rajDataSet.alexa);
            }
            if(text == "alexa show the feedback table")
            {
                say("okay admin");
                // TODO: This line of code loads data into the 'rajDataSet1.feedback' table. You can move, or remove it, as needed.
                this.feedbackTableAdapter.Fill(this.rajDataSet1.feedback);
            }
            if(text == "Tell me the total number of users")
            {
                label1.Visible = true;
                SqlConnection con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("select count(name)from alexa", con);
                SqlDataReader data = cmd.ExecuteReader();
                if (data.Read())
                {
                    String count = data.GetValue(0).ToString();
                    label1.Text = count;
                    say("Currently " + count + "members are registered in the application");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            SqlConnection con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            /*
            SqlCommand cmd = new SqlCommand("select count(name)from alexa", con);
            SqlDataReader data = cmd.ExecuteReader();
            if (data.Read())
            {
                String count = data.GetValue(0).ToString();
                label1.Text = count;
            }
            */
            SqlCommand cmd = new SqlCommand("select name from alexa", con);
            int count = 0;
            string str = "";
            bool result = true;
            
            SqlDataReader data = cmd.ExecuteReader();
            /*
            while (data.Read())
            {
                str += data.GetValue(0).ToString() + "\n";
                listBox1.Items.Add(str);
            }
            */
            
            while (result == true)
            {
                while (data.Read())
                {
                    str += data.GetValue(0).ToString() + "\n";
                    count++;
                    if (count == 10)
                        break;
                }
                MessageBox.Show(str);
                string[] a = { str };
                
                result = data.NextResult();
            }
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //using dataset
        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            adapter = new SqlDataAdapter("select pin from alexa", con);
            ds = new DataSet();
            adapter.Fill(ds);
            dataGridView3.DataSource = ds.Tables[0];
        }
        // using data table
        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            adapter = new SqlDataAdapter("select password from alexa", con);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView4.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommandBuilder cmb = new SqlCommandBuilder(adapter);
            adapter.Update(ds);
        }
    }
}
