using AngleSharp.Html.Parser;
using ToyParser.Models;

namespace ToyParser.Parsers
{
    public class ProductParser : ParserBase<ProductModel>
    {
        private const string BASE_URL = "https://www.toy.ru";
        public ProductParser(string url) : base(BASE_URL + url)
        {
        }

        public override async Task<ProductModel> ParseHtmlAsync()
        {
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
            ParseProduct();
            return Model;
        }
        private void ParseProduct()
        {
            var tags = Document.All.Where(x => x.ClassName == "img-block gtm-click");
            var name = Document.All.Where(x => x.ClassName == "content-title").FirstOrDefault()?.TextContent;
            var price = Document.All.Where(x => x.ClassName == "price" && x.GetAttribute("itemprop") == "price").FirstOrDefault()?.TextContent;
            var priceOld = Document.All.Where(x => x.ClassName == "old-price").FirstOrDefault()?.TextContent;
            var availability = Document.All.Where(x => x.ClassName == "not-in-stock-text").FirstOrDefault()?.TextContent;
            var divImages = Document.All
                .FirstOrDefault(x => x.LocalName == "div" && x.GetAttribute("thumbsslider") != null)
                ?.QuerySelector("div.swiper-wrapper")
                 .QuerySelectorAll("div");
            var region = Document.QuerySelector("div.header-top-block").QuerySelector("span").TextContent.Trim();
            var imagesSrc = new List<string>(divImages.Count());
            foreach (var image in divImages)
            {
                var a = image.QuerySelector("a");
                // Проверка на видео
                if (a == null)
                {
                    var src = image.QuerySelector("img").GetAttribute("src").Split("/");
                    var route = src[6];
                    var imageName = src[8];
                    var base_path = $"https://cdn.toy.ru/upload/iblock/{route}/{imageName}";

                    imagesSrc.Add(base_path);
                }

            }
            Model = new ProductModel()
            {
                Name = name,
                Price = price,
                PriceOld = priceOld,
                Availability = availability == null ? "В наличии" : "Не в наличии",
                ImageLinks = imagesSrc,
                ProductLink = Url,
                Region = region
            };

        }
    }
}
