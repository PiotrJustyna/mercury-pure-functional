using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MercuryHost
{
    internal static class Program
    {
        internal static async Task Main(string[] args)
        {
            Log($"function is starting...");

            var apiUrlFormat = args[0];

            var domain = args[1];

            WhoisResponse response = await GetWhoisResponse(
                apiUrlFormat,
                domain);
            
            Log(response.ToString());
            Log($"function execution finished");
        }

        private static async Task<WhoisResponse> GetWhoisResponse(
            string apiUrlFormat,
            string domain)
        {
            WhoisResponse response = null;

            // responsibility: argument preparation
            if (string.IsNullOrWhiteSpace(apiUrlFormat))
            {
                throw new ArgumentException(nameof(apiUrlFormat));
            }

            if (string.IsNullOrWhiteSpace(domain))
            {
                throw new ArgumentException(nameof(domain));
            }

            var apiUrl = string.Format(apiUrlFormat, domain);

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));

            var cancellationToken = cancellationTokenSource.Token;

            // responsibility: api call
            var client = new HttpClient();

            var apiResponse = await client.GetAsync(
                apiUrl,
                cancellationToken);

            if (apiResponse.IsSuccessStatusCode)
            {
                var serializer = new XmlSerializer(typeof(WhoisRecord));

                await using Stream reader = await apiResponse.Content.ReadAsStreamAsync(cancellationToken);

                WhoisRecord whoisRecord = (WhoisRecord) serializer.Deserialize(reader);

                // responsibility: mapping
                if (whoisRecord != null &&
                    whoisRecord.audit != null)
                {
                    _ = DateTime.TryParse(whoisRecord.createdDate, out var createdDate);

                    _ = DateTime.TryParse(whoisRecord.updatedDate, out var updatedDate);

                    _ = DateTime.TryParse(whoisRecord.expiresDate, out var expiresDate);

                    _ = DateTime.TryParse(whoisRecord.audit.createdDate, out var auditCreatedDate);

                    _ = DateTime.TryParse(whoisRecord.audit.updatedDate, out var auditUpdatedDate);

                    var now = DateTime.UtcNow;

                    response = new WhoisResponse
                    {
                        Domain = domain,

                        // responsibility: translating datetime to duration
                        DomainAgeInDays = (now - createdDate).Days,

                        DomainLastUpdatedInDays = (now - updatedDate).Days,

                        DomainExpirationInDays = (expiresDate - now).Days,

                        AuditCreated = auditCreatedDate,

                        AuditUpdated = auditUpdatedDate,
                    };
                }
            }

            return response;
        }

        private static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:hh:mm:ss.fff}: {message}");
        }
    }

    public class WhoisRecord
    {
        public string createdDate { get; set; }

        public string updatedDate { get; set; }

        public string expiresDate { get; set; }

        public string status { get; set; }

        public Audit audit { get; set; }
    }

    public class Audit
    {
        public string createdDate { get; set; }

        public string updatedDate { get; set; }
    }

    public class WhoisResponse
    {
        public string Domain { get; set; }

        public int DomainAgeInDays { get; set; }

        public int DomainLastUpdatedInDays { get; set; }

        public int DomainExpirationInDays { get; set; }

        public DateTime AuditCreated { get; set; }

        public DateTime AuditUpdated { get; set; }

        public override string ToString()
        {
            return $"\"{Domain}\": {DomainAgeInDays} days since domain creation, {DomainLastUpdatedInDays} days since domain last updated, {DomainExpirationInDays} until domain expires";
        }
    }
}