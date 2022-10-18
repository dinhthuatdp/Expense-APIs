using System;
using _2.ExpenseManagement.Api.DTOs.Attachments;
using _2.ExpenseManagement.Api.Entities;
using CommonLib.DTOs.ResponseModel;

namespace _2.ExpenseManagement.Api.Services.Attachments
{
    /// <summary>
    /// Attachment service interface.
    /// </summary>
    public interface IAttachmentService
    {
        /// <summary>
        /// Add new attachment.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response<AttachmentAddResponse>> Add(AttachmentAddRequest request);

        /// <summary>
        /// Add new attachment with new expense.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="expense"></param>
        /// <returns></returns>
        Task<Response<AttachmentAddResponse>> Add(AttachmentAddRequest request, Expense expense);

        Task<Response<AttachmentEditResponse>> Edit(AttachmentEditRequest request, Expense expense);
    }
}

