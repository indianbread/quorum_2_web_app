// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Threading.Tasks;
// using kata_frameworkless_web_app.Repositories;
//
// namespace kata_frameworkless_web_app.Services
// {
//     public class UserService
//     {
//         public UserService(IUserRepository userRepository) 
//         {
//             _userRepository = userRepository;
//         }
//         
//         private readonly IUserRepository _userRepository;
//
//         public IEnumerable<string> GetNameList()
//         {
//             var users = _userRepository.GetUsers();
//             return users.Select(user => user.FirstName);
//             
//         }
//         
//         public RequestResult AddUser(string name)
//         {
//  
//             if (string.IsNullOrWhiteSpace(name))
//             {
//                 return RequestResult.CreateError("Name cannot be empty", HttpStatusCode.OK);
//             }
//
//
//             string result = string.Empty;
//
//             try
//             {
//               _userRepository.AddUserAsync(name);
//               result = "User created successfully";
//               return  RequestResult.CreateSuccess(result, HttpStatusCode.OK);
//
//
//             }
//             catch (Exception e)
//             {
//                 result = e.Message;
//                 return  RequestResult.CreateError(result, HttpStatusCode.InternalServerError);
//             }
//         }
//         
//         private bool UserExists(string name)
//         {
//             return _userRepository.FindUserByName(name) != null;
//         }
//     }
// }