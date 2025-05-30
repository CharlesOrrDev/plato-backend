using System.Net.Http.Headers;
using System.Net.Mime;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace plato_backend.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(string connectionString, string containerName = "images")
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = containerName;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string fileName = null!)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                var extension = Path.GetExtension(file.FileName);
                fileName = $"{Guid.NewGuid()}{extension}";
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = GetContentType(file.FileName)
            };

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteImageAsync(string blobUrl)
        {
            try
            {
                var blobName = GetBlobNameFromUrl(blobUrl);
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                var response = await blobClient.DeleteIfExistsAsync();
                return response.Value;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Stream> DownloadImageAsync(string blobUrl)
        {
            var blobName = GetBlobNameFromUrl(blobUrl);
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var response = await blobClient.DownloadStreamingAsync();
            return response.Value.Content;
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                _ => "application/octet-stream"
            };
        }

        private string GetBlobNameFromUrl(string blobUrl)
        {
            var uri = new Uri(blobUrl);
            return Path.GetFileName(uri.LocalPath);
        }
    }
}