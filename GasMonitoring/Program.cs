using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;

using GasMonitoring.AWS;


namespace GasMonitoring
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var s3Client = new AmazonS3Client();
            var locationsFetcher = new LocationsFetcher(s3Client);
            var locationsTask = await locationsFetcher.FetchLocations();
            foreach (var location in locationsTask)
            {
                Console.Write(location.Id, location.X, location.Y);
            }
           
        }
     
    }
}