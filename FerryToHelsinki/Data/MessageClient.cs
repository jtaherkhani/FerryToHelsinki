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
        public static string BaseURL =>
            #if DEBUG
                "https://localhost:44330/";
            #else
                "https://ferrytohelsinki.azurewebsites.net/";
            #endif

        public async Task CreateMessage(Message message)
        {
            using var httpBase = new HttpClient();
            var uri = new Uri(BaseURL + "api/Messages");

            string json = JsonConvert.SerializeObject(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await httpBase.PostAsync(uri, content);
        }
    }
}
