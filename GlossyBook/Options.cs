using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloss
{
    public class Options
    {
        private static Options instance;

        private string optionsFile = "Options.json";

        public OptionsData data;

        private Options()
        {

        }

        private async Task InitializeOptions()
        {
            if(File.Exists(optionsFile)) await LoadOptions();
            else
            {
                await CreateOptions();
                await LoadOptions();
            }
        }

        public async static Task<Options> Get()
        {
            if(instance == null) instance = new Options();
            await instance.InitializeOptions();
            return instance;
        }

        private async Task CreateOptions()
        {
            OptionsData data = OptionsData.Default();

            await FileWorker.Write(optionsFile, JsonConvert.SerializeObject(data));
        }

        private async Task LoadOptions()
        {
            data = JsonConvert.DeserializeObject<OptionsData>(await FileWorker.Read(optionsFile));
        }
    }

    public struct OptionsData
    {
        public string searchTableName;
        public string pagesFolder;
        public string chineseParser;
        public string parserTrader;

        public OptionsData(string searchTableName, string pagesFolder, string chineseParser, string parserTrader)
        {
            this.searchTableName = searchTableName;
            this.pagesFolder = pagesFolder;
            this.chineseParser = chineseParser;
            this.parserTrader = parserTrader;
        }

        public static OptionsData Default()
        {
            return new OptionsData(
                "search.txt",
                "pages",
                "main.py",
                "message.txt");
        }
    }
}
