using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace kata_frameworkless_web_app.AwsDynamoDb
{
    public class UserTableConstant
    {
        public const string TableName = "User";

        public static readonly List<AttributeDefinition> AttributeDefinitions = new List<AttributeDefinition>()
        {
            new AttributeDefinition() {AttributeName =  "Id", AttributeType = "N"},
            new AttributeDefinition() {AttributeName = "FirstName", AttributeType = "S"}
        };
        
        public static readonly List<KeySchemaElement> KeySchemaElements = new List<KeySchemaElement>()
        {
            new KeySchemaElement() {AttributeName = "Id", KeyType = "HASH"}
        };
        
        public static readonly ProvisionedThroughput ProvisionedThroughput = new ProvisionedThroughput()
        {
            ReadCapacityUnits = 10,
            WriteCapacityUnits = 5
        };
    }
}