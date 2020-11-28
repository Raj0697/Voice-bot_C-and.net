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
using System.Data.SqlClient;
using System.IO;

namespace Voice_recognition
{
    public partial class media : Form
    {
        public media()
        {
            InitializeComponent();
            axWindowsMediaPlayer1.settings.volume = 50;
        }
        String username = Login.Username;
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader read;
        bool result = true;
        int count = 0;
        string[] files, path;
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SpeechRecognitionEngine md = new SpeechRecognitionEngine();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    files = openFileDialog1.SafeFileNames;
                    path = openFileDialog1.FileNames;
                    for (int i = 0; i < files.Length; i++)
                    {
                        listBox1.Items.Add(files[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OKCancel);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "mp4 files(.mp4)|*.mp4|mkv files(.mkv)|*.mkv|avi files(.avi)|*.avi|mov files(.mov)|*.mov|flv files(.flv)|*.flv|webm files(.webm)|*.webm";
            if(openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                axWindowsMediaPlayer1.URL = openFileDialog2.FileName;
            }
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }
        public void say(String s)
        {
            syn.SelectVoiceByHints(VoiceGender.Female);
            syn.Speak(s);
        }
        private void media_Load(object sender, EventArgs e)
        {
            listBox2.Visible = false;
            Choices clist = new Choices(File.ReadAllLines(@"mediacommands.txt"));
            Grammar gr = new Grammar(new GrammarBuilder(clist));
            syn.SelectVoiceByHints(VoiceGender.Female);
            rec.RequestRecognizerUpdate();
            rec.LoadGrammar(gr);
            rec.SpeechRecognized += Rec_SpeechRecognized;
            rec.SetInputToDefaultAudioDevice();
            rec.RecognizeAsync(RecognizeMode.Multiple);
            
            say("Just tell show commands to know how to access the voice based media player");
            
        }

        private void Md_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var t = e.Result.Text;
            con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            cmd = new SqlCommand("select names from songlist where user='"+username+"'",con);
            read = cmd.ExecuteReader();
            while (read.Read())
            {
                string nam = read.GetValue(0).ToString();
                string[] nn = nam.Split(',');
                foreach (string n in nn)
                {
                    
                    Choices clist2 = new Choices(n);
                    Grammar gr2 = new Grammar(new GrammarBuilder(clist2));
                    syn.SelectVoiceByHints(VoiceGender.Female);
                    md.RequestRecognizerUpdate();
                    md.LoadGrammar(gr2);

                    if (t == n)
                    {
                        MessageBox.Show(n);
                        int index = listBox1.FindString(n);
                        listBox1.SelectedIndex = index;
                    }
                }
             }
        }

       
        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var text = e.Result.Text;
            if(text == "open song folder")
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    files = openFileDialog1.SafeFileNames;
                    path = openFileDialog1.FileNames;
                    for (int i = 0; i < files.Length; i++)
                    {
                        listBox3.Items.Add(files[i]);
                    }
                }
            }
            if(text == "open video folder")
            {
                openFileDialog1.Title = "select the video";
                openFileDialog2.Filter = "mp4 files(.mp4)|*.mp4|mkv files(.mkv)|*.mkv|avi files(.avi)|*.avi|mov files(.mov)|*.mov|flv files(.flv)|*.flv|webm files(.webm)|*.webm";
                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    files = openFileDialog2.SafeFileNames;
                    path = openFileDialog2.FileNames;
                    for(int i=0; i < files.Length; i++)
                    {
                        listBox1.Items.Add(files[i]);
                    }
                   // axWindowsMediaPlayer1.URL = openFileDialog2.FileName;
                }
            }
            if(text == "fullscreen mode")
            {
                if(axWindowsMediaPlayer1.URL.Length != 0)
                {
                    axWindowsMediaPlayer1.fullScreen = true;
                }
            }
            if(text == "normal mode")
            {
                if (axWindowsMediaPlayer1.URL.Length != 0)
                {
                    axWindowsMediaPlayer1.fullScreen = false;
                }
            }
            if(text == "decrease volume")
            {
                axWindowsMediaPlayer1.settings.volume = 15;
            }
            if(text == "increase volume")
            {
                axWindowsMediaPlayer1.settings.volume = 85;
            }
            if(text == "set full volume")
            {
                axWindowsMediaPlayer1.settings.volume = 100;
            }
            if(text == "mute volume")
            {
                axWindowsMediaPlayer1.settings.mute = true;
            }
            if(text == "unmute")
            {
                axWindowsMediaPlayer1.settings.mute = false;
            }
            if(text == "exit")
            {
                Application.Exit();
            }
            if(text == "select the song")
            {
                listBox3.SelectionMode = SelectionMode.One;
                listBox3.SetSelected(0, true);
            }
            if(text == "select the video")
            {
                listBox1.SelectionMode = SelectionMode.One;
                listBox1.SetSelected(0, true);
            }

            if(text == "play the song")
            {
                axWindowsMediaPlayer1.URL = path[listBox3.SelectedIndex];
                //SendKeys.Send(" ");
            }
            if(text == "pause the song")
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
            if (text == "next song")
            {
                listBox3.SelectedIndex = listBox3.SelectedIndex + 1;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            if (text == "previous song")
            {
                listBox3.SelectedIndex = listBox3.SelectedIndex - 1;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            if(text == "stop the song")
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
            if(text == "play the video")
            {
                axWindowsMediaPlayer1.URL = openFileDialog2.FileName;
            }
            if(text == "pause the video")
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
            if(text == "stop the video")
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
            if(text == "next video")
            {
                listBox1.SelectedIndex += 1;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            if(text == "previous video")
            {
                listBox1.SelectedIndex -=  1;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            if(text == "play first song")
            {
                listBox3.SelectionMode = SelectionMode.One;
                listBox3.SetSelected(0, true);
            }
            if(text == "play second song")
            {
                listBox3.SelectionMode = SelectionMode.One;
                listBox3.SetSelected(1, true);
            }
            if(text == "alexa play")
            {
                int index = listBox3.FindString("raj");
                if(index > 0)
                {
                    listBox3.SelectedIndex = index;
                }
                else
                {
                    say("The song " + text + " not found");
                }
            }
            if(text == "play tokyo song")
            {
                int index = listBox3.FindString("tokyo");
                listBox3.SelectedIndex = index;
            }
            if(text == "play worldcup song")
            {
                int index = listBox3.FindString("World Cup");
                listBox3.SelectedIndex = index;
            }
            if(text == "play taki taki")
            {
                int index = listBox3.FindString("taki");
                listBox3.SelectedIndex = index;
            }
            if (text == "show commands")
            {
                string[] comm = (File.ReadAllLines(@"mediacommands.txt"));
                listBox2.Items.Clear();
                listBox2.SelectionMode = SelectionMode.None;
                listBox2.Visible = true;
                foreach (string command in comm)
                {
                    listBox2.Items.Add(command);
                }
            }

            if (text == "hide commands")
            {
                listBox2.Visible = false;
            }
            if(text == "rewind")
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition -= 10;
            }
            if(text == "forward")
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition += 10;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition += 10;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            cmd = new SqlCommand("select names from songlist", con);
            read = cmd.ExecuteReader();
            String str = "";

            while (result == true)
            {
                while (read.Read())
                {
                    str += read.GetValue(0).ToString() + "\n";
                    count++;
                    if (count == 10)
                        break;
                }
                MessageBox.Show(str);
                result = read.NextResult();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            cmd = new SqlCommand("select names from songlist where user='" + username + "'", con);
            read = cmd.ExecuteReader();
            while (read.Read())
            {
                string nam = read.GetValue(0).ToString();
                string[] na = nam.Split(',');
                foreach(string nn in na)
                {
                    MessageBox.Show(nn);
                    Choices clist2 = new Choices(nn);
                    Grammar gr2 = new Grammar(new GrammarBuilder(clist2));
                    syn.SelectVoiceByHints(VoiceGender.Female);
                    md.RequestRecognizerUpdate();
                    md.LoadGrammar(gr2);
                    md.SpeechRecognized += Md_SpeechRecognized;
                    md.SetInputToDefaultAudioDevice();
                }
                
                

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            cmd = new SqlCommand("select names from songlist where user='" + username + "'", con);
            read = cmd.ExecuteReader();
            while (read.Read())
            {
                string nam = read.GetValue(0).ToString();
                string[] na = nam.Split(',');
                foreach (string nn in na)
                {
                    MessageBox.Show(nn);
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer1.URL = path[listBox3.SelectedIndex];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer1.URL = path[listBox1.SelectedIndex];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
