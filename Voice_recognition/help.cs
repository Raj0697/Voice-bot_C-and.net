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

namespace Voice_recognition
{
    public partial class help : Form
    {
        public help()
        {
            InitializeComponent();
        }
        SpeechSynthesizer syn = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();

        private void button1_Click(object sender, EventArgs e)
        {
            syn.SelectVoiceByHints(VoiceGender.Female);
            syn.Speak("Welcome to alexa world , the following points are important points you should remember to access the application");
            syn.Speak("The first thing you should keep in mind is alexa cannot reply to commands other than trained commands");
            syn.Speak("The second thing is that you can simply say show commands to see what are the commands available to access the application ");
            syn.Speak("The third thing is that some commands will not work without internet connection, so please ensure that you are having good internet connection");
            syn.Speak("The fourth thing is that only registered users can access the application");
            syn.Speak("The fifth thing while registering and logging in to the application, if you want to view the password you entered you can simply say show password,show confirm password and for hide password vice versa");
            syn.Speak("If you want to send any audio files to someone, you can simply type in the input box and save the audio");
            syn.Speak("At last if you found any errors in the application , you can post in the feedback form");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
