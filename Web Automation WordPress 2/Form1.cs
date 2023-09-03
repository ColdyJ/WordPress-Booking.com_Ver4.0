//TODO : FeaturedMedia 이거 왜 id로 들어가는데 안되는지 ㅅㅂ 확인 필요 
// Nuget : https://github.com/wp-net/WordPressPCL/commit/2dde3f7f614f551842b663d34fafc89bb1304dd0

// TODO : Tag 기능 살릴것

// TODO : 사진 저장 폴더 지정 



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
        private string selectedFolder; // 클래스 레벨 변수로 폴더 경로를 저장할 변수
        private string translation;


        private void FolderPath1Btn1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // 대화 상자의 제목 설정 (선택 사항)
                folderBrowserDialog.Description = "폴더 선택";

                // 초기 디렉토리 설정 (선택 사항)
                folderBrowserDialog.SelectedPath = "C:\\"; // 시작 디렉토리를 원하는 폴더로 설정

                // 폴더 선택 대화 상자를 표시하고 사용자의 선택을 확인
                DialogResult result = folderBrowserDialog.ShowDialog();

                // 사용자가 폴더를 선택하고 확인 버튼을 누른 경우
                if (result == DialogResult.OK)
                {
                    selectedFolder = folderBrowserDialog.SelectedPath;
                    FolderPath1.Text = selectedFolder; // 선택한 폴더 경로를 텍스트 상자에 표시

                }
            }
        }


        private void StartBtn1_Click(object sender, EventArgs e)
        {
            LogBox1.AppendText($"포스팅 시작");
            LogBox1.AppendText(Environment.NewLine);

            WP_API_Auto();
        }


        private async void WP_API_Auto()
        {
            // if (dalleBox1 != null && !string.IsNullOrWhiteSpace(dalleBox1.Text)) //비어있지 않을 시 실행
            // {
            //DALL-E로부터 IMG 출력
            string content = ""; // \n을 <br>로 변경 , result를 content에 할당
            int result_1 = 0;
            string prompt2 = dalleBox1.Text;
            translation = Papago(prompt2);
            content = await RequestDALLE(translation, selectedFolder); // 로컬 저장 + 이미지 파일 업로드 후 API 전송요청 // 이방식은 첫줄에 사진 경로가 뜸
            LogBox1.AppendText($"이미지 출력 완료");
            LogBox1.AppendText(Environment.NewLine);
            //}

            //GPT로부터 Content 출력
            string result_2 = "";
            string prompt1 = gptBox1.Text.Trim(); // 질문
            var task1 = Task.Run(() => RequestGPT(prompt1)); // ChatCPT에 요청
            result_2 = await task1; // GPT로부터 나온 답변
            string content_2 = result_2.Replace("\n", "<br>") + "<br>"; // \n을 <br>로 변경 , result를 content에 할당
            LogBox1.AppendText($"GPT 출력 완료");
            LogBox1.AppendText(Environment.NewLine);

            // GPT+DALL-E 
            content += "\r" + content_2;

            //WP 업로드 
            var client = new WordPressClient(Define.WP_URL);
            client.Auth.UseBasicAuth(Define.WP_ID, Define.WP_PW); // 아이디 비번칸은 따로 만들어도 좋을듯

            var post = new Post()
            {
                Title = new Title(titleBox1.Text),
                Content = new Content(content),
                //FeaturedMedia = result_1,
                Categories = new List<int> { comboBox1_SelectedItem() }, // ComboBox에서 선택한 카테고리 ID 설정
                CommentStatus = OpenStatus.Open, // 댓글 상태
                Status = Status.Publish, // 포스팅 상태 공개,임시

                //Meta = new Description(dataObj.Description), // 메타 데이터
                //Tags = tagBox1.Text.Split(',').Select(tag => tag.Trim()).ToList(), // 쉼표로 구분된 태그 목록 추가,
                //Excerpt = new Excerpt(dataObj.Excerpt), // 발췌
            };
            await client.Posts.CreateAsync(post); // 포스팅 요청 보내기

            LogBox1.AppendText($"포스팅 완료");
            LogBox1.AppendText(Environment.NewLine);
        }

        private int comboBox1_SelectedItem()
        {
            string selectedItem = comboBox1.SelectedItem.ToString();
            int value = 0;

            switch (selectedItem)
            {
                case "오희 리빙템":
                    value = 4;
                    break;
                case "오희 면접준비":
                    value = 134;
                    break;
                case "오희 잡담":
                    value = 77;
                    break;
                case "오희 지원정보":
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
                Model = Models.Gpt_3_5_Turbo_16k, //모델명.
                Temperature = 0.6F,      //대답의 자유도(다양성 - Diversity)). 자유도가 낮으면 같은 대답, 높으면 좀 아무말?
                MaxTokens = 4000,      //이게 길수록 글자가 많아짐. 이 토큰 단위를 기준으로 트래픽이 매겨지고, (유료인경우) 과금 책정이 됨)
                N = 1   //경우의 수(대답의 수). N=3으로 하면 3번 다른 회신을 배열에 담아줌
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
            // return responseBody : 바로 content로 등록 가능. 단,이미지와 이미지url이 함께 출력
            // return imageUrl : string content = $"<img src=\"{imageUrl}\">"; 로 사용가능. 단,대표이미지로 자동 등록 안됨(텍스트취급)
            // return localImagePath : string content = $"<img src=\"{imageUrl}\">"; 로 사용가능. 단,대표이미지로만 나오고 본문에는 엑스박스뜸

            var dalle = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = Define.OPENAI_API_KEY
            });

            // 이미지 생성 요청 데이터를 준비합니다.
            var imageResult = await dalle.Image.CreateImage(new ImageCreateRequest
            {
                Prompt = translation, // 이미지 생성을 위한 프롬프트
                N = 1, // 이미지 개수 (한 번에 하나의 이미지만 생성)
                Size = "512x512", // 원하는 이미지 크기
                ResponseFormat = "url", // 응답 형식: 이미지 URL로 받기
                User = "TestUser"
            });

            string fileName = $"{translation}.png"; // 저장할 파일명
            string imageUrl = imageResult.Results.FirstOrDefault()?.Url;  // 생성된 이미지의 URL을 가져옵니다.
            string localImagePath = Path.Combine(selectedFolder, fileName); // 이미지를 로컬로 다운로드합니다.


            if (imageResult.Successful)
            {
                // 이미지 생성 후 로컬로 다운로드
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(imageUrl, localImagePath);
                }

                // 로컬로 다운로드 한 이미지를 서버에 업로드 
                string WP_URL = "https://oheeliving.com/wp-json/wp/v2/media"; // 워드프레스 사이트 주소
                string WP_USERNAME = Define.WP_ID; // 사용자 이름
                string WP_PASSWORD = Define.WP_PW; // 비밀번호

                // 이미지 업로드
                byte[] imageBytes = File.ReadAllBytes(localImagePath);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // 인증 정보 설정
                    var byteArray = Encoding.ASCII.GetBytes($"{WP_USERNAME}:{WP_PASSWORD}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    // 이미지 업로드
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

                        return source_url; // id값은 int로 return하고 FeaturedMedia로 해야하는데 작동안함 ㅆㅂ
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

            //요청 URL
            string url = "https://openapi.naver.com/v1/papago/n2mt";

            // 파라미터에 값넣기 (파파고 NMT API가이드에서 -d부분이 파라미터)
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // 헤더 추가하기 + 서버에 요청
            request.Headers.Add("X-Naver-Client-Id", "3nH6qrBF9E2Rxf17oim1");
            request.Headers.Add("X-Naver-Client-Secret", "Da8uQylPSd");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // 파라미터를 character Set에 맞게 변경
            string query = prompt2;
            byte[] byteDataParams = Encoding.UTF8.GetBytes("source=ko&target=en&text=" + query);

            // 요청 데이터 길이
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();

            // 응답 데이터 가져오기 (출력포맷)
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            Console.WriteLine(text);

            // JSON 출력포맷에서 피요한 부분(번역된 문장)만 가져오기
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

            //WP 업로드 
            var client = new WordPressClient("https://oheeliving.com/wp-json/");

            client.Auth.UseBasicAuth("kwon92088@gmail.com", "SV9H TFv0 pMC6 sptk 8s7q 7BsV"); // 아이디 비번칸은 따로 만들어도 좋을듯

            var post = new Post()
            {
                Title = new Title("테스트 제목입니다"),
                Content = new Content("테스트 내용 입니다"),
                Categories = new List<int> { comboBox1_SelectedItem() }, // ComboBox에서 선택한 카테고리 ID 설정
                CommentStatus = OpenStatus.Open, // 댓글 상태
                Status = Status.Draft // 포스팅 상태 공개,임시

                //Meta = new Description(dataObj.Description), // 메타 데이터
                //Excerpt = new Excerpt(dataObj.Excerpt), // 발췌
            };

            await client.Posts.CreateAsync(post); // 포스팅 요청 보내기
            LogBox1.AppendText($"포스팅 완료");
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
