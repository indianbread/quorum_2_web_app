using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace kata_frameworkless_web_app.AwsDynamoDb
{
    public class AwsDynamoDbTable
    {
        private readonly AmazonDynamoDBClient _client;

        public AwsDynamoDbTable(AmazonDynamoDBClient client)
        {
            _client = client;
        }
        public async Task CreateTableAsync(string tableName, List<AttributeDefinition> tableAttributes,
            List<KeySchemaElement> tableKeySchema, ProvisionedThroughput provisionedThroughput)
        {
            Console.WriteLine("Creating a new table named " + tableName);
            if (await TableExistsAsync(tableName))
            {
                Console.WriteLine(tableName + " table already exists");
                return;
            }

            var newTable =
                CreateNewTableAsync(tableName, tableAttributes, tableKeySchema, provisionedThroughput);

            await newTable;
            
        }

        private async Task CreateNewTableAsync(string tableName, List<AttributeDefinition> tableAttributes, List<KeySchemaElement> tableKeySchema, ProvisionedThroughput provisionedThroughput)
        {
            var request = new CreateTableRequest()
            {
                TableName = tableName,
                AttributeDefinitions = tableAttributes,
                KeySchema = tableKeySchema,
                ProvisionedThroughput = provisionedThroughput
            };

            try
            {
                await _client.CreateTableAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private async Task<bool> TableExistsAsync(string tableName)
        {
            var tableResponse = await _client.ListTablesAsync();
            return tableResponse.TableNames.Contains(tableName);
        }
    }
}