
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        private readonly IWebHostEnvironment _environment;

        public BlobHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            Guid fileName = Guid.NewGuid();

            string folder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                containerName);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, fileName.ToString());

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }

        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            Guid fileName = Guid.NewGuid();

            string folder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                containerName);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, fileName.ToString());

            await File.WriteAllBytesAsync(path, file);

            return fileName;
        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            Guid fileName = Guid.NewGuid();

            string folder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                containerName);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, fileName.ToString());

            await using var source = File.OpenRead(image);
            await using var destination = File.Create(path);

            await source.CopyToAsync(destination);

            return fileName;
        }
    }
}





















//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Blob;
//using System;
//using System.IO;
//using System.Threading.Tasks;

//namespace SchoolManagementSystem.Helpers
//{
//    public class BlobHelper : IBlobHelper
//    {
//        private readonly CloudBlobClient _blobClient;

//        public BlobHelper(IConfiguration configuration)
//        {
//            // Connect to Azure Blob Storage
//            string keys = configuration["Blob:ConnectionStrings"];
//            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
//            _blobClient = storageAccount.CreateCloudBlobClient();
//        }

//        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
//        {
//            Stream stream = file.OpenReadStream();
//            return await UploadStreamAsync(stream, containerName);
//        }

//        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
//        {
//            MemoryStream stream = new MemoryStream(file);
//            return await UploadStreamAsync(stream, containerName);
//        }

//        public async Task<Guid> UploadBlobAsync(string image, string containerName)
//        {
//            Stream stream = File.OpenRead(image);
//            return await UploadStreamAsync(stream, containerName);
//        }

//        private async Task<Guid> UploadStreamAsync(Stream stream, string containerName)
//        {
//            Guid name = Guid.NewGuid();
//            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
//            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}");
//            await blockBlob.UploadFromStreamAsync(stream);
//            return name;
//        }
//    }
//}
