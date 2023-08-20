using AngleSharp.Html.Dom;

namespace ToyParser.Parsers
{
    public abstract class ParserBase<T>
    {
        public IHtmlDocument Document { get; set; }
        public string Url { get; }
        protected T Model { get; set; }
        public ParserBase(string url)
        {
            Url = url;
        }
        public abstract Task<T> ParseHtmlAsync();
      
        protected async Task<HttpResponseMessage> FetchAsync()
        {
            // Максимальное количество попыток
            int maxRetryCount = 3;

            for (int retry = 0; retry < maxRetryCount; retry++)
            {
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        // Выводим сообщение о задержке
                        await Console.Out.WriteLineAsync("Задержка 4 секунды");
                        await Task.Delay(4000);

                        HttpResponseMessage request = await httpClient.GetAsync(Url);

                        if (request.IsSuccessStatusCode)
                        {
                            return request;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }

                // Задержка перед следующей попыткой 
                // Ждем 2 секунды перед следующей попыткой
                await Task.Delay(2000);
                if (retry < maxRetryCount)
                {
                    await Console.Out.WriteLineAsync("\nПовторная попытка отправить запрос");
                }
            }

            return null;
        }
    }
}
