using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ToyParser.Date;
using ToyParser.Models;
using ToyParser.Writers;

namespace ToyParser.Parsers
{
    public class SiteParser : ParserBase<ToyModel>
    {
        public SiteParser(string url) : base(url)
        {

        }

        public override async Task<ToyModel> ParseHtmlAsync()
        {
            await Console.Out.WriteLineAsync("Получение страниц");
            var request = await FetchAsync();
            if (request == null)
            {
                await Console.Out.WriteLineAsync("Ошибка при обращении к серверу");
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

            await GetScrapeResults(Document);
            return Model;
        }
        private async Task GetScrapeResults(IHtmlDocument document)
        {
            int quantityPages;
            int.TryParse(document.All.Where(x => x.ClassName == "page-link").Reverse().Skip(1).FirstOrDefault().TextContent, out quantityPages);
            List<IWriter<ProductModel>> writers = new List<IWriter<ProductModel>>();
            writers.Add(new WriterExcelProduct());
            writers.Add(new WriterDatabase(new ApplicationDbContext()));
            Model = new ToyModel();
            for (int i = 1; i <= quantityPages; i++)
            {
                
                PagePareser pageParser = new PagePareser($"https://www.toy.ru/catalog/boy_transport/?filterseccode%5B0%5D=transport&PAGEN_5={i}", writers);
                var page = await pageParser.ParseHtmlAsync();
                Model.Pages.Add(page);
                await Console.Out.WriteLineAsync($"Page{i}\t quantity product:{page.Products.Count()}");
            }
        }
    }

}
