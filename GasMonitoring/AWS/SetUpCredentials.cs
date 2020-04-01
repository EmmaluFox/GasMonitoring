namespace GasMonitoring.AWS
{
    public class SetUpCredentials
    {
        public class Credentials
        {
            public string BucketName { get; set; }
            public string FileName { get; set; }
            public string TopicArn { get; set; }
        }
    }
}