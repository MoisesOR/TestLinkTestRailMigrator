using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using TestLinkTestRailMigrator.Main.Driver;
using TestLinkTestRailMigrator.Main.Pages;
using TestLinkTestRailMigrator.Main.Utils;

namespace TestLinkTestRailMigrator
{
    public class Program
    {
        static void Main()
        {
            WebDriver webDriver = new WebDriver();
            CustomPrompt customPrompt = new CustomPrompt();
            TestLinkPage testLinkPage = new TestLinkPage(webDriver);
            TestRailPage testRailPage = new TestRailPage(webDriver);
            XMLConvertor xmlConvertor = new XMLConvertor();
            string rootDir;

            // Set Configuration (Path, ProjectCode, ProjectName)
            customPrompt.TitleMenu("Configuration");
            bool headless = customPrompt.GetYesNo("Do you want Chrome CLI?", false);
            bool config = customPrompt.GetYesNo("Do you want to set custom download path?", false);
            if (config)
            {
                rootDir = customPrompt.GetString("Set custom path:");
            }
            else
            {
                rootDir = $@"C:\Users\{Environment.UserName}{ConfigurationManager.AppSettings["RootPath"]}";
                customPrompt.Write("Default path: ", false);
                customPrompt.Write($"{rootDir}", true, ConsoleColor.White);
            }
            webDriver.ProjectCode = customPrompt.GetString("Set Project Code:");
            webDriver.ProjectName = customPrompt.GetString("Set Project Name:");

            //Main menu
            while (true)
            {
                customPrompt.TitleMenu("Menu");
                List<string> optionList = new List<string> { "TestLink Export", "TestRail Import", "XML Conversor" };
                customPrompt.MenuPromp(optionList);
                int option = customPrompt.GetInt("Select option:");

                if (option == 1) {
                    webDriver.SetUp(headless, $@"{rootDir}\TestLinkXML");

                    customPrompt.TitleMenu("TestLink");
                    string urlTestLink = customPrompt.GetString("TestLink Url:");
                    string userTestLink = customPrompt.GetString("TestLink User:");
                    string passwordTestLink = customPrompt.GetString("TestLink Password:");

                    customPrompt.TitleMenu("Accesing TestLink");
                    customPrompt.Write(">>> AccessTestlinkPage", true);

                    testLinkPage.AccessTestlinkPage(urlTestLink);
                    customPrompt.Write(">>> Login", true);
                    testLinkPage.Login(userTestLink, passwordTestLink);
                    customPrompt.Write(">>> SelectProject", true);
                    testLinkPage.SelectProject($"{webDriver.ProjectCode}:{webDriver.ProjectName}");
                    customPrompt.Write(">>> AccessTestSpecification", true);
                    testLinkPage.AccessTestSpecification();
                    customPrompt.Write(">>> ObtainFolders", true);
                    testLinkPage.ObtainFolders();

                    customPrompt.Write(">>> TearDown", true);
                    webDriver.TearDown();
                }
                else if (option == 2)
                {
                    webDriver.SetUp(headless);

                    customPrompt.TitleMenu("XML Conversor");
                    xmlConvertor.XMLFilesReaderConverter(rootDir);

                    customPrompt.TitleMenu("TestRail");
                    string urlTestRail = customPrompt.GetString("TestRail Url:");
                    string userTestRail = customPrompt.GetString("TestRail User:");
                    string passwordTestRail = customPrompt.GetString("TestRail Password:");

                    customPrompt.TitleMenu("Accesing TestRail");
                    customPrompt.Write(">>> AccessTestRailPage", true);
                    testRailPage.AccessTestRailPage(urlTestRail);
                    customPrompt.Write(">>> Login", true);
                    testRailPage.Login(userTestRail, passwordTestRail);
                    customPrompt.Write(">>> SelectProject", true);
                    testRailPage.SelectProject(webDriver.ProjectName, rootDir);

                    customPrompt.Write(">>> TearDown", true);
                    webDriver.TearDown();
                }
                else if (option == 3)
                {
                    customPrompt.TitleMenu("XML Conversor");
                    xmlConvertor.XMLFilesReaderConverter(rootDir);
                }
                else if (option == 4)
                {
                    customPrompt.Write("Closing application...", false);
                    Thread.Sleep(1000);
                    break;
                }
                else
                {
                    customPrompt.InvalidText("Invalid option, please select valid option.");
                }
            }
        }
    }
}
