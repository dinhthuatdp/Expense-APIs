using System;
using _2.ExpenseManagement.Api.DTOs.File;
using CommonLib.DTOs.ResponseModel;
using CommonLib.Services;
using Microsoft.Extensions.Localization;

namespace _2.ExpenseManagement.Api.Services.File
{
    /// <summary>
    /// File service.
    /// </summary>
    public class FileService : BaseService, IFileService
    {
        #region ---- Variables ----
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<FileService> _logger;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stringLocalizer"></param>
        /// <param name="logger"></param>
        /// <param name="hostingEnvironment"></param>
        public FileService(IStringLocalizer<FileService> stringLocalizer,
            ILogger<FileService> logger,
            IWebHostEnvironment hostingEnvironment)
            : base(stringLocalizer)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        #endregion

        #region ---- Public methods ----
        /// <summary>
        /// Upload file.
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
        public async Task<Response<FileUploadResponse>> Upload(FileUploadRequest fileUpload)
        {
            var fileDic = "Files";
            string dicPath = Path.Combine(_hostingEnvironment.ContentRootPath, fileDic);
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            if (fileUpload.Files is null ||
                fileUpload.Files.Count == 0)
            {
                return ToErrorResponse<FileUploadResponse>(ResponseStatusCode.NotFound,
                    "File cannot empty");
            }
            string filePath;
            FileUploadResponse response = new FileUploadResponse()
            {
                Files = new List<FileUploadData>()
            };
            string name;
            fileUpload.Files.ForEach(async file =>
            {
                if (file.Length < 0)
                {
                    return;
                }
                name = $"{file.FileName}__{Guid.NewGuid().ToString()}";
                filePath = Path.Combine(dicPath,
                    name);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    try
                    {;
                        response.Files.Add(new FileUploadData
                        {
                            OriginFileName = file.FileName,
                            SavedFileName = name,
                            Url = filePath
                        });
                        await file.CopyToAsync(stream);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Upload file error", ex);
                        stream.Dispose();
                    }
                }
            });

            return ToResponse(response, ResponseStatusCode.Success);
        }
        #endregion
    }
}

