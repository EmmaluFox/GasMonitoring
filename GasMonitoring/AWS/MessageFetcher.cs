
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime.Internal;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace GasMonitoring.AWS
{
    public class MessageFetcher : IDisposable
    {
        private string _queueUrl = "";
        public MessageFetcher(AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient,
            string topicArn, CreateQueueRequest createQueueRequest)
        {
            

        }

        public async Task<IEnumerable<Message>> FetchMessages(AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient, string topicArn, CreateQueueRequest createQueueRequest)
        {
            _queueUrl = sqsClient.CreateQueueAsync(createQueueRequest.QueueName).Result.QueueUrl;
            await snsClient.SubscribeQueueAsync(topicArn, sqsClient, _queueUrl);
            Task<ReceiveMessageResponse> messageTask = sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest(_queueUrl)
            {
                QueueUrl = _queueUrl,
                WaitTimeSeconds = 20,
                MaxNumberOfMessages = 10
            });
            await messageTask.ConfigureAwait(messageTask.IsCompleted);
            
            return messageTask.Result.Messages;
        }

        private DeleteQueueRequest DeleteQueue()
        {
            return new DeleteQueueRequest(_queueUrl);
        }

        private UnsubscribeRequest UnsubscribeQueue()
        {
            return new UnsubscribeRequest(_queueUrl);
        }
        public void Dispose()
        {
            UnsubscribeQueue();
            DeleteQueue();
        }
    }
}