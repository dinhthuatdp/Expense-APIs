using System;
using _2.ExpenseManagement.Api.DTOs.Attachments;
using _2.ExpenseManagement.Api.DTOs.Expense;
using _2.ExpenseManagement.Api.DTOs.File;
using _2.ExpenseManagement.Api.Entities;
using _2.ExpenseManagement.Api.Services.Attachments;
using _2.ExpenseManagement.Api.Services.File;
using _2.ExpenseManagement.Api.UoW;
using CommonLib.DTOs.RequestModel;
using CommonLib.DTOs.ResponseModel;
using CommonLib.Extensions;
using CommonLib.Middlewares;
using CommonLib.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace _2.ExpenseManagement.Api.Services.Expenses
{
    /// <summary>
    /// Expense Service.
    /// </summary>
    public class ExpenseService : BaseService, IExpenseService
    {
        #region ---- Variables ----
        private readonly ILogger<ExpenseService> _logger;
        private readonly IFileService _fileService;
        private readonly IAttachmentService _attachmentService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stringLocalizer"></param>
        /// <param name="logger"></param>
        /// <param name="fileService"></param>
        /// <param name="attachmentService"></param>
        /// <param name="unitOfWork"></param>
        public ExpenseService(IStringLocalizer<ExpenseService> stringLocalizer,
            ILogger<ExpenseService> logger,
            IFileService fileService,
            IAttachmentService attachmentService,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
            : base(stringLocalizer, httpContextAccessor)
        {
            _logger = logger;
            _fileService = fileService;
            _attachmentService = attachmentService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region ---- Public methods ----
        /// <summary>
        /// Add new expense.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<ExpenseAddResponse>> Add(ExpenseAddRequest request)
        {
            if (request.Cost is null ||
                request.TypeID is null ||
                request.CategoryID is null)
            {
                _logger.LogError("Cost/TypeID/CategoryID is required", request);
                return ToErrorResponse<ExpenseAddResponse>(ResponseStatusCode.Error,
                    "Cost/TypeID/CategoryID is required");
            }
            var expense = new Expense
            {
                Cost = request.Cost.Value,
                Date = request.Date,
                Description = request.Description,
                TypeID = request.TypeID.Value,
                CategoryID = request.CategoryID.Value
            };
            _unitOfWork.ExpenseRepository
                .Insert(expense);

            if (request.Attachments is null)
            {
                await _unitOfWork.SaveChangeAsync();

                return ToResponse(new ExpenseAddResponse());
            }
            // Store files to folder.
            var uploadedFiles = await UploadFiles(request.Attachments);
            // Add url stored to db.
            var attachmentRes = await AddAttachments(uploadedFiles, expense);
            if (attachmentRes is null)
            {
                return ToErrorResponse<ExpenseAddResponse>(ResponseStatusCode.Error,
                    "Add Attachments error");
            }
            await _unitOfWork.SaveChangeAsync();
            return ToResponse(new ExpenseAddResponse());
        }

        public async Task<Response<ExpenseListResponse>> GetAll(ExpenseListRequest request)
        {
            if (CurrentUser.User is null)
            {
                return ToErrorResponse<ExpenseListResponse>(ResponseStatusCode.Error,
                    "Current user not found");
            }
            var currentUser = CurrentUser.User;
            var dataQuery = _unitOfWork.ExpenseRepository
                .Find(x => x.CreatedBy == currentUser.UserName)
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Select(x => new ExpenseListData
                {
                    ID = x.ID,
                    Category = x.Category == null ? string.Empty : x.Category.Name,
                    Type = x.Type == null ? string.Empty : x.Type.Name,
                    CategoryID = x.Category == null ? Guid.Empty : x.Category.ID,
                    TypeID = x.Type == null ? Guid.Empty : x.Type.ID,
                    Cost = x.Cost,
                    Date = x.Date,
                    Description = x.Description
                });
            var (data, total) = await dataQuery.Paging(request);
            var response = new ExpenseListResponse
            {
                Expenses = data
            };
            return ToPagedResponse<ExpenseListResponse>(total
                , response, request);
        }
        #endregion

        #region ---- Private methods ----
        /// <summary>
        /// Upload files to folder.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private async Task<List<FileUploadData>?> UploadFiles(List<IFormFile> files)
        {
            var uploadedFiles = await _fileService.Upload(new DTOs.File.FileUploadRequest
            {
                Files = files
            });
            if (uploadedFiles is null ||
                !string.IsNullOrEmpty(uploadedFiles.Message) ||
                (uploadedFiles.Status != null &&
                uploadedFiles.Status.StatusCode != ResponseStatusCode.Success))
            {
                string? message = string.IsNullOrEmpty(uploadedFiles?.Message)
                    ? uploadedFiles?.Status?.Status
                    : uploadedFiles.Message;
                _logger.LogError("Upload files error", uploadedFiles);
                return null;
            }

            return uploadedFiles.Data?.Files;
        }

        /// <summary>
        /// Add attachment files.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="expense"></param>
        /// <returns></returns>
        private async Task<AttachmentAddResponse?> AddAttachments(List<FileUploadData>? files
            , Expense expense)
        {
            if (files is null ||
                expense is null)
            {
                return new AttachmentAddResponse();
            }
            var attachments = files
                .Select(x => new AttachmentAddData
                {
                    Name = x.OriginFileName,
                    Url = x.Url
                })
                .ToList();
            var attachmentResonse = await _attachmentService.Add(new DTOs.Attachments.AttachmentAddRequest
            {
                Attachments = attachments
            }, expense);
            if (attachmentResonse is null ||
                !string.IsNullOrEmpty(attachmentResonse.Message) ||
                (attachmentResonse.Status != null &&
                attachmentResonse.Status.StatusCode != ResponseStatusCode.Success))
            {
                _logger.LogError("Add Expense Attachment Error", attachmentResonse);
                return null;
            }

            return new AttachmentAddResponse();
        }
        #endregion
    }
}

