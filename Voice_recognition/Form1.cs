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
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Media;
using System.Data.SqlClient;

namespace Voice_recognition
{
    public partial class Form1 : Form
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader data;
        public Form1()
        {
            InitializeComponent();
        }
        WMPLib.WindowsMediaPlayer wp = new WMPLib.WindowsMediaPlayer();
        System.Media.SoundPlayer pl = new System.Media.SoundPlayer();
        public string user = Login.Username;
        public String temp, condition;
        public Boolean search = false;

        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();

        SpeechRecognitionEngine robo = new SpeechRecognitionEngine();
        Boolean wake = true;
        int rectime = 0;

        //SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);

        public String GetWeather(String input)
        {
            try
            {
                String query = String.Format("https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='chennai, or')&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
                XmlDocument wData = new XmlDocument();
                wData.Load(query);

                XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
                manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

                XmlNode channel = wData.SelectSingleNode("query").SelectSingleNode("results").SelectSingleNode("channel");
                XmlNodeList nodes = wData.SelectNodes("query/results/channel");
            
                    temp = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;
                    condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;
                  //  high = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["high"].Value;
                  //  low = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["low"].Value;
                    if (input == "temp")
                    {
                        return temp;
                    }
                    if (input == "high")
                    {
                        //return high;
                    }
                    if (input == "low")
                    {
                       // return low;
                    }
                    if (input == "cond")
                    {
                        return condition;
                    }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return "error";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void restart()
        {
            Process.Start(@"E:\Dot_Net\alexa\Voice_recognition.exe");
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            syn.SelectVoiceByHints(VoiceGender.Female);
            syn.Speak("welcome Mr " +user+ ", my name is voice robot ,And my nick name is alexa");
            volumetrackbar.LargeChange = 10;
            volumetrackbar.SmallChange = 2;
            speedtrackbar.SmallChange = 2;
            speedtrackbar.LargeChange = 10;
            listBox1.Visible = false;
            //InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            syn.Volume = volumetrackbar.Value;
            syn.Rate = speedtrackbar.Value;

            if (comboBox1.Text == "male")
            {
                syn.SelectVoiceByHints(VoiceGender.Male);
            }
            if (comboBox1.Text == "female")
            {
                syn.SelectVoiceByHints(VoiceGender.Female);
            }

            pb.ClearContent();
            pb.AppendText(richTextBox1.Text);
            syn.Speak(pb);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            button2.Enabled = false;
            button1.Enabled = true; 
            button3.Enabled = true;
            Choices clist = new Choices(File.ReadAllLines(@"Commands.txt"));
            Grammar gr = new Grammar(new GrammarBuilder(clist));
            
            try
            {

                if (comboBox1.Text == "male")
                {
                    syn.SelectVoiceByHints(VoiceGender.Male);
                }
                if (comboBox1.Text == "female")
                {
                    syn.SelectVoiceByHints(VoiceGender.Female);
                }
                
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(gr);
                rec.SpeechRecognized += Rec_SpeechRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);

                robo.SetInputToDefaultAudioDevice();
                robo.LoadGrammarAsync(new Grammar(new Choices(File.ReadAllLines(@"Commands.txt"))));
                robo.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(robo_SpeechRecognized);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"error",MessageBoxButtons.AbortRetryIgnore,MessageBoxIcon.Asterisk);
            }
            
        }

        private void robo_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string t = e.Result.Text;
            if(t == "hey robo")
            {
                say("");
            }
        }

        public void say(String s)
        {
            syn.Speak(s);
            richTextBox2.AppendText(s + "\n");
        }

        public static void killpgm(String k)
        {
            System.Diagnostics.Process[] procs = null;


            try
            {
                procs = Process.GetProcessesByName(k);
                Process prog = procs[0];

                if (!prog.HasExited)
                {
                    prog.Kill();
                }
            }
            finally
            {
                if (procs != null)
                {
                    foreach(Process p in procs)
                    {
                        p.Dispose();
                    }
                }
            }
        }

        String[] greetings = { "hi", "how are you", "hello", "how u doing" };
        String[] media = { @"D:\songs\raj.mp3", @"D:\songs\tokyo.mp3" };
        public String greeting()
        {
            Random r = new Random();
            return greetings[r.Next(4)];
        }
        public String player()
        {
            Random rr = new Random();
            return media[rr.Next(2)];
        }

        private void Rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            rectime = 0;
            var text = e.Result.Text;
            Random rd = new Random();
            int ran;

            // MessageBox.Show("speech recognized" + e.Result.Text.ToString());
            if (text == "wake" || text == "hey robo")
            {
                wake = true;
                say(greeting());
                label6.Text = "state : Awake";
            }
            if (text == "sleep")
            {
                wake = false;
                label6.Text = "state : Sleep mode";
            }
            if (search)
            {
                Process.Start("https://www.google.com/#q="+text);
                search = false;
            }
            
