using System;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SignInLogIn
{
    public partial class Text_translator : Form
    {
        public Text_translator()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public static Socket client;

        string result = null;
        int mode = 1;

        private void translate_Click(object sender, EventArgs e)
        {
            if (lang1.Text == String.Empty || lang2.Text == String.Empty || lang1.Text == lang2.Text)
            {
                MessageBox.Show("Please choose the language/Please select a different choice for each one");
                return;
            }
            if (input.Text == String.Empty)
            {
                MessageBox.Show("Please enter your text");
                return;
            }
            if (output.Text != String.Empty)
            {
                output.Text = String.Empty;
            }
            mode = 2;
            string mess = "text:" + lang1.Text + ":" + lang2.Text + ":" + input.Text + '\n';
            client.Send(Serialize(mess));
        }

        private void speech2text_Click(object sender, EventArgs e)
        {
            result = null;
            // Checking the user's input
            if (input.Text == string.Empty)
            {
                MessageBox.Show("There is nothing to read. Please enter a word/sentence!", "Attention");
                return;
            }
            mode = 1;
            // Send input to server
            string mess = "detect:" + input.Text + '\n';
            client.Send(Serialize(mess));
        }

        /// <summary>
        /// serialize (phân mảnh)
        /// </summary>
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }


        /// <summary>
        /// deserialize (tổng hợp mảnh)
        /// </summary>
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        private void Text_translator_Load(object sender, EventArgs e)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(ipAddress, 9999);
            try
            {
                client.Connect(iPEndPoint);
                output.Text += "Bạn đã vào Server thành công." + '\n';
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            Thread clientThread = new Thread(ReceiveMessage);
            clientThread.IsBackground = true;
            clientThread.Start();
        }

        void ReceiveMessage()
        {
            string returnData;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    returnData = (string)Deserialize(data);

                    // Receive server's response
                    result = returnData;
                    DisplayMessage();
                }
            }
            catch 
            {
                //tcpClient.Close();
                client.Close();
            }
        }

        void DisplayMessage()
        {
            if(result != null)
            {
                switch(mode)
                {
                    case 1:
                        // Deserialized the json string to detectionreponse class object
                        Azure_Translator_Service.detectionResponse json1 = JsonConvert.DeserializeObject<Azure_Translator_Service.detectionResponse>(result);
                        if (json1.language != "en")
                        {
                            MessageBox.Show("Speech to text service is only available in English! Sorry", "Attention");
                        }
                        else
                        {
                            Speech();
                        }
                        break;
                    case 2:
                        // Deserialized the json string to translatetextsreponse class object
                        Azure_Translator_Service.translatetextResponse json2 = JsonConvert.DeserializeObject<Azure_Translator_Service.translatetextResponse>(result);

                        // Display the result
                        foreach (Azure_Translator_Service.translatetext tx in json2.translations)
                        {
                            output.Text += tx.text + "\n";
                        }
                        if (output.Text == String.Empty)
                        {
                            output.Text = "Word not found/ Cannot be translated :( Maybe you should check the language again or choose Detect language.";
                        }
                        break;
                    default:
                        output.Text += "Cannot deserialize object\n";
                        break;
                }
            }
        }
        void Speech()
        {
            // Initialize a new instance of the speech synthesizer.
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            // Select the female voice coz i like it
            speechSynthesizer.SelectVoice("Microsoft Zira Desktop");
            // Configure the synthesizer to send output to the default audio device.
            speechSynthesizer.SetOutputToDefaultAudioDevice();
            // Speak the user's input.
            speechSynthesizer.Speak(input.Text.Trim());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    } // Class ended
}
