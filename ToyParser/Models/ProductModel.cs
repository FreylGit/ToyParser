namespace ToyParser.Models
{
    public class ProductModel
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string PriceOld { get; set; }
        public string Availability { get; set; }
        public List<string> ImageLinks { get; set; }
        public string ProductLink { get; set; }
        public string Region { get; set; }
        public override string ToString()
        {
            string imageLinks = ImageLinks != null ? string.Join(", ", ImageLinks) : "No images";
            return $"Name: {Name}\n" +
                   $"Price: {Price}\n" +
                   $"Old Price: {PriceOld}\n" +
                   $"Availability: {Availability}\n" +
                   $"Image Links: {imageLinks}\n" +
                   $"Product Link: {ProductLink}";
        }
    }
}
