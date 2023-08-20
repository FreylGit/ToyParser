using ToyParser.Parsers;


string siteUrl = "https://www.toy.ru/catalog/boy_transport/";

SiteParser parser = new SiteParser(siteUrl);
var site = await parser.ParseHtmlAsync();

Console.WriteLine("Статистика:");
Console.WriteLine("Количество страниц:" + site.Pages.Count());
int index = 1;
foreach (var page in site.Pages)
{
    Console.WriteLine("Количество товаров на странице " + index + " " + page.Products.Count());
    index++;
}