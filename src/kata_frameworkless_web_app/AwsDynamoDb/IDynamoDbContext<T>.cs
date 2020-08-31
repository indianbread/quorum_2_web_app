// using System;
// using System.Runtime.InteropServices;
// using System.Threading.Tasks;
//
// namespace kata_frameworkless_web_app.AwsDynamoDb
// {
//     public interface IDynamoDbContext<T> : IDisposable where T : class
//     {
//         Task<T> GetByIdAsync(string id);
//         Task SaveAsync(T item);
//         Task DeleteByIdAsync(T item);
//     }
// }