// using System;
// using System.Threading.Tasks;
// using Amazon.DynamoDBv2;
// using Amazon.DynamoDBv2.DataModel;
//
// namespace kata_frameworkless_web_app.AwsDynamoDb
// {
//     public class DynamoDbService
//     {
//         public DynamoDbService(IDynamoDBContext dynamoDbContext)
//         {
//             _dynamoDbContext = dynamoDbContext;
//         }
//         
//         private readonly IDynamoDBContext _dynamoDbContext;
//         private IAmazonDynamoDB _dynamoDbClient;
//
//
//         public async Task<T> GetAsync<T>(string id)
//         {
//             try
//             {
//                 return await _dynamoDbContext.LoadAsync<T>(id);
//             }
//             catch (Exception e)
//             {
//                 throw new Exception($"Amazon error in Get operation! Error: {e}");
//             }
//         }
//
//         public async Task WriteAsync<T>(T item)
//         {
//             try
//             {
//                 await _dynamoDbContext.SaveAsync(item);
//             }
//             catch (Exception e)
//             {
//                 throw new Exception($"Amazon error in Write operation! Error: {e}");
//             }
//         }
//
//         public async Task DeleteAsync<T>(T item)
//         {
//             try
//             {
//                 await _dynamoDbContext.DeleteAsync(item);
//
//             }
//             catch (Exception e)
//             {
//                 throw new Exception($"Amazon error in Delete operation! Error: {e}");
//             }
//         }
//     }
// }