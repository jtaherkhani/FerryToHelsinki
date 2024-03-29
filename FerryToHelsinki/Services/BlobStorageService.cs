﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using FerryToHelsinki.Models.AppConfig;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryToHelsinki.Services
{
    public class BlobStorageService
    {
        private const string ContainerName = "images";
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService(BlobStorageConfiguration storageConfiguration)
        {
            _blobContainerClient = new BlobServiceClient(storageConfiguration.ConnectionString).GetBlobContainerClient(ContainerName);
        }

        public async Task<string> UploadBrowserFileAsync(IBrowserFile browserFile)
        {
            if (!await _blobContainerClient.ExistsAsync())
            {
                await _blobContainerClient.CreateAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            string fileName = Guid.NewGuid().ToString();
            var blockBlobClient = _blobContainerClient.GetBlockBlobClient(fileName);

            var browserFileImgSize = await browserFile.RequestImageFileAsync("image/jpeg", 900, 900);
            try
            {
                await blockBlobClient.UploadAsync(browserFileImgSize.OpenReadStream());
            }
            catch (Exception)
            {
                Console.WriteLine(browserFileImgSize.Size);
                return string.Empty;
            }

            return blockBlobClient.Uri.OriginalString;
        }

    }
}
