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
        private const string idlistUrl_1 = ("https://coldyj.github.io/joon.github.io/Tistory_idlist_1.txt"); // �������� ������ ������ URL (Ƽ���丮 ���̵� ����)
        private int result_thumbNail;
        public Form1()
        {
            InitializeComponent();
            LoadConfig(); // Textbox ���尪 ���α׷� ���� �� ���� �ε�
            LogBox1.ScrollBars = ScrollBars.Vertical;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        private IWebDriver driver;
        ChromeOptions options = new ChromeOptions();
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
        // ���� ���̵� ���� (Ƽ���丮 ���̵� ����)
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
            int isRegistered = CheckUserID_1(userID); // ��� ���̵� üũ
            if (isRegistered == 1)
            {
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"������ ����" + Environment.NewLine);

                GetHotelListup();  // ����Ʈ�� �� ȣ�ڵ� ���������� ��ȯ

                // HotelUrl ���ڿ��� ��ǥ�� �и��Ͽ� ����Ʈ�� ��ȯ
                List<string> hotelUrlList = HotelUrl.Split(',').ToList();

                // hotelUrlList�� ��ȸ�ϸ鼭 ó�� ����
                foreach (string url in hotelUrlList)
                {
                    PostingHotel = url;
                    await WP_API_Auto();
                    DelayHr();
                }
            }
            else
            {
                MessageBox.Show("��ϵ� ���̵� �ƴմϴ�. �����ͷ� �����ּ���");
            }
        }

        // Delay �Լ� 3~5��
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

        //Delay �Լ� 61��~80��
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

        // �̹��� ũ�Ѹ� - �Ϸ�
        private void Crawling_Naver()
        {
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"ũ�Ѹ� ����" + Environment.NewLine);

            // ũ��â ����
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            //options.AddArguments("--headless"); // �������� ����
            driver = new ChromeDriver(driverService, options);
            Delay();

            //�̹��� �˻� : CCL ����� �̿밡�� 
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"�̹��� �˻�" + Environment.NewLine);
            string baseUrl = $"https://search.naver.com/search.naver?where=image&section=image&query={crollBox1.Text}";
            string endUrl = "&res_fr=0&res_to=0&sm=tab_opt&color=&ccl=2&nso=so%3Ar%2Ca%3Aall%2Cp%3Aall&recent=0&datetype=0&startdate=0&enddate=0&gif=0&optStr=&nso_open=1&pq=";
            driver.Navigate().GoToUrl(baseUrl + endUrl);
            Delay();
            ScrollToBottom(driver);

            try
            {
                // �̹��� ��Ҹ� ã�Ƽ� ó��
                var imgElements = driver.FindElements(By.ClassName("image_tile_bx"));
                LogBox1.AppendText($"�� ���� ��: {imgElements.Count}��");
                LogBox1.AppendText(Environment.NewLine);

                // "fe_image_tab_content_thumbnail_image" Ŭ������ ���� ��� �̹��� ��Ҹ� ã�� (ù��° �̹��� Ŭ��)
                var imageElements = driver.FindElements(By.CssSelector("img._fe_image_tab_content_thumbnail_image"));
                if (imageElements.Count > 0) imageElements[0].Click();// ù ��° �̹����� Ŭ��

                for (int i = 0; i < imgElements.Count - 1; i++)
                {
                    // "���� �̹���" ��ư ��Ҹ� ã�� + ��ư ������
                    IWebElement nextButton = driver.FindElement(By.CssSelector("a.btn_next._fe_image_viewer_next_button"));
                    nextButton.Click();

                    // �̹��� ��Ҹ� ã��
                    IWebElement imageElement = driver.FindElement(By.CssSelector("img._fe_image_viewer_image_fallback_target"));
                    string imageUrl = imageElement.GetAttribute("src");
                    if (!string.IsNullOrEmpty(imageUrl) && (imageUrl.StartsWith("http://") || imageUrl.StartsWith("https://")))
                    {
                        string basePath = selectedFolder; // �⺻ ���� ���
                        int fileCount = 1;
                        string fileName = $"{fileCount}.jpg"; // ������ �̹��� ���� �̸�
                        while (File.Exists(Path.Combine(basePath, fileName)))
                        {
                            fileCount++;
                            fileName = $"{fileCount}.jpg";
                        }
                        // �̹��� �ٿ�ε�
                        using (WebClient client = new WebClient())
                        {
                            // ���� ������ �̹��� ����
                            byte[] imageData = client.DownloadData(imageUrl);
                            string filePath = Path.Combine(basePath, fileName);
                            File.WriteAllBytes(filePath, imageData);
                            Delay();
                            LogBox1.AppendText($"�ٿ�ε� ��: ({fileCount}/{imgElements.Count})" + Environment.NewLine);
                        }
                    }
                }
                LogBox1.AppendText($"�̹��� ũ�Ѹ� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
        }
        private void ScrollToBottom(IWebDriver driver)
        {
            for (int i = 0; i < 12; i++)
            {
                // JavaScript�� �����Ͽ� ��ũ���� �Ʒ��� �̵��մϴ�.
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(50, document.body.scrollHeight);");
                Delay();
            }
        }

        // ���ϸ� ���� - �Ϸ�
        private int renameCounter = 1;
        private void Rename()
        {
            try
            {
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"���ϸ� ��ȯ ����..." + Environment.NewLine);

                if (!string.IsNullOrEmpty(selectedFolder) && Directory.Exists(selectedFolder))
                {
                    string[] files = Directory.GetFiles(selectedFolder);
                    foreach (string filePath in files)
                    {
                        string extension = Path.GetExtension(filePath);
                        // .xml ������ �����ϰ� ó��
                        if (string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase))
                        {
                            string newFileName = $"{renameCounter}{extension}";
                            string newFilePath = Path.Combine(selectedFolder, newFileName);
                            // �̹� �����ϴ� ������ ��� renameCounter�� ������Ű�� ���ο� ���� ��� ����
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
                    LogBox1.AppendText($"���ϸ� ��ȯ �Ϸ�..." + Environment.NewLine);
                    LogBox1.AppendText($"===========================" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }

        }

        // ����Ʈ�� �� ��ŷ���� ȣ������������ ���� ũ�Ѹ� �� Rename 
        private void Auto_Crawling()
        {
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"ũ�Ѹ� ����" + Environment.NewLine);

            // ũ��â ����
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            options.AddArguments("--headless"); // �������� ����
            driver = new ChromeDriver(driverService, options);
            Delay();

            //�̹��� �˻� 
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"�̹��� �˻�" + Environment.NewLine);
            string baseUrl = $"{PostingHotel}";
            driver.Navigate().GoToUrl(baseUrl);
            Delay();
            // ���ο� �� ���� (ù��° �ǿ��� ȣ�� �ּ� �ٷ� ���� ����Ʈ�� �̵��Ǿ �ι�° ���� �����ϴ� ���)
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open('about:blank','_blank');");
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[1]); // �� ��° ������ ��ȯ
            driver.Navigate().GoToUrl(baseUrl);

            try
            {
                // �̹��� ������ Ŭ��: ".bh-photo-grid-thumb-more-inner-2" (Ŭ������ ���� �̹��� ��Ҹ� ã��)
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                string imageElements = "document.querySelector('.bh-photo-grid-thumb-more-inner-2').click();";
                jsExecutor.ExecuteScript(imageElements);
                Delay();

                // �̹��� ��Ҹ� ã�� (bh-photo-modal-masonry-grid-item caption_hover)
                var imgElements = driver.FindElements(By.XPath("//li[contains(@class, 'bh-photo-modal-masonry-grid-item caption_hover')]/a"));
                LogBox1.AppendText($"�� ���� ��: {imgElements.Count}��");
                LogBox1.AppendText(Environment.NewLine);
                Delay();

                // �̹����� ������ ��� ��� ���� (bh-photo-modal-masonry-grid-item caption_hover)
                IReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath("//li[contains(@class, 'bh-photo-modal-masonry-grid-item caption_hover')]/a"));
                foreach (var element in elements)
                {
                    // �̹��� URL ����
                    string imageUrl = element.GetAttribute("href");
                    if (!string.IsNullOrEmpty(imageUrl) && (imageUrl.StartsWith("http://") || imageUrl.StartsWith("https://")))
                    {
                        string basePath = selectedFolder; // �⺻ ���� ���
                        int fileCount = 1;
                        string fileName = $"{fileCount}.jpg"; // ������ �̹��� ���� �̸�
                        while (File.Exists(Path.Combine(basePath, fileName)))
                        {
                            fileCount++;
                            fileName = $"{fileCount}.jpg";
                        }
                        // �̹��� �ٿ�ε�
                        using (WebClient client = new WebClient())
                        {
                            // ���� ������ �̹��� ����
                            byte[] imageData = client.DownloadData(imageUrl);
                            string filePath = Path.Combine(basePath, fileName);
                            File.WriteAllBytes(filePath, imageData);
                            Delay();
                            LogBox1.AppendText($"�ٿ�ε� ��: ({fileCount}/{imgElements.Count})" + Environment.NewLine);
                        }
                    }
                }
                driver.Quit();
                Rename(); // ���� ���� ��й�
                LogBox1.AppendText($"�̹��� ũ�Ѹ� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
        }

        // ���� ���� �� JPG���� ���� ����
        private void DeleteAllJpgFilesInFolder(string folderPath)
        {
            try
            {
                // ������ �������� ��� JPG ������ �˻��մϴ�.
                string[] jpgFiles = Directory.GetFiles(folderPath, "*.jpg");

                // �˻��� ��� JPG ������ �����մϴ�.
                foreach (string jpgFile in jpgFiles)
                {
                    File.Delete(jpgFile);
                }
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
        }

        /*===============================================*/

        /*===============================================*/

        //ȣ�� ����Ʈ�� ����ȭ HotelListBox1
        private void GetHotelListAsync()
        {
            string url = HotelListBox1.Text; // url
            string excelFilePath = Path.Combine(selectedFolder, "HotelList.xlsx");
            var excelFile = new FileInfo(excelFilePath);
            int currentpage = 1;

            // ���� ������ ���ٸ� ���� ����
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
            //options.AddArguments("--headless"); // �������� ����
            driver = new ChromeDriver(driverService, options);
            driver.Navigate().GoToUrl(url);

            while (true)
            {
                try
                {
                    string html = driver.PageSource;
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var titles = doc.DocumentNode.SelectNodes("//div[@class='f6431b446c a15b38c233']"); // ȣ�ڸ�
                    var hrefs = doc.DocumentNode.SelectNodes("//h3[@class='aab71f8e4e']//a"); // ȣ�� �ּ�
                    int startRow = worksheet.Dimension?.Rows ?? 0; // ���� ��ũ��Ʈ 0��

                    if (titles != null && hrefs != null)
                    {
                        for (int i = 0; i < titles.Count; i++)
                        {
                            string title = titles[i].InnerText.Trim();
                            string href = hrefs[i].GetAttributeValue("href", "");

                            // �ߺ� �˻�: �̹� ������ ����� �����Ϳ� ��
                            bool isDuplicate = false;
                            for (int row = 1; row <= startRow; row++)
                            {
                                if (worksheet.Cells[row, 1].Text == title && worksheet.Cells[row, 2].Text == href)
                                {
                                    isDuplicate = true;
                                    break; // �ߺ� �߰�
                                }
                            }

                            if (!isDuplicate)// �ߺ��� �ƴ� ��쿡�� ����
                            {
                                worksheet.Cells[startRow + i + 1, 1].Value = title;
                                worksheet.Cells[startRow + i + 1, 2].Value = href;
                            }
                        }
                    }

                    Delay();
                    try
                    {
                        // JavaScript�� ����Ͽ� Ŭ��
                        string script = "document.querySelector('button[aria-label=\"�α��� ���� �ȳ� â �ݱ�.\"]').click();";
                        ((IJavaScriptExecutor)driver).ExecuteScript(script);
                    }
                    catch (Exception ex)
                    {
                        LogBox1.AppendText($"���� ������: {currentpage}" + Environment.NewLine);
                    }

                    currentpage++;
                    Delay();
                    try
                    {
                        // JavaScript�� ����Ͽ� Ŭ��
                        string script = $"document.querySelector('button.a83ed08757.a2028338ea[aria-label=\" {currentpage}\"]').click();"; // currentpage�� ���� ��ĭ ���� �Ѥ�
                        ((IJavaScriptExecutor)driver).ExecuteScript(script);
                    }
                    catch (Exception ex)
                    {
                        LogBox1.AppendText($"���� �������� ����: ����: {ex.Message}" + Environment.NewLine);
                        break;
                    }
                    Delay();
                }
                catch (Exception ex)
                {
                    LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
                }
            }
            excelPackage.Save();
            driver.Quit();
        }

        // ����Ʈ�� �� ȣ�ڵ� ���������� ��ȯ
        private void GetHotelListup()
        {
            // ���� ���� ���
            string excelFilePath = Path.Combine(selectedFolder, "HotelList.xlsx");

            // ExcelPackage�� ����Ͽ� ���� ���� ����
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                // ��ũ��Ʈ ���� (0�� ù ��° ��ũ��Ʈ�� �ǹ�)
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;

                // B�� (�� ��° ��)���� �� �����Ͽ� ���ڿ��� �߰�
                for (int row = 1; row <= rowCount; row++)
                {
                    string url = worksheet.Cells[row, 2].Text;
                    HotelUrl += url + ","; // URL�� ��ǥ�� �����Ͽ� ���ڿ��� �߰�
                }
            }
            HotelUrl = HotelUrl.TrimEnd(','); // ������ ��ǥ ����
        }

        /*===============================================*/

        // ȣ�� ���� ����
        private async Task<string> GetHotelInfoAsync()
        {
            string url = PostingHotel; // url�� .html �տ� ko�� ������ ����
            if (!url.Contains(".ko."))
            {
                // ���� ǥ������ ����Ͽ� ".html" �տ� "ko"�� ���� ��� "ko.html"�� �߰�
                url = Regex.Replace(url, @"(?<!ko)\.html", ".ko.html");
            }
            string combinedInfo = "";
            using (HttpClient client = new HttpClient())
            {

                // HtmlAgilityPack�� ����Ͽ� HTML �Ľ�
                string html = await client.GetStringAsync(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                List<string> additionalReviews = new List<string>();

                // ���ϴ� ������ ����, ���� ���, <title> ����� ������ ����
                var names = doc.DocumentNode.SelectNodes("//h2[@class='d2fee87262 pp-header__title']");
                var infos = doc.DocumentNode.SelectNodes("//p[@class='a53cbfa6de b3efd73f69']");
                var points = doc.DocumentNode.SelectNodes("//div[@class='a3b8729ab1 d86cee9b25']"); // ����
                var checkinTimes = doc.DocumentNode.SelectNodes("//div[@id='checkin_policy']//p[2]");
                var checkoutTimes = doc.DocumentNode.SelectNodes("//div[@id='checkout_policy']//p[2]");

                //���� ���� ����
                var hotel_address = doc.DocumentNode.SelectNodes("//a[@id='hotel_address']");
                foreach (var element in hotel_address)
                {
                    string latLngAttribute = element.GetAttributeValue("data-atlas-latlng", ""); // data-atlas-latlng �Ӽ� ���� �����ɴϴ�.
                    string[] latLng = latLngAttribute.Split(','); // lat�� lng�� �����մϴ�.
                    // lat�� lng ���� ����մϴ�.
                    if (latLng.Length == 2)
                    {
                        lat = latLng[0];
                        lng = latLng[1];
                    }
                }

                // ���� ����
                var reviewNodes = doc.DocumentNode.SelectNodes("//div[@class='a53cbfa6de b5726afd0b']"); // reviews�� ���� ��ҷ� ����
                if (reviewNodes != null)
                {
                    // reviewCount�� 5 �̸��� ��� reviewCount��ŭ ���
                    int reviewCount = reviewNodes.Count;
                    int maxReviewsToDisplay = Math.Min(5, reviewCount);

                    if (reviewCount < 5)
                    {
                        for (int i = 0; i < maxReviewsToDisplay; i++)
                        {
                            // reviewNodes���� ���� ��������
                            var reviewNode = reviewNodes[i];
                            string additionalReview = reviewNode.InnerText.Trim();

                            // ���� ��Ͽ� ����
                            additionalReviews.Add(additionalReview);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            // reviewNodes���� ���� ��������
                            var reviewNode = reviewNodes[i];
                            string additionalReview = reviewNode.InnerText.Trim();

                            // ���� ��Ͽ� ����
                            additionalReviews.Add(additionalReview);
                        }
                    }
                }

                // ����� ���� ���
                try
                {
                    hotelName = names[0].InnerText.Trim();
                    string info = infos[0].InnerText.Trim();
                    string checkinTime = checkinTimes[0].InnerText.Trim();
                    string checkoutTime = checkoutTimes[0].InnerText.Trim();
                    string point = "���� ����";
                    if (points != null) { point = points[0].InnerText.Trim(); }

                    // containers�� �ִ� ������ ���ڿ��� ����
                    combinedInfo = $"���� ��: {hotelName}<p>&nbsp;</p>\n���� ����: {info}<p>&nbsp;</p>\n���� ����: {point}<p>&nbsp;</p>\nCheck-in Time: {checkinTime}\nCheck-out Time: {checkoutTime}<p>&nbsp;</p>\n���� ����:\n- {string.Join("<p>&nbsp;</p>- ", additionalReviews)}";
                    string[] keywords = { "���� ��:", "���� ����:", "���� ����:", "Check-in Time:", "Check-out Time:", "���� ����:" };
                    foreach (var keyword in keywords)
                    {
                        combinedInfo = Regex.Replace(combinedInfo, keyword, $"<h3><span style='color: #FF8C00; font-weight: bold;'>{keyword}</span></h3>");
                    }
                    // ��� ���
                    Console.WriteLine(combinedInfo);

                    // ����� ����, ����
                    HtmlNode imgNode = doc.DocumentNode.SelectSingleNode("//a[@class='bh-photo-grid-item bh-photo-grid-photo1 active-image ']/img");
                    if (imgNode != null)
                    {
                        string imageUrl = imgNode.GetAttributeValue("src", "");
                        string basePath = selectedFolder; // �⺻ ���� ���� ���
                        string imagePath = Path.Combine(basePath, "Thum_1.jpg"); // ��� ����
                        try
                        {
                            using (WebClient webclient = new WebClient())
                            {
                                webclient.DownloadFile(imageUrl, imagePath);
                                Console.WriteLine("�̹��� �ٿ�ε� �� ���� �Ϸ�: " + imagePath);
                                Image image = Image.FromFile(imagePath);

                                using (Graphics graphics = Graphics.FromImage(image))
                                {
                                    string text = hotelName + "\n" + "���� ���� �ı�\n������ �����ڵ�";
                                    Font font = new Font("NPSfont_regular", 50, FontStyle.Bold);

                                    Brush grayBrush = Brushes.LightGray;
                                    Brush whitebrush = Brushes.White;

                                    Color transparentColor = Color.FromArgb(170, Color.White); //���� ����
                                    Brush transparentBrush = new SolidBrush(transparentColor);

                                    Color shadowColor = Color.FromArgb(180, Color.Black); // �׸��� ���� (������ �����ϱ� ���� Alpha ä�� ����)
                                    Brush shadowBrush = new SolidBrush(shadowColor);

                                    // Calculate the position to insert the text in the middle of the image
                                    float x = (image.Width - graphics.MeasureString(text, font).Width) / 2;
                                    float y = (image.Height - graphics.MeasureString(text, font).Height) / 2;

                                    // �Ͼ�� �� �׸���
                                    int barHeight = 380; // ���� ���̸� �����ϼ���
                                    graphics.FillRectangle(transparentBrush, 0, y - 50, image.Width, barHeight); // �簢�� ��

                                    // �׸��� ȿ���� �߰��ϱ� ���� �Ӽ� ����
                                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                                    // �׸��� ȿ�� ����
                                    int shadowOffset = 5; // �׸����� ������
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
                            LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
                        }
                    }

                    return combinedInfo;
                }
                catch (Exception ex)
                {
                    LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
                    return combinedInfo;
                }
            }
        }

        // ���۸�
        static async Task<string> google_map()
        {
            // API Ű ���� �����մϴ�.
            string apiKey = "AIzaSyAnHzeNRM0qu_meS7GRfjaTz3QUm8vhJG8";

            // HTML ���ڿ��� �����մϴ�.
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

        // ����� ���
        private async Task<string> ThumnailAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���
            string responseImg = "";
            try
            {
                translation = Papago(hotelName + " ��ǥ ����");
                string localThumnailPath = Path.Combine(selectedFolder, $"EditedThum_1.jpg"); // �̹��� ���� ��� ��������
                var createdThumMedia = await client.Media.CreateAsync(localThumnailPath, $"{translation}.jpg"); // localImagePath�� media({translation}.jpg) ����
                responseImg = $"<img class=\"aligncenter\" src=\"{createdThumMedia.SourceUrl}\">"; // createdMedia���� ��ȯ �������� img src�� ����
                result_thumbNail = createdThumMedia.Id;
            }
            catch (Exception ex)
            {
                // ���� ó�� - ���ܰ� �߻��� ��� ó��
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
            return responseImg; // �̹��� ���ε� ����� ����Ʈ�� ��ȯ

        }

        // ī�װ� �з� �ż���
        private async Task<int> AddCategoriesAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���
            int categoryId = 0;
            var categories = await client.Categories.GetAllAsync();
            foreach (var category in categories)
            {
                string categoryName = category.Name; // ����Ʈ�� Link ���� ����
                categoryId = category.Id; // �Խù��� ������ �����ɴϴ�.
                if (CategoryBox1.Text == categoryName) return categoryId;
            }

            //var categories = await client.Categories.GetByIDAsync(CategoryBox1.Text);
            return 1;
        }

        private string GPT_Prompt(string prompt)
        {
            /*
             1. ���׿��� ã�ư��� ��� / 2. �ܰ� / 3. ���� �� ���� / 4. �δ�ü� / 5.�������� / 6.���ڰ���
             */
            string prompt1 = $"'{prompt}'�� ���õ� ��α� ���� �ۼ��Ұž�. �� ���ø��� ���缭 ���� ��� �ڼ��ϰ� ���ٷ�? 1.�ý����� �ؾ� �ϴ� ����:, 2.��޴�Ƽ: , 3.����: , 4.����: , 5.�κ�/�δ�ü�: , 6.�ܰ�/���׸���:  , 7.���ڰ���:";
            return prompt1;
        }

        // GPT ��� ���� content�� ����
        private async Task<string> AddGPTToContentAsync()
        {
            string result = "";
            string prompt1 = GPT_Prompt(hotelName + " ���� �ı�"); // GPT Prompt ����
            try
            {
                result = await RequestGPT(prompt1); // GPT�� ��û�ϰ� ����� ����ϴ�.
            }
            catch (Exception ex)
            {
                // ���� ó�� - ���ܰ� �߻��� ��� ó��
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
            string content = result.Replace("\n", "\n\r"); // \n�� \n\r�� ���� , HTML�� �ٹٲ� 
            string pattern = @"\d+\.\s*[\p{L}\d\s]+(?::)?"; // ����ǥ���� = 1. 2. 3. �� ���ڷ� �з��� ������ �۲ú����� ���� ����
            Regex regex = new Regex(pattern);
            // ã�� ������ ������ �����ϰ� ũ�� ǥ���մϴ�.
            string result_GPT = regex.Replace(content, match =>
            {
                return $"<h3>{match.Value}</h3>"; // ��� �̹��� + GPT �����κп��� H3 ������ ���ֹǷ� �ʿ� ���� �� ������ �ϴ� ����
            });
            return result_GPT;
        }

        // �̹��� ���ε� �������
        private async Task<List<string>> ImagesAsyncList()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���
            translation = Papago(hotelName + " ���� �ı�");
            int count = 0, i = 0;
            List<string> responseImgList = new List<string>(); // �̹��� ���ε� ����� ������ ����Ʈ

            while (count != 8) // �� 7���� ������ url�� ����Ʈ
            {
                // �̹��� ���� ��� ��������
                string localImagePath = Path.Combine(selectedFolder, $"{i}.jpg");
                if (File.Exists(localImagePath))
                {
                    var createdMedia = await client.Media.CreateAsync(localImagePath, $"{translation + '_' + i}.jpg"); // localImagePath�� media({translation}.jpg) ����
                    string responseImg = $"<img class=\"aligncenter\" src=\"{createdMedia.SourceUrl}\">"; // createdMedia���� ��ȯ �������� img src�� ����
                    responseImgList.Add(responseImg);
                    count++;
                    i++;
                }
                else
                {
                    i++;
                }
            }
            return responseImgList; // �̹��� ���ε� ����� ����Ʈ�� ��ȯ
        }
        // GPT ��� ���� ���̿� �̹��� �߰�
        private string AddImagesToContent(string result_GPT, List<string> result_ImgList)
        {
            string pattern = @"\d+\.\s*[\p{L}\d\s]+(?::)?";  // �̹����� ���� �ڸ� ã�� ���� ǥ���� ����
            // result_GPT ���ڿ����� ���� ǥ���� ���ϰ� ��ġ�ϴ� �κ��� ����
            MatchCollection matches = Regex.Matches(result_GPT, pattern);

            // result_ImgList�� img src�� ����� MatchCollection ��ġ�� ����
            foreach (Match match in matches)
            {
                string imageInfo = match.Value.Trim();

                // �̹��� �������� ������ ����
                string[] parts = imageInfo.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    // �̹��� ������ <br> �±׿� �Բ� �߰�
                    string imageSrc = result_ImgList.FirstOrDefault(); // �̹��� URL�� ������
                    if (!string.IsNullOrEmpty(imageSrc))
                    {
                        result_GPT = result_GPT.Replace(imageInfo, $"\r<p>&nbsp;</p><br>{imageSrc}\r<h3><span style='color: #FF8C00; font-weight: bold;'>{imageInfo}</span></h3>");
                        result_ImgList.RemoveAt(0); // ����� �̹��� URL�� ����Ʈ���� ����
                    }
                }
            }
            return result_GPT;
        }


        // GPT�� Tag ���
        private async Task<int> AddTagAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���
            string tags = "'" + hotelName + " ���� �ı�" + "'" + "�� ������ �α� �˻��� 8���� ','�� �����ؼ� �˷���";
            string tagResult = "";
            try
            {
                tagResult = await RequestGPT(tags); // GPT�� ��û�ϰ� ����� ����ϴ�.
            }
            catch (Exception ex)
            {
                // ���� ó�� - ���ܰ� �߻��� ��� ó��
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
            tagResult = Regex.Replace(tagResult, @"\d", ",");
            if (tagResult.Contains(", #"))
            {
                tagResult = tagResult.Replace(", #", ",");
            }
            var tag = new Tag()
            {
                Name = tagResult // ���� TAG�� ������
            };
            var createdtag = await client.Tags.CreateAsync(tag);
            LogBox1.AppendText(tagResult + Environment.NewLine + Environment.NewLine); // �±� ���
            return createdtag.Id;
        }


        // ���� ������ ��ũ ���� �ż���
        private async Task<string> AddOldPostAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���
            var posts = await client.Posts.GetAllAsync();

            string addOldPostLinks = "<h3>�ٸ� ���� �ıⰡ �ñ��Ͻôٸ� �Ʒ��� �� ���� ������ �ֽ��ϴ� :) </h3>\r\n";
            string oldPostsLinks = ""; // �� ��ũ�� ���� ���ڷ� ����

            List<string> postLinks = new List<string>(); // ����Ʈ�� Link ���� ������ ����Ʈ�� ����ϴ�.
            foreach (var postli in posts)
            {
                string postLink = postli.Link; // ����Ʈ�� Link ���� ����
                string postTitle = postli.Title.Rendered; // �Խù��� ������ �����ɴϴ�.
                string linkHtml = $"<a title=\"{postTitle}\" href=\"{postLink}\">&nbsp;��{postTitle}</a>";
                postLinks.Add(linkHtml);
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
            oldPostsLinks = string.Join("\r\n", selectedLinks); // �� ��ũ�� ���� ���ڷ� ����

            return addOldPostLinks + "<p>&nbsp;</p>" + oldPostsLinks;
        }


        //�ܺ� ��ũ ����
        private string AddOutLinksAsync()
        {
            string addOutLinks = "���� ���� ������Ʈ �ٷΰ��� :) ����\r\n";
            string outLinks = ""; // �� ��ũ�� ���� ���ڷ� ����

            try
            {
                //AffiliateBox1
                List<string> urls = new List<string> // 3���� URL�� ����Ʈ�� �߰�
                {// ������� �ͽ��ǵ��, ȣ�ڽ�����, Ʈ������
                    "https://expedia.com/affiliate/jVHhwTh",
                    "https://www.hotels.com/affiliate/l9VN1VM",
                    "https://kr.trip.com/?Allianceid=4004476&SID=25361194&trip_sub1=&trip_sub3=D108514",
                    "https://www.agoda.com/partners/partnersearch.aspx?pcs=1&cid=1919982&hl=ko-kr"
                };
                // ���õ� URL�� linkHtml �������� ����ϴ�.
                string expediaLink = $"���ͽ��ǵ�� ������ �˻���"; // 
                string hotelscomLink = $"��ȣ�ڽ����� ������ �˻���"; // 
                string tripcomLink = $"��Ʈ������ ������ �˻���"; // 
                string agodaLink = $"���ư�� ������ �˻���"; // 

                string expediaHtml = $"<a title=\"{expediaLink}\" href=\"{urls[0]}\">&nbsp;{expediaLink}</a>";
                string hotelscomHtml = $"<a title=\"{hotelscomLink}\" href=\"{urls[1]}\">&nbsp;{hotelscomLink}</a>";
                string tripcomHtml = $"<a title=\"{tripcomLink}\" href=\"{urls[2]}\">&nbsp;{tripcomLink}</a>";
                string agodaHtml = $"<a title=\"{agodaLink}\" href=\"{urls[3]}\">&nbsp;{agodaLink}</a>";

                // ���õ� URL�� outlinks ���ڿ��� �߰�
                string[] selectedOutLinks = new string[] { expediaHtml, hotelscomHtml, tripcomHtml, agodaHtml };
                outLinks = string.Join("\r\n", selectedOutLinks); // �� ��ũ�� ���� ���ڷ� ����
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
            return addOutLinks + "<p>&nbsp;</p>" + outLinks;
        }

        //�ܺ� ��ũ ����
        private string AddOutBannersAsync()
        {
            string addOutBanners = "���� �Ⱓ ���� ������ �˻��� ���⼭ ����\r\n";
            string outLinks = ""; // �� ��ũ�� ���� ���ڷ� ����

            try
            {
                List<string> urls = new List<string> // 30���� URL�� ����Ʈ�� �߰�
                {
                    "<iframe border=\"0\" src=\"https://kr.trip.com/partners/ad/S110279?Allianceid=4004476&SID=25361194&trip_sub1=\" style=\"width:300px;height:300px\" frameborder=\"0\" scrolling=\"no\" style=\"border:none\" id=\"S110279\"></iframe>",
                    "<div class=\"eg-widget\" data-widget=\"search\" data-program=\"kr-expedia\" data-lobs=\"stays,flights\" data-network=\"pz\" data-camref=\"1101lS7wB\"></div>\r\n<script class=\"eg-widgets-script\" src=\"https://affiliates.expediagroup.com/products/widgets/assets/eg-widgets.js\"></script>\r\n",
                    "<div class=\"eg-widget\" data-widget=\"search\" data-program=\"kr-hcom\" data-lobs=\"\" data-network=\"pz\" data-camref=\"1011lS9WN\"></div>\r\n<script class=\"eg-widgets-script\" src=\"https://affiliates.expediagroup.com/products/widgets/assets/eg-widgets.js\"></script>"
                };
                List<string> selectedOutLinks = new List<string>();
                Random random = new Random(); // �����ϰ� 1���� Link ���� �����մϴ�.
                for (int i = 0; i < 1; i++)// �����ϰ� 1���� URL ����
                {
                    int index = random.Next(urls.Count);
                    string selectedLink = urls[index];
                    urls.RemoveAt(index); // �ߺ� ���� ������ ���� ������ URL�� ����Ʈ���� �����մϴ�.

                    // ���õ� URL�� linkHtml �������� ����ϴ�.
                    //string postTitle = $"����ũ��"; // ���ϴ� ������ �����ϼ���
                    //string linkHtml = $"<a title=\"{postTitle}\" href=\"{selectedLink}\">&nbsp;{postTitle}</a>";
                    string linkText = $"<!-- wp:html -->{selectedLink}<!-- /wp:html -->";
                    selectedOutLinks.Add(linkText);
                }
                // ���õ� URL�� outlinks ���ڿ��� �߰�
                outLinks = string.Join("\r\n", selectedOutLinks); // �� ��ũ�� ���� ���ڷ� ����
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
            return addOutBanners + "<p>&nbsp;</p>" + outLinks;
        }


        // GPT ��� ���� ������� ����
        private async Task<string> AddGPTToExcerptAsync(string result_GPT)
        {
            string result = "";
            string prompt1 = "�Ʒ� ������ ������� \n\r" + result_GPT; // GPT Prompt ����
            try
            {
                result = await RequestGPT(prompt1); // GPT�� ��û�ϰ� ����� ����ϴ�.
            }
            catch (Exception ex)
            {
                // ���� ó�� - ���ܰ� �߻��� ��� ó��
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
            string content_Excerpt = result; // \n�� <br>�� ���� , result�� content�� �Ҵ�
            return content_Excerpt;
        }


        public async Task WP_API_Auto()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���

            try
            {
                // ȣ�� ����
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"ȣ�� ���� �߰�..." + Environment.NewLine);
                string result_Hotel = await GetHotelInfoAsync();
                string result_ThumnailImg = await ThumnailAsync(); // ����� ���id �� img src
                LogBox1.AppendText($"ȣ�� ���� �߰� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // �̹��� ũ�Ѹ�
                LogBox1.AppendText($"�̹��� ũ�Ѹ� ����..." + Environment.NewLine);
                Auto_Crawling();
                LogBox1.AppendText($"�̹��� ũ�Ѹ� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // ���� ����
                LogBox1.AppendText($"���� ���� �߰�..." + Environment.NewLine);
                string result_GoogleMap = await google_map();
                LogBox1.AppendText($"���� ���� �߰� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // ī�װ� ���
                LogBox1.AppendText($"ī�װ� �з� ����..." + Environment.NewLine);
                int result_Categories = await AddCategoriesAsync(); // �Ϸ�
                LogBox1.AppendText($"ī�װ� �з� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // GPT ���� ���
                LogBox1.AppendText($"GPT ��� ����..." + Environment.NewLine);
                string result_GPT = await AddGPTToContentAsync(); // �Ϸ�
                LogBox1.AppendText($"GPT ��� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // GPT ���� + �̹��� ����
                LogBox1.AppendText($"�̹��� & ���� ���� ���� ����..." + Environment.NewLine);
                List<string> result_ImgList = await ImagesAsyncList(); // selectedFolder ���� �̹������� <img src=\"{createdMedia.SourceUrl}\"> �������� List
                string content = AddImagesToContent(result_GPT, result_ImgList); //resultText ���̿� resultImgList�� string���� �� �־��ָ��
                //content = $"<html><body>{content}</body></html>"; // ����� HTML �������� ǥ���մϴ�. ���� or �߰�
                string head_2 = $"<h2>{hotelName + addTitleBox1.Text + " + ���� ����Ʈ ���� ��õ ���� �ı�"}</h2>";
                LogBox1.AppendText($"�̹��� & ���� ���� ���� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // ���� ������ ��ũ ����
                LogBox1.AppendText($"���� ������ ��ũ ����..." + Environment.NewLine);
                string result_OldPostLinks = await AddOldPostAsync(); // �Ϸ�
                LogBox1.AppendText($"���� ������ ��ũ ���� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // �ܺ� ��ũ �Է�
                LogBox1.AppendText($"�ܺ� ��ũ ����..." + Environment.NewLine);
                string result_OutLinks = AddOutLinksAsync(); // �Ϸ�
                string result_OutBanners = AddOutBannersAsync(); // �Ϸ�
                LogBox1.AppendText($"�ܺ� ��ũ ���� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // �±� ���� (GPT)
                LogBox1.AppendText($"�±� ������..." + Environment.NewLine + Environment.NewLine);
                int result_TagId = await AddTagAsync(); // �Ϸ�
                LogBox1.AppendText($"�±� ���� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                // ��� ��û (GPT)
                LogBox1.AppendText($"�����..." + Environment.NewLine);
                string result_Excerpt = await AddGPTToExcerptAsync(result_GPT); // �Ϸ�
                LogBox1.AppendText($"��� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                //WP ���ε� 
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"���������� ���ε� ����" + Environment.NewLine);
                var post = new Post()
                {
                    Title = new Title(hotelName + addTitleBox1.Text + " + ���� ����Ʈ ���� ��õ ���� �ı�"), // TitleBox1.Text
                    Content = new Content(head_2 + "<p>&nbsp;</p>" + result_Excerpt + "<p>&nbsp;</p>" + result_ThumnailImg + "<p>&nbsp;</p>" + result_OutLinks + "<p>&nbsp;</p>" + result_OutBanners + "<p>&nbsp;</p>" + result_Hotel + "<p>&nbsp;</p>" + result_GoogleMap + "<p>&nbsp;</p>" + result_OutLinks + "<p>&nbsp;</p>" + result_OutBanners + "<p>&nbsp;</p>" + content + "<p>&nbsp;</p>" + result_OutLinks + "<p>&nbsp;</p>" + result_OutBanners + "<p>&nbsp;</p>" + result_OldPostLinks), // GPT
                    FeaturedMedia = result_thumbNail, // �����
                    Categories = new List<int> { result_Categories }, // ComboBox���� ������ ī�װ� ID ����
                    CommentStatus = OpenStatus.Open, // ��� ����
                    Tags = new List<int> { result_TagId },
                    Status = Status.Publish, // ������ ���� ����,�ӽ�
                    Excerpt = new Excerpt(result_Excerpt) // ����
                    //Meta = new Description("�׽�Ʈ�Դϴ�"), // ��Ÿ ������
                };
                var createdPost = await client.Posts.CreateAsync(post); // ������ ��û ������
                LogBox1.AppendText($"�� ���� ��� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"���������� ���ε� �Ϸ�" + Environment.NewLine);
                DeleteAllJpgFilesInFolder(selectedFolder);
                LogBox1.AppendText($"===========================" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
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
                    ChatMessage.FromSystem(SystemBox1.Text+"������"),
                    ChatMessage.FromUser(prompt1),
                },
                Model = Models.Gpt_3_5_Turbo_16k, //�𵨸�.
                Temperature = 0.6F,      //����� ������(�پ缺 - Diversity)). �������� ������ ���� ���, ������ �� �ƹ���?
                MaxTokens = 4000,      //�̰� ����� ���ڰ� ������. �� ��ū ������ �������� Ʈ������ �Ű�����, (�����ΰ��) ���� å���� ��)
                N = 1   //����� ��(����� ��). N=3���� �ϸ� 3�� �ٸ� ȸ���� �迭�� �����
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
        // Textbox �ε��� (Start btn�κ�)
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
