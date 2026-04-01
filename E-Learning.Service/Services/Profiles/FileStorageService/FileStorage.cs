using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.Services.Profiles.FileStorageService
{
    public class FileStorage:IFileStorage
    {

        private readonly string _wwwRootPath;
        private readonly string _baseUrl;
        private readonly ILogger<FileStorage> _logger;

        private static readonly HashSet<string> AllowedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        public FileStorage(
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            ILogger<FileStorage> logger)
        {
            _wwwRootPath = env.WebRootPath;
            _logger = logger;

            // Build base URL from the current request context (e.g. https://api.learnova.com)
            var request = httpContextAccessor.HttpContext?.Request;
            _baseUrl = request is not null
                ? $"{request.Scheme}://{request.Host}"
                : string.Empty;
        }

        // ─── SaveFileAsync ────────────────────────────────────────────────────
        // Validates → creates folder if needed → writes file → returns relative URL

        public async Task<string> SaveFileAsync(IFormFile file, string folder,CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(file);

            // ── Validation ────────────────────────────────────────────────────
            if (file.Length == 0)
                throw new ArgumentException("File is empty.", nameof(file));

            if (file.Length > MaxFileSizeBytes)
                throw new ArgumentException("File exceeds the 5 MB size limit.", nameof(file));

            var ext = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(ext))
                throw new ArgumentException(
                    $"File type '{ext}' is not allowed. Use JPG, PNG, or WebP.", nameof(file));

            // ── Build paths ───────────────────────────────────────────────────
            var folderPath = Path.Combine(_wwwRootPath, folder);
            Directory.CreateDirectory(folderPath); // no-op if already exists

            var fileName = $"{Guid.NewGuid()}{ext}";
            var absolutePath = Path.Combine(folderPath, fileName);
            var relativePath = Path.Combine(folder, fileName)
                                   .Replace('\\', '/');     // normalise for URLs

            // ── Write file ────────────────────────────────────────────────────
            await using var stream = new FileStream(
                absolutePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await file.CopyToAsync(stream);

            _logger.LogInformation("File saved: {RelativePath}", relativePath);
            return relativePath; // e.g. "profiles/abc123.jpg"
        }

        // ─── DeleteFileAsync ──────────────────────────────────────────────────
        // Resolves relative path → deletes from disk, silent no-op if missing

        public Task DeleteFileAsync(string relativePath, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return Task.CompletedTask;

            var absolutePath = Path.Combine(_wwwRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
                _logger.LogInformation("File deleted: {RelativePath}", relativePath);
            }
            else
            {
                _logger.LogWarning("DeleteFileAsync: file not found at {AbsolutePath}", absolutePath);
            }

            return Task.CompletedTask;
        }

        // ─── GetPublicUrl ─────────────────────────────────────────────────────
        // "profiles/abc.jpg" → "https://api.learnova.com/profiles/abc.jpg"

        public string GetPublicUrl(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;

            var normalized = relativePath.Replace('\\', '/').TrimStart('/');
            return $"{_baseUrl}/{normalized}";
        }
    }
}
