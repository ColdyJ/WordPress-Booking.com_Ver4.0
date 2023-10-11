//TODO : FeaturedMedia 이거 왜 id로 들어가는데 안되는지 ㅅㅂ 확인 필요 
// Nuget : https://github.com/wp-net/WordPressPCL/commit/2dde3f7f614f551842b663d34fafc89bb1304dd0

// TODO : Tag 기능 살릴것




using Newtonsoft.Json.Linq;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI;
using System.Net;
using System.Text;
using OpenAI.ObjectModels;
using WordPressPCL.Models;
using WordPressPCL;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;
using OpenAI.ObjectModels.ResponseModels;
using System;

namespace Web_Automation_WordPress_2
{
    public partial class Form1 : Form
    {
        private const string idlistUrl_1 = ("https://coldyj.github.io/joon.github.io/Tistory_idlist_1.txt"); // 서버에서 버전을 가져올 URL (티스토리 아이디 관리)

        public Form1()
        {
            InitializeComponent();
            LoadConfig(); // Textbox 저장값 프로그램 시작 시 설정 로드
            LogBox1.ScrollBars = ScrollBars.Vertical;
        }
        private IWebDriver driver;
        ChromeOptions options = new ChromeOptions();
        private string selectedFolder; // 클래스 레벨 변수로 폴더 경로를 저장할 변수
        private string translation;


