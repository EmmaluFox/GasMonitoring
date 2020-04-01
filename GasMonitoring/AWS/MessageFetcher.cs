
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
        private string queueUrl = "";
        public MessageFetcher(AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient,
            string topicArn, CreateQueueRequest createQueueRequest)
        {
            

        }

        public async Task<IEnumerable<Message>> FetchMessages(AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient, string topicArn, CreateQueueRequest createQueueRequest)
        {
            queueUrl = sqsClient.CreateQueueAsync(createQueueRequest.QueueName).Result.QueueUrl;
            await snsClient.SubscribeQueueAsync(topicArn, sqsClient, queueUrl);

            Task<ReceiveMessageResponse> messageTask = sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = 20
            });

            return messageTask.Result.Messages;
        }

        public DeleteQueueRequest DeleteQueue()
        {
            DeleteQueueRequest deleteQueue = new DeleteQueueRequest(queueUrl);
            return deleteQueue;
        }
        
        
    }
}