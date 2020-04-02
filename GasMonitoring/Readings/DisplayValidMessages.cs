using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using GasMonitoring.AWS;

namespace GasMonitoring.Readings
{
    public class DisplayValidMessages
    {
        private async Task<List<string>> MessageStream(IAmazonS3 s3Client, string bucketName, string fileName, AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient, string topicArn, CreateQueueRequest createQueueRequest)
        {
            List<string> messageStream = new List<string>();
            LocationChecker locationChecker = new LocationChecker();
            using MessageFetcher messageFetcher = new MessageFetcher(sqsClient, snsClient, topicArn, createQueueRequest);
            var messages = (await messageFetcher.FetchMessages(sqsClient, snsClient, topicArn, createQueueRequest)).ToList();
            MessageParser messageParser = new MessageParser();
            var readings = messages.Select(message => messageParser.ParseMessage(message));
            foreach (var reading in readings)
            {
                var messageX = @$"
            Timestamp: {reading.Timestamp}
            Location ID: {reading.LocationId}
            Event ID: {reading.EventId}
            Value: {reading.Value}
            ";
                if (await locationChecker.CheckLocation(reading.LocationId, s3Client, bucketName, fileName))
                {
                    messageStream.Add(messageX);
                }
            }
            messageFetcher.Dispose();
            return messageStream;
        }

        public async Task<string> PrintStream(IAmazonS3 s3Client, string bucketName, string fileName, AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient, string topicArn, CreateQueueRequest createQueueRequest)
        {
            var messageStream =  await new DisplayValidMessages().MessageStream(s3Client, bucketName, fileName, sqsClient, snsClient, topicArn, createQueueRequest);
            var count = 1;
            foreach (var message in messageStream)
            {
                Console.WriteLine($"Message {count}: {message}");
                count++;
            }
            return "Complete";
        }

    }
}