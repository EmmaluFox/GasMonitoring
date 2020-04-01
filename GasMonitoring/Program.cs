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
            var setCredentials = new SetUpCredentials.Credentials(){BucketName = "gasmonitoring-locationss3bucket-pgef0qqmgwba", FileName = "locations.json", TopicArn = "arn:aws:sns:eu-west-2:099421490492:GasMonitoring-snsTopicSensorDataPart1-1YOM46HA51FB"};
            SetUpConnections setUpConnections = new SetUpConnections();
           
            var locationsFetcher = new LocationsFetcher();

            var locationsTask = await locationsFetcher.FetchLocations(setUpConnections.S3Client, setCredentials.BucketName, setCredentials.FileName);

            foreach (var location in locationsTask)
            {
             Console.Write(location.Id, location.X, location.Y);
            }
            
            var messageFetcher = new MessageFetcher(setUpConnections.SqsClient, setUpConnections.SnsClient, setCredentials.TopicArn, setUpConnections.CreateQueueRequest);
            var messageTask = await messageFetcher.FetchMessages(setUpConnections.SqsClient, setUpConnections.SnsClient, setCredentials.TopicArn, setUpConnections.CreateQueueRequest);
            foreach (var message in messageTask)
            {
             Console.Write(message.Body);
             
            }
         
        }
     
    }
}