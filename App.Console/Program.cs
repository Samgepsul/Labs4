// See https://aka.ms/new-console-template for more information
using App.Console.Model;
using App.Console.Services;

Console.WriteLine("Добро пожаловать в программу словаря однокоренных слов!");

int action = -1;
while (action != 7)
{
    RenderMenu();

    try
    {

        int.TryParse(Console.ReadLine(), out action);

        var dict = new WordDictionary();

        switch (action)
        {
            case 1:
                await AddWord();
                break;

            case 2:
                await dict.ImportJSON();
                break;

            case 3:
                await dict.ImportXML();
                break;

            case 4:
                await dict.ExportJson();
                break;

            case 5:
                await dict.ExportXML();
                break;

            case 6:
                await Find();
                break;

            case 7: continue;

            default:
                Console.WriteLine("Не корректное значение");
                break;
        }
    }
    catch (Exception ex) { Console.WriteLine(ex.Message); }

    WaitAndClear();
    continue;
}

static void WaitAndClear()
{
    Console.ReadLine();
    Console.Clear();
}

static void RenderMenu()
{
    Console.WriteLine("Выберите действие");
    Console.WriteLine("1. Добавить слово вручную");
    Console.WriteLine("2. Импорт слов из файла JSON");
    Console.WriteLine("3. Импорт слов из файла XMl");
    Console.WriteLine("4. Экспорт данных в JSON");
    Console.WriteLine("5. Экспорт данных в XML");
    Console.WriteLine("6. Поиск однокоренных слов по корню");
    Console.WriteLine("7. Выход из программы");
}

static async Task Find()
{
    Console.Write("Введите корень: ");
    string root = Console.ReadLine();

    var dict = new WordDictionary();
    var wordList = (await dict.FindByRoot(root)).ToList();

    if (wordList.Count == 0)
    {
        Console.WriteLine("Не найденно внесенных однокоренных слов");
        return;
    }
    else
    {
        Console.WriteLine("Однокоренные слова:");
        wordList.ForEach(x => Console.WriteLine(x.Full));
    }
}

static async Task AddWord()
{
    Console.Write("Введите слово: ");
    string inputWord = Console.ReadLine().Trim();
    await AddWordToDictionary(inputWord);
}

static async Task AddWordToDictionary(string word)
{
    var dict = new WordDictionary();

    string prefix = "";
    while (true)
    {
        Console.Write("Приставка: ");
        string input = Console.ReadLine().Trim();
        if (string.IsNullOrWhiteSpace(input)) break;
        prefix += input;
    }

    Console.Write("Корень: ");
    string root = Console.ReadLine().Trim();

    string suffix = "";
    while (true)
    {
        Console.Write("Суффикс или окончание: ");
        string input = Console.ReadLine().Trim();
        if (string.IsNullOrWhiteSpace(input)) break;
        suffix += input;
    }

    string newWord = ($"{prefix}{root}{suffix}");

    if (!IsWordValid(newWord, word))
    {
        Console.WriteLine("Неверные части слова. Пожалуйста, повторите ввод.");
        return;
    }

    Word w = new Word();
    w.Prefix = prefix;
    w.Root = root;
    w.Suffix = suffix;
    w.Full = word;

    await dict.Add(w);

    Console.WriteLine($"Слово \"{newWord}\" добавлено.");
}

static bool IsWordValid(string word, string originalWord)
{
    string[] parts = word.Split('-');
    string assembledWord = string.Join("", parts);

    return assembledWord == originalWord;
}
