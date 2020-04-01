using System.Collections.Generic;
 using System.IO;
 using System.Threading.Tasks;
 using Amazon.S3;
 using Newtonsoft.Json;

 namespace GasMonitoring.AWS
 {
     public class LocationsFetcher
     {

         public async Task<IEnumerable<Location>> FetchLocations(IAmazonS3 s3Client, string bucketName, string fileName)
         {
             var response = await s3Client.GetObjectAsync(bucketName, fileName);
             using var streamReader = new StreamReader(response.ResponseStream);
             var content = streamReader.ReadToEnd();
             var locations = JsonConvert.DeserializeObject<List<Location>>(content);
             return locations;
         }
     }
     
 }