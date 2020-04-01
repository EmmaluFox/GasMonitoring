using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            List<Location> locations = new List<Location>();
            List<Message> messages = new List<Message>();
            var locationsFetcher = new LocationsFetcher();
            Console.Write($"There are {locations.Count} locations and {messages.Count} messages in the list.");

            var locationsTask = await locationsFetcher.FetchLocations(setUpConnections.S3Client, setCredentials.BucketName, setCredentials.FileName);
            foreach (var location in locationsTask)
            {
                locations.Add(location);
                Console.WriteLine(location.Id);
                Console.WriteLine(location.X);
                Console.WriteLine(location.Y);
            }
            var messageFetcher = new MessageFetcher(setUpConnections.SqsClient, setUpConnections.SnsClient, setCredentials.TopicArn, setUpConnections.CreateQueueRequest);
            var messageTask = await messageFetcher.FetchMessages(setUpConnections.SqsClient, setUpConnections.SnsClient, setCredentials.TopicArn, setUpConnections.CreateQueueRequest);
            foreach (var message in messageTask)
            {
                messages.Add(message);
             Console.Write(message.Body);
            }
            messageFetcher.DeleteQueue();
            Console.WriteLine($"There are {locations.Count} locations and {messages.Count} messages in the list.");
        }
     
    }
}