using GasMonitoring.AWS;
using NUnit.Framework;

namespace GasMon.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            var setCredentials = new SetUpCredentials.Credentials(){BucketName = "gasmonitoring-locationss3bucket-pgef0qqmgwba", FileName = "locations.json", TopicArn = "arn:aws:sns:eu-west-2:099421490492:GasMonitoring-snsTopicSensorDataPart1-1YOM46HA51FB"};
            var setUpConnections = new SetUpConnections();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}