            if(text == "stop listening")
            {
                say("If you need me just ask" + user);
            }
            if(text == "stop talking")
            {
                //syn.SpeakAsyncCancelAll();
                ran = rd.Next(2);
                if(ran == 1)
                {
                    say("Yes sir");
                }
                if(ran == 2)
                {
                    say("I am sorry i will be quiet");
                }
            }
            if(text == "alexa play a song")
            {
                say("alexa will play the default song that developer has programmed,if you want to play more songs access media player");
                wp.URL = @"D:\songs\cheap thrills.mp3";
                wp.controls.play();
            }
            if(text == "stop the song")
            {
                wp.controls.stop();
            }
            if(text == "resume the song")
            {
                wp.controls.play();
            }
            if(text == "change the song")
            {
                wp.URL = @"D:\songs\tokyo.mp3";
                wp.controls.next();
            }
            if(text == "pause the song")
            {
                wp.controls.pause();
            }
            if(text == "read selected text")
            {
                say(richTextBox1.SelectedText);
            }
            if(text == "alexa select the male voice")
            {
                say("okay master");
                comboBox1.Text = "male";
                syn.SelectVoiceByHints(VoiceGender.Male);
            }
            if(text == "alexa select the female voice")
            {
                say("okay dude");
                comboBox1.Text = "female";
                syn.SelectVoiceByHints(VoiceGender.Female);
            }
            if(text == "alexa Type something in the input box")
            {
                say("okay master");
                richTextBox1.Text = "Hi This is " + user + ". How are you ,This is the audio message sent by " + user;
                say("Now you can convert the text to speech by saying save the audio");
            }
            if(text == "alexa clear the input box")
            {
                say("okay master");
                richTextBox1.Clear();
            }
            
            if (wake == true && search == false)
            {
                if(text == "search for")
                {
                    say("If you are having the godd internet connection or not");
                    DialogResult dr = MessageBox.Show("msg", "error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if(dr == DialogResult.Yes)
                    {
                        search = true;
                    }
                    else
                    {
                        say("Please make sure you are having the internet connection");
                    }
                    
                }
                if(text == "alexa tell me a joke")
                {
                    say("I am not a comedian to tell you a joke");
                    say("hahan");
                } 
                if(text == "save the audio")
                {
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "Mp3 files|*.mp3|wav files|*.wav";
                        sfd.Title = "save the audio";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                            syn.SetOutputToWaveStream(fs);
                            syn.Speak(richTextBox1.Text);
                        }
                    }
                }
                /*
                if(text == "light on")
                {
                    port.Open();
                    port.WriteLine("A");
                    port.Close();
                }
                
                if (text == "light off")
                {
                    port.Open();
                    port.WriteLine("B");
                    port.Close();
                }
                */
                if(text == "whats the weather like")
                {
                    say("the sky is, " + GetWeather("cond") + ".");
                }
                if(text == "open office")
                {
                    Process.Start(@"C:\Program Files (x86)\OpenOffice 4\program\soffice.exe");
                }
                if(text == "close office")
                {
                    killpgm("soffice.bin");
                }
                if(text == "minimize")
                {
                    say("okay master");
                    this.WindowState = FormWindowState.Minimized;
                }
                if(text == "maximize")
                {
                    say("yeah dude");
                    this.WindowState = FormWindowState.Maximized;
                }
                
                if(text == "open spotify")
                {
                    Process.Start(@"D:\Spotify.exe");
                }
                if(text == "close spotify")
                {
                    killpgm("Spotify");
                }
                if(text == "play" || text == "pause")
                {
                    SendKeys.Send(" ");
                }
                if(text == "next")
                {
                    SendKeys.Send("^{RIGHT}");
                }
                if(text == "previous")
                {
                    SendKeys.Send("^{LEFT}");
                }
                if(text == "open media player")
                {
                    //Process.Start(@"C:\Program Files (x86)\Windows Media Player\wmplayer.exe");
                    media m = new media();
                    m.Show();
                }

                if(text == "close media player")
                {
                    killpgm("wmplayer");
                }
                if(text == "increase volume")
                {
                    volumetrackbar.Value = 100;
                }
                if(text == "decrease volume")
                {
                    volumetrackbar.Value = 40;
                    volumetrackbar.Value = 20;
                }
                if(text == "mute volume")
                {
                    volumetrackbar.Value = 0;
                }
                
