using System;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Http;

namespace CommonLib.Extensions
{
    public static class FileExtension
    {
        public static string? MapStaticFile(this string img,
            IHttpContextAccessor httpContextAccessor)
        {
            if (img is null)
            {
                return null;
            }
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            var sss = new UriBuilder
            {
                Scheme = httpContextAccessor.HttpContext.Request.Scheme,
                Host = httpContextAccessor.HttpContext.Request.Host.Host,
                Port = httpContextAccessor.HttpContext.Request.Host.Port ?? -1,
                Path = "/Files"
            }.ToString();
            var result = img.Replace(directory, sss);

            return result;
        }
    }
}

