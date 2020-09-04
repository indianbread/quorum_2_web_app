using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace kata.users.repositories
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