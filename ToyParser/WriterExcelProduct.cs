using OfficeOpenXml;
using ToyParser.Models;

namespace ToyParser
{
    public class WriterExcelProduct : IWriter<ProductModel>
    {
        public void Write(ProductModel model)
        {
            string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filePath = Path.Combine(projectRoot, "Products.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            FileInfo fileInfo = new FileInfo(filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet != null && (worksheet.Dimension == null || worksheet.Dimension.End.Row < 6))
                {
                    // Добавляем заголовки
                    AddColumnHeaders(worksheet);
                }

                int lastUsedRow = worksheet.Dimension?.End.Row ?? 1;
                worksheet.Cells[lastUsedRow + 1, 1].Value = model.Name;
                worksheet.Cells[lastUsedRow + 1, 2].Value = model.Price;
                worksheet.Cells[lastUsedRow + 1, 3].Value = model.PriceOld;
                worksheet.Cells[lastUsedRow + 1, 4].Value = model.Availability;
                worksheet.Cells[lastUsedRow + 1, 5].Value = model.Region;
                worksheet.Cells[lastUsedRow + 1, 6].Value = string.Join(", ", model.ImageLinks);
                worksheet.Cells[lastUsedRow + 1, 7].Value = model.ProductLink;

                package.SaveAs(fileInfo);
            }
        }
        private void AddColumnHeaders(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, 1].Value = "Название";
            worksheet.Cells[1, 2].Value = "Цена";
            worksheet.Cells[1, 3].Value = "Старая цена";
            worksheet.Cells[1, 4].Value = "Наличие";
            worksheet.Cells[1, 5].Value = "Регион";
            worksheet.Cells[1, 6].Value = "Ссылки на картинки";
            worksheet.Cells[1, 7].Value = "Ссылка на товар";

            // Настройка стиля для жирных заголовков
            var headerStyle = worksheet.Cells[1, 1, 1, 7].Style;
            headerStyle.Font.Bold = true;

            // Установка ширины столбцов (ширина измеряется в символах)
            worksheet.Column(1).Width = 20;
            worksheet.Column(2).Width = 10;
            worksheet.Column(3).Width = 10;
            worksheet.Column(4).Width = 20;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 30;
            worksheet.Column(6).Width = 30;
        }



    }
}
