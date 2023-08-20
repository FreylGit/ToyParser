using ToyParser.Date;
using ToyParser.Models;

namespace ToyParser.Writers
{
    public class WriterDatabase : IWriter<ProductModel>
    {
        private readonly ApplicationDbContext _context;
        public WriterDatabase(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Write(ProductModel model)
        {
            try
            {
                _context.Products.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при сохранении данных в базу данных: {ex.Message}");
            }
        }
    }
}
