//TODO : FeaturedMedia �̰� �� id�� ���µ� �ȵǴ��� ���� Ȯ�� �ʿ� 
// Nuget : https://github.com/wp-net/WordPressPCL/commit/2dde3f7f614f551842b663d34fafc89bb1304dd0

// TODO : Tag ��� �츱��




using Newtonsoft.Json.Linq;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI;
using System.Net;
using System.Text;
using OpenAI.ObjectModels;
using WordPressPCL.Models;
using WordPressPCL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenAI.ObjectModels.ResponseModels;
using System.Xml.Linq;

namespace Web_Automation_WordPress_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadConfig(); // Textbox ���尪 ���α׷� ���� �� ���� �ε�
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
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"������ ����" + Environment.NewLine);

            SaveConfig();
            WP_API_Auto();
        }


        private async void WP_API_Auto()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���ĭ�� ���� ���� ������

            try
            {
                LogBox1.AppendText($"�̹��� ���� ����..." + Environment.NewLine);
                //DALL-E�κ��� IMG ���
                string content_2 = ""; // \n�� <br>�� ���� , result�� content�� �Ҵ�
                string prompt_2 = dalleBox1.Text;
                translation = Papago(prompt_2);
                content_2 = await RequestDALLE(translation, selectedFolder); // ���� ���� + �̹��� ���� ���ε� �� API ���ۿ�û // �̹���� ù�ٿ� ���� ��ΰ� ��
                var createdMedia = await client.Media.CreateAsync(content_2, $"{translation}.jpg"); // filepath�� media ����
                content_2 = $"<img src=\"{createdMedia.SourceUrl}\">"; // content_2�� createdMedia���� ��ȯ �������� img src�� ����
                LogBox1.AppendText($"�̹��� ��� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);

                //GPT�κ��� Content ���
                LogBox1.AppendText($"GPT ��� ����..." + Environment.NewLine);
                string result_1 = "";
                string prompt_1 = gptBox1.Text.Trim(); // ����
                var task1 = Task.Run(() => RequestGPT(prompt_1)); // ChatCPT�� ��û
                result_1 = await task1; // GPT�κ��� ���� �亯
                string content_1 = result_1.Replace("\n", "<br>") + "<br>"; // \n�� <br>�� ���� , result�� content�� �Ҵ�
                LogBox1.AppendText($"GPT ��� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"�±� ������..." + Environment.NewLine + Environment.NewLine);
                string tags = titleBox1.Text + "��� ��α� ���� ��︮�� �±� " + 10 + "���� ������ ','�� �Ἥ �˷���";
                task1 = Task.Run(() => RequestGPT(tags)); // ChatCPT�� ��û
                result_1 = "";
                result_1 = await task1; // GPT�κ��� ���� �±�
                if (result_1.Contains("#"))
                {
                    result_1 = result_1.Replace("#", ",");
                }
                var tag = new Tag()
                {
                    Name = result_1 // ���� TAG�� ������
                };
                var createdtag = await client.Tags.CreateAsync(tag);
                LogBox1.AppendText(result_1 + Environment.NewLine + Environment.NewLine); // �±� ���
                LogBox1.AppendText($"�±� ���� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // ���� ������ ��ũ ���� �ż���
                LogBox1.AppendText($"���� ������ ��ũ ����..." + Environment.NewLine);
                var posts = await client.Posts.GetAllAsync();
                List<string> postLinks = new List<string>(); // ����Ʈ�� Link ���� ������ ����Ʈ�� ����ϴ�.
                foreach (var postli in posts)
                {
                    string postLink = postli.Link; // ����Ʈ�� Link ���� ����
                    postLinks.Add(postLink);
                }
                Random random = new Random(); // �����ϰ� 3���� Link ���� �����մϴ�.
                List<string> selectedLinks = new List<string>();

                for (int i = 0; i < 3; i++)
                {
                    int index = random.Next(postLinks.Count); // ������ �ε��� ����
                    string selectedLink = postLinks[index]; // ���õ� Link ��
                    selectedLinks.Add(selectedLink);
                    postLinks.RemoveAt(index); // �ߺ� ���� ������ ���� ������ Link ���� ����Ʈ���� �����մϴ�.
                }
                // ���õ� Link ���� oldposts ���ڿ��� �߰��մϴ�.
                string oldposts = string.Join("\r", selectedLinks);
                LogBox1.AppendText($"���� ������ ��ũ ���� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                //WP ���ε� 
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"���������� ���ε� ����" + Environment.NewLine);
                var post = new Post()
                {
                    Title = new Title(titleBox1.Text),
                    Content = new Content(content_2 + "\r" + content_1), // GPT
                    FeaturedMedia = createdMedia.Id, // DALL-E
                    Categories = new List<int> { comboBox1_SelectedItem() }, // ComboBox���� ������ ī�װ� ID ����
                    CommentStatus = OpenStatus.Open, // ��� ����
                    Tags = new List<int> { createdtag.Id },
                    Link = oldposts,
                    Status = Status.Publish, // ������ ���� ����,�ӽ�


                    Meta = new Description("�׽�Ʈ�Դϴ�"), // ��Ÿ ������
                    //Excerpt = new Excerpt(dataObj.Excerpt), // ����
                };
                await client.Posts.CreateAsync(post); // ������ ��û ������
                LogBox1.AppendText($"�� ���� ��� �Ϸ�" + Environment.NewLine);

                LogBox1.AppendText($"���������� ���ε� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                createdtag = null;
            }
            catch
            {
                LogBox1.AppendText($"Line:117 ��ĭ Ȯ��");
                LogBox1.AppendText(Environment.NewLine);
            }
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
                ApiKey = OPENAI_API_KEY
            });

            var completionResult = await gpt.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem(SystemBox1.Text+"������"),
                    ChatMessage.FromUser(prompt1),
                },
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

        private async Task<string> RequestDALLE(string translation, string selectedFolder)
        {
            // return responseBody : �ٷ� content�� ��� ����. ��,�̹����� �̹���url�� �Բ� ���
            // return imageUrl : string content = $"<img src=\"{imageUrl}\">"; �� ��밡��. ��,��ǥ�̹����� �ڵ� ��� �ȵ�(�ؽ�Ʈ���)
            // return localImagePath : string content = $"<img src=\"{imageUrl}\">"; �� ��밡��. ��,��ǥ�̹����θ� ������ �������� �����ڽ���
            var dalle = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = OPENAI_API_KEY
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

            string fileName = $"{translation}.jpg"; // ������ ���ϸ�
            string imageUrl = imageResult.Results.FirstOrDefault()?.Url;  // ������ �̹����� URL�� �����ɴϴ�.
            string localImagePath = Path.Combine(selectedFolder, fileName); // �̹����� ���÷� �ٿ�ε� ��� ����
            using (WebClient client1 = new WebClient()) // �̹��� ���� �� ���÷� �ٿ�ε�
            {
                client1.DownloadFile(imageUrl, localImagePath);
            }

            return localImagePath;
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




















        private string WP_ID = "";
        private string WP_PW = "";
        private string WP_URL = "";
        private string OPENAI_API_KEY = "";

        private void APIKeybox1_TextChanged(object sender, EventArgs e)
        {
            OPENAI_API_KEY = APIKeybox1.Text;
        }
        private void IdBox1_TextChanged(object sender, EventArgs e)
        {
            WP_ID = IdBox1.Text;
        }
        private void PwBox1_TextChanged(object sender, EventArgs e)
        {
            WP_PW = PwBox1.Text;
        }
        private void UrlBox1_TextChanged(object sender, EventArgs e)
        {
            WP_URL = UrlBox1.Text;
        }






        private void SaveConfig()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string configFile = Path.Combine(myDocumentsPath, "T_Post_config.xml");

            XDocument doc = new XDocument(new XElement("Settings",
                new XElement("InputValue1", IdBox1.Text),
                new XElement("InputValue2", PwBox1.Text),
                new XElement("InputValue3", UrlBox1.Text),
                new XElement("InputValue4", APIKeybox1.Text)));
            doc.Save(configFile);
        }
        // Textbox �ε��� (Start btn�κ�)
        private void LoadConfig()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string configFile = Path.Combine(myDocumentsPath, "T_Post_config.xml");

            if (File.Exists(configFile))
            {
                XDocument doc = XDocument.Load(configFile);
                IdBox1.Text = doc.Root.Element("InputValue1")?.Value;
                PwBox1.Text = doc.Root.Element("InputValue2")?.Value;
                UrlBox1.Text = doc.Root.Element("InputValue3")?.Value;
                APIKeybox1.Text = doc.Root.Element("InputValue4")?.Value;
            }
        }


        private void SaveBtn1_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    XDocument doc = new XDocument(new XElement("Settings",
                        new XElement("InputValue1", IdBox1.Text),
                        new XElement("InputValue2", PwBox1.Text),
                        new XElement("InputValue3", UrlBox1.Text),
                        new XElement("InputValue4", APIKeybox1.Text)));

                    doc.Save(saveFileDialog.FileName);
                    MessageBox.Show("Settings saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void LoadBtn1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    XDocument doc = XDocument.Load(openFileDialog.FileName);
                    IdBox1.Text = doc.Root.Element("InputValue1")?.Value;
                    PwBox1.Text = doc.Root.Element("InputValue2")?.Value;
                    UrlBox1.Text = doc.Root.Element("InputValue3")?.Value;
                    APIKeybox1.Text = doc.Root.Element("InputValue4")?.Value;

                    MessageBox.Show("Settings loaded.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }












    static class Define
    {
        public const string WP_ID = "kwon92088@gmail.com";
        public const string WP_PW = "SV9H TFv0 pMC6 sptk 8s7q 7BsV";
        public const string WP_URL = "https://oheeliving.com/wp-json/";
    }
}
