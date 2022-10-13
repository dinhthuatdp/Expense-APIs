using System;
using _2.ExpenseManagement.Api.DTOs.Attachments;
using _2.ExpenseManagement.Api.Services.Attachments;
using CommonLib.DTOs.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _2.ExpenseManagement.Api.Controllers
{
    /// <summary>
    /// Attachment controller.
    /// </summary>
    [ApiController]
    [Route("api/attachments")]
    public class AttachmentController : MyControllerBase
    {
        #region ---- Variables ----
        private readonly IAttachmentService _attachmentService;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="attachmentService"></param>
        public AttachmentController(ILogger<AttachmentController> logger,
            IAttachmentService attachmentService)
            : base(logger)
        {
            _attachmentService = attachmentService;
        }
        #endregion

        #region ---- APIs ----
        /// <summary>
        /// Add attachments.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<Response<AttachmentAddResponse>> Add(AttachmentAddRequest request)
        {
            var response = await _attachmentService.Add(request);

            return response;
        }
        #endregion
    }
}

