using System;
using _2.ExpenseManagement.Api.DTOs.Attachments;
using _2.ExpenseManagement.Api.Entities;
using _2.ExpenseManagement.Api.Repositories.Interfaces;
using _2.ExpenseManagement.Api.UoW;
using CommonLib.DTOs.ResponseModel;
using CommonLib.Services;
using Microsoft.Extensions.Localization;

namespace _2.ExpenseManagement.Api.Services.Attachments
{
    /// <summary>
    /// Attachment service.
    /// </summary>
    public class AttachmentService : BaseService, IAttachmentService
    {
        #region ---- Variables ----
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stringLocalizer"></param>
        /// <param name="unitOfWork"></param>
        public AttachmentService(IStringLocalizer<AttachmentService> stringLocalizer,
            IUnitOfWork unitOfWork)
            : base(stringLocalizer)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region ---- Public methods ----
        /// <summary>
        /// Add new attachment.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<AttachmentAddResponse>> Add(AttachmentAddRequest request)
        {
            if (request.Attachments is null ||
                request.Attachments.Count == 0)
            {
                return ToResponse(new AttachmentAddResponse());
            }
            var entities = request.Attachments
                .Select(x => new Attachment
                {
                    ExpenseID = x.ExpenseID,
                    Name = x.Name,
                    Url = x.Url
                });
            await _unitOfWork.AttachmentRepository
                .InsertRange(entities);
            await _unitOfWork.SaveChangeAsync();

            return ToResponse(new AttachmentAddResponse());
        }

        /// <summary>
        /// Add new attachment with new expense.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="expense"></param>
        /// <returns></returns>
        public async Task<Response<AttachmentAddResponse>> Add(AttachmentAddRequest request,
            Expense expense)
        {
            if (request.Attachments is null ||
                request.Attachments.Count == 0)
            {
                return ToResponse(new AttachmentAddResponse());
            }
            var entities = request.Attachments
                .Select(x => new Attachment
                {
                    Expense = expense,
                    Name = x.Name,
                    Url = x.Url
                });
            await _unitOfWork.AttachmentRepository
                .InsertRange(entities);
            await _unitOfWork.SaveChangeAsync();

            return ToResponse(new AttachmentAddResponse());
        }
        #endregion
    }
}

