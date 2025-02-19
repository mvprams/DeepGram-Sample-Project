using Deepgram;
using System.Text.Json;
using System.Windows;
using Deepgram.Models.PreRecorded.v1;
using System.Net.Http;
using System.Text;
using Deepgram.Models.Manage.v1;
using System.IO;
using Microsoft.Win32;

namespace DeepGram_Sample_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static string apiKey = "{Api_key}";
        internal static string apiUrl = "https://api.deepgram.com/v1/listen?smart_format=true&model=nova-2&language=en-US";
        internal static string jsonPayload = "{\"url\":\"https://static.deepgram.com/examples/Bueller-Life-moves-pretty-fast.wav\"}";
        internal static string path = String.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (true == dialog.ShowDialog())
            {
                path = dialog.FileName;
            }
            tb_fileName.Text = path;
        }


        internal static async Task<string> MakeApiCallForWebsiteSample()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Token {apiKey}");
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
        }

        internal static async Task<string> MakeApiCall()
        {
            using (HttpClient client = new HttpClient())
            { 
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.deepgram.com/v1/listen");
                client.DefaultRequestHeaders.Add("Authorization", $"Token {apiKey}");
                request.Content = new StreamContent(File.OpenRead(path));
                var response = await client.SendAsync(request);
               // response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Initialize Library with default logging ("Info" level)
            Library.Initialize();

            // The API key we created in step 3
            //var deepgramClient = new ListenRESTClient(secret2);

            // Hosted sample file
            //var audioUrl = "https://static.deepgram.com/examples/Bueller-Life-moves-pretty-fast.wav";
            try
            {
                string result = Task.Run(async () => await MakeApiCall()).GetAwaiter().GetResult();
                tbb_response.Text = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
