using System.Runtime.CompilerServices;
using Amazon;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace GasMonitoring.AWS
{
    public class SetUpConnections
    {
        public AmazonSQSClient SqsClient { get; set; }
        public AmazonSimpleNotificationServiceClient SnsClient { get; set; }
        public CreateQueueRequest CreateQueueRequest { get; set; }
        public IAmazonS3 S3Client { get; set; }
        
        public SetUpConnections()
        {
        this.SqsClient = new AmazonSQSClient();
        this.SnsClient = new AmazonSimpleNotificationServiceClient();
        this.CreateQueueRequest = new CreateQueueRequest("Gas-Monitor-Queue");
        this.S3Client = new AmazonS3Client();
        }

       

        
      
    }
}