        private void FolderPath1Btn1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // 대화 상자의 제목 설정 (선택 사항)
                folderBrowserDialog.Description = "폴더 선택";
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
        // 서버 아이디 관리 (티스토리 아이디 관리)
        private int CheckUserID_1(string userID)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string serverData = client.DownloadString(idlistUrl_1);
                    string[] registeredIDs = serverData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (Array.Exists(registeredIDs, id => id.Equals(userID)))
                    {
                        return 1; // User ID found in server file
                    }
                    else
                    {
                        return 0; // User ID not found in server file
                    }
                }
            }
            catch (Exception ex)
            {
                return -1; // Error occurred
            }
        }

        private void StartBtn1_Click(object sender, EventArgs e)
        {
            SaveConfig();

            string userID = UrlBox1.Text;
            int isRegistered = CheckUserID_1(userID); // 등록 아이디 체크
            if (isRegistered == 1)
            {
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"포스팅 시작" + Environment.NewLine);

                WP_API_Auto();
            }
            else
            {
                MessageBox.Show("등록된 아이디가 아닙니다. 고객센터로 연락주세요");
            }
        }

        // Delay 함수 3~5초
        public void Delay()
        {
            int minMs = 3000;
            int maxMs = 5000;
            Random random = new Random();
            int delayMs = random.Next(minMs, maxMs);
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddMilliseconds(delayMs);
            while (DateTime.Now < endTime)
            {
                Application.DoEvents();
            }
            return;
        }

        // 이미지 크롤링 - 완료
        private void Crawling_Naver()
        {
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"Line:xxx 크롤링 시작" + Environment.NewLine);

            // 크롬창 생성
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            options.AddArguments("--headless"); // 브라우저를 숨김
            driver = new ChromeDriver(driverService, options);
            Delay();

            //이미지 검색 : CCL 상업적 이용가능 
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"Line:xxx 이미지 검색" + Environment.NewLine);
            string baseUrl = $"https://search.naver.com/search.naver?where=image&section=image&query={TitleBox1.Text}";
            string endUrl = "&res_fr=0&res_to=0&sm=tab_opt&color=&ccl=2&nso=so%3Ar%2Ca%3Aall%2Cp%3Aall&recent=0&datetype=0&startdate=0&enddate=0&gif=0&optStr=&nso_open=1&pq=";
            driver.Navigate().GoToUrl(baseUrl + endUrl);
            Delay();
            ScrollToBottom(driver);

            // 이미지 요소를 찾아서 처리
            var imgElements = driver.FindElements(By.CssSelector("img._image._listImage"));
            LogBox1.AppendText($"Line:xxx 총 사진 수: {imgElements.Count}장");
            LogBox1.AppendText(Environment.NewLine);

            foreach (var imgElement in imgElements) // 이미지 다운로드 루프
            {
                string imageUrl = imgElement.GetAttribute("src");
                if (!string.IsNullOrEmpty(imageUrl) && (imageUrl.StartsWith("http://") || imageUrl.StartsWith("https://")))
                {
                    string basePath = selectedFolder; // 기본 저장 경로
                    int fileCount = 1;
                    string fileName = $"{fileCount}.jpg"; // 저장할 이미지 파일 이름
                    while (File.Exists(Path.Combine(basePath, fileName)))
                    {
                        fileCount++;
                        fileName = $"{fileCount}.jpg";
                    }
                    // 이미지 다운로드
                    using (WebClient client = new WebClient())
                    {
                        // 로컬 폴더에 이미지 저장
                        byte[] imageData = client.DownloadData(imageUrl);
                        string filePath = Path.Combine(basePath, fileName);
                        File.WriteAllBytes(filePath, imageData);
                        Delay();
                        LogBox1.AppendText($"Line:xxx 다운로드 중: ({fileCount}/{imgElements.Count})" + Environment.NewLine);
                    }
                }
            }
            LogBox1.AppendText($"이미지 크롤링 완료..." + Environment.NewLine);
            LogBox1.AppendText($"===========================" + Environment.NewLine);
        }
        private void ScrollToBottom(IWebDriver driver)
        {
            for (int i = 0; i < 12; i++)
            {
                // JavaScript를 실행하여 스크롤을 아래로 이동합니다.
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(50, document.body.scrollHeight);");
                Delay();
            }
        }

        /*===============================================*/
        private string GPT_Prompt(string prompt)
        {
            /*
             1. 공항에서 찾아가는 방법 / 2. 외관 / 3. 내부 방 설명 / 4. 부대시설 / 5.주위맛집 / 6.숙박경험
             */
            string prompt1 = $"'{prompt}'에 관련된 블로그 글을 작성할거야. 이 템플릿에 맞춰서 아주 길고 자세하게 써줄래? 1.공항에서 찾아가는 방법:, 2.외관: , 3.내부 스타일: , 4.부대시설: , 5.주위맛집:  , 6.숙박경험: ...... 그리고 말투는 '했어요' 이런식으로 써줘";
            return prompt1;
        }

        // GPT 출력 내용 content로 가공
        private async Task<string> AddGPTToContentAsync()
        {
            string result = "";
            string prompt1 = GPT_Prompt(TitleBox1.Text); // GPT Prompt 전달
            try
            {
                result = await RequestGPT(prompt1); // GPT에 요청하고 결과를 얻습니다.
            }
            catch (Exception ex)
            {
                // 오류 처리 - 예외가 발생한 경우 처리
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
            string content = result.Replace("\n", "<br>") + "<br>"; // \n을 <br>로 변경 , HTML로 줄바꿈 
            string pattern = @"\d+\.\s*[\p{L}\d\s]+(?::)?"; // 정규표현식 = 1. 2. 3. 등 숫자로 분류된 소제목 글꼴변경을 위한 패턴
            Regex regex = new Regex(pattern);
            // 찾은 소제목 패턴을 강조하고 크게 표시합니다.
            string result_GPT = regex.Replace(content, match =>
            {
                return $"<span style='color: #FF8C00; font-size:130%; font-weight: bold;'>{match.Value}</span><br>";
            });
            return result_GPT;
        }

        // 이미지 업로드 결과저장
        private async Task<List<string>> ImagesAsyncList()
        {
            // TODO :
            // 1.mageAsyncList()에서 진행 : localImagePath를 List<string> 으로 만들고 결과값을 createdMedia으로 실행한 후 그 결과값을들 $"<img src=\"{createdMedia.SourceUrl}\">"의 형식으로 List<string> 에 저장해야함 - 했음 + 검증필요
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // 아이디 비번
            translation = Papago(TitleBox1.Text);
            int count = 0, i = 0;
            List<string> responseImgList = new List<string>(); // 이미지 업로드 결과를 저장할 리스트

            while (count != 15) // 총 15장의 사진을 url로 리스트
            {
                // 이미지 파일 경로 가져오기
                string localImagePath = Path.Combine(selectedFolder, $"{i}.jpg");
                if (File.Exists(localImagePath))
                {
                    var createdMedia = await client.Media.CreateAsync(localImagePath, $"{translation + '_' + i}.jpg"); // localImagePath로 media({translation}.jpg) 생성
                    string responseImg = $"<img src=\"{createdMedia.SourceUrl}\">"; // content_2는 createdMedia에서 변환 시켰으니 img src로 변경
                    responseImgList.Add(responseImg);
                    count++;
                    i++;
                }
                else
                {
                    i++;
                }
            }
            return responseImgList; // 이미지 업로드 결과를 리스트로 반환
        }
        // GPT 출력 내용 사이에 이미지 추가
        private string AddImagesToContent(string result_GPT, List<string> result_ImgList)
        {
            // TODO :
            // 2. AddImagesToContent() 에서 실행시키고 pattern은 (<img src>를 찾도록)변경해야함 
            string pattern = @"\d+\.\s*[\p{L}\d\s]+(?::)?";  // 이미지를 넣을 자리 찾는 정규 표현식 패턴

            // result_GPT 문자열에서 정규 표현식 패턴과 일치하는 부분을 추출
            MatchCollection matches = Regex.Matches(result_GPT, pattern);

            // result_ImgList의 img src를 저장된 MatchCollection 위치에 삽입
            foreach (Match match in matches)
            {
                string imageInfo = match.Value.Trim();

                // 이미지 정보에서 소제목 추출
                string[] parts = imageInfo.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    // 이미지 정보를 <br> 태그와 함께 추가
                    string imageSrc = result_ImgList.FirstOrDefault(); // 이미지 URL을 가져옴
                    if (!string.IsNullOrEmpty(imageSrc))
                    {
                        result_GPT = result_GPT.Replace(imageInfo, $"\r+{imageSrc}+\r+<br>{imageInfo}");
                        result_ImgList.RemoveAt(0); // 사용한 이미지 URL을 리스트에서 제거
                    }
                }
            }
            return result_GPT;
        }


        // GPT로 Tag 출력
        private async Task<int> AddTagAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // 아이디 비번
            string tags = "'" + TagBox1.Text.Trim() + "'" + "을 포함한 인기 검색어 10개를 ','로 구분해서 알려줘";
            string tagResult = "";
            try
            {
                tagResult = await RequestGPT(tags); // GPT에 요청하고 결과를 얻습니다.
            }
            catch (Exception ex)
            {
                // 오류 처리 - 예외가 발생한 경우 처리
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
            if (tagResult.Contains(", #"))
            {
                tagResult = tagResult.Replace(", #", ",");
            }
            var tag = new Tag()
            {
                Name = tagResult // 글의 TAG로 들어가버림
            };
            var createdtag = await client.Tags.CreateAsync(tag);
            LogBox1.AppendText(tagResult + Environment.NewLine + Environment.NewLine); // 태그 출력
            return createdtag.Id;
        }


        // 지난 포스팅 링크 추출 매서드
        private async Task<string> AddOldPostAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // 아이디 비번
            var posts = await client.Posts.GetAllAsync();
            List<string> postLinks = new List<string>(); // 포스트의 Link 값을 저장할 리스트를 만듭니다.
            foreach (var postli in posts)
            {
                string postLink = postli.Link; // 포스트의 Link 값을 추출
                string postTitle = postli.Title.Rendered; // 게시물의 제목을 가져옵니다.
                string linkHtml = $"<a title=\"{postTitle}\" href=\"{postLink}\">&nbsp;▶{postTitle}</a>";
                postLinks.Add(linkHtml);
            }
            Random random = new Random(); // 랜덤하게 3개의 Link 값을 선택합니다.
            List<string> selectedLinks = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                int index = random.Next(postLinks.Count); // 랜덤한 인덱스 선택
                string selectedLink = postLinks[index]; // 선택된 Link 값
                selectedLinks.Add(selectedLink);
                postLinks.RemoveAt(index); // 중복 선택 방지를 위해 선택한 Link 값을 리스트에서 제거합니다.
            }
            // 선택된 Link 값을 oldposts 문자열에 추가합니다.
            string oldPostsLinks = string.Join("\r\n", selectedLinks); // 각 링크를 개행 문자로 구분
            return oldPostsLinks;
        }


        //외부 링크 추출
        private string AddOutLinksAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // 아이디 비번
            List<string> urls = new List<string> // 30개의 URL을 리스트에 추가
                {
                    "https://m.blog.naver.com/jhkim6281/223020766231?referrerCode=1","https://m.blog.naver.com/jhkim6281/223020776708?referrerCode=1","https://m.blog.naver.com/jhkim6281/223025461120?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223033987549?referrerCode=1","https://m.blog.naver.com/jhkim6281/223035429797?referrerCode=1","https://m.blog.naver.com/jhkim6281/223035853701?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223036889995?referrerCode=1","https://m.blog.naver.com/jhkim6281/223037039200?referrerCode=1","https://m.blog.naver.com/jhkim6281/223037877338?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223037985973?referrerCode=1","https://m.blog.naver.com/jhkim6281/223038143608?referrerCode=1","https://m.blog.naver.com/jhkim6281/223038241526?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223038253154?referrerCode=1","https://m.blog.naver.com/jhkim6281/223038384369?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039198024?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223039206222?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039240687?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039265755?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223039593229?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039613837?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039637726?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223039657911?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039689956?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040233776?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223040254238?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040409460?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040432520?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223040473469?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040519864?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040598554?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223044247527?referrerCode=1","https://m.blog.naver.com/jhkim6281/223047532120?referrerCode=1","https://m.blog.naver.com/jhkim6281/223047685223?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223090692705?referrerCode=1","https://m.blog.naver.com/jhkim6281/223092826518?referrerCode=1","https://m.blog.naver.com/jhkim6281/223105279622?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223106542250?referrerCode=1"
                };
            List<string> selectedOutLinks = new List<string>();
            Random random = new Random(); // 랜덤하게 3개의 Link 값을 선택합니다.
            for (int i = 0; i < 2; i++)// 랜덤하게 2개의 URL 선택
            {
                int index = random.Next(urls.Count);
                string selectedLink = urls[index];
                urls.RemoveAt(index); // 중복 선택 방지를 위해 선택한 URL을 리스트에서 제거합니다.

                // 선택된 URL을 linkHtml 형식으로 만듭니다.
                string postTitle = $"▶{TitleBox1.Text} 에서 참고한 글◀"; // 원하는 제목을 지정하세요
                string linkHtml = $"<a title=\"{postTitle}\" href=\"{selectedLink}\">&nbsp;{postTitle}</a>";
                selectedOutLinks.Add(linkHtml);
            }
            // 선택된 URL을 outlinks 문자열에 추가
            string outLinks = string.Join("\r\n", selectedOutLinks); // 각 링크를 개행 문자로 구분
            return outLinks;
        }


        // GPT 출력 내용 요약으로 가공
        private async Task<string> AddGPTToExcerptAsync(string result_GPT)
        {
            string result = "";
            string prompt1 = "아래 내용을 요약해줘 \n\r" + result_GPT; // GPT Prompt 전달
            try
            {
                result = await RequestGPT(prompt1); // GPT에 요청하고 결과를 얻습니다.
            }
            catch (Exception ex)
            {
                // 오류 처리 - 예외가 발생한 경우 처리
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
            string content_Excerpt = result; // \n을 <br>로 변경 , result를 content에 할당
            return content_Excerpt;
        }


        private async void WP_API_Auto()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // 아이디 비번
            string prompt = "";
            string result_1 = "";

            try
            {

                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"구글 지도 추가..." + Environment.NewLine);
                // TODO : 구글 API 함수
                LogBox1.AppendText($"구글 지도 추가 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"호텔 정보 추가..." + Environment.NewLine);
                // TODO : 호텔 정보 API
                LogBox1.AppendText($"호텔 정보 추가 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"GPT 출력 시작..." + Environment.NewLine);
                string result_GPT = await AddGPTToContentAsync(); // 완료
                LogBox1.AppendText($"GPT 출력 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"이미지 & 내용 패턴 변경 시작..." + Environment.NewLine);
                List<string> result_ImgList = await ImagesAsyncList(); // selectedFolder 안의 이미지들을 <img src=\"{createdMedia.SourceUrl}\"> 형식으로 List
                string content = AddImagesToContent(result_GPT, result_ImgList); //resultText 사이에 resultImgList의 string값을 잘 넣어주면됨
                content = $"<html><body>{content}</body></html>"; // 결과를 HTML 형식으로 표시합니다. 삭제 or 추가
                LogBox1.AppendText($"이미지 & 내용 패턴 변경 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"지난 포스팅 링크 추출..." + Environment.NewLine);
                string result_OldPostLinks = await AddOldPostAsync(); // 완료
                LogBox1.AppendText($"지난 포스팅 링크 추출 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"외부 링크 추출..." + Environment.NewLine);
                string result_OutLinks = AddOutLinksAsync(); // 완료
                LogBox1.AppendText($"외부 링크 추출 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"태그 생성중..." + Environment.NewLine + Environment.NewLine);
                int result_TagId = await AddTagAsync(); // 완료
                LogBox1.AppendText($"태그 생성 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);

                LogBox1.AppendText($"요약중..." + Environment.NewLine);
                string result_Excerpt = await AddGPTToExcerptAsync(result_GPT); // 완료
                LogBox1.AppendText($"요약 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);

                //WP 업로드 
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"워드프레스 업로드 시작" + Environment.NewLine);
                var post = new Post()
                {
                    Title = new Title(TitleBox1.Text),
                    Content = new Content(result_OutLinks + "\r" + content + "\r" + result_OldPostLinks), // GPT
                    //FeaturedMedia = createdMedia.Id, // DALL-E (썸네일)
                    Categories = new List<int> { comboBox1_SelectedItem() }, // ComboBox에서 선택한 카테고리 ID 설정
                    CommentStatus = OpenStatus.Open, // 댓글 상태
                    Tags = new List<int> { result_TagId },
                    Status = Status.Publish, // 포스팅 상태 공개,임시
                    Excerpt = new Excerpt(result_Excerpt) // 발췌

                    //Meta = new Description("테스트입니다"), // 메타 데이터

                };
                var createdPost = await client.Posts.CreateAsync(post); // 포스팅 요청 보내기
                LogBox1.AppendText($"글 본문 출력 완료" + Environment.NewLine);
                LogBox1.AppendText($"워드프레스 업로드 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);



































                //DALL-E로부터 IMG 출력
                LogBox1.AppendText($"이미지 생성 시작..." + Environment.NewLine);
                string content_Dalle = "";
                prompt = dalleBox1.Text;
                translation = Papago(prompt);
                content_Dalle = await RequestDALLE(translation, selectedFolder); // localImagePath 반환
                var createdMedia = await client.Media.CreateAsync(content_Dalle, $"{translation}.jpg"); // localImagePath로 media({translation}.jpg) 생성
                content_Dalle = $"<img src=\"{createdMedia.SourceUrl}\">"; // content_2는 createdMedia에서 변환 시켰으니 img src로 변경
                LogBox1.AppendText($"이미지 출력 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                //GPT로부터 Content 출력
                LogBox1.AppendText($"GPT 출력 시작..." + Environment.NewLine);
                string content_GPT = "";
                prompt = gptBox1.Text.Trim(); // 질문
                try
                {
                    result_1 = await RequestGPT(prompt); // GPT에 요청하고 결과를 얻습니다.
                }
                catch (Exception ex)
                {
                    // 오류 처리 - 예외가 발생한 경우 처리
                    LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
                }
                content_GPT = result_1.Replace("\n", "<br>") + "<br>"; // \n을 <br>로 변경 , result를 content에 할당
                LogBox1.AppendText($"GPT 출력 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                //GPT로부터 Tags 출력
                LogBox1.AppendText($"태그 생성중..." + Environment.NewLine + Environment.NewLine);
                string tags = "'" + TagBox1.Text.Trim() + "'" + "을 포함한 인기 검색어 10개를 ','로 구분해서 알려줘";
                try
                {
                    result_1 = await RequestGPT(tags); // GPT에 요청하고 결과를 얻습니다.
                }
                catch (Exception ex)
                {
                    // 오류 처리 - 예외가 발생한 경우 처리
                    LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
                }
                if (result_1.Contains(", #"))
                {
                    result_1 = result_1.Replace(", #", ",");
                }
                var tag = new Tag()
                {
                    Name = result_1 // 글의 TAG로 들어가버림
                };
                var createdtag = await client.Tags.CreateAsync(tag);
                LogBox1.AppendText(result_1 + Environment.NewLine + Environment.NewLine); // 태그 출력
                LogBox1.AppendText($"태그 생성 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                //GPT로부터 Summary 출력
                LogBox1.AppendText($"요약중..." + Environment.NewLine);
                prompt = content_GPT + "을 요약해줘";
                try
                {
                    result_1 = await RequestGPT(prompt); // GPT에 요청하고 결과를 얻습니다.
                }
                catch (Exception ex)
                {
                    // 오류 처리 - 예외가 발생한 경우 처리
                    LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
                }
                string content_Excerpt = result_1; // \n을 <br>로 변경 , result를 content에 할당
                LogBox1.AppendText($"요약 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // 지난 포스팅 링크 추출 매서드
                LogBox1.AppendText($"지난 포스팅 링크 추출..." + Environment.NewLine);
                var posts = await client.Posts.GetAllAsync();
                List<string> postLinks = new List<string>(); // 포스트의 Link 값을 저장할 리스트를 만듭니다.
                foreach (var postli in posts)
                {
                    string postLink = postli.Link; // 포스트의 Link 값을 추출
                    string postTitle = postli.Title.Rendered; // 게시물의 제목을 가져옵니다.
                    string linkHtml = $"<a title=\"{postTitle}\" href=\"{postLink}\">&nbsp;▶{postTitle}</a>";
                    postLinks.Add(linkHtml);
                }
                Random random = new Random(); // 랜덤하게 3개의 Link 값을 선택합니다.
                List<string> selectedLinks = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    int index = random.Next(postLinks.Count); // 랜덤한 인덱스 선택
                    string selectedLink = postLinks[index]; // 선택된 Link 값
                    selectedLinks.Add(selectedLink);
                    postLinks.RemoveAt(index); // 중복 선택 방지를 위해 선택한 Link 값을 리스트에서 제거합니다.
                }
                // 선택된 Link 값을 oldposts 문자열에 추가합니다.
                string oldPostsLinks = string.Join("\r\n", selectedLinks); // 각 링크를 개행 문자로 구분
                LogBox1.AppendText($"지난 포스팅 링크 추출 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);



                LogBox1.AppendText($"외부 링크 추출..." + Environment.NewLine);
                List<string> urls = new List<string> // 30개의 URL을 리스트에 추가
                {
                    "https://m.blog.naver.com/jhkim6281/223020766231?referrerCode=1","https://m.blog.naver.com/jhkim6281/223020776708?referrerCode=1","https://m.blog.naver.com/jhkim6281/223025461120?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223033987549?referrerCode=1","https://m.blog.naver.com/jhkim6281/223035429797?referrerCode=1","https://m.blog.naver.com/jhkim6281/223035853701?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223036889995?referrerCode=1","https://m.blog.naver.com/jhkim6281/223037039200?referrerCode=1","https://m.blog.naver.com/jhkim6281/223037877338?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223037985973?referrerCode=1","https://m.blog.naver.com/jhkim6281/223038143608?referrerCode=1","https://m.blog.naver.com/jhkim6281/223038241526?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223038253154?referrerCode=1","https://m.blog.naver.com/jhkim6281/223038384369?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039198024?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223039206222?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039240687?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039265755?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223039593229?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039613837?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039637726?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223039657911?referrerCode=1","https://m.blog.naver.com/jhkim6281/223039689956?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040233776?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223040254238?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040409460?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040432520?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223040473469?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040519864?referrerCode=1","https://m.blog.naver.com/jhkim6281/223040598554?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223044247527?referrerCode=1","https://m.blog.naver.com/jhkim6281/223047532120?referrerCode=1","https://m.blog.naver.com/jhkim6281/223047685223?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223090692705?referrerCode=1","https://m.blog.naver.com/jhkim6281/223092826518?referrerCode=1","https://m.blog.naver.com/jhkim6281/223105279622?referrerCode=1",
                    "https://m.blog.naver.com/jhkim6281/223106542250?referrerCode=1"
                };
                List<string> selectedOutLinks = new List<string>();
                for (int i = 0; i < 2; i++)// 랜덤하게 2개의 URL 선택
                {
                    int index = random.Next(urls.Count);
                    string selectedLink = urls[index];
                    urls.RemoveAt(index); // 중복 선택 방지를 위해 선택한 URL을 리스트에서 제거합니다.

                    // 선택된 URL을 linkHtml 형식으로 만듭니다.
                    string postTitle = $"▶{TitleBox1.Text} 에서 참고한 글◀"; // 원하는 제목을 지정하세요
                    string linkHtml = $"<a title=\"{postTitle}\" href=\"{selectedLink}\">&nbsp;{postTitle}</a>";
                    selectedOutLinks.Add(linkHtml);
                }
                // 선택된 URL을 outlinks 문자열에 추가
                string outLinks = string.Join("\r\n", selectedOutLinks); // 각 링크를 개행 문자로 구분
                LogBox1.AppendText($"외부 링크 추출 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                //WP 업로드 
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"워드프레스 업로드 시작" + Environment.NewLine);
                var post = new Post()
                {
                    Title = new Title(TitleBox1.Text),
                    Content = new Content(outLinks + "\r" + content_Dalle + "\r" + content_GPT + oldPostsLinks), // GPT
                    FeaturedMedia = createdMedia.Id, // DALL-E (썸네일)
                    Categories = new List<int> { comboBox1_SelectedItem() }, // ComboBox에서 선택한 카테고리 ID 설정
                    CommentStatus = OpenStatus.Open, // 댓글 상태
                    Tags = new List<int> { createdtag.Id },
                    Status = Status.Publish, // 포스팅 상태 공개,임시
                    Excerpt = new Excerpt(content_Excerpt) // 발췌

                    //Meta = new Description("테스트입니다"), // 메타 데이터

                };
                var createdPost = await client.Posts.CreateAsync(post); // 포스팅 요청 보내기
                LogBox1.AppendText($"글 본문 출력 완료" + Environment.NewLine);

                /*
                // 게시물의 Link와 Meta 정보 수정
                int postId = createdPost.Id;
                var postToUpdate = await client.Posts.GetByIDAsync(postId);
                string newMetaDescription = "새로운 Meta Description 내용을 여기에 추가합니다."; // 새로운 Meta Description 설정
                postToUpdate.Meta.Add("footnotes", newMetaDescription);
                var updatedPost = await client.Posts.UpdateAsync(postToUpdate); // 게시물 업데이트
                */

                LogBox1.AppendText($"워드프레스 업로드 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                createdtag = null;
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
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
        /*
        public string GetIdFromName(string nameToFind)
        {
            TistoryAPI api = new TistoryAPI();
            api.SetAccessToken(AccessToken);

            // XML로 결과를 얻을 경우
            string result = api.GetCategory(blogName);

            // XmlDocument 인스턴스 생성
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            // "id"와 "name" 값을 불러와서 출력
            XmlNodeList categoryNodes = xmlDoc.SelectNodes("/tistory/item/categories/category");
            foreach (XmlNode categoryNode in categoryNodes)
            {
                string id = categoryNode.SelectSingleNode("id").InnerText;
                string name = categoryNode.SelectSingleNode("name").InnerText;
                if (name.Equals(nameToFind, StringComparison.OrdinalIgnoreCase)) // 대소문자를 무시하고 일치 여부를 확인합니다.
                {
                    return id; // 일치하는 "name"을 찾으면 해당 "id" 값을 반환합니다.
                }
            }
            // "nameToFind"에 해당하는 "name"을 찾지 못한 경우 null 또는 다른 적절한 값을 반환할 수 있습니다.
            return "0";
        }
        */


        private async Task<string> RequestGPT(string prompt1)
        {
            string result = "";
            var gpt = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = OPENAI_API_KEY,
            },
            new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(600)
            });

            var completionResult = await gpt.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem(SystemBox1.Text+"전문가"),
                    ChatMessage.FromUser(prompt1),
                },
                Model = Models.Gpt_3_5_Turbo_16k, //모델명.
                Temperature = 0.6F,      //대답의 자유도(다양성 - Diversity)). 자유도가 낮으면 같은 대답, 높으면 좀 아무말?
                MaxTokens = 4000,      //이게 길수록 글자가 많아짐. 이 토큰 단위를 기준으로 트래픽이 매겨지고, (유료인경우) 과금 책정이 됨)
                N = 1   //경우의 수(대답의 수). N=3으로 하면 3번 다른 회신을 배열에 담아줌
            });
            LogBox1.AppendText($"GPT DEBUG #1" + Environment.NewLine);

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
            // return responseBody : 바로 content로 등록 가능. 단,이미지와 이미지url이 함께 출력
            // return imageUrl : string content = $"<img src=\"{imageUrl}\">"; 로 사용가능. 단,대표이미지로 자동 등록 안됨(텍스트취급)
            // return localImagePath : string content = $"<img src=\"{imageUrl}\">"; 로 사용가능. 단,대표이미지로만 나오고 본문에는 엑스박스뜸
            var dalle = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = OPENAI_API_KEY
            },
            new HttpClient()
            {
                Timeout = TimeSpan.FromMinutes(20)
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

            string fileName = $"{translation}.jpg"; // 저장할 파일명
            string imageUrl = imageResult.Results.FirstOrDefault()?.Url;  // 생성된 이미지의 URL을 가져옵니다.
            string localImagePath = Path.Combine(selectedFolder, fileName); // 이미지를 로컬로 다운로드 경로 지정
            using (WebClient client1 = new WebClient()) // 이미지 생성 후 로컬로 다운로드
            {
                client1.DownloadFile(imageUrl, localImagePath);
            }

            return localImagePath;
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
            string configFile = Path.Combine(myDocumentsPath, "WP_Post_config.xml");

            XDocument doc = new XDocument(new XElement("Settings",
                new XElement("InputValue1", IdBox1.Text),
                new XElement("InputValue2", PwBox1.Text),
                new XElement("InputValue3", UrlBox1.Text),
                new XElement("InputValue4", APIKeybox1.Text)));
            doc.Save(configFile);
        }
        // Textbox 로드기능 (Start btn부분)
        private void LoadConfig()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string configFile = Path.Combine(myDocumentsPath, "WP_Post_config.xml");

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

        private void CrollBtn1_Click(object sender, EventArgs e)
        {
            Crawling_Naver();
        }
        // 파일명 변경 버튼 Rename
        private int renameCounter = 1;
        private void RenameBtn1_Click(object sender, EventArgs e)
        {
            try
            {
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"Line:xxx 파일명 변환 시작..." + Environment.NewLine);

                if (!string.IsNullOrEmpty(selectedFolder) && Directory.Exists(selectedFolder))
                {
                    string[] files = Directory.GetFiles(selectedFolder);
                    foreach (string filePath in files)
                    {
                        string extension = Path.GetExtension(filePath);
                        // .xml 파일은 제외하고 처리
                        if (!string.Equals(extension, ".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            string newFileName = $"{renameCounter}{extension}";
                            string newFilePath = Path.Combine(selectedFolder, newFileName);
                            // 이미 존재하는 파일일 경우 renameCounter를 증가시키고 새로운 파일 경로 생성
                            while (File.Exists(newFilePath))
                            {
                                renameCounter++;
                                newFileName = $"{renameCounter}{extension}";
                                newFilePath = Path.Combine(selectedFolder, newFileName);
                            }
                            File.Move(filePath, newFilePath);
                            renameCounter = 1;
                        }
                    }
                    LogBox1.AppendText($"Line:xxx 파일명 변환 완료..." + Environment.NewLine);
                    LogBox1.AppendText($"===========================" + Environment.NewLine);
                }
            }
            catch
            {
                LogBox1.AppendText($"Line:xxx 폴더가 없습니다" + Environment.NewLine);
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
