//Ваша задача — написать программу ,которая позволит понять, какие 10 слов чаще всего встречаются в тексте.

class Program
{
    public static void Main()
    {        
        string filePath = string.Empty;
        Console.Write("Введите путь к файлу: ");
        filePath = Console.ReadLine();

        if (File.Exists(filePath))
        {
            //объявляем разделители
            char[] delimiters = new char[] { ' ', '\r', '\n' };

            TopTenWords(delimiters, filePath);
        }
        else Console.WriteLine("Файл не найден...");

        Console.ReadKey();
    }

    public static void TopTenWords(char[] delimiters, string filePath)
    {
        var wordsQuantity = new Dictionary<string, int>();
        var sortedWordsQuantity = new SortedDictionary<int, List<string>>(); // тут нам нужен список в значениях, т. к. может быть одинаковое количество повторов слов

        // читаем весь файл строку текста и приводим к нижнему регистру
        string text = File.ReadAllText(filePath).ToLower();

        // убираем знаки препинания
        string noPunctuationText = new string(text.Where(c => !char.IsPunctuation(c)).ToArray());

        // разбиваем нашу строку текста, используя ранее перечисленные символы-разделители
        var words = noPunctuationText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words) // добавляем слова в словарь с подсчетом слов
        {
            if (wordsQuantity.ContainsKey(word)) wordsQuantity[word]++;
            else wordsQuantity.Add(word, 1);
        }

        foreach (var word in wordsQuantity) // добавляем слова в сортированный словарь, но ключом уже выступает количество повторов!
        {
            if (sortedWordsQuantity.ContainsKey(word.Value)) sortedWordsQuantity[word.Value].Add(word.Key);
            else sortedWordsQuantity.Add(word.Value, new List<string>() { word.Key });
        }

        Console.WriteLine("Топ-10 встречающихся слов и их частота:");

        int topWordCount = 0; // переменная для подсчета фактического количества выведенных в топ слов

        //определяем, выводим 10 слов или меньше, если слов изначально было меньше
        int countToShow = sortedWordsQuantity.Count < 10 ? sortedWordsQuantity.Count - 1 : 9;

        // идем с конца сортированного словаря - там наши топ-слова
        for (int i = sortedWordsQuantity.Count; i >= sortedWordsQuantity.Count - countToShow; i--)
        {
            topWordCount += sortedWordsQuantity.ElementAt(i - 1).Value.Count(); // увеличиваем счетчик

            // выводим слова из списка словаря по ключу
            foreach (var quantity in sortedWordsQuantity.ElementAt(i - 1).Value)
                Console.WriteLine($"{quantity} - {sortedWordsQuantity.ElementAt(i - 1).Key}");

            // если суммарно уже набилось слов больше 10 (в случае, если какие-то топ-слова встречаются одинаковое количество раз)
            if (topWordCount > countToShow + 1) break;
        }
    }
}