﻿using System;
 using System.Collections.Generic;
 using System.IO;
 using Amazon.S3;

 namespace GasMonitoring.AWS
 {
     // public class AwsClient
     // {
     //    
     //     
     //     public string SecretKey = Environment.GetEnvironmentVariable("_secret");
     //     
     //     
     // }

     public class LocationsFetcher : AmazonS3Client
     {
         private const string BucketName = "gasmonitoring-locationss3bucket-pgef0qqmgwba";
         private const string FileName = "locations.json";
         
         
         public List<Location> FetchLocations(AmazonS3Client s3Client)
         {
             var response = s3Client.GetObject(BucketName, FileName).ResponseStream;
             using var streamReader = new StreamReader(response);
             var content = streamReader.ReadToEnd();
             return new List<Location>();
         }
     }

     public class Location
     {
     }
 }