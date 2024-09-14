using AutoMapper;
using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Services
{
    public class ProductCreateService
    {
        private readonly BlossomContext _context;
        private readonly IMapper _mapper;

        public ProductCreateService(BlossomContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool IsSuccess, string ErrorMessage, ProductResponseDto Product)> CreateProductAsync(ProductCreateDto productCreateDto)
        {
            // Validate the main category exists
            var mainCategory = await _context.Categories.FindAsync(productCreateDto.MainCategoryId);
            if (mainCategory is null)
            {
                return (false, "Main category not found.", null);
            }

            // Validate the brand exists
            var brand = await _context.Brands.FindAsync(productCreateDto.BrandId);
            if (brand is null)
            {
                return (false, "Brand not found.", null);
            }

            // Validate additional categories exist if provided
            List<Category> additionalCategories = new();
            if (productCreateDto.AdditionalCategoryIds.Any())
            {
                additionalCategories = await _context.Categories
                    .Where(c => productCreateDto.AdditionalCategoryIds.Contains(c.CategoryId))
                    .ToListAsync();

                if (additionalCategories.Count != productCreateDto.AdditionalCategoryIds.Count)
                {
                    return (false, "One or more additional categories not found.", null);
                }
            }

            // Validate characteristics exist if provided
            List<Characteristic> characteristics = new();
            if (productCreateDto.CharacteristicIds.Any())
            {
                characteristics = await _context.Characteristics
                    .Where(c => productCreateDto.CharacteristicIds.Contains(c.CharacteristicId))
                    .ToListAsync();

                if (characteristics.Count != productCreateDto.CharacteristicIds.Count)
                {
                    return (false, "One or more characteristics not found.", null);
                }
            }

            // Map DTO to Product entity
            var product = _mapper.Map<Product>(productCreateDto);

            // Assign relationships
            product.MainCategory = mainCategory;
            product.AdditionalCategories = additionalCategories;
            product.Characteristics = characteristics;
            product.Brand = brand;

            // Add and save the new product
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Map the saved product to a response DTO
            var productResponse = _mapper.Map<ProductResponseDto>(product);

            return (true, string.Empty, productResponse);
        }

        public async Task<(bool IsSuccess, List<ProductResponseDto> Products, string ErrorMessage)> CreateProductBatchAsync(List<ProductCreateDto> productCreateDtos)
        {
            var validationErrors = new List<string>();
            var createdProducts = new List<ProductResponseDto>();

            foreach (var productCreateDto in productCreateDtos)
            {
                // Validate the main category exists
                var mainCategory = await _context.Categories.FindAsync(productCreateDto.MainCategoryId);
                if (mainCategory is null)
                {
                    validationErrors.Add($"Main category not found for product '{productCreateDto.Name}'.");
                    continue;
                }

                // Validate the brand exists
                var brand = await _context.Brands.FindAsync(productCreateDto.BrandId);
                if (brand is null)
                {
                    validationErrors.Add($"Brand not found for product '{productCreateDto.Name}'.");
                    continue;
                }

                // Validate additional categories exist if provided
                List<Category> additionalCategories = new();
                if (productCreateDto.AdditionalCategoryIds.Any())
                {
                    additionalCategories = await _context.Categories
                        .Where(c => productCreateDto.AdditionalCategoryIds.Contains(c.CategoryId))
                        .ToListAsync();

                    if (additionalCategories.Count != productCreateDto.AdditionalCategoryIds.Count)
                    {
                        validationErrors.Add($"One or more additional categories not found for product '{productCreateDto.Name}'.");
                        continue;
                    }
                }

                // Validate characteristics exist if provided
                List<Characteristic> characteristics = new();
                if (productCreateDto.CharacteristicIds.Any())
                {
                    characteristics = await _context.Characteristics
                        .Where(c => productCreateDto.CharacteristicIds.Contains(c.CharacteristicId))
                        .ToListAsync();

                    if (characteristics.Count != productCreateDto.CharacteristicIds.Count)
                    {
                        validationErrors.Add($"One or more characteristics not found for product '{productCreateDto.Name}'.");
                        continue;
                    }
                }

                // Map DTO to Product entity
                var product = _mapper.Map<Product>(productCreateDto);

                // Assign relationships
                product.MainCategory = mainCategory;
                product.AdditionalCategories = additionalCategories;
                product.Characteristics = characteristics;
                product.Brand = brand;

                // Add and save the new product
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                // Map the saved product to a response DTO
                var productResponse = _mapper.Map<ProductResponseDto>(product);
                createdProducts.Add(productResponse);
            }

            if (validationErrors.Any())
            {
                return (false, null, string.Join("; ", validationErrors));
            }

            return (true, createdProducts, string.Empty);
        }
    }
}
