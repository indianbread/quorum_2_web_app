using System;
using System.Net.Sockets;
using Amazon.DynamoDBv2;

namespace kata_frameworkless_web_app
{
    public class AwsDynamoDbClient
    {
        public static bool createClient(bool useDynamoDBLocal)
        {
            if (useDynamoDBLocal)
            {
                OperationSucceeded = false;
                OperationFailed = false;
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
                    Console.WriteLine("Error: DynamoDB Local does not appear to have been started");
                    OperationFailed = true;
                    return false;
                }

                Console.WriteLine("Setting up a DynamoDB Local client");
                var ddbConfig = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
                try
                {
                    _client = new AmazonDynamoDBClient(ddbConfig);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to create a DynamoDBLocal client:" + e.Message);
                    OperationFailed = true;
                    return false;
                }
            }
            else
            {
                try
                {
                    _client = new AmazonDynamoDBClient();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to create a DynamoDB Client: " + e.Message);
                    OperationFailed = true;
                }
            }

            OperationSucceeded = true;
            return true;
        }

        public static bool OperationSucceeded { get; set; }
        public static bool OperationFailed { get; set; }
        private static AmazonDynamoDBClient _client;
    }
}