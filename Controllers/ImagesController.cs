using Microsoft.AspNetCore.Mvc;
using BlossomApi.Services;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlossomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly ImageService _imageService;

        public ImagesController(ImageService imageService)
        {
            _imageService = imageService;
        }

        // POST: api/images/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided or file is empty.");
            }

            using var stream = file.OpenReadStream();
            var fileName = await _imageService.UploadImageAsync(file.FileName, stream);
            return Ok(new { FileName = fileName });
        }


        
    }
}
