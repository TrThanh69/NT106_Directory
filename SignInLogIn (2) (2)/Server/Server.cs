using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Server
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        // Azure API needed
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
        private static readonly string key1 = "457b567e0bd24a6fb8bd64f93be1cfa5";
        private static readonly string location = "southeastasia";

        // HttpClient for application 
        private static readonly HttpClient client = new HttpClient();

        // List of client
        List<Socket> clientList = new List<Socket>();
        TcpListener tcpListener;

        // Translate needed
        char[] charsToTrim = { '*', ' ', '\'', '\n', '\t', ',', '-' };
        string result, tolanguage, fromlanguage, wordToTranslate;
        string[] mess;

        private void btnListen_Click(object sender, EventArgs e)
        {
            Thread serverThread = new Thread(new ThreadStart(Listener));
            serverThread.Start();
            btnListen.Enabled = false;
        }

        void Listener()
        {
            // Associates a Socket with a local endpoint.
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            tcpListener.Start();
            while(true)
            {
                Socket client = tcpListener.AcceptSocket();
                clientList.Add(client);
                Thread clientThread = new Thread(new ParameterizedThreadStart(Receive));
                clientThread.Start(client);
            }  
        }
        void Receive(object obj)
        {
            Socket clientSocket = obj as Socket;
            var endPoint = (IPEndPoint)clientSocket.RemoteEndPoint;
            view.Text += "New client connected from: " + endPoint + '\n';
            try
            {
                while (clientSocket.Connected)
                {
                    string text = "";
                    do
                    {
                        // Create a data receive buffer 
                        byte[] recv = new byte[1024 * 5000];
                        //byte[] recv = new byte[1];

                        //Receives data from a bound Socket into a receive buffer.
                         int bytesReceived = clientSocket.Receive(recv);

                        // Giai ma buffer byte thanh ki tu string 
                        text = (string) Deserialize(recv);
                        //text += Encoding.Unicode.GetString(recv);
                    } while (text[text.Length - 1] != '\n'); // Keep listen until receive '\n' char

                    // Analyze client's messagestring
                    if (text != null)
                    {
                        mess = text.Trim(charsToTrim).Split(':');
                        if (mess[0] != null)
                        {
                            view.Text += "Received from client: " + mess[0] + " - ";
                        }
                        Mode(clientSocket);
                    }
                    else
                        view.Text += "Message from client is null\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                clientList.Remove(clientSocket);
                clientSocket.Close();
                view.Text += "A client from " + endPoint + " disconnected\n";
            }
        }
        void Mode(Socket clientSocket)
        {
            switch (mess[0])
            {
                case "dict":
                    fromlanguage = (mess[1] == "English") ? "en" : "vi";
                    tolanguage = (mess[2] == "English") ? "en" : "vi";
                    wordToTranslate = mess[3];
                    view.Text += wordToTranslate + "\n";
                    WordLookUp(clientSocket);
                    break;
                case "text":
                    tolanguage = (mess[2] == "English") ? "en" : "vi";
                    wordToTranslate = mess[3];
                    view.Text += wordToTranslate + "\n";
                    string route = "";
                    if (mess[1] == "Detect language")
                    {
                        // Output languages are defined as parameters, input language detected.
                        route = $"/translate?api-version=3.0&to={tolanguage}";
                    }
                    else
                    {
                        fromlanguage = (mess[1] == "English") ? "en" : "vi";
                        route = $"/translate?api-version=3.0&from={fromlanguage}&to={tolanguage}";
                    }
                    TextTranslate(route, clientSocket);
                    break;
                case "detect":
                    wordToTranslate = mess[1];
                    view.Text += wordToTranslate + "\n";
                    Text2Speech(clientSocket);
                    break;
                case "exam":
                    fromlanguage = (mess[1] == "English") ? "en" : "vi";
                    tolanguage = (mess[2] == "English") ? "en" : "vi";
                    string[] examples = mess[3].Split('\n');
                    foreach(string example in examples)
                    {
                        string[] src = example.Trim().Split('\t');
                        string textsrc = src[0], trans = src[1];
                        view.Text += textsrc + ", " + trans + "\n";
                        Examples(textsrc, trans, clientSocket);
                    }
                    break;
                default:
                    result = "Error in message by client. Missing something?";
                    break;
            }
        }

        // Phan/Tong hop manh
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        // Dictionary examples
        public async void Examples(string text, string trans, Socket clientSocket)
        {
            string route = $"/dictionary/examples?api-version=3.0&from={fromlanguage}&to={tolanguage}";

            object[] body = new object[] { new { Text = text, Translation = trans } };
            var requestbody = JsonConvert.SerializeObject(body);

            using (HttpRequestMessage requestMessage = new HttpRequestMessage())
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new StringContent(requestbody, Encoding.UTF8, "application/json");
                requestMessage.RequestUri = new Uri(endpoint + route);
                // Add authentication headers
                requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", key1); //passing the secret key
                requestMessage.Headers.Add("Ocp-Apim-Subscription-Region", location); //regional translator resource

                //Send the request and get the response
                HttpResponseMessage responseMessage = await client.SendAsync(requestMessage).ConfigureAwait(false);
                //Read the response as json string
                result = await responseMessage.Content.ReadAsStringAsync();
                // Get the correct JSON format
                result = result.Trim().TrimStart('[').TrimEnd(']');
            }
            // gui thong tin cho client ngay lap tuc
            clientSocket.Send(Serialize(result));
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(btnListen.Enabled == false)
            {
                tcpListener.Stop();
            }
        }

        // Word lookup
        private async void WordLookUp(Socket clientSocket)
        {
            /*
             * api-version = 3.0 is a must
             * from en/vi language
             * to vi/en language
             * $ - interpolated string
             */
            string route = $"/dictionary/lookup?api-version=3.0&from={fromlanguage}&to={tolanguage}";

            // Get the post info and convert the object of that info into a JSON string
            object[] body = new object[] { new { Text = wordToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            // Build the http post request.
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                // Combine endpoint and route to get the url that need to sent the post request 
                request.RequestUri = new Uri(endpoint + route);
                // Convert string into HTTP content
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                // Add authentication headers
                request.Headers.Add("Ocp-Apim-Subscription-Key", key1); //passing the secret key
                request.Headers.Add("Ocp-Apim-Subscription-Region", location); //regional translator resource

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read the json response as a string.
                result = await response.Content.ReadAsStringAsync();
                // Trim to get the right json type
                result = result.Trim().TrimStart('[').TrimEnd(']');
            }
            // gui thong tin cho client ngay lap tuc
            clientSocket.Send(Serialize(result));
            //clientSocket.Send(Encoding.Unicode.GetBytes(result));
        }

        // Text translate
        private async void TextTranslate(string route, Socket clientSocket)
        {

            object[] body = new object[] { new { Text = wordToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            // Create the http post request
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                // Build the http post request
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key1);
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                result = await response.Content.ReadAsStringAsync();
                // Get the correct JSON format
                result = result.Trim().TrimStart('[').TrimEnd(']');
            }
            // gui thong tin cho client ngay lap tuc
            clientSocket.Send(Serialize(result));
            //clientSocket.Send(Encoding.Unicode.GetBytes(result));
        }

        // Text to speech
        private async void Text2Speech(Socket clientSocket)
        {
            // Checking if input is english
            string route = "/detect?api-version=3.0";
            object[] body = new object[] { new { Text = wordToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            // Build the http post request
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key1);
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                result = await response.Content.ReadAsStringAsync();
                // Get the correct JSON format
                result = result.Trim().TrimStart('[').TrimEnd(']');
            }
            // gui thong tin cho client ngay lap tuc
            clientSocket.Send(Serialize(result));
            //clientSocket.Send(Encoding.UTF8.GetBytes(result));
        }
    } // Server class
}
