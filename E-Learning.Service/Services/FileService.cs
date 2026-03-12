using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Learning.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using E_Learning.Core.Base;
namespace E_Learning.Service.Services
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment _env;
        private readonly ResponseHandler _responseHandler;

        public FileService(IHostEnvironment env,ResponseHandler responseHandler)
        {
            _env = env;
            this._responseHandler = responseHandler;
        }

        public async Task<string> UploadFileAsync<T>(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file provided");

           
            var allowedExtensions = new[] { ".pdf", ".docx", ".txt" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                _responseHandler.BadRequest<T>("Invalid file type Only the following formats are allowed: .pdf, .docx, .txt");
 
            
            var maxSize = 5 * 1024 * 1024;
            if (file.Length > maxSize)
                _responseHandler.BadRequest<T>("File size exceeds limit (5MB)");
 
            
            var uploadFolder = Path.Combine(_env.ContentRootPath, "wwwroot", folderName);
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadFolder, fileName);

             using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

             var url = $"/{folderName}/{fileName}";
            return url;
        }
    }
 }
