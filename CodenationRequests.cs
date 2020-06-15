using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JulioCesarDecrypt
{
    public class CodenationRequests
    {
        private static HttpClient _client;
        private static string _token = "";

        public CodenationRequests()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://api.codenation.dev/v1/challenge/dev-ps/");
        }

        public async Task<string> Get()
        {
            string path = "generate-data?token=" + _token;
            return await _client.GetStringAsync(path);                   
        }

        public async void Post(Message message)
        {
            string path = "submit-solution?token=" + _token;

            FileStream fs = File.Open("answer.json",FileMode.Open);

            MultipartFormDataContent multipartContent = new MultipartFormDataContent();

            multipartContent.Add(
                new StreamContent(fs),
                "answer",
                "answer.json");

            HttpResponseMessage result = await _client.PostAsync(path, multipartContent);

            var contents = await result.Content.ReadAsStringAsync();

            Console.WriteLine(contents);

        }
    }    
}