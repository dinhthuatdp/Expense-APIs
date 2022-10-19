using System;
using _2.ExpenseManagement.Api.Contants;
using _2.ExpenseManagement.Api.DTOs.Attachments;
using _2.ExpenseManagement.Api.DTOs.Categories;
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
        private const string STR_EXPENSE = "Expense";
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
            var dataSearch = GetSearchParams(request);
            var filter = ExpressionExtension.BuildFilter<ExpenseListData>(ExpressionMethods.Contains,
                dataSearch, ExpressionCondition.Or);

            var currentUser = CurrentUser.User;
            var dataQuery = _unitOfWork.ExpenseRepository
                .Find(x => x.CreatedBy == currentUser.UserName &&
                x.DeletedDate == null)
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
                })
                .Where(filter)
                .OrderByDescending(x => x.Date);
            var (data, total) = await dataQuery.Paging(request);
            var response = new ExpenseListResponse
            {
                Expenses = data
            };
            return ToPagedResponse<ExpenseListResponse>(total
                , response, request);
        }

        public async Task<Response<ExpenseDetailsResponse>> Get(Guid id)
        {
            string messageError;
#pragma warning disable CS8604 // Possible null reference argument.
            var expense = await _unitOfWork.ExpenseRepository
                .Find(x => x.ID == id)
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Include(x => x.Attachments.Where(a => a.DeletedDate == null))
                .FirstOrDefaultAsync();
#pragma warning restore CS8604 // Possible null reference argument.

            if (expense is null)
            {
                messageError = _stringLocalizer[MessageErrorCode.NotFound].ToString();
                return ToErrorResponse<ExpenseDetailsResponse>(ResponseStatusCode.NotFound,
                    string.Format(messageError, STR_EXPENSE));
            }

            return ToResponse(new ExpenseDetailsResponse
            {
                Category = expense.Category?.Name,
                CategoryID = expense.Category == null ? Guid.Empty : expense.Category.ID,
                ID = expense.ID,
                Type = expense.Type?.Name,
                TypeID = expense.Type == null ? Guid.Empty : expense.Type.ID,
                Cost = expense.Cost,
                Date = expense.Date,
                Description = expense.Description,
                Attachments = expense.Attachments == null ? null
                    : expense.Attachments?.Select(x => x.Url),
            });
        }

        /// <summary>
        /// Edit expense.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<ExpenseEditResponse>> Edit(Guid id, ExpenseEditRequest request)
        {
            string messageError;
            var expense = await _unitOfWork.ExpenseRepository
                .GetById(id);
            if (expense is null)
            {
                messageError = _stringLocalizer[MessageErrorCode.NotFound].ToString();
                return ToErrorResponse<ExpenseEditResponse>(ResponseStatusCode.NotFound,
                    string.Format(messageError, STR_EXPENSE));
            }

            expense.CategoryID = request.CategoryID;
            expense.TypeID = request.TypeID;
            expense.Date = request.Date;
            expense.Cost = request.Cost;
            expense.Description = request.Description;
            // Upload attachments.
            if (request.Attachments is null ||
                !request.Attachments.Any())
            {
                expense.Attachments = null;
                await _unitOfWork.SaveChangeAsync();
                return ToResponse(new ExpenseEditResponse());
            }
            // Store files to folder.
            var uploadedFiles = await UploadFiles(request.Attachments);
            // Add url stored to db.
            var attachmentRes = await EditAttachments(uploadedFiles, expense);
            if (attachmentRes is null)
            {
                return ToErrorResponse<ExpenseEditResponse>(ResponseStatusCode.Error,
                    "Edit Expense Attachments error");
            }

            return ToResponse(new ExpenseEditResponse());
        }

        public async Task<Response<ExpenseDeleteResponse>> Delete(Guid id)
        {
            string messageError;
            var expense = await _unitOfWork.ExpenseRepository
                .GetById(id);
            if (expense is null)
            {
                messageError = _stringLocalizer[MessageErrorCode.NotFound].ToString();
                return ToErrorResponse<ExpenseDeleteResponse>(ResponseStatusCode.NotFound,
                    string.Format(messageError, STR_EXPENSE));
            }
            _unitOfWork.ExpenseRepository
                .Delete(expense);
            await _unitOfWork.SaveChangeAsync();

            return ToResponse(new ExpenseDeleteResponse());
        }

        public async Task<Response<ExpenseSpentSnapshotResponse>> GetExpenseSnapshot(ExpenseSpentSnapshotRequest request)
        {
            var today = DateTime.UtcNow.Date;
            var month = today.Month;
            var expenseQuery = _unitOfWork.ExpenseRepository
                .Find(x => x.DeletedDate == null &&
                x.Date != null && x.Date.Value.Month == month);
            var group = await expenseQuery
                .GroupBy(x => x.Date)
                .Select(x => new
                {
                    Today = x.Key == today ? x.Sum(c => c.Cost) : 0,
                    Yesterday = x.Key == today.AddDays(-1) ? x.Sum(c => c.Cost) : 0,
                    Month = x.Sum(c => c.Cost)
                })
                .ToListAsync();

            var commonQuery = _unitOfWork.ExpenseRepository
                .Find(x => x.DeletedDate == null &&
                    x.Date != null && x.Date.Value.Year == today.Year)
                .Join(_unitOfWork.CategoryRepository
                    .Find(x => x.DeletedDate == null &&
                        x.IsCommon == true),
                x => x.CategoryID, y => y.ID,
                (x, y) => new
                {
                    Expense = x,
                    Date = x.Date,
                    Category = y.Name
                });
#pragma warning disable CS8629 // Nullable value type may be null.
            var commonList = await commonQuery
                .Where(x => x.Date.Value.Month < today.Month)
                .GroupBy(x => x.Category)
                .Select(x => new CommonData
                {
                    PassAverage = x.Average(c => c.Expense.Cost),
                    SpentExtra = x.Average(c => c.Expense.Cost) -
                        x.Where(c => c.Date.Value.Month == today.Month).Sum(c => c.Expense.Cost),
                    ThisMonth = x.Where(c => c.Date.Value.Month == today.Month).Sum(c => c.Expense.Cost),
                    CategoryName = x.Key
                })
                .ToListAsync();
#pragma warning restore CS8629 // Nullable value type may be null.

            return ToResponse(new ExpenseSpentSnapshotResponse
            {
                SoFarThisMonth = group.Sum(x => x.Month),
                Today = group.Sum(x => x.Today),
                Yesterday = group.Sum(x => x.Yesterday),
                CommonDatas = commonList
            });
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
        private async Task<AttachmentEditResponse?> EditAttachments(List<FileUploadData>? files
            , Expense expense)
        {
            if (files is null ||
                expense is null)
            {
                return new AttachmentEditResponse();
            }
            var attachments = files
                .Select(x => new AttachmentAddData
                {
                    Name = x.OriginFileName,
                    Url = x.Url
                })
                .ToList();
            var attachmentResonse = await _attachmentService.Edit(new DTOs.Attachments.AttachmentEditRequest
            {
                Attachments = attachments
            }, expense);
            if (attachmentResonse is null ||
                !string.IsNullOrEmpty(attachmentResonse.Message) ||
                (attachmentResonse.Status != null &&
                attachmentResonse.Status.StatusCode != ResponseStatusCode.Success))
            {
                _logger.LogError("Edit Expense Attachment Error", attachmentResonse);
                return null;
            }

            return new AttachmentEditResponse();
        }
        #endregion
    }
}

