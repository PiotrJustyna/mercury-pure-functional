using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MercuryHost
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Log("function is starting...");

            var apiUrlFormat = args[0];

            var domain = args[1];

            MercuryLibrary.Models.WhoisResponse response = await GetWhoisResponse(
                apiUrlFormat,
                domain);

            Log(response.ToString());
            Log("function execution finished");
        }

        private static async Task<MercuryLibrary.Models.WhoisResponse> GetWhoisResponse(
            string apiUrlFormat,
            string domain)
        {
            MercuryLibrary.Models.WhoisResponse response = null;

            MercuryLibrary.InputValidation.whoisInputValidation(apiUrlFormat, domain);

            var apiUrl = string.Format(apiUrlFormat, domain);

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));

            var cancellationToken = cancellationTokenSource.Token;

            var client = new HttpClient();

            var apiResponse = await client.GetAsync(
                apiUrl,
                cancellationToken);

            if (apiResponse.IsSuccessStatusCode)
            {
                var serializer = new XmlSerializer(typeof(MercuryLibrary.Models.WhoisRecord));

                await using Stream reader = await apiResponse.Content.ReadAsStreamAsync(cancellationToken);

                MercuryLibrary.Models.WhoisRecord whoisRecord =
                    (MercuryLibrary.Models.WhoisRecord) serializer.Deserialize(reader);

                if (whoisRecord != null &&
                    whoisRecord.audit != null)
                {
                    _ = DateTime.TryParse(whoisRecord.createdDate, out var createdDate);

                    _ = DateTime.TryParse(whoisRecord.updatedDate, out var updatedDate);

                    _ = DateTime.TryParse(whoisRecord.expiresDate, out var expiresDate);

                    _ = DateTime.TryParse(whoisRecord.audit.createdDate, out var auditCreatedDate);

                    _ = DateTime.TryParse(whoisRecord.audit.updatedDate, out var auditUpdatedDate);

                    var now = DateTime.UtcNow;

                    response = new MercuryLibrary.Models.WhoisResponse(
                        domain,
                        (now - createdDate).Days,
                        (now - updatedDate).Days,
                        (expiresDate - now).Days,
                        auditCreatedDate,
                        auditUpdatedDate);
                }
            }

            return response;
        }

        private static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:hh:mm:ss.fff}: {message}");
        }
    }
}