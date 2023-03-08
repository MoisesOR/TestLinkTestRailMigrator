using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using TestLinkTestRailMigrator.Main.Utils;

namespace TestLinkTestRailMigrator.Main.Driver
{
    public class WebDriver
    {
        public Actions actions;
        private IWebDriver driver;
        private WebDriverWait myWaitVar;
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public IWebDriver GetDriver => driver;
        
        #region Waits Until
        public void WaitUntilPresenceOfElementLocatedById(string id) => myWaitVar.Until(ExpectedConditions.ElementToBeClickable(By.Id(id)));
        public void WaitUntilPresenceOfElementLocatedByCssSelector(string cssSelector) => myWaitVar.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(cssSelector)));
        public void WaitUntilPresenceOfElementLocatedByXpath(string xpath) => myWaitVar.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));
        #endregion
        #region Find Elements + Waits Until
        public IWebElement FindElementById(string id)
        {
            WaitUntilPresenceOfElementLocatedById(id);
            return driver.FindElement(By.Id(id));
        }
        public IWebElement FindElementByXpath(string xpath)
        {
            WaitUntilPresenceOfElementLocatedByXpath(xpath);
            return driver.FindElement(By.XPath(xpath));
        }
        public IWebElement FindElementByCssSelector(string cssSelector)
        {
            WaitUntilPresenceOfElementLocatedByCssSelector(cssSelector);
            return driver.FindElement(By.CssSelector(cssSelector));
        }
        public IList<IWebElement> FindElementsById(string id)
        {
            WaitUntilPresenceOfElementLocatedById(id);
            return driver.FindElements(By.Id(id));
        }
        public IList<IWebElement> FindElementsByXpath(string xpath)
        {
            WaitUntilPresenceOfElementLocatedByXpath(xpath);
            return driver.FindElements(By.XPath(xpath));
        }
        public IList<IWebElement> FindElementsByCssSelector(string cssSelector)
        {
            WaitUntilPresenceOfElementLocatedByCssSelector(cssSelector);
            return driver.FindElements(By.CssSelector(cssSelector));
        }
        #endregion
        public WebDriver() { }
        public void SetUp(bool headless, [Optional] string downloadDir)
        {
            driver = WebDriverCreator.Create(headless, downloadDir);
            driver.Manage().Window.Maximize();
            myWaitVar = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
            driver.Manage().Cookies.DeleteAllCookies();
        }
        public void ForceClick(IWebElement element, int seconds)
        {
            bool isClicked;
            do
            {
                try
                {
                    element.Click();
                    isClicked = true;
                }
                catch
                {
                    Thread.Sleep(seconds);
                    isClicked = false;
                }
            } while (!isClicked);
        }
        public void TearDown()
        {
            driver.Quit();
            driver = null;
        }
    }
}
