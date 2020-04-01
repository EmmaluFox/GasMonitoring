
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime.Internal;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace GasMonitoring.AWS
{
    public class MessageFetcher
    {
        public MessageFetcher(AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient,
            string topicArn, CreateQueueRequest createQueueRequest)
        {
            

        }

        public async Task<IEnumerable<Message>> FetchMessages(AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient, string topicArn, CreateQueueRequest createQueueRequest)
        {
            var queueUrl = sqsClient.CreateQueueAsync(createQueueRequest.QueueName).Result.QueueUrl;
            await snsClient.SubscribeQueueAsync(topicArn, sqsClient, queueUrl);

            Task<ReceiveMessageResponse> messageTask = sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = 20
            });
            
            using var streamReader = new StreamReader(messageTask.Result.ToString());
            var content = streamReader.ReadToEnd();
            var messages = JsonConvert.DeserializeObject<List<Message>>(content);
            return messages;
        }
        
        
    }
}