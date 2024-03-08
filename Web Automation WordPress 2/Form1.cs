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
using OfficeOpenXml;
using Google.Apis.Services;
using Google.Apis.Translate.v2.Data;
using Google.Apis.Translate.v2;


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
			LogBox2.ScrollBars = ScrollBars.Vertical;
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}
		ChromeOptions options = new ChromeOptions();
		private IWebDriver driver;
		private string selectedFolder, translation; // Ŭ���� ���� ������ ���� ��θ� ������ ����
		private string OPENAI_API_KEY, WP_ID, WP_PW, WP_URL, Folder_Path = "";
		private string Topic, Category, WP_Title = "";
		private int global_i = 1; // �� ȣ�ں� ���ջ��� �ѹ�


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
			catch
			{
				return -1; // Error occurred
			}
		}

		private async void StartBtn1_Click(object sender, EventArgs e)
		{
			SaveConfig();

			int count = 1;
			string userID = UrlBox1.Text;
			int isRegistered = CheckUserID_1(userID); // ��� ���̵� üũ
			if (isRegistered == 1)
			{
				try
				{
					while (true)
					{
						LogBox2.AppendText($"===========================" + Environment.NewLine);
						LogBox2.AppendText($"{count}�� ������ ����" + Environment.NewLine);

						for (int i = 2; i <= 4; i++)
						{
							GetIdList(i); // ������ �� ��α� ����
							await WP_API_Auto_2();
							global_i = 1;
						}
						LogBox2.AppendText($"{count}�� ������ �Ϸ�" + Environment.NewLine);
						DateTime currentTime = DateTime.Now; // ���� �ð��� �����ͼ� ���
						LogBox2.AppendText("���ε� �ð�: " + currentTime + Environment.NewLine);
						LogBox2.AppendText($"===========================" + Environment.NewLine);

						DelayHr();
						count++;
					}
				}
				catch (Exception ex)
				{
					LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
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

		//Delay �Լ� 121��~181��
		public void DelayHr()
		{
			int min = 121;
			int max = 181;
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

		/*=================================================================================================================*/

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
			string baseUrl = $"{HotelUrl}";
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
						string basePath = Folder_Path; // �⺻ ���� ���
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
				LogBox1.AppendText($"�̹��� ���� ��й�..." + Environment.NewLine);
			}
			catch (Exception ex)
			{
				LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
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

				if (!string.IsNullOrEmpty(Folder_Path) && Directory.Exists(Folder_Path))
				{
					string[] files = Directory.GetFiles(Folder_Path);
					Random rng = new Random();
					files = files.OrderBy(x => rng.Next()).ToArray(); // ���� �迭 ����ȭ
					foreach (string filePath in files)
					{
						string extension = Path.GetExtension(filePath);
						// .xml ������ �����ϰ� ó��
						if (string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase))
						{
							string newFileName = $"{renameCounter}{extension}";
							string newFilePath = Path.Combine(Folder_Path, newFileName);
							// �̹� �����ϴ� ������ ��� renameCounter�� ������Ű�� ���ο� ���� ��� ����
							while (File.Exists(newFilePath))
							{
								renameCounter++;
								newFileName = $"{renameCounter}{extension}";
								newFilePath = Path.Combine(Folder_Path, newFileName);
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

		/*=================================================================================================================*/

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
					catch
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

		/*=================================================================================================================*/

		// ���� ���� �� ID ����Ʈ ���� (������ �� ��α� ����)
		private void GetIdList(int i)
		{
			string excelFilePath = Path.Combine(selectedFolder, "HOTEL_ID_LIST.xlsx");
			try
			{
				// ExcelPackage�� ����Ͽ� ���� ���� ����
				using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
				{
					// ��ũ��Ʈ ���� (0�� ù ��° ��ũ��Ʈ�� �ǹ�)
					ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

					// 2�� ���� ������ �б� [��,��]
					OPENAI_API_KEY = worksheet.Cells[i, 1].Text;
					WP_ID = worksheet.Cells[i, 2].Text;
					WP_PW = worksheet.Cells[i, 3].Text;
					WP_URL = worksheet.Cells[i, 4].Text;
					Folder_Path = worksheet.Cells[i, 5].Text;
					Topic = worksheet.Cells[i, 6].Text;
					Category = worksheet.Cells[i, 7].Text;
					WP_Title = worksheet.Cells[i, 8].Text;

					if (WP_URL != "" || WP_URL != null)
					{
						LogBox2.AppendText($"������: {WP_URL}" + Environment.NewLine);
					}
				}
			}
			catch (Exception ex)
			{
				LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
			}
		}

		// ��α׿��� ������ �� ù��° ������ ����
		private void GetHotelList(string folderpath)
		{
			// ���� ���� ���
			string excelFilePath = Path.Combine(folderpath, "HotelList.xlsx");
			try
			{
				// ExcelPackage�� ����Ͽ� ���� ���� ����
				using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
				{
					// ��ũ��Ʈ ���� (0�� ù ��° ��ũ��Ʈ�� �ǹ�)
					ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

					// 1�� ���� ������ �б�
					string name = worksheet.Cells[1, 1].Text;
					string url = worksheet.Cells[1, 2].Text;
					if (name != "" || name != null)
					{
						HotelUrl = url;
						LogBox1.AppendText($"���õ� ������: {name}" + Environment.NewLine);
					}
					else { HotelUrl = ""; }
				}
			}
			catch (Exception ex)
			{
				LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
			}
		}

		// ���� ���� �� ȣ�ڸ���Ʈ ù��° ������ ����
		private void DeleteHotelList(string folderpath)
		{
			// ���� ���� ���
			string excelFilePath = Path.Combine(folderpath, "HotelList.xlsx");

			try
			{
				// ExcelPackage�� ����Ͽ� ���� ���� ����
				using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
				{
					// ��ũ��Ʈ ���� (0�� ù ��° ��ũ��Ʈ�� �ǹ�)
					ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

					// 1�� ���� ������ �б�
					string name = worksheet.Cells[1, 1].Text;
					if (name != "" || name != null)
					{
						// 1�� �� ����
						worksheet.DeleteRow(1, 1);
						// ����� ������ ����
						package.Save();
						// �ʿ信 ���� �޽��� �ڽ� ���� ���� ����ڿ��� �˸�
						LogBox1.AppendText($"���� ���� ����" + Environment.NewLine);
					}
				}
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
				LogBox1.AppendText($"���� ���� ����" + Environment.NewLine);
			}
			catch (Exception ex)
			{
				LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
			}
		}

		/*===============================================*/

		// ȣ�� ���� ����
		private async Task<string> GetHotelInfoAsync()
		{
			string url = HotelUrl; // url�� .html �տ� ko�� ������ ����
			if (!url.Contains(".ko."))
			{
				// ���� ǥ������ ����Ͽ� ".html" �տ� "ko"�� ���� ��� "ko.html"�� �߰�
				url = Regex.Replace(url, @"(?<!ko)\.html", ".ko.html");
			}
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
				var Amenities = doc.DocumentNode.SelectNodes("//ul[@class ='c807d72881 d1a624a1cc e10711a42e']"); // TODO: ���ǽü��ε�.. ��� �߰� �ؾ�����?

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
					// reviewCount�� 6 �̸��� ��� reviewCount��ŭ ���
					int reviewCount = reviewNodes.Count;
					int maxReviewsToDisplay = Math.Min(7, reviewCount);

					if (reviewCount < 7)
					{
						for (int i = 0; i < maxReviewsToDisplay; i++)
						{
							// reviewNodes���� ���� ��������
							var reviewNode = reviewNodes[i];
							string additionalReview = reviewNode.InnerText.Trim();
							translation = Google_Trans(additionalReview, "ko"); // $"#{i} " +

							// ���� ��Ͽ� ����
							additionalReviews.Add(translation);
						}
					}
					else
					{
						for (int i = 0; i < 7; i++)
						{
							// reviewNodes���� ���� ��������
							var reviewNode = reviewNodes[i];
							string additionalReview = reviewNode.InnerText.Trim();
							translation = Google_Trans(additionalReview, "ko");

							// ���� ��Ͽ� ����
							additionalReviews.Add(translation);
						}
					}
				}

				// ����� ���� ���
				try
				{
					hotelName = names[0].InnerText.Trim();
					string combinedInfo = $"<h2>{hotelName}: {WP_Title} ������ 5���� ���Ÿ� ȣ�� ��õ</h2><br>\n";
					string info = Google_Trans(infos[0].InnerText.Trim(), "ko"); // ���� ������ �ѱ۷� ����ȭ
					string checkinTime = checkinTimes[0].InnerText.Trim();
					string checkoutTime = checkoutTimes[0].InnerText.Trim();
					string point = "���� ����";
					if (points != null) { point = points[0].InnerText.Trim(); }

					// containers�� �ִ� ������ ���ڿ��� ����
					combinedInfo += $"ȣ���� ������ ����: {info}<br>\n���� ����: {point}<br>\n�Խ� / ��� �ð�: {checkinTime + " / " + checkoutTime}<br>\n���� ����:\n- {string.Join("<p>&nbsp;</p>- ", additionalReviews)}";
					string[] keywords = { "ȣ���� ������ ����:", "���� ����:", "�Խ� / ��� �ð�:", "���� ����:" };
					foreach (var keyword in keywords)
					{
						combinedInfo = Regex.Replace(combinedInfo, keyword, $"<h3>{keyword}</h3>");
					}

					// ����� �ٲ� translation�� ȣ�ڸ����� ����
					string shortenedName = hotelName;
					if (shortenedName.Length > 15)
					{
						shortenedName = hotelName.Substring(0, Math.Min(15, hotelName.Length)); // ȣ�ڸ��� �ʹ� �涧 �ִ� 15�ڱ��� �ڸ����� ��
					}
                    translation = Google_Trans(shortenedName + " Thumnail", "ko"); // $"#{i} " +

					// ��� ���
					Console.WriteLine(combinedInfo);

					// ����� ����
					HtmlNode imgNode = doc.DocumentNode.SelectSingleNode("//a[@class='bh-photo-grid-item bh-photo-grid-photo1 active-image ']/img");
					if (imgNode != null)
					{
						string imageUrl = imgNode.GetAttributeValue("src", "");
						string basePath = Folder_Path; // �⺻ ���� ���� ���
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
									string text = WP_Title + "\n" + "����Ʈ ȣ�� ��õ" + "\n" + "TOP 5";
									Font font = new Font("NPSfont_regular", 55, FontStyle.Bold);

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

								string outputPath = Path.Combine(basePath, "EditedThum_1.png");
								image.Save(outputPath);
								image.Dispose();
							}
						}
						catch (Exception ex)
						{
							LogBox1.AppendText($"����� ���� ���� �߻�: {ex.Message}" + Environment.NewLine);
							throw;
						}
					}

					return combinedInfo;
				}
				catch (Exception ex)
				{
					LogBox1.AppendText($"ȣ�� ���� ���� ���� �߻�: {ex.Message}" + Environment.NewLine);
					throw;
				}
			}
		}


		// ����� ��� (WP)
		private async Task<string> ThumnailAsync_WP()
		{
			var client = new WordPressClient(WP_URL);
			client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���
			string responseImg = "";
			try
			{
				string localThumnailPath = Path.Combine(Folder_Path, $"EditedThum_1.png"); // �̹��� ���� ��� ��������
				var createdThumMedia = await client.Media.CreateAsync(localThumnailPath, $"{translation}.png"); // localImagePath�� media({translation}.jpg) ����
				responseImg = $"<img class=\"aligncenter\" src=\"{createdThumMedia.SourceUrl}\">"; // createdMedia���� ��ȯ �������� img src�� ����
				result_thumbNail = createdThumMedia.Id;
			}
			catch (Exception ex)
			{
				// ���� ó�� - ���ܰ� �߻��� ��� ó��
				LogBox1.AppendText($"����� ��� ���� �߻�: {ex.Message}" + Environment.NewLine);
				throw;
			}
			return responseImg; // �̹��� ���ε� ����� ����Ʈ�� ��ȯ
		}


		// ���۸�
		static Task<string> google_map(string apiKey)
		{
			// HTML ���ڿ��� �����մϴ�.
			string maphtml =
				$@"
				<!DOCTYPE html>
				<html>
				<head>
					<title>Simple Google Map</title>
				</head>
				<body>
					<iframe
						width=100%
						height=450
						frameborder=0 style=border:0
						src=https://www.google.com/maps/embed/v1/place?key={apiKey}&q={lat},{lng}
						allowfullscreen>
					</iframe>
				</body>
				</html>
				";
			maphtml = $"<h3>���� ��ġ</h3><br><!-- wp:html -->{maphtml}<!-- /wp:html -->";
			lat = null;
			lng = null;
			return Task.FromResult(maphtml);
		}


		// �ٿ�ε� �� �̹��� ���� (WP)
		private async Task<List<string>> ImagesAsyncList_WP()
		{
			var client = new WordPressClient(WP_URL);
			client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���

			List<string> responseImgList = new List<string>(); // �̹��� ���ε� ����� ������ ����Ʈ
			int startNumber = 1; // �̹��� ���� �̸��� ����� ���� ����
			int maxImages = 4; // �� �̹��� ���� ����
			string[] imagePaths = new string[maxImages];
			string basePath = Folder_Path; // �⺻ ���� ���� ���
			LogBox1.AppendText("�̹����� Debug#0-1" + Environment.NewLine);

			for (int i = 0; i < maxImages; i++)
			{
				LogBox1.AppendText("�̹����� Debug#0-2" + Environment.NewLine);

				string imagePath;
				do
				{
					imagePath = Path.Combine(basePath, $"{startNumber}.jpg");
					startNumber++;
					if (startNumber > 100) break;
					LogBox1.AppendText("�̹����� Debug#0-3" + Environment.NewLine);
				} while (!File.Exists(imagePath));
				LogBox1.AppendText("�̹����� Debug#0-4" + Environment.NewLine);
				imagePaths[i] = imagePath;
				LogBox1.AppendText("�̹����� Debug#0-5" + Environment.NewLine);
			}
			LogBox1.AppendText("�̹����� Debug#1" + Environment.NewLine);

			// �� �̹����� ũ�⸦ Ȯ���ϰ� ��ü �̹����� ũ�⸦ �����մϴ�.
			int width = 0, height = 0;
			string outputPath = "";
			foreach (var path in imagePaths)
			{
				using (var image = Image.FromFile(path))
				{
					width = Math.Max(width, image.Width);
					height = Math.Max(height, image.Height);
				}
			}
			LogBox1.AppendText("�̹����� Debug#2" + Environment.NewLine);
			// �� �̹����� ����� �� �̹����� �̿� �����մϴ�.
			using (var newImage = new Bitmap(width * 2, height * 2))
			using (var graphics = Graphics.FromImage(newImage))
			{
				graphics.Clear(Color.White); // ���� ����

				for (int i = 0; i < imagePaths.Length; i++)
				{
					using (var image = Image.FromFile(imagePaths[i]))
					{
						int x = (i % 2) * width;
						int y = (i / 2) * height;
						graphics.DrawImage(image, new Rectangle(x, y, width, height));
					}
				}
				LogBox1.AppendText("�̹����� Debug#3" + Environment.NewLine);
				// �Ͼ�� ���ڼ� �׸���
				Pen whitePen = new Pen(Color.White, 10); // �� ���� (�Ͼ��, �β� 10)

				graphics.DrawLine(whitePen, width, 0, width, newImage.Height); // ������
				graphics.DrawLine(whitePen, 0, height, newImage.Width, height); // ����
				LogBox1.AppendText("�̹����� Debug#4" + Environment.NewLine);

				outputPath = Path.Combine(basePath, $"{translation + '_' + global_i}.jpg");
				newImage.Save(outputPath);
				LogBox1.AppendText("�̹����� Debug#5" + Environment.NewLine);
				newImage.Dispose();
				LogBox1.AppendText("�̹����� Debug#6" + Environment.NewLine);
			}
			//WP �̵�� ���ε� �۾�
			var createdMedia = await client.Media.CreateAsync(outputPath, $"{translation + '_' + global_i}.jpg"); // localImagePath�� media({translation}.jpg) ����
			string responseImg = $"<img class=\"aligncenter\" src=\"{createdMedia.SourceUrl}\">"; // createdMedia���� ��ȯ �������� img src�� ����
			responseImgList.Add(responseImg);
			global_i++;
			LogBox1.AppendText("�̹����� ���������� ���յǾ����ϴ�." + Environment.NewLine);
			return responseImgList; // �̹��� ���ε� ����� ����Ʈ�� ��ȯ
		}


		// {hotelName}: {WP_Title} ������ 5���� ���Ÿ� ȣ�� ��õ �Ʒ��� ���� �߰� (<h2>��<h3>����)
		private string AddImagesToContent(string result_Text, List<string> result_ImgList)
		{
			string pattern = @"<h3>.*?</h3>";

			// result_Text ���ڿ����� ���� ǥ���� ���ϰ� ��ġ�ϴ� �κ��� ����
			MatchCollection matches = Regex.Matches(result_Text, pattern);

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
						result_Text = result_Text.Replace(imageInfo, $"{imageSrc}\r<br>{imageInfo}");
						result_ImgList.RemoveAt(0); // ����� �̹��� URL�� ����Ʈ���� ����
					}
				}
			}
			return result_Text;
		}


		//�ܺ� ��ũ ����
		private string AddOutLinksAsync()
		{
			string addOutLinks = "<h3>���� �ڼ��� �������� ����</h3>\r\n";
			string addOutLinks_2 = "<b>���� ȯ�� ���� ���� !</b>";
			string outLinks = ""; // �� ��ũ�� ���� ���ڷ� ����

			try
			{
				//AffiliateBox1
				List<string> urls = new List<string> // 5���� URL�� ����Ʈ�� �߰�
                {// ������� �ư��, ȣ�ڽ�����, �ͽ��ǵ��, ȣ�ڽ��Ĺ���, Ʈ����������
                    "https://linkmoa.kr/click.php?m=agoda&a=A100688386&l=0000", // �ư��
                    "https://linkmoa.kr/click.php?m=hotelskr&a=A100688386&l=0000", // ȣ�ڽ�����
                    "https://linkmoa.kr/click.php?m=expedia&a=A100688386&l=0000", // �ͽ��ǵ��
                    "https://linkmoa.kr/click.php?m=hcombine&a=A100688386&l=0000", // ȣ�ڽ��Ĺ���
                    "https://linkmoa.kr/click.php?m=tripadviso&a=A100688386&l=0000", // Ʈ����������
                };
				// ���õ� URL�� linkHtml �������� ����ϴ�.
				string agodaLink = $"���ư�٢�"; // ���� ����Ʈ
				string hotelscomLink = $"��ȣ�ڽ����Ģ�"; // ���� ����Ʈ
				string expediaLink = $"���ͽ��ǵ�Ƣ�"; // �������Ʈ
				string hotelcombineLink = $"��ȣ�ڽ��Ĺ��ο��� ������ �񱳢�"; // ���� ��
				string tripadvLink = $"��Ʈ�������������� ������ �񱳢�"; // ���� ��

				string agodaHtml = $"<a href='{urls[0]}' class='custom-up-button'>{agodaLink}</a>&nbsp;&nbsp;&nbsp;";
				string hotelscomHtml = $"<a href='{urls[1]}' class='custom-up-button'>{hotelscomLink}</a>&nbsp;&nbsp;&nbsp;";
				string expediaHtml = $"<a href='{urls[2]}' class='custom-up-button'>{expediaLink}</a>&nbsp;&nbsp;&nbsp;<p>&nbsp;</p>";
				string hotelcombineHtml = $"<a href='{urls[3]}' class='custom-down-button'>{hotelcombineLink}</a><p>&nbsp;&nbsp;&nbsp;</p>";
				string tripadvHtml = $"<a href='{urls[4]}' class='custom-down-button'>{tripadvLink}</a>&nbsp;&nbsp;&nbsp;<p>&nbsp;</p>";

				// ���õ� URL�� outlinks ���ڿ��� �߰�
				string[] selectedOutLinks = new string[] { agodaHtml, hotelscomHtml, expediaHtml, hotelcombineHtml, tripadvHtml };
				outLinks = string.Join("\r\n", selectedOutLinks); // �� ��ũ�� ���� ���ڷ� ����
			}
			catch (Exception ex)
			{
				LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
			}
			string comment = "���� eSIM ���θ�� ���� ����: �̹��� Ŭ���� �̵� ����\r\n";
			string url = "<a target=\"_blank\" href=\"http://click.linkprice.com/click.php?m=airalo&a=A100688386&l=lnVz&u_id=\"><img src=\"https://img.linkprice.com/files/glink/airalo/20230406/e00xVgcWzw680_320_100.png\" border=\"0\" width=\"320\" height=\"100\"></a> <img src=\"http://track.linkprice.com/lpshow.php?m_id=airalo&a_id=A100688386&p_id=0000&l_id=lnVz&l_cd1=2&l_cd2=0\" width=\"1\" height=\"1\" border=\"0\" nosave style=\"display:none\">";
			string link = $"<!-- wp:html -->{url}<!-- /wp:html -->";

			return "<div style='text-align: center;'>" + addOutLinks + "<br>" + addOutLinks_2 + "<p>&nbsp;</p>" + outLinks + comment + "<br>" + link + "</div>";
		}


		// GPT�� Tag ��� (WP)
		private async Task<int> AddTagAsync_WP()
		{
			var client = new WordPressClient(WP_URL);
			client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���
			string tags = "'" + hotelName + WP_Title +" ȣ�� ��õ" + "'" + "�� ������ �α� �˻��� 4���� ','�� �����ؼ� �˷���";
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


		// GPT ��� ���� content�� ����
		private string GPT_Prompt(string prompt)
		{
			string prompt1 = $"'{prompt}'�� ���õ� ��α� ���� �ۼ��Ұž�. {WP_Title}�� � ������ ª�� 3������ 1. 2. 3. �̷��� ���ڷ� �з��ؼ� �˷���";
			return prompt1;
		}
		private async Task<string> AddGPTToContentAsync()
		{
			string result = "";
			string prompt1 = GPT_Prompt(WP_Title + " ȣ�� ��õ"); // GPT Prompt ����
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
				return $"<br>{match.Value}";
			});
			return result_GPT;
		}


		// GPT�� ȣ�� ���� ���� 
		private async Task<string> AddGPTToCommentAsync()
		{
			string result = "";
			string prompt1 = WP_Title + " �� �ִ� ���Ҹ� �̿������� ������ ª�� 3������ 1. 2. 3. �̷��� ���ڷ� �з��ؼ� �˷���"; // GPT Prompt ����
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
				return $"<br>{match.Value}";
			});
			return $"<h2>{WP_Title} ���ҿ� ���� �������� ����</h2>" + "<p>&nbsp;</p>" + result_GPT;
		}


		// ���� ������ ��ũ ���� �ż��� (WP)
		private async Task<string> AddOldPostAsync_WP()
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
				selectedLinks.Add(selectedLink + "<br>");
				postLinks.RemoveAt(index); // �ߺ� ���� ������ ���� ������ Link ���� ����Ʈ���� �����մϴ�.
			}
			// ���õ� Link ���� oldposts ���ڿ��� �߰��մϴ�.
			oldPostsLinks = string.Join("\r\n", selectedLinks); // �� ��ũ�� ���� ���ڷ� ����

			return addOldPostLinks + "<br>" + oldPostsLinks;
		}


		// ī�װ� �з� �ż��� (WP)
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
				if (Category == categoryName) return categoryId;
			}
			//var categories = await client.Categories.GetByIDAsync(CategoryBox1.Text);
			return 1;
		}


		public async Task WP_API_Auto_2()
		{
			var client = new WordPressClient(WP_URL);
			client.Auth.UseBasicAuth(WP_ID, WP_PW); // ���̵� ���

			try
			{
				string separator = "<hr class=\"wp-block-separator has-alpha-channel-opacity\">";
				string head_2 = $"<h2>{WP_Title + " ����Ʈ ����Ʈ ���� TOP 5 | �����ڵ�"}</h2>";
				string mergeContent = ""; // �� ȣ�ں� �Խñ� ����(5ȸ)
				int count = 0;
				while (count < 5)
				{
					try
					{
						LogBox2.AppendText($"({count+1}/5) ������" + Environment.NewLine);

						// ��α׿� ������ �� ȣ�� ����
						LogBox1.AppendText($"===========================" + Environment.NewLine);
						LogBox1.AppendText($"ȣ�� ����..." + Environment.NewLine);
						GetHotelList(Folder_Path);
						if (HotelUrl == "") break; // ���� ����
						LogBox1.AppendText($"ȣ�� ���� �Ϸ�..." + Environment.NewLine);
						LogBox1.AppendText($"===========================" + Environment.NewLine);


						// ȣ�� ����
						LogBox1.AppendText($"ȣ�� ���� �߰�..." + Environment.NewLine);
						string result_Hotel = await GetHotelInfoAsync();
						LogBox1.AppendText($"ȣ�� �߰� �Ϸ�..." + Environment.NewLine);
						LogBox1.AppendText($"===========================" + Environment.NewLine);


						// �̹��� ũ�Ѹ�
						LogBox1.AppendText($"�̹��� ũ�Ѹ� ����..." + Environment.NewLine);
						Auto_Crawling();
						LogBox1.AppendText($"�̹��� ũ�Ѹ� �Ϸ�..." + Environment.NewLine);
						LogBox1.AppendText($"===========================" + Environment.NewLine);


						// ���� ����
						LogBox1.AppendText($"���� ���� �߰�..." + Environment.NewLine);
						string result_GoogleMap = await google_map(googleApiBox1.Text);
						LogBox1.AppendText($"���� ���� �߰� �Ϸ�..." + Environment.NewLine);
						LogBox1.AppendText($"===========================" + Environment.NewLine);


						// ���� ���� : ȣ�ڸ� + �������� + ȣ������
						LogBox1.AppendText($"�̹��� & ���� ���� ���� ����..." + Environment.NewLine);
						List<string> result_ImgList = await ImagesAsyncList_WP(); // Folder_Path ���� �̹������� <img src=\"{createdMedia.SourceUrl}\"> �������� List
						string content = AddImagesToContent(result_Hotel, result_ImgList); //resultText ���̿� resultImgList�� string���� �� �־��ָ��
						LogBox1.AppendText($"�̹��� & ���� ���� ���� �Ϸ�..." + Environment.NewLine);
						LogBox1.AppendText($"===========================" + Environment.NewLine);

						// �ܺ� ��ũ �Է�
						LogBox1.AppendText($"�ܺ� ��ũ ����..." + Environment.NewLine);
						string result_OutLinks = AddOutLinksAsync(); // �Ϸ�
						LogBox1.AppendText($"�ܺ� ��ũ ���� �Ϸ�" + Environment.NewLine);
						LogBox1.AppendText($"===========================" + Environment.NewLine);

						mergeContent += content + "<p>&nbsp;</p>" + result_GoogleMap + "<p>&nbsp;</p>"
										+ result_OutLinks + "<p>&nbsp;</p>" + separator + "<p>&nbsp;</p>";

						count++;// while�� 5ȸ �ݺ�
					}
					catch (Exception ex)
					{
						LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
						LogBox1.AppendText($"���� ȣ�ڷ� �Ѿ�ϴ�" + Environment.NewLine);
					}
					finally
					{
						LogBox1.AppendText($"���õ� ȣ�� & ���� ����..." + Environment.NewLine);
						DeleteHotelList(Folder_Path);  // ���� ù��° �� ����
						DeleteAllJpgFilesInFolder(Folder_Path); // ���� �� ���� ���� (.jpg)
						LogBox1.AppendText($"���õ� ȣ�� & ���� ���� �Ϸ�..." + Environment.NewLine);
						LogBox1.AppendText($"===========================" + Environment.NewLine);
					}
				}

				// Tag�� ������ �� ���� ���� try-catch-finally�� �з�
				int result_TagId=500;
				try
				{
					// �±� ���� (GPT)
					LogBox1.AppendText($"�±� ������..." + Environment.NewLine + Environment.NewLine);
					result_TagId = await AddTagAsync_WP(); // �Ϸ�
					LogBox1.AppendText($"�±� ���� �Ϸ�..." + Environment.NewLine);
					LogBox1.AppendText($"===========================" + Environment.NewLine);
				}
				catch (Exception ex)
				{
					LogBox1.AppendText($"Tag ���� �߻�: {ex.Message}" + Environment.NewLine);
				}
				finally
				{
					LogBox1.AppendText($"ȣ�� ����� ���..." + Environment.NewLine);
					string result_ThumnailImg = await ThumnailAsync_WP(); // ����� ���id �� img src (.png)

					// GPT ���� + ���� �ǰ� ���
					LogBox1.AppendText($"GPT ���� + ���� �ǰ� ��� ����..." + Environment.NewLine);
					string result_GPT = await AddGPTToContentAsync();
					string result_Comment = await AddGPTToCommentAsync();
					LogBox1.AppendText($"GPT ���� + ���� �ǰ� ��� �Ϸ�..." + Environment.NewLine);
					LogBox1.AppendText($"===========================" + Environment.NewLine);

					// ���� ������ ��ũ ����
					LogBox1.AppendText($"���� ������ ��ũ ����..." + Environment.NewLine);
					string result_OldPostLinks = await AddOldPostAsync_WP(); // �Ϸ�
					LogBox1.AppendText($"���� ������ ��ũ ���� �Ϸ�" + Environment.NewLine);
					LogBox1.AppendText($"===========================" + Environment.NewLine);

					// ī�װ� ���
					LogBox1.AppendText($"ī�װ� �з� ����..." + Environment.NewLine);
					int result_Categories = await AddCategoriesAsync(); // �Ϸ�
					LogBox1.AppendText($"ī�װ� �з� �Ϸ�" + Environment.NewLine);
					LogBox1.AppendText($"===========================" + Environment.NewLine);

					//WP ���ε� 
					LogBox1.AppendText($"===========================" + Environment.NewLine);
					LogBox1.AppendText($"���������� ���ε� ����" + Environment.NewLine);
					var post = new Post()
					{
						Title = new Title(WP_Title + " ���� ��õ ����Ʈ 5 |" + WP_Title + "ȣ�� ���� |" + WP_Title + "������ ȣ�� |" + hotelName), // TitleBox1.Text
						Content = new Content(head_2 + "<p>&nbsp;</p>" + result_ThumnailImg + "<p>&nbsp;</p>" + result_GPT + separator
											  + "<p>&nbsp;</p>" + mergeContent + "<p>&nbsp;</p>"
											  + result_OldPostLinks + "<p>&nbsp;</p>" + result_Comment), // GPT
						FeaturedMedia = result_thumbNail, // �����
						Categories = new List<int> { result_Categories }, // ComboBox���� ������ ī�װ� ID ����
						CommentStatus = OpenStatus.Open, // ��� ����
						Tags = new List<int> { result_TagId },
						Status = Status.Publish, // ������ ���� ����,�ӽ�
						Excerpt = new Excerpt(result_GPT) // ����
														  //Meta = new Description("�׽�Ʈ�Դϴ�"), // ��Ÿ ������
					};
					var createdPost = await client.Posts.CreateAsync(post); // ������ ��û ������
					LogBox1.AppendText($"�� ���� ��� �Ϸ�" + Environment.NewLine);
					LogBox1.AppendText($"���������� ���ε� �Ϸ�" + Environment.NewLine);
					LogBox1.AppendText($"===========================" + Environment.NewLine);
				}
			}
			catch (Exception ex)
			{
				LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
			}
		}














		/*=============================================================================================================================================*/

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
					ChatMessage.FromSystem(Topic),
					ChatMessage.FromUser(prompt1),
				},
				Model = Models.Gpt_3_5_Turbo_16k, //�𵨸�.
				Temperature = 0.6F,      //����� ������(�پ缺 - Diversity)). �������� ������ ���� ���, ������ �� �ƹ���?
				MaxTokens = 12000,      //�̰� ����� ���ڰ� ������. �� ��ū ������ �������� Ʈ������ �Ű�����, (�����ΰ��) ���� å���� ��)
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

		// Source language�� �ڵ� ������ �ȵǼ� ��Ȯ�Ҷ��� ����ؾ� �ҵ�
		private string? Papago(string prompt2)
		{
			string translatedText = "";

			try
			{
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

				// ���� ������ �������� (�������) // ���⿡ ������ �����
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
			catch (Exception ex)
			{
				LogBox1.AppendText($"���� �߻�: {ex.Message}" + Environment.NewLine);
				return null;
			}
		}

		private string Google_Trans(string prompt2, string targetlang)
		{
			TranslateService service = new TranslateService(new BaseClientService.Initializer()
			{
				ApiKey = $"{googleApiBox1.Text}", // ���⸸ �ٲٸ� ��.
				ApplicationName = " "
			});

			string Original_string = prompt2; // �ش� �� ���� ��û
			string detectionLanguange = "";
			string translatedText = ""; //������ �ؽ�Ʈ

			try //���� ��û , ��� �ڵ� ���� ���� ��
			{
				// ��� ����
				var detectionResponse = service.Detections.List(Original_string).Execute();
				detectionLanguange = detectionResponse.Detections[0][0].Language;

				// ������ �� �ѱ�� �ƴ� ��� ���� ����
				if (detectionLanguange != "ko")
				{
					TranslationsListResponse response = service.Translations.List(Original_string, targetlang).Execute();
					translatedText = response.Translations[0].TranslatedText;
					return translatedText;
				}
				return Original_string;
			}
			catch (Exception ex)
			{
				LogBox1.AppendText($"���� �߻� #2: {ex.Message}" + Environment.NewLine);
				return null;
			}
		}







		private string hotelName = "";
		private string HotelUrl = "";
		private static string lat = "";
		private static string lng = "";

		private void SaveConfig()
		{
			string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string configFile = Path.Combine(myDocumentsPath, "WP_Post_config.xml");

			XDocument doc = new XDocument(new XElement("Settings",
				new XElement("InputValue6", googleApiBox1.Text),
				new XElement("InputValue7", UrlBox1.Text)));
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
				googleApiBox1.Text = doc.Root.Element("InputValue6")?.Value;
				UrlBox1.Text = doc.Root.Element("InputValue7")?.Value;
			}
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
}
