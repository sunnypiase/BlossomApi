﻿using AutoMapper;
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

        public ProductUpdateService(BlossomContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            // Fetch and validate related entities if necessary
            Category? mainCategory = null;
            if (productUpdateDto.MainCategoryId.HasValue)
            {
                mainCategory = await _context.Categories.FindAsync(productUpdateDto.MainCategoryId.Value);
                if (mainCategory == null)
                {
                    throw new ArgumentNullException(nameof(mainCategory), "Main category cannot be null.");
                }
            }

            List<Category>? additionalCategories = null;
            if (productUpdateDto.AdditionalCategoryIds != null)
            {
                additionalCategories = await _context.Categories
                    .Where(c => productUpdateDto.AdditionalCategoryIds.Contains(c.CategoryId))
                    .ToListAsync();
            }

            List<Characteristic>? characteristics = null;
            if (productUpdateDto.CharacteristicIds != null)
            {
                characteristics = await _context.Characteristics
                    .Where(c => productUpdateDto.CharacteristicIds.Contains(c.CharacteristicId))
                    .ToListAsync();
            }



            // Map the remaining properties
            _mapper.Map(productUpdateDto, product);

            // Handle Characteristics separately
            if (characteristics != null)
            {
                product.Characteristics = characteristics;
            }
            if (mainCategory != null)
            {
                product.MainCategory = mainCategory; // This will update MainCategoryId and rearrange the Categories list
            }

            if (additionalCategories != null)
            {
                product.AdditionalCategories = additionalCategories; // This will update the Categories list
            }
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
