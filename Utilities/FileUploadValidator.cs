using Microsoft.AspNetCore.Http;

namespace NouvoStudio.Utilities
{
    public static class FileUploadValidator
    {
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private static readonly string[] AllowedImageMimeTypes = 
        {
            "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp"
        };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        public static (bool IsValid, string ErrorMessage) ValidateImageFile(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return (false, "No file uploaded.");
            }

            if (file.Length > MaxFileSize)
            {
                return (false, $"File size exceeds the maximum limit of {MaxFileSize / (1024 * 1024)}MB.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !AllowedImageExtensions.Contains(extension))
            {
                return (false, $"Invalid file type. Allowed types: {string.Join(", ", AllowedImageExtensions)}");
            }

            if (!AllowedImageMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                return (false, "Invalid file MIME type.");
            }

            return (true, string.Empty);
        }

        public static string GetSafeFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var fileName = $"{Guid.NewGuid():N}{extension}";
            return fileName;
        }
    }
}

