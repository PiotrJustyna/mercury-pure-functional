using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MercuryLibrary;
using Microsoft.FSharp.Core;

namespace MercuryHost
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Log("function is starting...");

            var apiUrlFormat = args[0];

            var domain = args[1];

            FSharpOption<Models.WhoisResponse> response = await GetWhoisResponse(
                apiUrlFormat,
                domain);

            Log(response.ToString());
            Log("function execution finished");
        }

        private static async Task<FSharpOption<Models.WhoisResponse>> GetWhoisResponse(
            string apiUrlFormat,
            string domain)
        {
            FSharpOption<Models.WhoisResponse> response = null;

            InputValidation.whoisInputValidation(apiUrlFormat, domain);

            var apiUrl = string.Format(apiUrlFormat, domain);

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));

            var cancellationToken = cancellationTokenSource.Token;

            var client = new HttpClient();

            var apiResponse = await client.GetAsync(
                apiUrl,
                cancellationToken);

            if (apiResponse.IsSuccessStatusCode)
            {
                var serializer = new XmlSerializer(typeof(Models.WhoisRecord));

                await using Stream reader = await apiResponse.Content.ReadAsStreamAsync(cancellationToken);

                response = Mappers.toWhoisResponse(
                    DateTime.Now,
                    domain,
                    (Models.WhoisRecord)serializer.Deserialize(reader));
            }

            return response;
        }

        private static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:hh:mm:ss.fff}: {message}");
        }
    }
}