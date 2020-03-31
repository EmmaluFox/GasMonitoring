using System.Collections.Generic;
 using System.IO;
 using System.Threading.Tasks;
 using Amazon.S3;
 using Newtonsoft.Json;

 namespace GasMonitoring.AWS
 {
     public class LocationsFetcher
     {
         private const string BucketName = "gasmonitoring-locationss3bucket-pgef0qqmgwba";
         private const string FileName = "locations.json";
         private readonly IAmazonS3 _s3Client;
         

         public LocationsFetcher(IAmazonS3 s3Client)
         {
             this._s3Client = s3Client;
         }

         public async Task<IEnumerable<Location>> FetchLocations()
         {
             var response = await _s3Client.GetObjectAsync(BucketName, FileName);
             using var streamReader = new StreamReader(response.ResponseStream);
             var content = streamReader.ReadToEnd();
             var locations = JsonConvert.DeserializeObject<List<Location>>(content);
             return locations;
         }
     }
     
 }