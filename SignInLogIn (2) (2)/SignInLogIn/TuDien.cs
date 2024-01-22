using System;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TuDien
{
    public partial class TuDien : Form
    {
        public TuDien()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public static Socket client;

        string result = null; // JSON string receive from server
        int mode = 1; // detect = 1, word = 2, text = 3, exam = 4

        // Examples button needed
        string examples = null;
        private bool btnWordWasClicked = false;

        private void TuDien_Load(object sender, EventArgs e)
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

        private void DisplayMessage()
        {
            if (result != null)
            {
                switch (mode)
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
                        // Deserialized the json string to translationsreponse class object
                        SignInLogIn.translationsResponse json2 = JsonConvert.DeserializeObject<SignInLogIn.translationsResponse>(result);
                        // Display the translation result 
                        foreach (var translation in json2.translations)
                        {
                            if (translation.displayTarget.ToLower() != input.Text)
                            {
                                output.Text += translation.displayTarget + " (" + translation.posTag + ")\n";
                            }
                            foreach (var backtrans in translation.backTranslations)
                            {
                                if (backtrans.displayText.ToLower() != input.Text)
                                {
                                    output2.Text += backtrans.displayText + "\n";
                                    if (backtrans.normalizedText != translation.normalizedTarget.ToLower())
                                    {
                                        examples += (backtrans.normalizedText + "\t" + translation.normalizedTarget) + "\n";
                                    }
                                }
                            }
                        }
                        if (output.Text == String.Empty)
                        {
                            output.Text = "Word not found/ Cannot be translated :( Maybe you can try the TRANSLATE button or try choose the right language!";
                        }
                        break;
                    case 3:
                        // Deserialized the json string to translatetextsreponse class object
                        Azure_Translator_Service.translatetextResponse json3 = JsonConvert.DeserializeObject<Azure_Translator_Service.translatetextResponse>(result);

                        // Display the result
                        foreach (Azure_Translator_Service.translatetext tx in json3.translations)
                        {
                            output.Text += tx.text + "\n";
                        }
                        if (output.Text == String.Empty)
                        {
                            output.Text = "Word not found/ Cannot be translated :( Maybe you should check the language again or choose Detect language.";
                        }
                        break;
                    case 4:
                        // Deserialized the json string to examplesresponse class object
                        SignInLogIn.examplesresponse json4 = JsonConvert.DeserializeObject<SignInLogIn.examplesresponse>(result);

                        // Display the examples
                        foreach (SignInLogIn.example ex in json4.examples)
                        {
                            view.AppendText(ex.sourcePrefix + ex.sourceTerm + ex.sourceSuffix + "\n=> "
                                + ex.targetPrefix + ex.targetTerm + ex.targetSuffix + "\n\n");
                        }
                        break;
                    default:
                        view.Text += "Cannot deserialize object\n";
                        break;
                }
            }
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

        // Speech to text
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

        // Text translate
        private void text_translate_Click(object sender, EventArgs e)
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
            result = null;
            mode = 3;
            if (output.Text != String.Empty)
            {
                output.Text = String.Empty;
            }
            string mess = "text:" + lang1.Text + ":" + lang2.Text + ":" + input.Text + '\n';
            client.Send(Serialize(mess));
        }

        private void TuDien_FormClosed(object sender, FormClosedEventArgs e)
        {
            //tcpClient.Close();
            client.Close();
        }

        // Word lookup
        private void wordlookup_Click(object sender, EventArgs e)
        {
            result = null;
            if (lang1.Text == String.Empty || lang2.Text == String.Empty || lang1.Text == lang2.Text)
            {
                MessageBox.Show("Please choose the language/ Please select a different choice for each one");
                return;
            }
            if (lang1.Text == "Detect language")
            {
                MessageBox.Show("Detect is only available for translate button.");
                return;
            }
            // Get the word that need to translate
            if (input.Text == string.Empty)
            {
                MessageBox.Show("Please enter a word");
                return;
            }
            if (input.Text.Split(' ').Length > 2)
            {
                MessageBox.Show("Please use translate button instead");
                return;
            }
            // Clear previous output
            if (output.Text != String.Empty)
            {
                output.Text = output2.Text = view.Text = String.Empty;
            }
            mode = 2;
            // Send input to server
            string mess = "dict:" + lang1.Text + ":" + lang2.Text + ":" + input.Text + '\n';
            client.Send(Serialize(mess));
            btnWordWasClicked = true;
        }
        
        // Get the examples
        private void btnExamples_Click(object sender, EventArgs e)
        {
            if(btnWordWasClicked == false)
            {
                MessageBox.Show("Please use the button Word first! Thank you.");
                return;
            }
            mode = 4;
            result = null;
            // Send input to server
            string mess = "exam:" + lang1.Text + ":" + lang2.Text + ":" + examples + '\n';
            client.Send(Serialize(mess));

            // Clear string
            examples = null;
            btnWordWasClicked = false;
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
    }
}
