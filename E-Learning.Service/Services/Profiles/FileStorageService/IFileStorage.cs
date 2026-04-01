using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Profiles.FileStorageService
{
    public interface IFileStorage
    {
        
       public  Task<string> SaveFileAsync(IFormFile file, string folder,CancellationToken ct);

        
      public  Task DeleteFileAsync(string relativePath,CancellationToken ct);

       
      public   string GetPublicUrl(string relativePath);
    }
}
