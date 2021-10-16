using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryToHelsinki.Models.AppConfig
{
    public class BlobStorageConfiguration
    {
        public BlobStorageConfiguration(IConfiguration configuration) => ConnectionString = configuration.GetValue<string>("StorageAccountConnectionString");

        public string ConnectionString { get; }
    }
}
