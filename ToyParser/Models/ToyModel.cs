namespace ToyParser.Models
{
    public class ToyModel
    {
        public const string BASE_URL = "https://www.toy.ru/";
        public List<PageModel> Pages { get; set; } = new List<PageModel>();
    }
}
