using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Serialization;

using App.RestApiClient;
using App.RestApiClient.Model;

using AutoMapper;

using Refit;

namespace App.Console.Services
{
    public class WordDictionary
    {
        private IMapper mapper;
        private IRestClient restClient;

        public WordDictionary()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Word, Model.Word>().ReverseMap());
            mapper = new Mapper(config);
            restClient = RestService.For<IRestClient>(Settings.Default.RestApiHost);
        }

        public async Task Add(Model.Word word)
        {
            var response = await restClient.WordCreate(new WordCreateRequest(word.Prefix, word.Root, word.Suffix, word.Full));
            if (response.Status != 0) throw new Exception(response.Detail);
        }

        public async Task<IEnumerable<Model.Word>> FindByRoot(string root)
        {
            var response = await restClient.WordSearch(new WordSearchRequest(root));
            if (response.Status != 0) throw new Exception(response.Detail);
            return mapper.Map<IEnumerable<Model.Word>>(response.Data);
        }

        public async Task ImportJSON()
        {
            System.Console.WriteLine("Введите путь к файлу");
            string path = System.Console.ReadLine();

            List<Model.Word> wordList = JsonSerializer.Deserialize<List<Model.Word>>(File.ReadAllText(path));

            int count = 0;
            foreach (var word in wordList)
            {
                var response = await restClient.WordCreate(new WordCreateRequest(word.Prefix, word.Root, word.Suffix, word.Full));
                if (response.Status == 0) count++;
            };

            System.Console.WriteLine($"Добавленно {count} записей");
        }

        public async Task ImportXML()
        {
            System.Console.WriteLine("Введите путь к файлу");
            string path = System.Console.ReadLine();

            var xmlSerializer = new XmlSerializer(typeof(List<Model.Word>));

            using var fs = new FileStream(path, FileMode.OpenOrCreate);
            var wordList = xmlSerializer.Deserialize(fs) as List<Model.Word>;

            int count = 0;
            foreach (var word in wordList)
            {
                var response = await restClient.WordCreate(new WordCreateRequest(word.Prefix, word.Root, word.Suffix, word.Full));
                if (response.Status == 0) count++;
            };

            System.Console.WriteLine($"Добавленно {count} записей");
        }

        public async Task ExportJson()
        {
            var response = await restClient.WordList();
            if (response.Status != 0) throw new Exception(response.Detail);

            string json = JsonSerializer.Serialize(mapper.Map<List<Model.Word>>(response.Data), new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true });
            var fileName = $"{GetNameDoc()}.json";
            File.WriteAllText(fileName, json);
            System.Console.WriteLine($"Экспорт завершен: \"{fileName}\"");
        }

        public async Task ExportXML()
        {
            var response = await restClient.WordList();
            if (response.Status != 0) throw new Exception(response.Detail);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Model.Word>));
            var fileName = $"{GetNameDoc()}.xml";
            using var writer = new StreamWriter(fileName);
            xmlSerializer.Serialize(writer, mapper.Map<List<Model.Word>>(response.Data));
            System.Console.WriteLine($"Экспорт завершен: \"{fileName}\"");
        }

        private string GetNameDoc()
        {
            DateTime _date = DateTime.Now;
            return $"{_date.Day} {_date.Month} {_date.Year} {_date.Hour} {_date.Minute} {_date.Second}";
        }
    }
}