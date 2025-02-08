using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Timesheet.Services
{
    public class BlobService
    {
        private BlobServiceClient _blobServiceClient;
        private string _containerName;

        public BlobService()
        {
            var cn = "";

            _blobServiceClient = new BlobServiceClient(cn);
            _containerName = "images";
        }

        public async Task<List<string>> ListFiles()
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobs = new List<string>();

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }

            return blobs;
        }

        public async Task UploadFile()
        {

        }

        public async Task<FileResponse> DownloadFile(string filename)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(filename);
            var result = await blobClient.DownloadContentAsync();

            return new FileResponse(result.Value.Content.ToStream(), result.Value.Details.ContentType);
        }

        public async Task DeleteFile()
        {

        }
    }

    public record FileResponse(Stream Stream, string ContentType);
}