                if(text == "increase speed")
                {
                    speedtrackbar.Value = 10;
                }
                if(text == "decrease speed")
                {
                    speedtrackbar.Value = -5;
                }
                /*
                if(text == "open music")
                {
                    Process.Start(@"C:\Program Files\WindowsApps\Microsoft.ZuneMusic_10.20011.10711.0_x64__8wekyb3d8bbwe\Music.UI.exe");
                }
                if(text == "close music")
                {
                    killpgm("Microsoft.ZuneMusic_10.20011.10711.0_x64__8wekyb3d8bbwe");
                }
                */
                if(text == "open explorer")
                {
                    Process.Start(@"C:\Windows\explorer.exe");
                }
                if(text == "close explorer")
                {
                    killpgm("explorer");
                }
                if(text == "alexa can you stop talking for sometime")
                {
                    rec.RecognizeAsyncStop();
                    button3.Enabled = false;
                    button2.Enabled = true;
                    say("okay Mr " + user);
                }
                if(text == "alexa close your mouth")
                {
                    try {
                        say("I am not a human being to close the mouth, instead I can pause me");
                        syn.Pause();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if(text == "alexa resume the application")
                {
                    say("okay master");
                    syn.Resume();
                }

                if(text == "alexa read the input text")
                {
                    syn.Volume = volumetrackbar.Value;
                    syn.Rate = speedtrackbar.Value;

                    if (comboBox1.Text == "male")
                    {
                        syn.SelectVoiceByHints(VoiceGender.Male);
                    }
                    if (comboBox1.Text == "female")
                    {
                        syn.SelectVoiceByHints(VoiceGender.Female);
                    }

                    pb.ClearContent();
                    pb.AppendText(richTextBox1.Text);
                    syn.Speak(pb);
                }
                if (text == "restart")
                {
                    say("restarting the application");
                    restart();
                }

                if (text == "exit")
                {
                    say("okay, bye master");
                    Application.Exit();
                }
                if (text == "hello")
                {
                    say("hi Mr "+user+",nice to see you");
                }
                if (text == "what time is it")
                {
                    say("current time is " + DateTime.Now.ToString("h:mm tt"));
                }
                if (text == "what is today")
                {
                    say(DateTime.Now.ToString("M/d/yyyy"));
                }
                if (text == "how are you")
                {
                    say("I am fine mr."+user+",what about you");
                }
                if (text == "open google")
                {
                    Process.Start("http://www.google.com");
                }
                if (text == "who developed you")
                {
                    say("Mr rajkumar developed me, that is voice robot");
                }
                if(text == "show commands")
                {
                    string[] comm = (File.ReadAllLines(@"Commands.txt"));
                    listBox1.Items.Clear();
                    listBox1.SelectionMode = SelectionMode.None;
                    listBox1.Visible = true;
                    foreach(string command in comm)
                    {
                        listBox1.Items.Add(command);
                    }
                }
                
                if(text == "hide commands")
                {
                    listBox1.Visible = false;
                }
                //richTextBox1.Text += " " + e.Result.Text.ToString();
                richTextBox1.AppendText(text + "\n");
            }
            if(text == "alexa tell me something about yourself")
            {
                say("I am alexa (voice robot)");
                say("My date of creation is 6th march 2020");
                say("I am a voice assistant useful to do some operations on your computer or laptop");
            }
            con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
            con.Open();
            cmd = new SqlCommand("select * from alexa where name='" + user + "'", con);
            data = cmd.ExecuteReader();
            while (data.Read())
            {
                string n = data.GetValue(0).ToString();
                string em = data.GetValue(3).ToString();
                string dob = data.GetValue(4).ToString();
                if (text == "alexa tell me something about myself")
                {
                    say("your name is : " + user);
                    say("Your date of birth is : " + dob);
                    say("Your email id is : " + em);
                }
            }
            
            if(text == "alexa tell me about the developer")
            {
                say("The developer name is Rajkumar");
                say("he is an MCA graduated student from madras christian college");
                say("He lives in chennai");
            }
     /*
            switch (e.Result.Text)
            {
                case "hello":
                    //syn.SpeakAsync("hello raj good to see you");
                    say("hi mr.raj, nice to see you");
                    break;
                case "open google":
                    System.Diagnostics.Process.Start("chrome", "http://www.google.com");
                    break;
                case "what time is it":
                    say("current time is : " + DateTime.Now.ToString("h:mm tt"));
                    break;
                case "how are you":
                    say("I am fine mr.raj,what about you");
                    break;
                case "who developed you":
                    say("Mr.Rajkumar developed me");
                    break;
                case "exit":
                    Application.Exit();
                    break;
            }
            */
            syn.Volume = volumetrackbar.Value;
            syn.Rate = speedtrackbar.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rec.RecognizeAsyncStop();
            button3.Enabled = false;
            button2.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //syn.Resume();
                listBox1.Visible = true;
                con = new SqlConnection(@"Data Source=RAJ;Initial Catalog=Raj;Integrated Security=True");
                con.Open();
                cmd = new SqlCommand("select name from alexa",con);
                data = cmd.ExecuteReader();
                while (data.Read())
                {
                    string[] arr = { data.GetValue(0).ToString() +"\n"};
                    for(int i=0; i<arr.Length; i++)
                    {
                        listBox1.Items.Add(arr[i]);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                syn.Pause();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Mp3 files|*.mp3|wav files|*.wav";
                        sfd.Title = "save to a mp3 file";
                    if(sfd.ShowDialog() == DialogResult.OK)
                    {
                        FileStream fs = new FileStream(sfd.FileName,FileMode.Create,FileAccess.Write);
                        syn.SetOutputToWaveStream(fs);
                        syn.Speak(richTextBox1.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SystemSounds.Question.Play();
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            feedback fb = new feedback();
            fb.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(rectime == 10)
            {
                rec.RecognizeAsyncCancel();
            }
            else if(rectime == 11)
            {
                timer1.Stop();
                rec.RecognizeAsync(RecognizeMode.Multiple);
                rectime = 0;
            }
        }

        private void volumetrackbar_Scroll(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

    }
}
