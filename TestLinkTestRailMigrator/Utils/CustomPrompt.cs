using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TestLinkTestRailMigrator.Main.Utils
{
    public class CustomPrompt
    {
        private ConsoleColor PromptColor = ConsoleColor.DarkBlue;
        public void TitleMenu(string title)
        {
            Write("-".PadRight(20, '-'), false, ConsoleColor.Blue);
            Write(title, false, ConsoleColor.Blue);
            Write("-".PadRight(20, '-'), true, ConsoleColor.Blue);
        }
        public bool GetYesNo(string prompt, bool defaultAnswer)
        {
            var answerHint = defaultAnswer ? "[Y/n]" : "[y/N]";
            do
            {
                Write($"{prompt} {answerHint}", false);
                Console.Write(' ');

                string resp = Console.ReadLine()?.ToLower()?.Trim();

                if (string.IsNullOrEmpty(resp))
                {
                    return defaultAnswer;
                }

                switch (resp)
                {
                    case "n":
                    case "no":
                        return false;
                    case "y":
                    case "yes":
                        return true;
                    default:
                        InvalidText($"Invalid response '{resp}'. Please answer 'y' or 'n' or CTRL+C to exit.");
                        break;
                }
            }
            while (true);
        }
        public string GetString(string prompt)
        {
            do
            {
                Write(prompt, false);
                Console.Write(' ');

                string resp = Console.ReadLine();

                if(resp.Length == 0 || resp == "")
                {
                    InvalidText("Empty string. Please fill out the field.");
                }
                else
                {
                    return resp;
                }
            }
            while (true);
        }
        public int GetInt(string prompt, int? defaultAnswer = null)
        {
            do
            {
                Write(prompt, false);
                if (defaultAnswer.HasValue)
                {
                    Write($" [{defaultAnswer.Value}]", false);
                }
                Console.Write(' ');

                string resp = Console.ReadLine()?.ToLower()?.Trim();

                if (string.IsNullOrEmpty(resp))
                {
                    if (defaultAnswer.HasValue)
                    {
                        return defaultAnswer.Value;
                    }
                    else
                    {
                        InvalidText("Please enter a valid number or press CTRL+C to exit.");
                        continue;
                    }
                }

                if (int.TryParse(resp, out var result))
                {
                    return result;
                }

                InvalidText($"Invalid number '{resp}'. Please enter a valid number or press CTRL+C to exit.");
            }
            while (true);
        }
        public void InvalidText(string text)
        {
            Write(text, true, ConsoleColor.Red);
        }
        public void MenuPromp(List<string> menuOption)
        {
            int optionNum = 1;
            foreach (string option in menuOption)
            {
                Write($"{optionNum}. {option}",true);
                optionNum++;
            }
            Write($"{optionNum}. Exit", true);
            Console.WriteLine();
        }
        public void Write(string value, bool newLine, [Optional]ConsoleColor? consoleColor)
        {
            if (consoleColor != null)
            {
                Console.ForegroundColor = consoleColor.Value;
            }
            else
            {
                Console.ForegroundColor = PromptColor;
            }
            if (newLine)
            {
                Console.WriteLine(value);
            }
            else
            {
                Console.Write(value);
            }
            Console.ResetColor();
        }
    }
}
