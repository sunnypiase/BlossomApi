using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlossomApi.Services
{
    public class ImageService
    {
        private const string STORAGE_ZONE_NAME = "blossom-blob";
        private const string storageHost = "https://blossom-blob.b-cdn.net";

        public async Task<string> UploadImageAsync(string fileName, Stream imageStream)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            string url = GetBunnyCDNUrl(uniqueFileName);
            string contentType = GetContentType(fileName[fileName.LastIndexOf('.')..]);

            HttpRequestMessage requestMessage = new(HttpMethod.Put, url)
            {
                Content = new StreamContent(imageStream)
            };
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            var httpClient = new BunnyCdnHttpClient();
            var uploadResponse = await httpClient.SendAsync(requestMessage);

            if (uploadResponse.StatusCode == HttpStatusCode.Created || uploadResponse.StatusCode == HttpStatusCode.OK)
            {
                return $"{storageHost}/{uniqueFileName}";
            }
            else
            {
                throw new Exception("Failed to upload image to BunnyCDN.");
            }
        }

        public async Task DeleteImageAsync(string fileName)
        {
            string url = GetBunnyCDNUrl(fileName);

            var httpClient = new BunnyCdnHttpClient();
            var deleteResponse = await httpClient.DeleteAsync(url);

            if (!deleteResponse.IsSuccessStatusCode)
            {
                throw new Exception("Failed to delete image from BunnyCDN.");
            }
        }

        private string GetBunnyCDNUrl(string fileName)
        {
            return $"https://storage.bunnycdn.com/{STORAGE_ZONE_NAME}/{fileName}";
        }

        private string GetContentType(string extension) => extension.ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            _ => "application/octet-stream", // default content type for unknown file types
        };
    }

    public class BunnyCdnHttpClient : HttpClient
    {
        private const string ACCESS_KEY = "9658e359-4520-4a78-bf0dd2a32b66-5764-410e"; // Replace with your actual API key
        private const string storageHost = "https://storage.bunnycdn.com";

        public BunnyCdnHttpClient()
        {
            BaseAddress = new Uri(storageHost);
            DefaultRequestHeaders.Add("AccessKey", ACCESS_KEY);
            DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
        }
    }
}
