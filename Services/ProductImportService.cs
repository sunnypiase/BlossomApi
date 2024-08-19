using BlossomApi.Models;
using BlossomApi.DB;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlossomApi.Dtos;

namespace BlossomApi.Services
{
    public class ProductImportService
    {
        private readonly BlossomContext _context;

        public ProductImportService(BlossomContext context)
        {
            _context = context;
        }

        public async Task<List<int>> ImportFromExcelAsync(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                throw new ArgumentException("No file uploaded or file is empty.");
            }

            var productIds = new List<int>();

            try
            {
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        var products = new List<Product>();

                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is headers
                        {
                            var productDto = new ProductCreateDto
                            {
                                Name = worksheet.Cells[row, 1].Value?.ToString(),
                                NameEng = worksheet.Cells[row, 2].Value?.ToString(),
                                Brand = worksheet.Cells[row, 3].Value?.ToString(),
                                Price = decimal.Parse(worksheet.Cells[row, 4].Value?.ToString() ?? "0"),
                                Discount = decimal.Parse(worksheet.Cells[row, 5].Value?.ToString() ?? "0"),
                                IsNew = bool.Parse(worksheet.Cells[row, 6].Value?.ToString() ?? "false"),
                                AvailableAmount = int.Parse(worksheet.Cells[row, 7].Value?.ToString() ?? "0"),
                                Article = worksheet.Cells[row, 8].Value?.ToString(),
                                Description = worksheet.Cells[row, 9].Value?.ToString(),
                                CategoryIds = worksheet.Cells[row, 10].Value?.ToString()
                                    ?.Split(',')
                                    .Select(int.Parse)
                                    .ToList() ?? new List<int>(),
                            };

                            var product = new Product();
                            UpdateProduct(product, productDto);
                            products.Add(product);
                        }

                        _context.Products.AddRange(products);
                        await _context.SaveChangesAsync();

                        productIds = products.Select(p => p.ProductId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during Excel import", ex);
            }

            return productIds;
        }

        private void UpdateProduct(Product product, ProductCreateDto productDto)
        {
            product.Name = productDto?.Name ?? product.Name;
            product.NameEng = productDto?.NameEng ?? product.NameEng;
            product.Brand = productDto?.Brand ?? product.Brand;
            product.Price = productDto?.Price ?? product.Price;
            product.Discount = productDto?.Discount ?? product.Discount;
            product.IsNew = productDto?.IsNew ?? product.IsNew;
            product.InStock = (productDto?.AvailableAmount ?? product.AvailableAmount) > 0;
            product.AvailableAmount = productDto?.AvailableAmount ?? product.AvailableAmount;
            product.Article = productDto?.Article ?? product.Article;
            product.DieNumbers = productDto?.DieNumbers ?? product.DieNumbers;
            product.Description = productDto?.Description ?? product.Description;
            product.Images ??= new(); // Ensure Images is not null

            // Update categories
            if (productDto?.CategoryIds != null)
            {
                product.Categories.Clear();
                foreach (var categoryId in productDto.CategoryIds)
                {
                    var category = _context.Categories.Find(categoryId);
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }
            }
        }
    }
}
