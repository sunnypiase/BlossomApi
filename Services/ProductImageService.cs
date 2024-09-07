using BlossomApi.Models;
using BlossomApi.DB;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Services
{
    public class ProductImageService
    {
        private readonly BlossomContext _context;
        private readonly ImageService _imageService;

        public ProductImageService(BlossomContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<List<string>> AddProductImagesAsync(int productId, IFormFileCollection imageFiles)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            if (imageFiles == null || imageFiles.Count == 0)
            {
                throw new ArgumentException("No image files provided");
            }

            var uploadedImageUrls = new List<string>();

            foreach (var imageFile in imageFiles)
            {
                if (imageFile.Length == 0)
                {
                    throw new ArgumentException($"File {imageFile.FileName} is empty");
                }

                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    stream.Position = 0;

                    var imageUrl = await _imageService.UploadImageAsync(imageFile.FileName, stream);
                    uploadedImageUrls.Add(imageUrl);
                }
            }

            product.Images = [.. product.Images, .. uploadedImageUrls];
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return uploadedImageUrls;
        }
    }
}
