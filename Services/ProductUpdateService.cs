using AutoMapper;
using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Services
{
    public class ProductUpdateService
    {
        private readonly BlossomContext _context;
        private readonly IMapper _mapper;
        private readonly ProductImageService _productImageService;
        private readonly ImageService _imageService;

        public ProductUpdateService(BlossomContext context, IMapper mapper, ProductImageService productImageService, ImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _productImageService = productImageService;
            _imageService = imageService;
        }

        public async Task<bool> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto)
        {
            var product = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Characteristics)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return false;
            }

            // Validate and fetch brand
            Brand? brand = null;
            if (productUpdateDto.BrandId.HasValue)
            {
                brand = await _context.Brands.FindAsync(productUpdateDto.BrandId.Value);
            }

            // Fetch and validate main category
            Category? mainCategory = null;
            if (productUpdateDto.MainCategoryId.HasValue)
            {
                mainCategory = await _context.Categories.FindAsync(productUpdateDto.MainCategoryId.Value);
                if (mainCategory == null)
                {
                    throw new ArgumentNullException(nameof(mainCategory), "Main category cannot be null.");
                }
            }

            // Fetch additional categories
            List<Category>? additionalCategories = null;
            if (productUpdateDto.AdditionalCategoryIds != null)
            {
                additionalCategories = await _context.Categories
                    .Where(c => productUpdateDto.AdditionalCategoryIds.Contains(c.CategoryId))
                    .ToListAsync();
            }

            // Fetch characteristics
            List<Characteristic>? characteristics = null;
            if (productUpdateDto.CharacteristicIds != null)
            {
                characteristics = await _context.Characteristics
                    .Where(c => productUpdateDto.CharacteristicIds.Contains(c.CharacteristicId))
                    .ToListAsync();
            }

            // Map other properties
            _mapper.Map(productUpdateDto, product);

            // Handle Categories, Characteristics, and Brand
            if (characteristics != null)
            {
                product.Characteristics = characteristics;
            }
            if (mainCategory != null)
            {
                product.MainCategory = mainCategory;
            }
            if (additionalCategories != null)
            {
                product.AdditionalCategories = additionalCategories;
            }
            if (brand != null)
            {
                product.Brand = brand;
            }

            // Handle images (deleting and adding new ones)
            if (productUpdateDto.ImagesToDelete != null && productUpdateDto.ImagesToDelete.Any())
            {
                await HandleImageDeletion(product, productUpdateDto.ImagesToDelete);
            }

            if (productUpdateDto.ImagesToAdd != null && productUpdateDto.ImagesToAdd.Any())
            {
                await HandleImageAddition(product, productUpdateDto.ImagesToAdd);
            }

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task HandleImageDeletion(Product product, List<string> imagesToDelete)
        {
            var remainingImages = product.Images.Where(i => !imagesToDelete.Contains(i)).ToList();
            product.Images = remainingImages;

            foreach (var imageUrl in imagesToDelete)
            {
                var fileName = imageUrl.Split('/').Last();
                try
                {
                    await _imageService.DeleteImageAsync(fileName);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete image: {ex.Message}");
                }
            }
        }

        private async Task HandleImageAddition(Product product, IFormFileCollection imagesToAdd)
        {
            var uploadedImageUrls = await _productImageService.AddProductImagesAsync(product.ProductId, imagesToAdd);
            product.Images.AddRange(uploadedImageUrls);
        }
    }
}
