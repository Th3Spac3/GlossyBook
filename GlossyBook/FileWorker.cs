using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Gloss
{
    public static class FileWorker
    {
        public static async Task<string> Read(string filepath)
        {
            using (StreamReader sr = File.OpenText(filepath))
            {
                string text = await sr.ReadToEndAsync();
                return text;
            }
        }

        public static async Task Write(string filepath, string text)
        {
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                await sw.WriteLineAsync(text);
            }
        }
    }
}
