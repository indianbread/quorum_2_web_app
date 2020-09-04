using Amazon;
using Amazon.DynamoDBv2;

namespace kata.users.repositories.DynamoDb
{
    public class DynamoDb
    {
        public static AmazonDynamoDBClient CreatClient(bool useDynamoDbLocal)
        {
            var config = useDynamoDbLocal ? GetLocalConfig() : GetProductionConfig();
            return new AmazonDynamoDBClient(config);
        }

        private static AmazonDynamoDBConfig GetLocalConfig()
        {
            return new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
        }

        private static AmazonDynamoDBConfig GetProductionConfig()
        {
            return new AmazonDynamoDBConfig() {RegionEndpoint = RegionEndpoint.APSoutheast2};
        }
        
    }
}