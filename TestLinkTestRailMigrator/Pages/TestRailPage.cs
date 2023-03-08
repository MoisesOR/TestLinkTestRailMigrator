using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestLinkTestRailMigrator.Main.Utils;

namespace TestLinkTestRailMigrator.Main.Pages
{
    public class TestRailPage
    {
        private Driver.WebDriver Driver { get; set; }
        public TestRailPage(Driver.WebDriver driver)
        {
            Driver = driver;
        }
        #region Elements
        private IWebElement Tr_login => Driver.FindElementById("name");
        private IWebElement Tr_password => Driver.FindElementById("password");
        private IWebElement Tr_login_button => Driver.FindElementByCssSelector("#button_primary");
        private IList<IWebElement> ProjectsList => Driver.FindElementsByCssSelector("div.summary-title>a");
        private IWebElement TestSuitesBtn => Driver.FindElementById("sidebar-suites-overview");
        private IWebElement TestSuite => Driver.FindElementByCssSelector("div.summary-title a"); //Si hay mas de 1 convertirlo en IList
        private IWebElement ImportSuiteICO => Driver.FindElementByCssSelector("a[tooltip-header='Import Cases']");
        private IWebElement ImportDropDownXMLOption => Driver.FindElementByCssSelector("#importDropdown div[class*='xml']");
        private IWebElement ImportXML => Driver.FindElementById("import");
        private IWebElement ImportSubmitXML => Driver.FindElementById("importSubmit");
        private IWebElement PopUpErrorButton => Driver.FindElementByCssSelector("#dialog-ident-messageDialog a.button");
        #endregion
        public void AccessTestRailPage(string url)
        {
            Driver.GetDriver.Navigate().GoToUrl(url);
        }
        public void Login(string user, string password)
        {
            Tr_login.SendKeys(user);
            Tr_password.SendKeys(password);
            Tr_login_button.Click();
        }
        public void SelectProject(string projectName, string rootPath)
        {
            IWebElement project = null;
            try
            {
                project = ProjectsList.Where(projectElement => projectElement.Text.Contains(projectName)).First();
                
            }
            catch 
            {
                PopUpErrorButton.Click();
            }
            project = ProjectsList.Where(projectElement => projectElement.Text.Contains(projectName)).First();
            Driver.ForceClick(project, 500);
            TestSuitesBtn.Click();
            TestSuite.Click();
            var testRailFolder = Directory.GetFiles($@"{rootPath}\TestRailXML");
            foreach (string folder in testRailFolder)
            {
                ImportTestSuites(folder);
            }
        }
        public void ImportTestSuites(string rootPath)
        {
            Driver.ForceClick(ImportSuiteICO, 1000);
            ImportDropDownXMLOption.Click();
            ImportXML.SendKeys(rootPath);
            ImportSubmitXML.Click();
            new WebDriverWait(Driver.GetDriver, TimeSpan.FromSeconds(20)).Until(ExpectedConditions.InvisibilityOfElementLocated(ImportSubmitXML));
        }
    }
}