using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZB
{
    public partial class Form1 : Form
    {

        public Setting Setting { get; set; }


        public Form1()
        {
            InitializeComponent();
            this.Setting = Setting.Load();
            this.txtSmtp.DataBindings.Add("Text", this.Setting, "Smtp");
            this.txtUserName.DataBindings.Add("Text", this.Setting, "UserName");
            this.txtPassword.DataBindings.Add("Text", this.Setting, "Password");
            this.txtEmailList.DataBindings.Add("Text", this.Setting, "EmailList");
        }
       

        private void button1_Click_1(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            list.Add("https://www.szggzy.com/jyxx/jsgc/zbgg3");
            list.Add("https://www.szggzy.com/jyxx/jsgc/zbgg3_2");
            list.Add("https://www.szggzy.com/jyxx/jsgc/zbgg3_3");
            list.Add("https://www.szggzy.com/jyxx/jsgc/zbgg3_4");
            list.Add("https://www.szggzy.com/jyxx/jsgc/zbgg3_5");

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var option = new ChromeOptions();
            option.AddArguments("--headless", "--no-sandbox", "--disable-web-security", "--disable-gpu", "--incognito", "--proxy-bypass-list=*", "--proxy-server='direct://'", "--log-level=3", "--hide-scrollbars");
            IWebDriver webDriver = new OpenQA.Selenium.Chrome.ChromeDriver(chromeDriverService, option);

            List<BidInfo> bids = new List<BidInfo>();
            foreach (String url in list)
            {
                webDriver.Navigate().GoToUrl(url);
                var el = webDriver.FindElement(By.ClassName("newsList"));
                if (el != null)
                {
                    var linkList = el.FindElements(By.TagName("a"));
                   
                    foreach (var link in linkList)
                    {
                        var detailUrl = link.GetAttribute("href");
                        this.richTextBox1.AppendText(link.Text+ "\n");
                        BidInfo bidInfo = GetDetail(detailUrl);
                        if (bidInfo != null)
                        {
                            bids.Add(bidInfo);
                        }
                    }
                }

                
            }
            webDriver.Close();
            webDriver.Quit();
            SaveToExcel(bids);
        }


        private BidInfo GetDetail(string url)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("招标项目编号", "BidCode");
            map.Add("招标项目名称", "ProjectName");
            map.Add("标段名称", "SectionName");
            map.Add("项目编号", "ProjectCode");
            map.Add("公示时间", "PublicityTime");
            map.Add("招标人", "Tenderee");
            map.Add("招标代理机构", "Agency");
            map.Add("招标方式", "BiddingMethod");
            map.Add("中标人", "Bidder");
            map.Add("中标价(万元)", "BidPrice");
            map.Add("中标工期", "Period");
            map.Add("项目经理", "ProjectManager");
            map.Add("资格等级", "QualificationLevel");
            map.Add("资格证书编号", "QualificationCode");
            map.Add("是否暂定金额", "PendingAmount");
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var option = new ChromeOptions();
            option.AddArguments("--headless", "--no-sandbox", "--disable-web-security", "--disable-gpu", "--incognito", "--proxy-bypass-list=*", "--proxy-server='direct://'", "--log-level=3", "--hide-scrollbars");
            IWebDriver webDriver = new OpenQA.Selenium.Chrome.ChromeDriver(chromeDriverService, option);
            webDriver.Navigate().GoToUrl(url);
            var elContent = webDriver.FindElements(By.ClassName("ueditortable")).First();
            Type type = typeof(BidInfo);
            BidInfo bidInfo = null;
            if (elContent != null)
            {
                bidInfo = new BidInfo();
                var trList = elContent.FindElements(By.TagName("tr"));
                foreach(var tr in trList)
                {
                    var tdList = tr.FindElements(By.TagName("td"));
                    if (tdList.Count != 2)
                    {
                        continue;
                    }

                    var fieldName = tdList[0].Text.Replace("\n", "").Replace("：", "").Trim();
                    var fieldValue = tdList[1].Text.Trim();
                    var propertyName = map[fieldName];

                    PropertyInfo propertyInfo = type.GetProperty(propertyName);
                    if(propertyInfo != null)
                    {
                        propertyInfo.SetValue(bidInfo, fieldValue);
                    }                    
                }
            }

            webDriver.Quit();

            return bidInfo;
        }

        private void SaveToExcel(List<BidInfo> bidInfos)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("招标项目编号", "BidCode");
            map.Add("招标项目名称", "ProjectName");
            map.Add("标段名称", "SectionName");
            map.Add("项目编号", "ProjectCode");
            map.Add("公示时间", "PublicityTime");
            map.Add("招标人", "Tenderee");
            map.Add("招标代理机构", "Agency");
            map.Add("招标方式", "BiddingMethod");
            map.Add("中标人", "Bidder");
            map.Add("中标价(万元)", "BidPrice");
            map.Add("中标工期", "Period");
            map.Add("项目经理", "ProjectManager");
            map.Add("资格等级", "QualificationLevel");
            map.Add("资格证书编号", "QualificationCode");
            map.Add("是否暂定金额", "PendingAmount");

            HSSFWorkbook excelBook = new HSSFWorkbook();
            ISheet sheet1 = excelBook.CreateSheet("深圳中标项目列表");
            IRow headRow = sheet1.CreateRow(0);
            int colIndex = 0;
            foreach(var kv in map)
            {
                headRow.CreateCell(colIndex).SetCellValue(kv.Key);
                colIndex++;
            }
            Type type = typeof(BidInfo);
            int rowIndex = 1;
            foreach (var bidInfo in bidInfos)
            {
                var dataRow = sheet1.CreateRow(rowIndex++);
                colIndex = 0;
                foreach (var kv in map)
                {
                    PropertyInfo propertyInfo = type.GetProperty(kv.Value);
                    String value = (string) propertyInfo.GetValue(bidInfo);
                    dataRow.CreateCell(colIndex).SetCellValue(value);
                    colIndex++;
                }
            }

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
            }

            string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", DateTime.Now.ToString("深圳中标yyyy-MM-dd-HH-mm-ss") + ".xlsx");
            FileStream fileStream = new FileStream(savePath, FileMode.Create,FileAccess.Write);
            excelBook.Write(fileStream);
            //fileStream.Close();
            excelBook.Close();
            fileStream.Dispose();
            SendEmail(savePath);
            System.Diagnostics.Process.Start("Explorer", "/select," + savePath);
        }

        private void SendEmail(string fileName)
        {
            if(String.IsNullOrEmpty(Setting.EmailList) || String.IsNullOrEmpty(Setting.Smtp) || String.IsNullOrEmpty(Setting.UserName) || String.IsNullOrEmpty(Setting.Password))
            {
                return;
            }

            SmtpClient client = new SmtpClient();
            client.Host = this.Setting.Smtp;
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(Setting.UserName, Setting.Password);
            MailMessage message = new MailMessage();
            message.Subject = "深圳公共资源交易中心中标每日结果公告";
            message.To.Add(Setting.EmailList);
            message.From = new MailAddress(Setting.UserName);
            message.Body = "邮件由系统自动推送，不要回复";
            message.BodyEncoding = Encoding.UTF8;
            Attachment attachment = new Attachment(fileName);
            message.Attachments.Add(attachment);
            client.Send(message);
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            Setting.Save(this.Setting);
        }
    }
}
