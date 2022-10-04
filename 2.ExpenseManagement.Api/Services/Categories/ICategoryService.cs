using System;
using _2.ExpenseManagement.Api.DTOs.Categories;
using CommonLib.DTOs.ResponseModel;

namespace _2.ExpenseManagement.Api.Services.Categories
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get all category.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response<List<CategoryListResponse>>> GetAll(CategoryListRequest request);

        /// <summary>
        /// Add new category.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response<CategoryAddResponse>> Add(CategoryAddRequest request);

        /// <summary>
        /// Edit category.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response<CategoryEditResponse>> Edit(Guid id, CategoryEditRequest request);

        /// <summary>
        /// Delete category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<CategoryDeteteResponse>> Delete(Guid id);
    }
}

