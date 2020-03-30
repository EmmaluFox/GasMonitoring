using System;
using Amazon.S3;

using GasMonitoring.AWS;


namespace GasMonitoring
{
    class Program
    {
        static void Main(string[] args)
        {
            var s3Client = new AmazonS3Client();
            var locationsFetcher = new LocationsFetcher();
            var locations = locationsFetcher.FetchLocations(s3Client);
            Console.Write(locations);
        }
     
    }
}