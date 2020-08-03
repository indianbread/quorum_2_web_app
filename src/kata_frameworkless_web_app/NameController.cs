using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public class NameController
    {
        public NameController(NameList nameList)
        {
            _nameList = nameList;
        }

        private readonly NameList _nameList;

        public async Task HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    await HandleGetRequest(request, response);
                    break;
                case "POST":
                    await HandlePostRequest(request, response);
                    break;
                default:
                    response.StatusCode = 404;
                    break;
            }
        }

        private async Task HandleGetRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.PathAndQuery)
            {
                case "/":
                    GetNameList(response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }

            await GetNameList(response);
        }

        private async Task GetNameList(HttpListenerResponse response)
        {
            var nameList = ResponseFormatter.GenerateNamesList(_nameList.Names);
            await ResponseFormatter.GenerateResponseBody(response, nameList);
        }

        private async Task HandlePostRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.QueryString["action"])
            {
                case "add":
                    await _nameList.AddName(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}