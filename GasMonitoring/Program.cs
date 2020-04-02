using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using GasMonitoring.AWS;
using GasMonitoring.Readings;


namespace GasMonitoring
{
    class Program
    {
        
        static async Task Main(string[] args)
        {
            var setCredentials = new SetUpCredentials.Credentials(){BucketName = "gasmonitoring-locationss3bucket-pgef0qqmgwba", FileName = "locations.json", TopicArn = "arn:aws:sns:eu-west-2:099421490492:GasMonitoring-snsTopicSensorDataPart1-1YOM46HA51FB"};
            var setUpConnections = new SetUpConnections();
            var displayValidMessages = new DisplayValidMessages();
            await displayValidMessages.PrintStream(setUpConnections.S3Client, setCredentials.BucketName, setCredentials.FileName, setUpConnections.SqsClient, setUpConnections.SnsClient, setCredentials.TopicArn, setUpConnections.CreateQueueRequest);
            
        }
     
    }
}