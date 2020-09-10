using Amazon;
using Amazon.DynamoDBv2;

namespace kata.users.repositories.DynamoDb
{
    public class DynamoDb
    {
        public static AmazonDynamoDBClient CreatClient(bool useDynamoDbLocal)
        {
            return useDynamoDbLocal ? GetLocalClient() : GetProductionClient();

        }

        private static AmazonDynamoDBClient GetLocalClient()
        {
            var config = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
            const string AWS_ACCESS_KEY_ID="X";
            const string AWS_SECRET_ACCESS_KEY = "X";
            return new AmazonDynamoDBClient(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, config);
            
        }

        private static AmazonDynamoDBClient GetProductionClient()
        {
            var config = new AmazonDynamoDBConfig() {RegionEndpoint = RegionEndpoint.APSoutheast2};
            return new AmazonDynamoDBClient(config);
        }
        
    }
}