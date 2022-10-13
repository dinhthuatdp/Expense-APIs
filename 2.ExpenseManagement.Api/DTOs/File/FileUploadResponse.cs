using System;
namespace _2.ExpenseManagement.Api.DTOs.File
{
    public class FileUploadResponse
    {
        public List<FileUploadData>? Files { get; set; }
    }

    public class FileUploadData
    {
        public string? OriginFileName { get; set; }

        public string? SavedFileName { get; set; }

        public string? Url { get; set; }
    }
}

