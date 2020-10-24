using FerryToHelsinkiWebsite.Data.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FerryToHelsinki.Data
{
    public class MessageClient
    {
        public static string baseURL = "https://localhost:44330/";

        public async Task CreateMessage(Message message)
        {
            using var httpBase = new HttpClient();
            var uri = new Uri(baseURL + "api/Messages");

            string json = JsonConvert.SerializeObject(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await httpBase.PostAsync(uri, content);
        }

    }
}
