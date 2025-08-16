using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gloss
{
    public class TextParser
    {
        private string pythonInterpreterPath;
        private string pythonScriptPath;
        private string textTraderPath;


        private TextParser()
        {

        }

        public static async Task<TextParser> TextParserFactory()
        {
            TextParser parser = new TextParser();
            var options = await Options.Get();

            parser.pythonScriptPath = FindPythonScript(options.data.chineseParser);
            parser.pythonInterpreterPath = FindPythonInterpreter("glossary");
            parser.textTraderPath = $@"{Directory.GetParent(parser.pythonScriptPath).FullName}\{options.data.parserTrader}";

            return parser;
        }

        public async Task<string> ParseText(string text)
        {
            var options = await Options.Get();
            string result;

            
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonInterpreterPath;
            start.Arguments = $"{pythonScriptPath} \"{text}\"";
            start.WorkingDirectory = Path.GetDirectoryName(pythonScriptPath);
            start.StandardOutputEncoding = Encoding.UTF8;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true; // Захват стандартного вывода ошибок
            start.CreateNoWindow = true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader errorReader = process.StandardError)
                {
                    string error = errorReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Ошибка: {error}");
                    }
                }

                process.WaitForExit();
            }

            var task = FileWorker.Read(textTraderPath);

            await Task.WhenAll(task);
            result = task.Result;
            File.Delete(textTraderPath);

            return result;
        }

        public static string FindPythonInterpreter(string venv)
        {
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            bool search = true;

            while (search)
            {
                foreach (var dir in Directory.GetDirectories(currentPath))
                {
                    DirectoryInfo info = new DirectoryInfo(dir);
                    if (info.Name == venv)
                    {
                        search = false;
                        break;
                    }
                }
                if (search) currentPath = Directory.GetParent(currentPath).FullName;
            }

            return $@"{currentPath}\{venv}\Scripts\python.exe";
        }

        public static string FindPythonScript(string script)
        {
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            bool search = true;

            while (search)
            {
                foreach (var file in Directory.GetFiles(currentPath))
                {
                    FileInfo info = new FileInfo(file);
                    if (info.Name == script)
                    {
                        search = false;
                        break;
                    }
                }
                if (search) currentPath = Directory.GetParent(currentPath).FullName;
            }

            return $@"{currentPath}\{script}";
        }
    }
}
