using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using GasMonitoring.AWS;


namespace GasMonitoring
{
    class Program
    {
        
        static async Task Main(string[] args)
        {
        var sqsClient = new AmazonSQSClient(RegionEndpoint.EUWest2);
        var snsClient = new AmazonSimpleNotificationServiceClient();
        var s3Client = new AmazonS3Client();
        var createQueueRequest = new CreateQueueRequest("Gas-Monitor-Queue");
            
        var locationsFetcher = new LocationsFetcher(s3Client);
        
        var locationsTask = await locationsFetcher.FetchLocations();
      
        foreach (var location in locationsTask)
        {
            Console.Write(location.Id, location.X, location.Y);
        }
            
        const string snsArn = "arn:aws:sns:eu-west-2:099421490492:GasMonitoring-snsTopicSensorDataPart1-1YOM46HA51FB";
            
        var messageFetcher = new MessageFetcher(sqsClient, snsClient, snsArn, createQueueRequest);
        var messageTask = await messageFetcher.FetchMessages(sqsClient, snsClient, snsArn, createQueueRequest);
        foreach (var message in messageTask)
        {
            Console.Write(message.Body);
            
        }

        }
     
    }
}