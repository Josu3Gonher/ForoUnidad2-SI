using Google.Cloud.Storage.V1;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class FirebaseStorageService
{
    private readonly StorageClient _storage;

    public FirebaseStorageService()
    {
        _storage = StorageClient.Create();
    }

    public async Task UploadImageAsync(string bucketName, string storagePath, object image)
    {
        if (image is IFormFile formFile)
        {
            // Manejar el caso de un archivo IFormFile
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                await _storage.UploadObjectAsync(bucketName, storagePath, null, memoryStream);
            }
        }
        else if (image is string imageUrl)
        {
            // Manejar el caso de una URL de imagen
            using (var client = new WebClient())
            {
                var imageData = client.DownloadData(imageUrl);
                using (var memoryStream = new MemoryStream(imageData))
                {
                    await _storage.UploadObjectAsync(bucketName, storagePath, null, memoryStream);
                }
            }
        }
        else
        {
            // Manejar otros casos o lanzar una excepción según tus necesidades
            throw new ArgumentException("El parámetro 'image' debe ser de tipo IFormFile o string (URL de imagen).");
        }
    }
}
