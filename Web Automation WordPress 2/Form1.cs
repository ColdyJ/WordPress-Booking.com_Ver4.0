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
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using OfficeOpenXml;

namespace Web_Automation_WordPress_2
{
    public partial class Form1 : Form
    {
        private const string idlistUrl_1 = ("https://coldyj.github.io/joon.github.io/Tistory_idlist_1.txt"); // 서버에서 버전을 가져올 URL (티스토리 아이디 관리)
        private int result_thumbNail;
        public Form1()
        {
            InitializeComponent();
            LoadConfig(); // Textbox 저장값 프로그램 시작 시 설정 로드
            LogBox1.ScrollBars = ScrollBars.Vertical;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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

        private async void StartBtn1_Click(object sender, EventArgs e)
        {
            SaveConfig();

            string userID = UrlBox1.Text;
            int isRegistered = CheckUserID_1(userID); // 등록 아이디 체크
            if (isRegistered == 1)
            {
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"포스팅 시작" + Environment.NewLine);

                GetHotelListup();  // 리스트업 된 호텔들 전역변수로 변환

                // HotelUrl 문자열을 쉼표로 분리하여 리스트로 변환
                List<string> hotelUrlList = HotelUrl.Split(',').ToList();

                // hotelUrlList을 순회하면서 처리 가능
                foreach (string url in hotelUrlList)
                {
                    PostingHotel = url;
                    await WP_API_Auto();
                    DelayHr();
                }
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

        //Delay 함수 61분~80분
        public void DelayHr()
        {
            int min = 61;
            int max = 80;
            Random random = new Random();
            int delay = random.Next(min, max);
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddMinutes(delay);
            while (DateTime.Now < endTime)
            {
                Application.DoEvents();
            }
            return;
        }

        /*===============================================*/

        // 이미지 크롤링 - 완료
        private void Crawling_Naver()
        {
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"크롤링 시작" + Environment.NewLine);

            // 크롬창 생성
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            //options.AddArguments("--headless"); // 브라우저를 숨김
            driver = new ChromeDriver(driverService, options);
            Delay();

            //이미지 검색 : CCL 상업적 이용가능 
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"이미지 검색" + Environment.NewLine);
            string baseUrl = $"https://search.naver.com/search.naver?where=image&section=image&query={crollBox1.Text}";
            string endUrl = "&res_fr=0&res_to=0&sm=tab_opt&color=&ccl=2&nso=so%3Ar%2Ca%3Aall%2Cp%3Aall&recent=0&datetype=0&startdate=0&enddate=0&gif=0&optStr=&nso_open=1&pq=";
            driver.Navigate().GoToUrl(baseUrl + endUrl);
            Delay();
            ScrollToBottom(driver);

            try
            {
                // 이미지 요소를 찾아서 처리
                var imgElements = driver.FindElements(By.ClassName("image_tile_bx"));
                LogBox1.AppendText($"총 사진 수: {imgElements.Count}장");
                LogBox1.AppendText(Environment.NewLine);

                // "fe_image_tab_content_thumbnail_image" 클래스를 가진 모든 이미지 요소를 찾기 (첫번째 이미지 클릭)
                var imageElements = driver.FindElements(By.CssSelector("img._fe_image_tab_content_thumbnail_image"));
                if (imageElements.Count > 0) imageElements[0].Click();// 첫 번째 이미지를 클릭

                for (int i = 0; i < imgElements.Count - 1; i++)
                {
                    // "다음 이미지" 버튼 요소를 찾기 + 버튼 누르기
                    IWebElement nextButton = driver.FindElement(By.CssSelector("a.btn_next._fe_image_viewer_next_button"));
                    nextButton.Click();

                    // 이미지 요소를 찾기
                    IWebElement imageElement = driver.FindElement(By.CssSelector("img._fe_image_viewer_image_fallback_target"));
                    string imageUrl = imageElement.GetAttribute("src");
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
                            LogBox1.AppendText($"다운로드 중: ({fileCount}/{imgElements.Count})" + Environment.NewLine);
                        }
                    }
                }
                LogBox1.AppendText($"이미지 크롤링 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
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

        // 파일명 정리 - 완료
        private int renameCounter = 1;
        private void Rename()
        {
            try
            {
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"파일명 변환 시작..." + Environment.NewLine);

                if (!string.IsNullOrEmpty(selectedFolder) && Directory.Exists(selectedFolder))
                {
                    string[] files = Directory.GetFiles(selectedFolder);
                    foreach (string filePath in files)
                    {
                        string extension = Path.GetExtension(filePath);
                        // .xml 파일은 제외하고 처리
                        if (string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase))
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
                    LogBox1.AppendText($"파일명 변환 완료..." + Environment.NewLine);
                    LogBox1.AppendText($"===========================" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }

        }

        // 리스트업 된 부킹닷컴 호텔페이지에서 사진 크롤링 후 Rename 
        private void Auto_Crawling()
        {
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"크롤링 시작" + Environment.NewLine);

            // 크롬창 생성
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            options.AddArguments("--headless"); // 브라우저를 숨김
            driver = new ChromeDriver(driverService, options);
            Delay();

            //이미지 검색 
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"이미지 검색" + Environment.NewLine);
            string baseUrl = $"{PostingHotel}";
            driver.Navigate().GoToUrl(baseUrl);
            Delay();
            // 새로운 탭 열기 (첫번째 탭에서 호텔 주소 바로 열면 리스트로 이동되어서 두번째 탭을 오픈하는 편법)
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open('about:blank','_blank');");
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[1]); // 두 번째 탭으로 전환
            driver.Navigate().GoToUrl(baseUrl);

