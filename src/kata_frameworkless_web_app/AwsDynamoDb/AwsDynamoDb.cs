using System;
using System.Net.Sockets;
using Amazon.DynamoDBv2;

namespace kata_frameworkless_web_app.AwsDynamoDb
{
    public static class AwsDynamoDb
    {
        public static AmazonDynamoDBClient CreateClient(bool useDynamoDBLocal)
        {
            AmazonDynamoDBClient client;
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
                    const string errorMessage = "DynamoDB Local does not appear to have been started";
                    throw new Exception(errorMessage);
                }

                Console.WriteLine("Setting up a DynamoDB Local client");
                var ddbConfig = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
                try
                {
                    client = new AmazonDynamoDBClient(ddbConfig);
                    
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to create a DynamoDBLocal client: " + e.Message);
                }
            }
            else
            {
                try
                {
                    client = new AmazonDynamoDBClient();

                }
                catch (Exception e)
                {
                    throw new Exception("Failed to create a DynamoDB Client: " + e.Message);
                }
            }

            return client;
        }
    }
}