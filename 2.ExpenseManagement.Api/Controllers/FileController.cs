using System;
using _2.ExpenseManagement.Api.DTOs.File;
using _2.ExpenseManagement.Api.Services.File;
using CommonLib.DTOs.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _2.ExpenseManagement.Api.Controllers
{
    /// <summary>
    /// File controller.
    /// </summary>
    [ApiController]
    [Route("api/files")]
    public class FileController : MyControllerBase
    {
        #region ---- Variables ----
        private readonly IFileService _fileService;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="fileService"></param>
        public FileController(ILogger<FileController> logger,
            IFileService fileService)
            : base(logger)
        {
            _fileService = fileService;
        }
        #endregion

        #region ---- APIs ----
        /// <summary>
        /// upload files.
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<FileUploadResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Response<FileUploadResponse>> Upload([FromForm] FileUploadRequest fileUpload)
        {
            var response = await _fileService.Upload(fileUpload);
            return response;
        }
        #endregion
    }
}

