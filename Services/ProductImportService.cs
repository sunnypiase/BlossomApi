using OfficeOpenXml;
using System.Globalization;
using BlossomApi.Dtos;
using BlossomApi.Models;

namespace BlossomApi.Services
{
    public class ProductImportService
    {
        private readonly ProductCreateService _productCreateService;

        public ProductImportService(ProductCreateService productCreateService)
        {
            _productCreateService = productCreateService;
        }

        public async Task<(bool IsSuccess, string ErrorMessage, List<int> ProductIds)> ImportProductsFromExcelAsync(Stream excelStream)
        {
            var productsToCreate = new List<ProductCreateDto>();
            var rowErrors = new List<string>();

            using var package = new ExcelPackage(excelStream);
            var worksheet = package.Workbook.Worksheets[0];

            // Map column headers to their positions
            var columnMapping = MapColumns(worksheet);

            for (int row = 2; row <= worksheet.Dimension.Rows; row++) // Assuming first row is header
            {
                // Check if the row is empty by verifying the required fields
                if (IsRowEmpty(worksheet, row, columnMapping))
                {
                    continue; // Skip empty row
                }

                try
                {
                    var productCreateDto = new ProductCreateDto
                    {
                        Name = GetRequiredValue(worksheet.Cells[row, columnMapping["Назва товару"]], row, "Назва товару", rowErrors),
                        NameEng = GetRequiredValue(worksheet.Cells[row, columnMapping["Назва товару eng"]], row, "Назва товару eng", rowErrors),
                        MainCategoryId = ParseRequiredInt(worksheet.Cells[row, columnMapping["Основна категорія (ід)"]], row, "Основна категорія", rowErrors),
                        Article = GetRequiredValue(worksheet.Cells[row, columnMapping["Артикул (Штрихкод)"]], row, "Артикул (Штрихкод)", rowErrors),
                        Price = ParseRequiredDecimal(worksheet.Cells[row, columnMapping["Ціна"]], row, "Ціна", rowErrors),
                        Discount = ParseDecimal(worksheet.Cells[row, columnMapping.GetValueOrDefault("Відсоток акції")], row, "Відсоток акції", rowErrors),
                        AvailableAmount = ParseRequiredInt(worksheet.Cells[row, columnMapping["Залишок"]], row, "Залишок", rowErrors),
                        Description = GetRequiredValue(worksheet.Cells[row, columnMapping["Опис товару"]], row, "Опис товару", rowErrors),
                        Ingridients = GetValueOrNull(worksheet.Cells[row, columnMapping.GetValueOrDefault("Склад")]),
                        CharacteristicIds = ParseIntList(worksheet.Cells[row, columnMapping.GetValueOrDefault("Характеристики (список ід)")], row, "Характеристики (список ід)", rowErrors),
                        AdditionalCategoryIds = ParseIntList(worksheet.Cells[row, columnMapping.GetValueOrDefault("Додаткові категорії (список ід)")], row, "Додаткові категорії (список ід)", rowErrors),
                        ImageUrls = ParseImageLinks(worksheet.Cells[row, columnMapping.GetValueOrDefault("Зображення (посилання)")], row, "Зображення", rowErrors),
                        MetaKeys = GetValueOrNull(worksheet.Cells[row, columnMapping.GetValueOrDefault("SEOKeywords")]),
                        MetaDescription = GetValueOrNull(worksheet.Cells[row, columnMapping.GetValueOrDefault("SEODescription")])
                    };

                    // Only add the product if there were no parsing errors for this row
                    if (!rowErrors.Any(e => e.Contains($"Рядок {row}")))
                    {
                        productsToCreate.Add(productCreateDto);
                    }
                }
                catch (Exception ex)
                {
                    rowErrors.Add($"Помилка під час обробки рядка {row}: {ex.Message}");
                }
            }

            // If there were any row errors, return them without creating any products
            if (rowErrors.Any())
            {
                return (false, string.Join("; ", rowErrors), null);
            }

            // Validate and create the batch of products
            var (isSuccess, createdProducts, errorMessage) = await _productCreateService.CreateProductBatchAsync(productsToCreate);

            if (!isSuccess)
            {
                return (false, errorMessage, null);
            }

            return (true, string.Empty, createdProducts.Select(x => x.Id).ToList());
        }

