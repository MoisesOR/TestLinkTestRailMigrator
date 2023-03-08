using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace TestLinkTestRailMigrator.Main.Pages
{
    public class TestLinkPage
    {
        private Driver.WebDriver Driver { get; set; }
        public enum FrameEnum
        {
            Title,
            Main,
            Tree,
            Work
        }
        #region Elements
        private IWebElement TlLogin => Driver.FindElementById("tl_login");
        private IWebElement TlPassword => Driver.FindElementById("tl_password");
        private IWebElement TlLoginButton => Driver.FindElementById("tl_login_button");
        private IWebElement ProjectSelector => Driver.FindElementByCssSelector("body>div.menu_bar>div>form>select");
        private IWebElement TestSpecification => Driver.FindElementByXpath("//a[img[contains(@title,'Test Specification')]]");
        private IWebElement ActionsButton => Driver.FindElementByCssSelector("img[title='Actions']");
        private IWebElement ExportFileName => Driver.FindElementByCssSelector("input[name='export_filename']");
        private IWebElement ExportButton => Driver.FindElementByCssSelector("input[value='Export']");
        private IList<IWebElement> TestSuits => Driver.FindElementsByCssSelector("ul.x-tree-node-ct[style='']>li span[title]");
        private IWebElement ExportTestSuitBtn => Driver.FindElementByCssSelector("form[action*='containerEdit'] img[title='Export']");
        #endregion
        public TestLinkPage(Driver.WebDriver driver)
        {
            Driver = driver;
        }
        public void GoToFrame(FrameEnum frameEnum)
        {
            Driver.GetDriver.SwitchTo().DefaultContent();
            switch (frameEnum)
            {
                case FrameEnum.Title:
                    SelectFrame("titlebar");
                    break;
                case FrameEnum.Main:
                    SelectFrame("mainframe");
                    break;
                case FrameEnum.Tree:
                    SelectFrame("mainframe");
                    SelectFrame("treeframe");
                    break;
                case FrameEnum.Work:
                    SelectFrame("mainframe");
                    SelectFrame("workframe");
                    break;
                default:
                    break;
            }
        }
        public void SelectFrame(string frame)
        {
            Driver.GetDriver.SwitchTo().Frame(frame);
        }
        public void AccessTestlinkPage(string Url)
        {
            Driver.GetDriver.Url = Url;
        }
        public void Login(string user, string password)
        {
            TlLogin.SendKeys(user);
            TlPassword.SendKeys(password);
            TlLoginButton.Click();
        }
        public void SelectProject(string project)
        {
            GoToFrame(FrameEnum.Title);
            SelectElement drpProject = new SelectElement(ProjectSelector);
            drpProject.SelectByText(project);
        }
        public void AccessTestSpecification()
        {
            GoToFrame(FrameEnum.Title);
            TestSpecification.Click();
        }
        public void ObtainFolders()
        {
            GoToFrame(FrameEnum.Tree);
            foreach (IWebElement suits in TestSuits)
            {
                GoToFrame(FrameEnum.Tree);
                string suitName = CleanFolderName(suits.Text);
                suits.Click();
                GoToFrame(FrameEnum.Work);
                ActionsButton.Click();
                ExportTestSuitBtn.Click();
                ExportFileName.Clear();
                ExportFileName.SendKeys($"{suitName}.xml");
                ExportButton.Click();
            }
        }
        public string CleanFolderName(string fileName)
        {
            fileName = fileName.Substring(0, fileName.IndexOf(" ("));
            fileName = fileName.Replace(' ', '-');
            return fileName;
        }
    }
}