using Azure.Storage.Blobs;
namespace RaceDayApi.Services
{
   public class BlobService
   {
       private readonly string _connectionString;
       private readonly string _containerName;
       public BlobService(IConfiguration config)
       {
           // We will get these from appsettings.json
           _connectionString = config["AzureBlobStorage:ConnectionString"] ?? "UseDevelopmentStorage=true";
           _containerName = config["AzureBlobStorage:ContainerName"] ?? "raceday-images";
       }
       public async Task<string> UploadFileAsync(IFormFile file, string folder)
       {
           if (file == null || file.Length == 0) throw new ArgumentException("File is empty");
           var containerClient = new BlobContainerClient(_connectionString, _containerName);
           await containerClient.CreateIfNotExistsAsync();
           string blobName = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
           var blobClient = containerClient.GetBlobClient(blobName);
           using var stream = file.OpenReadStream();
           await blobClient.UploadAsync(stream, true);
           return blobClient.Uri.ToString();
       }
   }
}