        // Helper methods

        private Dictionary<string, int> MapColumns(ExcelWorksheet worksheet)
        {
            var columnMapping = new Dictionary<string, int>();

            for (int col = 1; col <= worksheet.Dimension.Columns; col++)
            {
                var columnHeader = worksheet.Cells[1, col].Text.Trim();
                if (!string.IsNullOrEmpty(columnHeader) && !columnMapping.ContainsKey(columnHeader))
                {
                    columnMapping.Add(columnHeader, col);
                }
            }

            return columnMapping;
        }

        private bool IsRowEmpty(ExcelWorksheet worksheet, int row, Dictionary<string, int> columnMapping)
        {
            return string.IsNullOrWhiteSpace(worksheet.Cells[row, columnMapping["Назва товару"]].Text) &&
                   string.IsNullOrWhiteSpace(worksheet.Cells[row, columnMapping["Назва товару eng"]].Text) &&
                   string.IsNullOrWhiteSpace(worksheet.Cells[row, columnMapping["Артикул (Штрихкод)"]].Text);
        }

        private string? GetValueOrNull(ExcelRange cell)
        {
            return string.IsNullOrEmpty(cell.Text) ? null : cell.Text;
        }

        private string GetRequiredValue(ExcelRange cell, int row, string columnName, List<string> rowErrors)
        {
            var value = cell.Text?.Trim();
            if (string.IsNullOrEmpty(value))
            {
                rowErrors.Add($"Рядок {row}: поле '{columnName}' обов'язкове.");
                return string.Empty;
            }

            return value;
        }

        private decimal ParseRequiredDecimal(ExcelRange cell, int row, string columnName, List<string> rowErrors)
        {
            if (decimal.TryParse(cell.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                return value;
            }
            else
            {
                rowErrors.Add($"Рядок {row}: Невірне числове значення в колонці '{columnName}'.");
                return 0;
            }
        }

        private decimal ParseDecimal(ExcelRange cell, int row, string columnName, List<string> rowErrors)
        {
            if (string.IsNullOrWhiteSpace(cell.Text)) return 0;

            if (decimal.TryParse(cell.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                return value;
            }
            else
            {
                rowErrors.Add($"Рядок {row}: Невірне числове значення в колонці '{columnName}'.");
                return 0;
            }
        }

        private int ParseRequiredInt(ExcelRange cell, int row, string columnName, List<string> rowErrors)
        {
            if (int.TryParse(cell.Text, out var value))
            {
                return value;
            }
            else
            {
                rowErrors.Add($"Рядок {row}: Невірне числове значення в колонці '{columnName}'.");
                return 0;
            }
        }

        private List<int> ParseIntList(ExcelRange cell, int row, string columnName, List<string> rowErrors)
        {
            var text = cell.Text?.Trim();
            if (string.IsNullOrEmpty(text)) return new List<int>();

            var ids = text.Split(',').Select(idText =>
            {
                if (int.TryParse(idText.Trim(), out var id))
                {
                    return id;
                }
                else
                {
                    rowErrors.Add($"Рядок {row}: Невірне значення ідентифікатора у колонці '{columnName}'.");
                    return -1;
                }
            }).Where(id => id != -1).ToList();

            return ids;
        }

        private List<string> ParseImageLinks(ExcelRange cell, int row, string columnName, List<string> rowErrors)
        {
            var links = cell.Text?.Trim().Split(',').Select(link => link.Trim()).ToList() ?? new List<string>();

            if (links.Count == 0)
            {
                rowErrors.Add($"Рядок {row}: Невірні посилання у колонці '{columnName}'.");
            }

            return links;
        }
    }
}
