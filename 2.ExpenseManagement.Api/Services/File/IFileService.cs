using System;
using _2.ExpenseManagement.Api.DTOs.File;
using CommonLib.DTOs.ResponseModel;

namespace _2.ExpenseManagement.Api.Services.File
{
    /// <summary>
    /// File service interface.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Upload files.
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
        Task<Response<FileUploadResponse>> Upload(FileUploadRequest fileUpload);
    }
}

