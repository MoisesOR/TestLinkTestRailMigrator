using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TestLinkTestRailMigrator.Main.Utils
{
    public class XMLConvertor
    {
        readonly CustomPrompt customPrompt = new CustomPrompt();
        public void XMLFilesReaderConverter(string rootPath)
        {
            var testLinkFolder = Directory.GetFiles($@"{rootPath}\TestLinkXML").Where(f => f.Contains(".xml"));
            DirectoryCreator(rootPath);

            foreach (string file in testLinkFolder)
            {
                customPrompt.Write(file, false);
                Converter(GetFileName(file), rootPath);
            }
        }
        public void DirectoryCreator(string rootPath)
        {
            rootPath += @"\TestRailXML";
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
        }
        public string GetFileName(string path)
        {
            string fileName =path.Split('\\').Last();
            return fileName;
        }
        public void Converter(string filePath, string rootPath)
        {
            // Read XML files
            XDocument doc = XDocument.Load($@"{rootPath}\TestLinkXML\{filePath}");
            // Create the new document estructure
            XElement suite = new XElement("suite",
                new XElement("id", "idsuite"),
                new XElement("name", "namesuite"),
                new XElement("description", "Description TetSuite"),
                GetSections(doc.Elements("testsuite"))
            );
            //Replace sections white <section />
            suite.Descendants()
                    .Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value))
                    .Remove();
            // Save the new doc into file
            suite.Save($@"{rootPath}\TestRailXML\{filePath}");
            customPrompt.Write(" >>> The file has properly been converted.", true);
        }
        public XElement GetSections(IEnumerable<XElement> elements)
        {
            return new XElement("sections",
                elements.Select(e => new XElement("section",
                    new XElement("name", e.Attribute("name")?.Value),
                    new XElement("description", ReplaceCharacters(e.Element("details")?.Value)),
                    new XElement("cases",
                        from testCase in e.Elements("testcase")
                        select new XElement("case",
                            new XElement("id", testCase.Attribute("internalid")?.Value),
                            new XElement("title", testCase.Attribute("name")?.Value),
                            new XElement("template", "Test Case (Steps)"),
                            new XElement("type", "Other"),
                            new XElement("priority", "Medium"),
                            new XElement("estimate"),
                            new XElement("references"),
                            testCase.Element("steps")?.Elements("step").ToList().Count() > 0 ?
                            new XElement("custom",
                                new XElement("preconds", "Summary: " + ReplaceCharacters(testCase.Element("summary")?.Value)
                                + ReplaceCharacters(testCase.Element("preconditions")?.Value)),
                                StepsSeparated_section(testCase)
                            ) : new XElement("custom",
                                new XElement("preconds", ReplaceCharacters(testCase.Element("preconditions")?.Value))
                            )
                        )
                    ),
                    GetSections(e.Elements("testsuite")) // Recursive call
                )).ToArray()
            );
        }
        public XElement StepsSeparated_section(XElement testCase)
        {
            return new XElement("steps_separated",
                from step in testCase.Element("steps")?.Elements("step") ?? Enumerable.Empty<XElement>()
                select new XElement("step",
                    new XElement("index", ReplaceCharacters(step.Element("step_number")?.Value)),
                    new XElement("content", ReplaceCharacters(step.Element("actions")?.Value)),
                    new XElement("expected", ReplaceCharacters(step.Element("expectedresults")?.Value))
                )
            );
        }
        public string ReplaceCharacters(string input)
        {
            // Definition of replacement dictionary
            Dictionary<string, string> replacements = new Dictionary<string, string>
             {
                 { "&", " " },
                 { "&amp;", " " },
                 { "gt;", " " },
                 { "<.*?>", " " },
                 { "<[^>]+>|&[a-zA-Z]+;", " " },
                 { "nbsp;", " " },
                 { "&eacute;", "é" },
                 { "&iacute;", "í" },
                 { "&oacute;", "ó" },
                 { "&uacute;", "ú" },
                 { "&Aacute;", "Á" },
                 { "&Eacute;", "É" },
                 { "&Iacute;", "Í" },
                 { "&Oacute;", "Ó" },
                 { "&Uacute;", "Ú" },
                 { "&ntilde;", "ñ" },
                 { "&quot;", "\"" },
                 { "&hellip;", "..." },
                 { "&lt;", " " },
                 { "&gt;", " " },
                 { "<p>", " " },
                 { "</p>", " " }
             };
            // Create a regular expression that searches for special characters
            string pattern = @"(" + string.Join("|", replacements.Keys.Select(Regex.Escape)) + ")";
            // Replace the special characters with their corresponding characters
            string result = Regex.Replace(input, pattern, m => replacements[m.Value]);
            // Replacement of HTML tags
            result = Regex.Replace(input, "<.*?>", " "); 
            return result;
        }        
    }
}