            try
            {
                // 이미지 더보기 클릭: ".bh-photo-grid-thumb-more-inner-2" (클래스를 가진 이미지 요소를 찾기)
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                string imageElements = "document.querySelector('.bh-photo-grid-thumb-more-inner-2').click();";
                jsExecutor.ExecuteScript(imageElements);
                Delay();

                // 이미지 요소를 찾기 (bh-photo-modal-masonry-grid-item caption_hover)
                var imgElements = driver.FindElements(By.XPath("//li[contains(@class, 'bh-photo-modal-masonry-grid-item caption_hover')]/a"));
                LogBox1.AppendText($"총 사진 수: {imgElements.Count}장");
                LogBox1.AppendText(Environment.NewLine);
                Delay();

                // 이미지를 포함한 모든 요소 선택 (bh-photo-modal-masonry-grid-item caption_hover)
                IReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath("//li[contains(@class, 'bh-photo-modal-masonry-grid-item caption_hover')]/a"));
                foreach (var element in elements)
                {
                    // 이미지 URL 추출
                    string imageUrl = element.GetAttribute("href");
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
                            LogBox1.AppendText($"다운로드 중: ({fileCount}/{imgElements.Count})" + Environment.NewLine);
                        }
                    }
                }
                driver.Quit();
                Rename(); // 사진 랜덤 재분배
                LogBox1.AppendText($"이미지 크롤링 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
        }

        // 선택 폴더 내 JPG파일 전부 삭제
        private void DeleteAllJpgFilesInFolder(string folderPath)
        {
            try
            {
                // 지정된 폴더에서 모든 JPG 파일을 검색합니다.
                string[] jpgFiles = Directory.GetFiles(folderPath, "*.jpg");

                // 검색된 모든 JPG 파일을 삭제합니다.
                foreach (string jpgFile in jpgFiles)
                {
                    File.Delete(jpgFile);
                }
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
        }

        /*===============================================*/

        /*===============================================*/

        //호텔 리스트업 엑셀화 HotelListBox1
        private void GetHotelListAsync()
        {
            string url = HotelListBox1.Text; // url
            string excelFilePath = Path.Combine(selectedFolder, "HotelList.xlsx");
            var excelFile = new FileInfo(excelFilePath);
            int currentpage = 1;

            // 엑셀 파일이 없다면 새로 생성
            if (!excelFile.Exists)
            {
                var newExcelPackage = new ExcelPackage();
                newExcelPackage.Workbook.Worksheets.Add("HotelList");
                newExcelPackage.SaveAs(excelFile);
            }
            var excelPackage = new ExcelPackage(excelFile);
            var worksheet = excelPackage.Workbook.Worksheets[0];

            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            //options.AddArguments("--headless"); // 브라우저를 숨김
            driver = new ChromeDriver(driverService, options);
            driver.Navigate().GoToUrl(url);

            while (true)
            {
                try
                {
                    string html = driver.PageSource;
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var titles = doc.DocumentNode.SelectNodes("//div[@class='f6431b446c a15b38c233']"); // 호텔명
                    var hrefs = doc.DocumentNode.SelectNodes("//h3[@class='aab71f8e4e']//a"); // 호텔 주소
                    int startRow = worksheet.Dimension?.Rows ?? 0; // 엑셀 워크시트 0번

                    if (titles != null && hrefs != null)
                    {
                        for (int i = 0; i < titles.Count; i++)
                        {
                            string title = titles[i].InnerText.Trim();
                            string href = hrefs[i].GetAttributeValue("href", "");

                            // 중복 검사: 이미 엑셀에 저장된 데이터와 비교
                            bool isDuplicate = false;
                            for (int row = 1; row <= startRow; row++)
                            {
                                if (worksheet.Cells[row, 1].Text == title && worksheet.Cells[row, 2].Text == href)
                                {
                                    isDuplicate = true;
                                    break; // 중복 발견
                                }
                            }

                            if (!isDuplicate)// 중복이 아닌 경우에만 저장
                            {
                                worksheet.Cells[startRow + i + 1, 1].Value = title;
                                worksheet.Cells[startRow + i + 1, 2].Value = href;
                            }
                        }
                    }

                    Delay();
                    try
                    {
                        // JavaScript를 사용하여 클릭
                        string script = "document.querySelector('button[aria-label=\"로그인 혜택 안내 창 닫기.\"]').click();";
                        ((IJavaScriptExecutor)driver).ExecuteScript(script);
                    }
                    catch (Exception ex)
                    {
                        LogBox1.AppendText($"현재 페이지: {currentpage}" + Environment.NewLine);
                    }

                    currentpage++;
                    Delay();
                    try
                    {
                        // JavaScript를 사용하여 클릭
                        string script = $"document.querySelector('button.a83ed08757.a2028338ea[aria-label=\" {currentpage}\"]').click();"; // currentpage앞 띄어쓰기 한칸 주의 ㅡㅡ
                        ((IJavaScriptExecutor)driver).ExecuteScript(script);
                    }
                    catch (Exception ex)
                    {
                        LogBox1.AppendText($"다음 페이지가 없음: 종료: {ex.Message}" + Environment.NewLine);
                        break;
                    }
                    Delay();
                }
                catch (Exception ex)
                {
                    LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
                }
            }
            excelPackage.Save();
            driver.Quit();
        }

        // 리스트업 된 호텔들 전역변수로 변환
        private void GetHotelListup()
        {
            // 엑셀 파일 경로
            string excelFilePath = Path.Combine(selectedFolder, "HotelList.xlsx");

            // ExcelPackage를 사용하여 엑셀 파일 열기
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                // 워크시트 선택 (0은 첫 번째 워크시트를 의미)
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;

                // B열 (두 번째 열)에서 값 추출하여 문자열로 추가
                for (int row = 1; row <= rowCount; row++)
                {
                    string url = worksheet.Cells[row, 2].Text;
                    HotelUrl += url + ","; // URL을 쉼표로 구분하여 문자열에 추가
                }
            }
            HotelUrl = HotelUrl.TrimEnd(','); // 마지막 쉼표 제거
        }

        /*===============================================*/

        // 호텔 정보 추출
        private async Task<string> GetHotelInfoAsync()
        {
            string url = PostingHotel; // url에 .html 앞에 ko가 없으면 붙임
            if (!url.Contains(".ko."))
            {
                // 정규 표현식을 사용하여 ".html" 앞에 "ko"가 없는 경우 "ko.html"를 추가
                url = Regex.Replace(url, @"(?<!ko)\.html", ".ko.html");
            }
            string combinedInfo = "";
            using (HttpClient client = new HttpClient())
            {

                // HtmlAgilityPack를 사용하여 HTML 파싱
                string html = await client.GetStringAsync(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                List<string> additionalReviews = new List<string>();

                // 원하는 정보를 추출, 예를 들어, <title> 요소의 내용을 추출
                var names = doc.DocumentNode.SelectNodes("//h2[@class='d2fee87262 pp-header__title']");
                var infos = doc.DocumentNode.SelectNodes("//p[@class='a53cbfa6de b3efd73f69']");
                var points = doc.DocumentNode.SelectNodes("//div[@class='a3b8729ab1 d86cee9b25']"); // 평점
                var checkinTimes = doc.DocumentNode.SelectNodes("//div[@id='checkin_policy']//p[2]");
                var checkoutTimes = doc.DocumentNode.SelectNodes("//div[@id='checkout_policy']//p[2]");

                //지도 정보 추출
                var hotel_address = doc.DocumentNode.SelectNodes("//a[@id='hotel_address']");
                foreach (var element in hotel_address)
                {
                    string latLngAttribute = element.GetAttributeValue("data-atlas-latlng", ""); // data-atlas-latlng 속성 값을 가져옵니다.
                    string[] latLng = latLngAttribute.Split(','); // lat와 lng를 추출합니다.
                    // lat와 lng 값을 출력합니다.
                    if (latLng.Length == 2)
                    {
                        lat = latLng[0];
                        lng = latLng[1];
                    }
                }

                // 리뷰 추출
                var reviewNodes = doc.DocumentNode.SelectNodes("//div[@class='a53cbfa6de b5726afd0b']"); // reviews를 여러 요소로 선택
                if (reviewNodes != null)
                {
                    // reviewCount가 5 미만인 경우 reviewCount만큼 출력
                    int reviewCount = reviewNodes.Count;
                    int maxReviewsToDisplay = Math.Min(5, reviewCount);

                    if (reviewCount < 5)
                    {
                        for (int i = 0; i < maxReviewsToDisplay; i++)
                        {
                            // reviewNodes에서 리뷰 가져오기
                            var reviewNode = reviewNodes[i];
                            string additionalReview = reviewNode.InnerText.Trim();

                            // 리뷰 목록에 저장
                            additionalReviews.Add(additionalReview);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            // reviewNodes에서 리뷰 가져오기
                            var reviewNode = reviewNodes[i];
                            string additionalReview = reviewNode.InnerText.Trim();

                            // 리뷰 목록에 저장
                            additionalReviews.Add(additionalReview);
                        }
                    }
                }

                // 추출된 정보 출력
                try
                {
                    hotelName = names[0].InnerText.Trim();
                    string info = infos[0].InnerText.Trim();
                    string checkinTime = checkinTimes[0].InnerText.Trim();
                    string checkoutTime = checkoutTimes[0].InnerText.Trim();
                    string point = "정보 없음";
                    if (points != null) { point = points[0].InnerText.Trim(); }

                    // containers에 있는 정보를 문자열로 결합
                    combinedInfo = $"숙소 명: {hotelName}<p>&nbsp;</p>\n숙소 정보: {info}<p>&nbsp;</p>\n숙소 평점: {point}<p>&nbsp;</p>\nCheck-in Time: {checkinTime}\nCheck-out Time: {checkoutTime}<p>&nbsp;</p>\n숙소 리뷰:\n- {string.Join("<p>&nbsp;</p>- ", additionalReviews)}";
                    string[] keywords = { "숙소 명:", "숙소 정보:", "숙소 평점:", "Check-in Time:", "Check-out Time:", "숙소 리뷰:" };
                    foreach (var keyword in keywords)
                    {
                        combinedInfo = Regex.Replace(combinedInfo, keyword, $"<h3><span style='color: #FF8C00; font-weight: bold;'>{keyword}</span></h3>");
                    }
                    // 결과 출력
                    Console.WriteLine(combinedInfo);

                    // 썸네일 추출, 편집
                    HtmlNode imgNode = doc.DocumentNode.SelectSingleNode("//a[@class='bh-photo-grid-item bh-photo-grid-photo1 active-image ']/img");
                    if (imgNode != null)
                    {
                        string imageUrl = imgNode.GetAttributeValue("src", "");
                        string basePath = selectedFolder; // 기본 저장 폴더 경로
                        string imagePath = Path.Combine(basePath, "Thum_1.jpg"); // 경로 조합
                        try
                        {
                            using (WebClient webclient = new WebClient())
                            {
                                webclient.DownloadFile(imageUrl, imagePath);
                                Console.WriteLine("이미지 다운로드 및 저장 완료: " + imagePath);
                                Image image = Image.FromFile(imagePath);

                                using (Graphics graphics = Graphics.FromImage(image))
                                {
                                    string text = hotelName + "\n" + "숙박 솔직 후기\n★할인 예약코드";
                                    Font font = new Font("NPSfont_regular", 50, FontStyle.Bold);

                                    Brush grayBrush = Brushes.LightGray;
                                    Brush whitebrush = Brushes.White;

                                    Color transparentColor = Color.FromArgb(170, Color.White); //투명도 조절
                                    Brush transparentBrush = new SolidBrush(transparentColor);

                                    Color shadowColor = Color.FromArgb(180, Color.Black); // 그림자 색상 (투명도를 조절하기 위해 Alpha 채널 설정)
                                    Brush shadowBrush = new SolidBrush(shadowColor);

                                    // Calculate the position to insert the text in the middle of the image
                                    float x = (image.Width - graphics.MeasureString(text, font).Width) / 2;
                                    float y = (image.Height - graphics.MeasureString(text, font).Height) / 2;

                                    // 하얀색 바 그리기
                                    int barHeight = 380; // 바의 높이를 조절하세요
                                    graphics.FillRectangle(transparentBrush, 0, y - 50, image.Width, barHeight); // 사각형 바

                                    // 그림자 효과를 추가하기 위해 속성 설정
                                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                                    // 그림자 효과 적용
                                    int shadowOffset = 5; // 그림자의 오프셋
                                    graphics.DrawString(text, font, shadowBrush, new PointF(x + shadowOffset, y + shadowOffset));
                                    graphics.DrawString(text, font, whitebrush, new PointF(x, y));
                                }

                                string outputPath = Path.Combine(basePath, "EditedThum_1.jpg");
                                image.Save(outputPath);
                                image.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
                        }
                    }

                    return combinedInfo;
                }
                catch (Exception ex)
                {
                    LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
                    return combinedInfo;
                }
            }
        }

        // 구글맵
        static async Task<string> google_map()
        {
            // API 키 값을 설정합니다.
            string apiKey = "AIzaSyAnHzeNRM0qu_meS7GRfjaTz3QUm8vhJG8";

            // HTML 문자열을 생성합니다.
            string maphtml = $@"
<head>
    <title>Google Maps Example</title>
    <script src='https://maps.googleapis.com/maps/api/js?key={apiKey}'></script>
    <style>
        #map {{
            height: 400px;
            width: 100%;
        }}
    </style>
</head>
<body>
    <div id='map'></div>
    <script>
        function initMap() {{
            const position = {{ lat: {lat}, lng: {lng} }};
            const map = new google.maps.Map(document.getElementById('map'), {{
                zoom: 18,
                center: position
            }});
            const marker = new google.maps.Marker({{
                position: position,
                map: map,
                title: 'Uluru'
            }});
        }}
    </script>
    <script>
        google.maps.event.addDomListener(window, 'load', initMap);
    </script>
</body>
";
            maphtml = $"<!-- wp:html -->{maphtml}<!-- /wp:html -->";
            return maphtml;
        }

        // 썸네일 등록
        private async Task<string> ThumnailAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // 아이디 비번
            string responseImg = "";
            try
            {
                translation = Papago(hotelName + " 대표 사진");
                string localThumnailPath = Path.Combine(selectedFolder, $"EditedThum_1.jpg"); // 이미지 파일 경로 가져오기
                var createdThumMedia = await client.Media.CreateAsync(localThumnailPath, $"{translation}.jpg"); // localImagePath로 media({translation}.jpg) 생성
                responseImg = $"<img class=\"aligncenter\" src=\"{createdThumMedia.SourceUrl}\">"; // createdMedia에서 변환 시켰으니 img src로 변경
                result_thumbNail = createdThumMedia.Id;
            }
            catch (Exception ex)
            {
                // 오류 처리 - 예외가 발생한 경우 처리
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
            return responseImg; // 이미지 업로드 결과를 리스트로 반환

        }

        // 카테고리 분류 매서드
        private async Task<int> AddCategoriesAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // 아이디 비번
            int categoryId = 0;
            var categories = await client.Categories.GetAllAsync();
            foreach (var category in categories)
            {
                string categoryName = category.Name; // 포스트의 Link 값을 추출
                categoryId = category.Id; // 게시물의 제목을 가져옵니다.
                if (CategoryBox1.Text == categoryName) return categoryId;
            }

            //var categories = await client.Categories.GetByIDAsync(CategoryBox1.Text);
            return 1;
        }

        private string GPT_Prompt(string prompt)
        {
            /*
             1. 공항에서 찾아가는 방법 / 2. 외관 / 3. 내부 방 설명 / 4. 부대시설 / 5.주위맛집 / 6.숙박경험
             */
            string prompt1 = $"'{prompt}'에 관련된 블로그 글을 작성할거야. 이 템플릿에 맞춰서 아주 길고 자세하게 써줄래? 1.올스테이 해야 하는 이유:, 2.어메니티: , 3.조식: , 4.내부: , 5.로비/부대시설: , 6.외관/인테리어:  , 7.숙박경험:";
            return prompt1;
        }

        // GPT 출력 내용 content로 가공
        private async Task<string> AddGPTToContentAsync()
        {
            string result = "";
            string prompt1 = GPT_Prompt(hotelName + " 숙박 후기"); // GPT Prompt 전달
            try
            {
                result = await RequestGPT(prompt1); // GPT에 요청하고 결과를 얻습니다.
            }
            catch (Exception ex)
            {
                // 오류 처리 - 예외가 발생한 경우 처리
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
            string content = result.Replace("\n", "\n\r"); // \n을 \n\r로 변경 , HTML로 줄바꿈 
            string pattern = @"\d+\.\s*[\p{L}\d\s]+(?::)?"; // 정규표현식 = 1. 2. 3. 등 숫자로 분류된 소제목 글꼴변경을 위한 패턴
            Regex regex = new Regex(pattern);
            // 찾은 소제목 패턴을 강조하고 크게 표시합니다.
            string result_GPT = regex.Replace(content, match =>
            {
                return $"<h3>{match.Value}</h3>"; // 사실 이미지 + GPT 가공부분에서 H3 설정을 해주므로 필요 없을 것 같지만 일단 냅둠
            });
            return result_GPT;
        }

        // 이미지 업로드 결과저장
        private async Task<List<string>> ImagesAsyncList()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // 아이디 비번
            translation = Papago(hotelName + " 숙박 후기");
            int count = 0, i = 0;
            List<string> responseImgList = new List<string>(); // 이미지 업로드 결과를 저장할 리스트

            while (count != 8) // 총 7장의 사진을 url로 리스트
            {
                // 이미지 파일 경로 가져오기
                string localImagePath = Path.Combine(selectedFolder, $"{i}.jpg");
                if (File.Exists(localImagePath))
                {
                    var createdMedia = await client.Media.CreateAsync(localImagePath, $"{translation + '_' + i}.jpg"); // localImagePath로 media({translation}.jpg) 생성
                    string responseImg = $"<img class=\"aligncenter\" src=\"{createdMedia.SourceUrl}\">"; // createdMedia에서 변환 시켰으니 img src로 변경
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
                        result_GPT = result_GPT.Replace(imageInfo, $"\r<p>&nbsp;</p><br>{imageSrc}\r<h3><span style='color: #FF8C00; font-weight: bold;'>{imageInfo}</span></h3>");
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
            string tags = "'" + hotelName + " 숙박 후기" + "'" + "을 포함한 인기 검색어 8개를 ','로 구분해서 알려줘";
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
            tagResult = Regex.Replace(tagResult, @"\d", ",");
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

            string addOldPostLinks = "<h3>다른 숙박 후기가 궁금하시다면 아래에 더 많은 내용이 있습니다 :) </h3>\r\n";
            string oldPostsLinks = ""; // 각 링크를 개행 문자로 구분

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
            oldPostsLinks = string.Join("\r\n", selectedLinks); // 각 링크를 개행 문자로 구분

            return addOldPostLinks + "<p>&nbsp;</p>" + oldPostsLinks;
        }


        //외부 링크 추출
        private string AddOutLinksAsync()
        {
            string addOutLinks = "▼▼▼ 예약 웹사이트 바로가기 :) ▼▼▼\r\n";
            string outLinks = ""; // 각 링크를 개행 문자로 구분

            try
            {
                //AffiliateBox1
                List<string> urls = new List<string> // 3개의 URL을 리스트에 추가
                {// 순서대로 익스피디아, 호텔스닷컴, 트립닷컴
                    "https://expedia.com/affiliate/jVHhwTh",
                    "https://www.hotels.com/affiliate/l9VN1VM",
                    "https://kr.trip.com/?Allianceid=4004476&SID=25361194&trip_sub1=&trip_sub3=D108514",
                    "https://www.agoda.com/partners/partnersearch.aspx?pcs=1&cid=1919982&hl=ko-kr"
                };
                // 선택된 URL을 linkHtml 형식으로 만듭니다.
                string expediaLink = $"▶익스피디아 최저가 검색◀"; // 
                string hotelscomLink = $"▶호텔스닷컴 최저가 검색◀"; // 
                string tripcomLink = $"▶트립닷컴 최저가 검색◀"; // 
                string agodaLink = $"▶아고다 최저가 검색◀"; // 

                string expediaHtml = $"<a title=\"{expediaLink}\" href=\"{urls[0]}\">&nbsp;{expediaLink}</a>";
                string hotelscomHtml = $"<a title=\"{hotelscomLink}\" href=\"{urls[1]}\">&nbsp;{hotelscomLink}</a>";
                string tripcomHtml = $"<a title=\"{tripcomLink}\" href=\"{urls[2]}\">&nbsp;{tripcomLink}</a>";
                string agodaHtml = $"<a title=\"{agodaLink}\" href=\"{urls[3]}\">&nbsp;{agodaLink}</a>";

                // 선택된 URL을 outlinks 문자열에 추가
                string[] selectedOutLinks = new string[] { expediaHtml, hotelscomHtml, tripcomHtml, agodaHtml };
                outLinks = string.Join("\r\n", selectedOutLinks); // 각 링크를 개행 문자로 구분
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
            return addOutLinks + "<p>&nbsp;</p>" + outLinks;
        }

        //외부 링크 추출
        private string AddOutBannersAsync()
        {
            string addOutBanners = "▼▼▼ 기간 한정 최저가 검색은 여기서 ▼▼▼\r\n";
            string outLinks = ""; // 각 링크를 개행 문자로 구분

            try
            {
                List<string> urls = new List<string> // 30개의 URL을 리스트에 추가
                {
                    "<iframe border=\"0\" src=\"https://kr.trip.com/partners/ad/S110279?Allianceid=4004476&SID=25361194&trip_sub1=\" style=\"width:300px;height:300px\" frameborder=\"0\" scrolling=\"no\" style=\"border:none\" id=\"S110279\"></iframe>",
                    "<div class=\"eg-widget\" data-widget=\"search\" data-program=\"kr-expedia\" data-lobs=\"stays,flights\" data-network=\"pz\" data-camref=\"1101lS7wB\"></div>\r\n<script class=\"eg-widgets-script\" src=\"https://affiliates.expediagroup.com/products/widgets/assets/eg-widgets.js\"></script>\r\n",
                    "<div class=\"eg-widget\" data-widget=\"search\" data-program=\"kr-hcom\" data-lobs=\"\" data-network=\"pz\" data-camref=\"1011lS9WN\"></div>\r\n<script class=\"eg-widgets-script\" src=\"https://affiliates.expediagroup.com/products/widgets/assets/eg-widgets.js\"></script>"
                };
                List<string> selectedOutLinks = new List<string>();
                Random random = new Random(); // 랜덤하게 1개의 Link 값을 선택합니다.
                for (int i = 0; i < 1; i++)// 랜덤하게 1개의 URL 선택
                {
                    int index = random.Next(urls.Count);
                    string selectedLink = urls[index];
                    urls.RemoveAt(index); // 중복 선택 방지를 위해 선택한 URL을 리스트에서 제거합니다.

                    // 선택된 URL을 linkHtml 형식으로 만듭니다.
                    //string postTitle = $"▶링크◀"; // 원하는 제목을 지정하세요
                    //string linkHtml = $"<a title=\"{postTitle}\" href=\"{selectedLink}\">&nbsp;{postTitle}</a>";
                    string linkText = $"<!-- wp:html -->{selectedLink}<!-- /wp:html -->";
                    selectedOutLinks.Add(linkText);
                }
                // 선택된 URL을 outlinks 문자열에 추가
                outLinks = string.Join("\r\n", selectedOutLinks); // 각 링크를 개행 문자로 구분
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
            return addOutBanners + "<p>&nbsp;</p>" + outLinks;
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


        public async Task WP_API_Auto()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // 아이디 비번

            try
            {
                // 호텔 정보
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"호텔 정보 추가..." + Environment.NewLine);
                string result_Hotel = await GetHotelInfoAsync();
                string result_ThumnailImg = await ThumnailAsync(); // 썸네일 등록id 및 img src
                LogBox1.AppendText($"호텔 정보 추가 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // 이미지 크롤링
                LogBox1.AppendText($"이미지 크롤링 시작..." + Environment.NewLine);
                Auto_Crawling();
                LogBox1.AppendText($"이미지 크롤링 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // 구글 지도
                LogBox1.AppendText($"구글 지도 추가..." + Environment.NewLine);
                string result_GoogleMap = await google_map();
                LogBox1.AppendText($"구글 지도 추가 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // 카테고리 출력
                LogBox1.AppendText($"카테고리 분류 시작..." + Environment.NewLine);
                int result_Categories = await AddCategoriesAsync(); // 완료
                LogBox1.AppendText($"카테고리 분류 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // GPT 본문 출력
                LogBox1.AppendText($"GPT 출력 시작..." + Environment.NewLine);
                string result_GPT = await AddGPTToContentAsync(); // 완료
                LogBox1.AppendText($"GPT 출력 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // GPT 본문 + 이미지 가공
                LogBox1.AppendText($"이미지 & 내용 패턴 변경 시작..." + Environment.NewLine);
                List<string> result_ImgList = await ImagesAsyncList(); // selectedFolder 안의 이미지들을 <img src=\"{createdMedia.SourceUrl}\"> 형식으로 List
                string content = AddImagesToContent(result_GPT, result_ImgList); //resultText 사이에 resultImgList의 string값을 잘 넣어주면됨
                //content = $"<html><body>{content}</body></html>"; // 결과를 HTML 형식으로 표시합니다. 삭제 or 추가
                string head_2 = $"<h2>{hotelName + addTitleBox1.Text + " + 여행 베스트 숙소 추천 숙박 후기"}</h2>";
                LogBox1.AppendText($"이미지 & 내용 패턴 변경 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // 지난 포스팅 링크 추출
                LogBox1.AppendText($"지난 포스팅 링크 추출..." + Environment.NewLine);
                string result_OldPostLinks = await AddOldPostAsync(); // 완료
                LogBox1.AppendText($"지난 포스팅 링크 추출 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // 외부 링크 입력
                LogBox1.AppendText($"외부 링크 추출..." + Environment.NewLine);
                string result_OutLinks = AddOutLinksAsync(); // 완료
                string result_OutBanners = AddOutBannersAsync(); // 완료
                LogBox1.AppendText($"외부 링크 추출 완료" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // 태그 생성 (GPT)
                LogBox1.AppendText($"태그 생성중..." + Environment.NewLine + Environment.NewLine);
                int result_TagId = await AddTagAsync(); // 완료
                LogBox1.AppendText($"태그 생성 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // 요약 요청 (GPT)
                LogBox1.AppendText($"요약중..." + Environment.NewLine);
                string result_Excerpt = await AddGPTToExcerptAsync(result_GPT); // 완료
                LogBox1.AppendText($"요약 완료..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                //WP 업로드 
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"워드프레스 업로드 시작" + Environment.NewLine);
                var post = new Post()
                {
                    Title = new Title(hotelName + addTitleBox1.Text + " + 여행 베스트 숙소 추천 숙박 후기"), // TitleBox1.Text
                    Content = new Content(head_2 + "<p>&nbsp;</p>" + result_Excerpt + "<p>&nbsp;</p>" + result_ThumnailImg + "<p>&nbsp;</p>" + result_OutLinks + "<p>&nbsp;</p>" + result_OutBanners + "<p>&nbsp;</p>" + result_Hotel + "<p>&nbsp;</p>" + result_GoogleMap + "<p>&nbsp;</p>" + result_OutLinks + "<p>&nbsp;</p>" + result_OutBanners + "<p>&nbsp;</p>" + content + "<p>&nbsp;</p>" + result_OutLinks + "<p>&nbsp;</p>" + result_OutBanners + "<p>&nbsp;</p>" + result_OldPostLinks), // GPT
                    FeaturedMedia = result_thumbNail, // 썸네일
                    Categories = new List<int> { result_Categories }, // ComboBox에서 선택한 카테고리 ID 설정
                    CommentStatus = OpenStatus.Open, // 댓글 상태
                    Tags = new List<int> { result_TagId },
                    Status = Status.Publish, // 포스팅 상태 공개,임시
                    Excerpt = new Excerpt(result_Excerpt) // 발췌
                    //Meta = new Description("테스트입니다"), // 메타 데이터
                };
                var createdPost = await client.Posts.CreateAsync(post); // 포스팅 요청 보내기
                LogBox1.AppendText($"글 본문 출력 완료" + Environment.NewLine);
                LogBox1.AppendText($"워드프레스 업로드 완료" + Environment.NewLine);
                DeleteAllJpgFilesInFolder(selectedFolder);
                LogBox1.AppendText($"===========================" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"오류 발생: {ex.Message}" + Environment.NewLine);
            }
        }

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
        private string hotelName = "";
        private static string lat = "";
        private static string lng = "";
        private string HotelUrl = "";
        private string PostingHotel = "";



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
                new XElement("InputValue4", APIKeybox1.Text),
                new XElement("InputValue5", CategoryBox1.Text),
                new XElement("InputValue6", SystemBox1.Text)));
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
                SystemBox1.Text = doc.Root.Element("InputValue5")?.Value;
                CategoryBox1.Text = doc.Root.Element("InputValue6")?.Value;

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
                        new XElement("InputValue4", APIKeybox1.Text),
                        new XElement("InputValue5", CategoryBox1.Text),
                        new XElement("InputValue6", SystemBox1.Text)));
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
                    SystemBox1.Text = doc.Root.Element("InputValue5")?.Value;
                    CategoryBox1.Text = doc.Root.Element("InputValue6")?.Value;

                    MessageBox.Show("Settings loaded.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void CrollBtn1_Click(object sender, EventArgs e)
        {
            Crawling_Naver();
        }
        private void RenameBtn1_Click(object sender, EventArgs e)
        {
            Rename();
        }







        private void button1_Click(object sender, EventArgs e)
        {

            GetHotelListAsync();


        }
    }












    static class Define
    {
        public const string WP_ID = "kwon92088@gmail.com";
        public const string WP_PW = "SV9H TFv0 pMC6 sptk 8s7q 7BsV";
        public const string WP_URL = "https://oheeliving.com/wp-json/";
    }
}
