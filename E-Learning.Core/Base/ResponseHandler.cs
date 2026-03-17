using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace E_Learning.Core.Base
{
    public class ResponseHandler

    {
        public Response<T> Deleted<T>(string Message = null)
        {
            return new Response<T>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = Message == null ? "Deleted Sucessfully" : Message
            };
        }
        public Response<T> Forbidden<T>(string Message = null)
        {
            return new Response<T>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = Message == null ? "Access denied." : Message
            };
        }
        public Response<T> Success<T>(T entity)
        {
            return new Response<T>()
            {
                Data = entity,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = "Completed SuccessFully"
            };

        }
        public Response<T> Unauthorized<T>()
        {
            return new Response<T>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = "Unauthorized"
            };
        }
        public Response<T> BadRequest<T>(string Message = null)
        {
            return new Response<T>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message == null ? "BadRequest" : Message
            };

        }
        public Response<T> UnProcessableEntity<T>(string Message = null)
        {
            return new Response<T>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = Message == null ? "UnProcessable" : Message
            };

        }
        public Response<T> NotFound<T>(string Message = null)
        {
            return new Response<T>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = Message == null ? "NotFound" : Message
            };
        }
        public Response<T> Created<T>(T entity)
        {
            return new Response<T>()
            {
                Data = entity,
                HttpStatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created Sucessfully"
            };
        }

        public   Response<T> HandleModelStateErrors<T>(ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(x => x.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                .ToList();

            return new Response<T>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Errors = errors,
                Message = "Validation Failed"
            };
        }
    }
}
