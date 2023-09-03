//TODO : FeaturedMedia �̰� �� id�� ���µ� �ȵǴ��� ���� Ȯ�� �ʿ� 
// Nuget : https://github.com/wp-net/WordPressPCL/commit/2dde3f7f614f551842b663d34fafc89bb1304dd0

// TODO : Tag ��� �츱��

// TODO : ���� ���� ���� ���� 



using Newtonsoft.Json.Linq;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI;
using System.Net;
using System.Text;
using OpenAI.ObjectModels;
using WordPressPCL.Models;
using WordPressPCL;
using WordPressPCL.Client;
using WordPressPCL.Utility;
using System.Net.Http.Headers;
using OpenAI.ObjectModels.ResponseModels;

namespace Web_Automation_WordPress_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LogBox1.ScrollBars = ScrollBars.Vertical;
        }
        private string selectedFolder; // Ŭ���� ���� ������ ���� ��θ� ������ ����
        private string translation;


        private void FolderPath1Btn1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // ��ȭ ������ ���� ���� (���� ����)
                folderBrowserDialog.Description = "���� ����";

                // �ʱ� ���丮 ���� (���� ����)
                folderBrowserDialog.SelectedPath = "C:\\"; // ���� ���丮�� ���ϴ� ������ ����

                // ���� ���� ��ȭ ���ڸ� ǥ���ϰ� ������� ������ Ȯ��
                DialogResult result = folderBrowserDialog.ShowDialog();

                // ����ڰ� ������ �����ϰ� Ȯ�� ��ư�� ���� ���
                if (result == DialogResult.OK)
                {
                    selectedFolder = folderBrowserDialog.SelectedPath;
                    FolderPath1.Text = selectedFolder; // ������ ���� ��θ� �ؽ�Ʈ ���ڿ� ǥ��

                }
            }
        }


        private void StartBtn1_Click(object sender, EventArgs e)
        {
            LogBox1.AppendText($"������ ����");
            LogBox1.AppendText(Environment.NewLine);

            WP_API_Auto();
        }


        private async void WP_API_Auto()
        {
            // if (dalleBox1 != null && !string.IsNullOrWhiteSpace(dalleBox1.Text)) //������� ���� �� ����
            // {
            //DALL-E�κ��� IMG ���
            string content = ""; // \n�� <br>�� ���� , result�� content�� �Ҵ�
            int result_1 = 0;
            string prompt2 = dalleBox1.Text;
            translation = Papago(prompt2);
            content = await RequestDALLE(translation, selectedFolder); // ���� ���� + �̹��� ���� ���ε� �� API ���ۿ�û // �̹���� ù�ٿ� ���� ��ΰ� ��
            LogBox1.AppendText($"�̹��� ��� �Ϸ�");
            LogBox1.AppendText(Environment.NewLine);
            //}

            //GPT�κ��� Content ���
            string result_2 = "";
            string prompt1 = gptBox1.Text.Trim(); // ����
            var task1 = Task.Run(() => RequestGPT(prompt1)); // ChatCPT�� ��û
            result_2 = await task1; // GPT�κ��� ���� �亯
            string content_2 = result_2.Replace("\n", "<br>") + "<br>"; // \n�� <br>�� ���� , result�� content�� �Ҵ�
            LogBox1.AppendText($"GPT ��� �Ϸ�");
            LogBox1.AppendText(Environment.NewLine);

            // GPT+DALL-E 
            content += "\r" + content_2;

            //WP ���ε� 
            var client = new WordPressClient(Define.WP_URL);
            client.Auth.UseBasicAuth(Define.WP_ID, Define.WP_PW); // ���̵� ���ĭ�� ���� ���� ������

            var post = new Post()
            {
                Title = new Title(titleBox1.Text),
                Content = new Content(content),
                //FeaturedMedia = result_1,
                Categories = new List<int> { comboBox1_SelectedItem() }, // ComboBox���� ������ ī�װ� ID ����
                CommentStatus = OpenStatus.Open, // ��� ����
                Status = Status.Publish, // ������ ���� ����,�ӽ�

                //Meta = new Description(dataObj.Description), // ��Ÿ ������
                //Tags = tagBox1.Text.Split(',').Select(tag => tag.Trim()).ToList(), // ��ǥ�� ���е� �±� ��� �߰�,
                //Excerpt = new Excerpt(dataObj.Excerpt), // ����
            };
            await client.Posts.CreateAsync(post); // ������ ��û ������

            LogBox1.AppendText($"������ �Ϸ�");
            LogBox1.AppendText(Environment.NewLine);
        }

        private int comboBox1_SelectedItem()
        {
            string selectedItem = comboBox1.SelectedItem.ToString();
            int value = 0;

            switch (selectedItem)
            {
                case "���� ������":
                    value = 4;
                    break;
                case "���� �����غ�":
                    value = 134;
                    break;
                case "���� ���":
                    value = 77;
                    break;
                case "���� ��������":
                    value = 3;
                    break;
            }
            return value;
        }

        private async Task<string> RequestGPT(string prompt1)
        {
            string result = "";

            var gpt = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = Define.OPENAI_API_KEY
            });

            var completionResult = await gpt.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
            {
                Messages = new List<ChatMessage>(new ChatMessage[] { new ChatMessage("user", prompt1) }),
                Model = Models.Gpt_3_5_Turbo_16k, //�𵨸�.
                Temperature = 0.6F,      //����� ������(�پ缺 - Diversity)). �������� ������ ���� ���, ������ �� �ƹ���?
                MaxTokens = 4000,      //�̰� ����� ���ڰ� ������. �� ��ū ������ �������� Ʈ������ �Ű�����, (�����ΰ��) ���� å���� ��)
                N = 1   //����� ��(����� ��). N=3���� �ϸ� 3�� �ٸ� ȸ���� �迭�� �����
            });

            if (completionResult.Successful)
            {
                foreach (var choice in completionResult.Choices)
                {
                    result = choice.Message.Content;
                }
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
            }
            return result;
        }

        static async Task<string> RequestDALLE(string translation, string selectedFolder)
        {
            // return responseBody : �ٷ� content�� ��� ����. ��,�̹����� �̹���url�� �Բ� ���
            // return imageUrl : string content = $"<img src=\"{imageUrl}\">"; �� ��밡��. ��,��ǥ�̹����� �ڵ� ��� �ȵ�(�ؽ�Ʈ���)
            // return localImagePath : string content = $"<img src=\"{imageUrl}\">"; �� ��밡��. ��,��ǥ�̹����θ� ������ �������� �����ڽ���

            var dalle = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = Define.OPENAI_API_KEY
            });

            // �̹��� ���� ��û �����͸� �غ��մϴ�.
            var imageResult = await dalle.Image.CreateImage(new ImageCreateRequest
            {
                Prompt = translation, // �̹��� ������ ���� ������Ʈ
                N = 1, // �̹��� ���� (�� ���� �ϳ��� �̹����� ����)
                Size = "512x512", // ���ϴ� �̹��� ũ��
                ResponseFormat = "url", // ���� ����: �̹��� URL�� �ޱ�
                User = "TestUser"
            });

            string fileName = $"{translation}.png"; // ������ ���ϸ�
            string imageUrl = imageResult.Results.FirstOrDefault()?.Url;  // ������ �̹����� URL�� �����ɴϴ�.
            string localImagePath = Path.Combine(selectedFolder, fileName); // �̹����� ���÷� �ٿ�ε��մϴ�.


            if (imageResult.Successful)
            {
                // �̹��� ���� �� ���÷� �ٿ�ε�
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(imageUrl, localImagePath);
                }

                // ���÷� �ٿ�ε� �� �̹����� ������ ���ε� 
                string WP_URL = "https://oheeliving.com/wp-json/wp/v2/media"; // ���������� ����Ʈ �ּ�
                string WP_USERNAME = Define.WP_ID; // ����� �̸�
                string WP_PASSWORD = Define.WP_PW; // ��й�ȣ

                // �̹��� ���ε�
                byte[] imageBytes = File.ReadAllBytes(localImagePath);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // ���� ���� ����
                    var byteArray = Encoding.ASCII.GetBytes($"{WP_USERNAME}:{WP_PASSWORD}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    // �̹��� ���ε�
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    ByteArrayContent imageContent = new ByteArrayContent(imageBytes);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"image/png");
                    form.Add(imageContent, "file", $"{translation}.png");

                    HttpResponseMessage response = await client.PostAsync(WP_URL, form);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var mediaInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<MediaInfo>(responseContent);
                        string source_url = mediaInfo.source_url;

                        return source_url; // id���� int�� return�ϰ� FeaturedMedia�� �ؾ��ϴµ� �۵����� ����
                    }
                    else
                    {
                        throw new Exception("Unknown Error");
                    }
                }
            }
            else //handle errors
            {
                if (imageResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
                return null;
            }
        }



        private string Papago(string prompt2)
        {
            string translatedText = "";

            //��û URL
            string url = "https://openapi.naver.com/v1/papago/n2mt";

            // �Ķ���Ϳ� ���ֱ� (���İ� NMT API���̵忡�� -d�κ��� �Ķ����)
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // ��� �߰��ϱ� + ������ ��û
            request.Headers.Add("X-Naver-Client-Id", "3nH6qrBF9E2Rxf17oim1");
            request.Headers.Add("X-Naver-Client-Secret", "Da8uQylPSd");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // �Ķ���͸� character Set�� �°� ����
            string query = prompt2;
            byte[] byteDataParams = Encoding.UTF8.GetBytes("source=ko&target=en&text=" + query);

            // ��û ������ ����
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();

            // ���� ������ �������� (�������)
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            Console.WriteLine(text);

            // JSON ������˿��� �ǿ��� �κ�(������ ����)�� ��������
            JObject jObject = JObject.Parse(text);
            translatedText = jObject["message"]["result"]["translatedText"].ToString();
            return translatedText;
        }

        private void TextBtn1_Click(object sender, EventArgs e)
        {
            Test_API();
        }



        private async void Test_API()
        {

            //WP ���ε� 
            var client = new WordPressClient("https://oheeliving.com/wp-json/");

            client.Auth.UseBasicAuth("kwon92088@gmail.com", "SV9H TFv0 pMC6 sptk 8s7q 7BsV"); // ���̵� ���ĭ�� ���� ���� ������

            var post = new Post()
            {
                Title = new Title("�׽�Ʈ �����Դϴ�"),
                Content = new Content("�׽�Ʈ ���� �Դϴ�"),
                Categories = new List<int> { comboBox1_SelectedItem() }, // ComboBox���� ������ ī�װ� ID ����
                CommentStatus = OpenStatus.Open, // ��� ����
                Status = Status.Draft // ������ ���� ����,�ӽ�

                //Meta = new Description(dataObj.Description), // ��Ÿ ������
                //Excerpt = new Excerpt(dataObj.Excerpt), // ����
            };

            await client.Posts.CreateAsync(post); // ������ ��û ������
            LogBox1.AppendText($"������ �Ϸ�");
            LogBox1.AppendText(Environment.NewLine);

        }


    }












    static class Define
    {
        public const string WP_ID = "kwon92088@gmail.com";
        public const string WP_PW = "SV9H TFv0 pMC6 sptk 8s7q 7BsV";
        public const string WP_URL = "https://oheeliving.com/wp-json/";

        public const string OPENAI_API_KEY = "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";
    }

    public class MediaInfo
    {
        public int id { get; set; }
        public string src { get; set; }
        public string source_url { get; set; }
    }


}
