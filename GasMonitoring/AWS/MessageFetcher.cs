
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace GasMonitoring.AWS
{
    public class MessageFetcher
    {
        private readonly AmazonSQSClient _sqsClient = new AmazonSQSClient(RegionEndpoint.EUWest2);
        private readonly AmazonSimpleNotificationServiceClient _snsClient = new AmazonSimpleNotificationServiceClient();
        private readonly string _topicArn = "arn:aws:sns:eu-west-2:099421490492:GasMonitoring-snsTopicSensorDataPart1-1YOM46HA51FB";
        private readonly CreateQueueRequest _createQueueRequest = new CreateQueueRequest("Gas-Monitor-Queue");
        
        public MessageFetcher(AmazonSQSClient sqsClient, AmazonSimpleNotificationServiceClient snsClient,
            string topicArn, CreateQueueRequest createQueueRequest)
        {
            this._sqsClient = sqsClient;
            this._snsClient = snsClient;
            this._topicArn = topicArn;
            this._createQueueRequest = createQueueRequest;

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