using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using GasMonitoring.AWS;

namespace GasMonitoring.Readings
{
    public class LocationChecker
    {
        public async Task<bool> CheckLocation(string locationId, IAmazonS3 s3Client, string bucketName, string fileName)
        {
            var locationsFetcher = new LocationsFetcher();
            var locations = (await locationsFetcher.FetchLocations(s3Client, bucketName, fileName)).ToList();
            var locationValid = locations.Select(location => location.Id == locationId);
            return locationValid.Any();
        }
    }
}