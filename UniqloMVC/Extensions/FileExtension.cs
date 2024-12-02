﻿namespace UniqloMVC.Extensions
{
    public static class FileExtension
    {
        public static bool IsValidType(this IFormFile file ,string type)=>file.ContentType.StartsWith(type);
        public static bool IsValidSize (this IFormFile file ,int kb)=>file.Length<=kb*1024;
        
        public static async Task<String> UploadAsync(this IFormFile file,params string[] paths)
        {
            string UploadPath = Path.Combine(paths);
            if(!Path.Exists(UploadPath))
                Directory.CreateDirectory(UploadPath);
            string fileName = Path.GetRandomFileName()+Path.GetExtension(file.FileName);
            using(Stream stream=File.Create(Path.Combine(UploadPath, fileName)))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
        
    }
}
