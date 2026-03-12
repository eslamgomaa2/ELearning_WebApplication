using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace E_Learning.Service.Contract
{
    public interface IFileService
    {
         
        Task<string> UploadFileAsync<T>(IFormFile file, string folderName);
    }
}
