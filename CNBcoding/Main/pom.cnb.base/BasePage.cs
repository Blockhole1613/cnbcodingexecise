using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using CommonComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;

namespace CNBcoding
{
    [TestClass]
    public class BasePage
    {
        public static string ReportsDir;
        public static RemoteWebDriver _driver = null;
        public TestContext TestContext { get; set; }
        public static Dictionary<string, string> BankingTypes = new Dictionary<string, string>();
        [AssemblyInitialize]
        public static void Initilaize(TestContext context)
        {

            try
            {
                Constants.Properties = LoadProperties.GetProperties(context.Properties["config"].ToString());
                ReportsDir = Directory.CreateDirectory(@"C:\Reports\cnb" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")).ToString();
                Constants.htmlReporter = new ExtentHtmlReporter(ReportsDir);
                Constants.extent.AttachReporter(Constants.htmlReporter);
                Constants.extent.AddSystemInfo("Application", Constants.Properties["application"]);
                Constants.extent.AddSystemInfo("Environment", Constants.Properties["environment"]);
                Constants.htmlReporter.LoadConfig(@"C:\Reports\Config.xml");
                Browsers.KillProcess("chromedriver");
                BankingTypes.Add("Personal", "PERSONAL BANKING");
                BankingTypes.Add("Business & Commercial", "BUSINESS BANKING");
                BankingTypes.Add("Private Banking", "PRIVATE BANKING");
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        [AssemblyCleanup]
        public static void Teardown()
        {
            try
            {
                BankingTypes.Clear();
                _driver.Quit();
                Status logstatus;
                if (Constants._testFailed == false)
                    logstatus = Status.Pass;
                else
                    logstatus = Status.Fail;
                Constants.test.Log(logstatus, "Test execution ended with " + logstatus);
                Constants.extent.Flush();
                string attachment = ReportsDir + @"\" + Constants.Properties["environment"] + "-" + Constants.Properties["application"] + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".html";
                File.Move(@"C:\Reports\index.html", attachment);
                Wait.DefaultWait(1);
                // Adding Content Table to Body of Email Message
                string[] occurances = new string[50];
                int h = 0;
                if (Constants.Errors.Count > 0)
                {
                    while (h < Constants.Errors.Count)
                    {
                        occurances[h] = "<tr style='height:15.0pt'><td width=95 nowrap style='width:100.0pt;border:solid windowtext 1.0pt;border-top:none;padding:0in 5.4pt 0in 5.4pt;height:15.0pt'><p class=MsoNormal><spanstyle='color:black'>" + Constants.Errors[h].Split('#')[0] + " &nbsp; <o:p></o:p></span></p></td><td width=95 nowrap align=center style='width:100.0pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;padding:0in 5.4pt 0in 5.4pt;height:15.0pt'><pclass=MsoNormal><span style='color:black'>" + Constants.Errors[h].Split('#')[1] + "<o:p></o:p></span></p></td></tr>";

                        if (h == 0)
                        {
                            Constants.finaloccurance = occurances[h];
                        }
                        else
                        {
                            Constants.finaloccurance = Constants.finaloccurance + occurances[h];
                        }
                        h++;
                    }
                }
                string bodytext = "<body lang=EN-US link=\"#0563C1\" vlink=\"#954F72\"><div class=WordSection1><p class=MsoPlainText><o:p>&nbsp;</o:p></p><table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=75 style='width:75.0pt;margin-left:-.4pt;border-collapse:collapse'><tr style='height:15.0pt'><td width=95 nowrap align=center style='width:75.0pt;border:solid windowtext 1.0pt;background:#A5A5A5;padding:0in 5.4pt 0in 5.4pt;height:15.0pt'><p class=MsoNormal><b><span style='color:white'>Main<o:p></o:p></span></b></p></td><td width=95 nowrap align=center style='width:71.0pt;border:solid windowtext 1.0pt;border-left:none;background:#A5A5A5;padding:0in 5.4pt 0in 5.4pt;height:15.0pt'><p class=MsoNormal><b><span style='color:white'>Child<o:p></o:p></span></b></p></td></tr>" + Constants.finaloccurance + "</table></div></body>";
                OutlookEmail.SendEmailViaOutlook(Constants.Properties["sFromAddress"], Constants.Properties["sToAddress"], Constants.Properties["sCc"], "FAILURE Count" + " - " + Constants.Failure, bodytext, attachment, Constants.Properties["sBcc"]);
                Wait.DefaultWait(2);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        [TestInitialize]

        public void OpenURL()
        {
            _driver = Browsers.Browser("Chrome");
            _driver.Url = Constants.Properties["url"]; ;
            _driver.Manage().Window.Maximize();
            Wait.DefaultWait(8);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            try
            {
                if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
                    Constants._testFailed = true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _driver.Quit();
            }


        }
    }

}

