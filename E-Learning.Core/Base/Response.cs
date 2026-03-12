using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Learning.Core.Base
{
    public class Response<T> 
    {
        public Response() { }
        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public Response(string message)
        {
            Succeeded = true;
            Message = message;
        }

        public Response(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<string> Errors { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }


    }
}
