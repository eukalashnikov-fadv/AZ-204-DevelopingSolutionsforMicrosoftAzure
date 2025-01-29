using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace func
{
    public class FileParser
    {
        private readonly ILogger<FileParser> _logger;

        public FileParser(ILogger<FileParser> logger)
        {
            _logger = logger;
        }

        [Function("FileParser")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            string connectionString = Environment.GetEnvironmentVariable("StorageConnectionString");

            /* Create a new instance of the BlobClient class by passing in your
               connectionString variable, a  "drop" string value, and a
               "records.json" string value to the constructor */
            BlobClient blob = new BlobClient(connectionString, "drop", "records.json");

            // Download the content of the referenced blob 
            BlobDownloadResult downloadResult = blob.DownloadContent();

            // Retrieve the value of the downloaded blob and convert it to string
            response.WriteStringAsync(downloadResult.Content.ToString());

            //return the response
            return response;
        }
    }
}
