using AngleSharp.Html.Parser;
using ToyParser.Models;
using ToyParser.Writers;

namespace ToyParser.Parsers
{
    public class PagePareser : ParserBase<PageModel>
    {
        private List<string> _cardsLink;
        private ProductParser _productParser;
        private List<IWriter<ProductModel>> _writers;
        public PagePareser(string url, List<IWriter<ProductModel>> writers) : base(url)
        {
            _cardsLink = new List<string>();
            Model = new PageModel();
            _writers = writers;
        }

        public override async Task<PageModel> ParseHtmlAsync()
        {
            await Console.Out.WriteLineAsync("Парсинг страницы");
            var request = await FetchAsync();
            if (request == null)
            {
                Console.WriteLine("Ошибка при обращении к серверу");
                return null;
            }

            using (Stream responseStream = await request.Content.ReadAsStreamAsync())
            {
                HtmlParser parser = new HtmlParser();
                Document = parser.ParseDocument(responseStream);
            }
            if (Document == null)
            {
                return null;
            }
            // Получаем все ссылки товаров на странице
            GetLinksProducts();

            var products = await ParseProductAsync();
            Model.Products = products;
            if (products.Count() != _cardsLink.Count())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                await Console.Out.WriteLineAsync($"Не все продукты удалось спарсить, пропущено{_cardsLink.Count() - products.Count()} продуктов");
                Console.ResetColor();
            }
            return Model;
        }
        private void GetLinksProducts()
        {
            var tags = Document.All.Where(x => x.ClassName == "img-block gtm-click");
            foreach (var item in tags)
            {
                _cardsLink.Add(item.Attributes.FirstOrDefault(x => x.Name.Contains("href")).Value);
            }
        }

        private async Task<List<ProductModel>> ParseProductAsync()
        {
            var products = new List<ProductModel>();
            foreach (var link in _cardsLink)
            {
                _productParser = new ProductParser(link);
                var product = await _productParser.ParseHtmlAsync();
                products.Add(product);

                //Сохранение 
                foreach (var writer in _writers)
                {
                    writer.Write(product);
                }
                
                await Console.Out.WriteLineAsync($"Записан продукт:{product.Name}");
            }
            return products;
        }
    }
}
