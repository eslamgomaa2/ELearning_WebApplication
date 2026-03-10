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
        public List<string>? Errors { get; set; }
         public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static Response<T> Ok(T data , string message = "Completed Sucessfully")
            =>new() { Succeeded = true ,Message = message ,Data = data};
        public static Response<T> Fail(string message ,List<string>? errors  = null)
            => new() { Succeeded = false, Message = message, Errors = errors };

    }
}
