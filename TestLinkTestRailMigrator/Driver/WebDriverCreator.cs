using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Runtime.InteropServices;

namespace TestLinkTestRailMigrator.Main.Driver
{
    public class WebDriverCreator
    {
        public static IWebDriver Create(bool headless, [Optional]string downloadDir)
        {
            var chromeOptions = new ChromeOptions();
            if (!headless)
            {
                chromeOptions.AddArgument("--headless");
            }
            else
            {
                chromeOptions.AddArgument("start-maximized");
            }
            chromeOptions.AddArgument("--safebrowsing-disable-download-protection");
            chromeOptions.AddArgument("safebrowsing-disable-extension-blacklist");
            chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDir);
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
            chromeOptions.AddUserProfilePreference("download.extensions_to_open", "application/xml");
            chromeOptions.AcceptInsecureCertificates = true;          

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            var driver = new ChromeDriver(chromeDriverService, chromeOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            return driver;
        }
    }
}
