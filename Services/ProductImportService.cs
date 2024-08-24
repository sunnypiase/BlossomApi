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

            for (int row = 2; row <= worksheet.Dimension.Rows; row++) // Assuming first row is header
            {
                try
                {
                    var productCreateDto = new ProductCreateDto
                    {
                        Article = GetValueOrNull(worksheet.Cells[row, 1]),
                        Name = GetValueOrNull(worksheet.Cells[row, 2]),
                        NameEng = GetValueOrNull(worksheet.Cells[row, 3]),
                        Brand = GetValueOrNull(worksheet.Cells[row, 4]),
                        Price = ParseDecimal(worksheet.Cells[row, 5], row, "Price", rowErrors),
                        Discount = ParseDecimal(worksheet.Cells[row, 6], row, "Discount", rowErrors),
                        IsNew = ParseBool(worksheet.Cells[row, 7], row, "IsNew", rowErrors),
                        AvailableAmount = ParseInt(worksheet.Cells[row, 8], row, "AvailableAmount", rowErrors),
                        IsHit = ParseBool(worksheet.Cells[row, 9], row, "IsHit", rowErrors),
                        IsShown = ParseBool(worksheet.Cells[row, 10], row, "IsShown", rowErrors),
                        Description = GetValueOrNull(worksheet.Cells[row, 11]),
                        Ingridients = GetValueOrNull(worksheet.Cells[row, 12]),
                        MainCategoryId = ParseInt(worksheet.Cells[row, 13], row, "MainCategoryId", rowErrors),
                        PurchasePrice = ParseDecimal(worksheet.Cells[row, 14], row, "PurchasePrice", rowErrors),
                        UnitOfMeasurement = GetValueOrNull(worksheet.Cells[row, 15]),
                        ManufacturerBarcode = GetValueOrNull(worksheet.Cells[row, 16]),
                        ActualQuantity = ParseDecimal(worksheet.Cells[row, 17], row, "ActualQuantity", rowErrors),
                        DocumentQuantity = ParseDecimal(worksheet.Cells[row, 18], row, "DocumentQuantity", rowErrors),
                        Group = GetValueOrNull(worksheet.Cells[row, 19]),
                        Type = GetValueOrNull(worksheet.Cells[row, 20]),
                        UKTZED = GetValueOrNull(worksheet.Cells[row, 21]),
                        Markup = ParseDecimal(worksheet.Cells[row, 22], row, "Markup", rowErrors),
                        VATRate = ParseDecimal(worksheet.Cells[row, 23], row, "VATRate", rowErrors),
                        ExciseTaxRate = ParseDecimal(worksheet.Cells[row, 24], row, "ExciseTaxRate", rowErrors),
                        PensionFundRate = ParseDecimal(worksheet.Cells[row, 25], row, "PensionFundRate", rowErrors),
                        VATLetter = GetValueOrNull(worksheet.Cells[row, 26]),
                        ExciseTaxLetter = GetValueOrNull(worksheet.Cells[row, 27]),
                        PensionFundLetter = GetValueOrNull(worksheet.Cells[row, 28])
                    };

                    // Only add the product if there were no parsing errors for this row
                    if (!rowErrors.Any(e => e.Contains($"Row {row}")))
                    {
                        productsToCreate.Add(productCreateDto);
                    }
                }
                catch (Exception ex)
                {
                    rowErrors.Add($"Error parsing row {row}: {ex.Message}");
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

        private string? GetValueOrNull(ExcelRange cell)
        {
            return string.IsNullOrEmpty(cell.Text) ? null : cell.Text;
        }

        private decimal ParseDecimal(ExcelRange cell, int row, string columnName, List<string> rowErrors)
        {
            if (decimal.TryParse(cell.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                return value;
            }
            else
            {
                rowErrors.Add($"Row {row}: Invalid decimal value in column '{columnName}'.");
                return 0;
            }
        }

        private int ParseInt(ExcelRange cell, int row, string columnName, List<string> rowErrors)
        {
            if (int.TryParse(cell.Text, out var value))
            {
                return value;
            }
            else
            {
                rowErrors.Add($"Row {row}: Invalid integer value in column '{columnName}'.");
                return 0;
            }
        }

        private bool ParseBool(ExcelRange cell, int row, string columnName, List<string> rowErrors)
        {
            if (bool.TryParse(cell.Text, out var value))
            {
                return value;
            }
            else
            {
                rowErrors.Add($"Row {row}: Invalid boolean value in column '{columnName}'.");
                return false;
            }
        }
    }
}
