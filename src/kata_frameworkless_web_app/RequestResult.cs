// using System.Net;
//
// namespace kata_frameworkless_web_app
// {
//     public class RequestResult
//     {
//         public bool IsSuccess { get; set; }
//         public string SuccessMessage { get; set; }
//         public string ErrorMessage { get; set; }
//         public HttpStatusCode StatusCode { get; set; }
//
//         internal static RequestResult CreateSuccess(string successMessage, HttpStatusCode resultStatusCode)
//         {
//             return new RequestResult
//             {
//                 IsSuccess = true,
//                 SuccessMessage = successMessage,
//                 StatusCode = resultStatusCode
//             };
//         }
//
//         internal static RequestResult CreateError(string errorMessage, HttpStatusCode resultStatusCode)
//         {
//             return new RequestResult
//             {
//                 IsSuccess = false,
//                 ErrorMessage = "Error: " + errorMessage,
//                 StatusCode = resultStatusCode
//             };
//         }
//     }
// }