using System;
using _2.ExpenseManagement.Api.DTOs.Categories;
using _2.ExpenseManagement.Api.UoW;
using System.Linq;
using _2.ExpenseManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using _2.ExpenseManagement.Api.Contants;
using CommonLib.DTOs.ResponseModel;
using CommonLib.Services;

namespace _2.ExpenseManagement.Api.Services.Categories
{
    /// <summary>
    /// Category Services.
    /// </summary>
    public class CategoryService : BaseService, ICategoryService
    {
        #region ---- Variables ----
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<CategoryService> _stringLocalizer;
        #endregion

        #region ---- Constructors -----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        /// <param name="stringLocalizer"></param>
        public CategoryService(IUnitOfWork unitOfWork,
            ILogger<CategoryService> logger,
            IStringLocalizer<CategoryService> stringLocalizer)
            //: base(logger)
        {
            _unitOfWork = unitOfWork;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region ---- Actions ----
        /// <summary>
        /// Add new category.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<CategoryAddResponse>> Add(CategoryAddRequest request)
        {
            string messageError;
            var isNameExists = _unitOfWork.CategoryRepository
                .GetAll()
                .Any(x => !string.IsNullOrEmpty(x.Name) &&
                    x.Name.Equals(request.Name));
            if (isNameExists)
            {
                messageError = _stringLocalizer[MessageErrorCode.IsExists].ToString();
                return ToErrorResponse<CategoryAddResponse>(ResponseStatusCode.Error,
                    string.Format(messageError, "Category name"));
            }
            var model = new Category
            {
                Name = request.Name,
                CreatedBy = "Thuat",
                CreatedDate = DateTime.UtcNow,
            };
            _unitOfWork.CategoryRepository.Insert(model);
            await _unitOfWork.SaveChangeAsync();

            return ToResponse<CategoryAddResponse>(new CategoryAddResponse
            {
                Success = true
            }, ResponseStatusCode.Success);
        }

        /// <summary>
        /// Edit category.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<CategoryEditResponse>> Edit(Guid id, CategoryEditRequest request)
        {
            string errorMessage;
            if (request is null)
            {
                errorMessage = _stringLocalizer[MessageErrorCode.Required].ToString();
                return ToErrorResponse<CategoryEditResponse>(ResponseStatusCode.Error,
                    string.Format(errorMessage, "request"));
            }
            var category = await _unitOfWork.CategoryRepository
                .GetById(id);
            if (category is null)
            {
                errorMessage = _stringLocalizer[MessageErrorCode.NotFound].ToString();
                return ToErrorResponse<CategoryEditResponse>(ResponseStatusCode.Error,
                    string.Format(errorMessage, "Category"));
            }
            category.Name = request.Name;
            await _unitOfWork.SaveChangeAsync();

            return ToResponse<CategoryEditResponse>(new CategoryEditResponse
            {
                IsSuccess = true
            }, ResponseStatusCode.Success);
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<List<CategoryListResponse>>> GetAll(CategoryListRequest request)
        {
            var data = await _unitOfWork.CategoryRepository.GetAll()
                .ToListAsync();
            var result = data.Select(x => new CategoryListResponse
            {
                ID = x.ID,
                Name = x.Name
            });

            return ToResponse<List<CategoryListResponse>>(result.ToList(),
                ResponseStatusCode.Success);
        }
        #endregion
    }
}

