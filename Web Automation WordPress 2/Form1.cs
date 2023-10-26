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
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Net.Http;
using System;
using System.Collections.Generic;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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

        private void StartBtn1_Click(object sender, EventArgs e)
        {
            SaveConfig();

            string userID = UrlBox1.Text;
            int isRegistered = CheckUserID_1(userID); // ��� ���̵� üũ
            if (isRegistered == 1)
            {
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"������ ����" + Environment.NewLine);

                WP_API_Auto();
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



        private async Task<string> GetHotelInfoAsync()
        {
            string url = HotelUrlBox1.Text;
            string combinedInfo = "";
            using (HttpClient client = new HttpClient())
            {
                // HtmlAgilityPack�� ����Ͽ� HTML �Ľ�
                string html = await client.GetStringAsync(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                // ���ϴ� ������ ����
                // ���� ���, <title> ����� ������ ����
                var names = doc.DocumentNode.SelectNodes("//h2[@class='d2fee87262 pp-header__title']");
                var infos = doc.DocumentNode.SelectNodes("//p[@class='a53cbfa6de b3efd73f69']");
                var reviewNodes = doc.DocumentNode.SelectNodes("//div[@class='a53cbfa6de b5726afd0b']"); // reviews�� ���� ��ҷ� ����
                int reviewCount = reviewNodes.Count;
                var checkinTimes = doc.DocumentNode.SelectNodes("//div[@id='checkin_policy']//p[2]");
                var checkoutTimes = doc.DocumentNode.SelectNodes("//div[@id='checkout_policy']//p[2]");

                List<string> additionalReviews = new List<string>();

                // ����� ���� ���
                try
                {
                    string name = names[0].InnerText.Trim();
                    string info = infos[0].InnerText.Trim();
                    string checkinTime = checkinTimes[0].InnerText.Trim();
                    string checkoutTime = checkoutTimes[0].InnerText.Trim();
                    for (int i = 0; i < reviewCount - 5; i++)
                    {
                        // reviewNodes���� ���� ��������
                        var reviewNode = reviewNodes[i];
                        string additionalReview = reviewNode.InnerText.Trim();

                        // ���� ��Ͽ� ����
                        additionalReviews.Add(additionalReview);
                    }
                    // containers�� �ִ� ������ ���ڿ��� ����
                    combinedInfo = $"Name: {name}\n\rInfo: {info}\n\rCheck-in Time: {checkinTime}\nCheck-out Time: {checkoutTime}\n\rReviews:\n- {string.Join("\n- ", additionalReviews)}";
                    // ��� ���
                    Console.WriteLine(combinedInfo);
                    return combinedInfo;
                }
                catch (Exception ex)
                {
                    LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
                    return combinedInfo;
                }
            }
        }










        /*===============================================*/

        // �̹��� ũ�Ѹ� - �Ϸ�
        private void Crawling_Naver()
        {
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"Line:xxx ũ�Ѹ� ����" + Environment.NewLine);

            // ũ��â ����
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            options.AddArguments("--headless"); // �������� ����
            driver = new ChromeDriver(driverService, options);
            Delay();

            //�̹��� �˻� : CCL ����� �̿밡�� 
            LogBox1.AppendText($"===========================" + Environment.NewLine);
            LogBox1.AppendText($"Line:xxx �̹��� �˻�" + Environment.NewLine);
            string baseUrl = $"https://search.naver.com/search.naver?where=image&section=image&query={TitleBox1.Text}";
            string endUrl = "&res_fr=0&res_to=0&sm=tab_opt&color=&ccl=2&nso=so%3Ar%2Ca%3Aall%2Cp%3Aall&recent=0&datetype=0&startdate=0&enddate=0&gif=0&optStr=&nso_open=1&pq=";
            driver.Navigate().GoToUrl(baseUrl + endUrl);
            Delay();
            ScrollToBottom(driver);

            // �̹��� ��Ҹ� ã�Ƽ� ó��
            var imgElements = driver.FindElements(By.CssSelector("img._image._listImage"));
            LogBox1.AppendText($"Line:xxx �� ���� ��: {imgElements.Count}��");
            LogBox1.AppendText(Environment.NewLine);

            foreach (var imgElement in imgElements) // �̹��� �ٿ�ε� ����
            {
                string imageUrl = imgElement.GetAttribute("src");
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
                        LogBox1.AppendText($"Line:xxx �ٿ�ε� ��: ({fileCount}/{imgElements.Count})" + Environment.NewLine);
                    }
                }
            }
            LogBox1.AppendText($"�̹��� ũ�Ѹ� �Ϸ�..." + Environment.NewLine);
            LogBox1.AppendText($"===========================" + Environment.NewLine);
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

        /*===============================================*/
        private string GPT_Prompt(string prompt)
        {
            /*
             1. ���׿��� ã�ư��� ��� / 2. �ܰ� / 3. ���� �� ���� / 4. �δ�ü� / 5.�������� / 6.���ڰ���
             */
            string prompt1 = $"'{prompt}'�� ���õ� ��α� ���� �ۼ��Ұž�. �� ���ø��� ���缭 ���� ��� �ڼ��ϰ� ���ٷ�? 1.���׿��� ã�ư��� ���:, 2.�ܰ�: , 3.���� ��Ÿ��: , 4.�δ�ü�: , 5.��������:  , 6.���ڰ���: ...... �׸��� ������ '�߾��' �̷������� ����";
            return prompt1;
        }

        // GPT ��� ���� content�� ����
        private async Task<string> AddGPTToContentAsync()
        {
            string result = "";
            string prompt1 = GPT_Prompt(TitleBox1.Text); // GPT Prompt ����
            try
            {
                result = await RequestGPT(prompt1); // GPT�� ��û�ϰ� ����� ����ϴ�.
            }
            catch (Exception ex)
            {
                // ���� ó�� - ���ܰ� �߻��� ��� ó��
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
            }
            string content = result.Replace("\n", "<br>") + "<br>"; // \n�� <br>�� ���� , HTML�� �ٹٲ� 
            string pattern = @"\d+\.\s*[\p{L}\d\s]+(?::)?"; // ����ǥ���� = 1. 2. 3. �� ���ڷ� �з��� ������ �۲ú����� ���� ����
            Regex regex = new Regex(pattern);
            // ã�� ������ ������ �����ϰ� ũ�� ǥ���մϴ�.
            string result_GPT = regex.Replace(content, match =>
            {
                return $"<h3>{match.Value}</h3><br>";
            });
            return result_GPT;
        }

        // �̹��� ���ε� �������
        private async Task<List<string>> ImagesAsyncList()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���
            translation = Papago(TitleBox1.Text);
            int count = 0, i = 0;
            List<string> responseImgList = new List<string>(); // �̹��� ���ε� ����� ������ ����Ʈ

            while (count != 7) // �� 7���� ������ url�� ����Ʈ
            {
                // �̹��� ���� ��� ��������
                string localImagePath = Path.Combine(selectedFolder, $"{i}.jpg");
                if (File.Exists(localImagePath))
                {
                    var createdMedia = await client.Media.CreateAsync(localImagePath, $"{translation + '_' + i}.jpg"); // localImagePath�� media({translation}.jpg) ����
                    string responseImg = $"<img src=\"{createdMedia.SourceUrl}\">"; // content_2�� createdMedia���� ��ȯ �������� img src�� ����
                    responseImgList.Add(responseImg);
                    count++;
                    i++;
                    result_thumbNail = createdMedia.Id;
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
                        result_GPT = result_GPT.Replace(imageInfo, $"\r{imageSrc}\r<br><span style='color: #FF8C00; font-size:150%; font-weight: bold;'>{imageInfo}</span>");
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
            string tags = "'" + TagBox1.Text.Trim() + "'" + "�� ������ �α� �˻��� 10���� ','�� �����ؼ� �˷���";
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
            string oldPostsLinks = string.Join("\r\n", selectedLinks); // �� ��ũ�� ���� ���ڷ� ����
            return oldPostsLinks;
        }


        //�ܺ� ��ũ ����
        private string AddOutLinksAsync()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���
            List<string> urls = new List<string> // 30���� URL�� ����Ʈ�� �߰�
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
            Random random = new Random(); // �����ϰ� 3���� Link ���� �����մϴ�.
            for (int i = 0; i < 2; i++)// �����ϰ� 2���� URL ����
            {
                int index = random.Next(urls.Count);
                string selectedLink = urls[index];
                urls.RemoveAt(index); // �ߺ� ���� ������ ���� ������ URL�� ����Ʈ���� �����մϴ�.

                // ���õ� URL�� linkHtml �������� ����ϴ�.
                string postTitle = $"��{TitleBox1.Text} ���� ������ �ۢ�"; // ���ϴ� ������ �����ϼ���
                string linkHtml = $"<a title=\"{postTitle}\" href=\"{selectedLink}\">&nbsp;{postTitle}</a>";
                selectedOutLinks.Add(linkHtml);
            }
            // ���õ� URL�� outlinks ���ڿ��� �߰�
            string addOutLinks = "�ٸ� ���� �ñ��Ͻôٸ� �Ʒ� ��ũ�� Ŭ�����ּ��� :)\r\n";
            string outLinks = string.Join("\r\n", selectedOutLinks); // �� ��ũ�� ���� ���ڷ� ����
            return addOutLinks + outLinks;
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


        private async void WP_API_Auto()
        {
            var client = new WordPressClient(WP_URL);
            client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���

            try
            {
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"���� ���� �߰�..." + Environment.NewLine);
                // TODO : ���� API �Լ�
                string result_GoogleMap = "";
                LogBox1.AppendText($"���� ���� �߰� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"ȣ�� ���� �߰�..." + Environment.NewLine);
                // TODO : ȣ�� �̹���, ����
                string result_Hotel = await GetHotelInfoAsync();
                LogBox1.AppendText($"ȣ�� ���� �߰� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"GPT ��� ����..." + Environment.NewLine);
                string result_GPT = await AddGPTToContentAsync(); // �Ϸ�
                LogBox1.AppendText($"GPT ��� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"�̹��� & ���� ���� ���� ����..." + Environment.NewLine);
                List<string> result_ImgList = await ImagesAsyncList(); // selectedFolder ���� �̹������� <img src=\"{createdMedia.SourceUrl}\"> �������� List
                string content = AddImagesToContent(result_GPT, result_ImgList); //resultText ���̿� resultImgList�� string���� �� �־��ָ��
                content = $"<html><body>{content}</body></html>"; // ����� HTML �������� ǥ���մϴ�. ���� or �߰�
                string head_2 = $"<h2>{TitleBox1.Text}</h2>";
                LogBox1.AppendText($"�̹��� & ���� ���� ���� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"���� ������ ��ũ ����..." + Environment.NewLine);
                string result_OldPostLinks = await AddOldPostAsync(); // �Ϸ�
                LogBox1.AppendText($"���� ������ ��ũ ���� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"�ܺ� ��ũ ����..." + Environment.NewLine);
                string result_OutLinks = AddOutLinksAsync(); // �Ϸ�
                LogBox1.AppendText($"�ܺ� ��ũ ���� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);


                LogBox1.AppendText($"�±� ������..." + Environment.NewLine + Environment.NewLine);
                int result_TagId = await AddTagAsync(); // �Ϸ�
                LogBox1.AppendText($"�±� ���� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);

                LogBox1.AppendText($"�����..." + Environment.NewLine);
                string result_Excerpt = await AddGPTToExcerptAsync(result_GPT); // �Ϸ�
                LogBox1.AppendText($"��� �Ϸ�..." + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);

                //WP ���ε� 
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"���������� ���ε� ����" + Environment.NewLine);
                var post = new Post()
                {
                    Title = new Title(TitleBox1.Text),
                    Content = new Content(head_2 + "<p>&nbsp;</p>" + result_Excerpt + "<p>&nbsp;</p>" + result_OutLinks + "<p>&nbsp;</p>" + result_Hotel  + "<p>&nbsp;</p>" + content + "<p>&nbsp;</p>" + result_OldPostLinks), // GPT
                    FeaturedMedia = result_thumbNail, // �����
                    Categories = new List<int> { comboBox1_SelectedItem() }, // ComboBox���� ������ ī�װ� ID ����
                    CommentStatus = OpenStatus.Open, // ��� ����
                    Tags = new List<int> { result_TagId },
                    Status = Status.Publish, // ������ ���� ����,�ӽ�
                    Excerpt = new Excerpt(result_Excerpt) // ����
                    //Meta = new Description("�׽�Ʈ�Դϴ�"), // ��Ÿ ������
                };
                var createdPost = await client.Posts.CreateAsync(post); // ������ ��û ������
                LogBox1.AppendText($"�� ���� ��� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"���������� ���ε� �Ϸ�" + Environment.NewLine);
                LogBox1.AppendText($"===========================" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
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
        /*
        public string GetIdFromName(string nameToFind)
        {
            TistoryAPI api = new TistoryAPI();
            api.SetAccessToken(AccessToken);

            // XML�� ����� ���� ���
            string result = api.GetCategory(blogName);

            // XmlDocument �ν��Ͻ� ����
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            // "id"�� "name" ���� �ҷ��ͼ� ���
            XmlNodeList categoryNodes = xmlDoc.SelectNodes("/tistory/item/categories/category");
            foreach (XmlNode categoryNode in categoryNodes)
            {
                string id = categoryNode.SelectSingleNode("id").InnerText;
                string name = categoryNode.SelectSingleNode("name").InnerText;
                if (name.Equals(nameToFind, StringComparison.OrdinalIgnoreCase)) // ��ҹ��ڸ� �����ϰ� ��ġ ���θ� Ȯ���մϴ�.
                {
                    return id; // ��ġ�ϴ� "name"�� ã���� �ش� "id" ���� ��ȯ�մϴ�.
                }
            }
            // "nameToFind"�� �ش��ϴ� "name"�� ã�� ���� ��� null �Ǵ� �ٸ� ������ ���� ��ȯ�� �� �ֽ��ϴ�.
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
        // ���ϸ� ���� ��ư Rename
        private int renameCounter = 1;
        private void RenameBtn1_Click(object sender, EventArgs e)
        {
            try
            {
                LogBox1.AppendText($"===========================" + Environment.NewLine);
                LogBox1.AppendText($"Line:xxx ���ϸ� ��ȯ ����..." + Environment.NewLine);

                if (!string.IsNullOrEmpty(selectedFolder) && Directory.Exists(selectedFolder))
                {
                    string[] files = Directory.GetFiles(selectedFolder);
                    foreach (string filePath in files)
                    {
                        string extension = Path.GetExtension(filePath);
                        // .xml ������ �����ϰ� ó��
                        if (!string.Equals(extension, ".xml", StringComparison.OrdinalIgnoreCase))
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
                    LogBox1.AppendText($"Line:xxx ���ϸ� ��ȯ �Ϸ�..." + Environment.NewLine);
                    LogBox1.AppendText($"===========================" + Environment.NewLine);
                }
            }
            catch
            {
                LogBox1.AppendText($"Line:xxx ������ �����ϴ�" + Environment.NewLine);
            }

        }







        private void button1_Click(object sender, EventArgs e)
        {
            GetHotelInfoAsync();
        }
    }












    static class Define
    {
        public const string WP_ID = "kwon92088@gmail.com";
        public const string WP_PW = "SV9H TFv0 pMC6 sptk 8s7q 7BsV";
        public const string WP_URL = "https://oheeliving.com/wp-json/";
    }
}
