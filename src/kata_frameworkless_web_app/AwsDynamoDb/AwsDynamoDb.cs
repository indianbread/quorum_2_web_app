using System;
using System.Net.Sockets;
using Amazon.DynamoDBv2;

namespace kata_frameworkless_web_app.AwsDynamoDb
{
    public class AwsDynamoDb
    {
        public AwsDynamoDbOperationResult CreateClient(bool useDynamoDBLocal)
        {
            if (useDynamoDBLocal)
            {
                var localFound = false;
                try
                {
                    using (var tcpClient = new TcpClient())
                    {
                        var result = tcpClient.BeginConnect("localhost", 8000, null, null);
                        localFound = result.AsyncWaitHandle.WaitOne(3000);
                        tcpClient.EndConnect(result);
                    }
                }
                catch
                {
                    localFound = false;
                }

                if (!localFound)
                {
                    var errorMessage = "DynamoDB Local does not appear to have been started";
                    Console.WriteLine("Error: " + errorMessage);
                    return AwsDynamoDbOperationResult.Failed(errorMessage);
                }

                Console.WriteLine("Setting up a DynamoDB Local client");
                var ddbConfig = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
                try
                {
                    Client = new AmazonDynamoDBClient(ddbConfig);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to create a DynamoDBLocal client:" + e.Message);
                    return AwsDynamoDbOperationResult.Failed(e.Message);
                }
            }
            else
            {
                try
                {
                    Client = new AmazonDynamoDBClient();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to create a DynamoDB Client: " + e.Message);
                    return AwsDynamoDbOperationResult.Failed(e.Message);
                }
            }
            
            return AwsDynamoDbOperationResult.Success();
        }

        public AmazonDynamoDBClient Client;
    }
}