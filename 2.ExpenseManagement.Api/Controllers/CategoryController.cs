using System;
using _2.ExpenseManagement.Api.Contants;
using _2.ExpenseManagement.Api.DTOs.Categories;
using _2.ExpenseManagement.Api.Services.Categories;
using CommonLib.DTOs.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _2.ExpenseManagement.Api.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoryController : MyControllerBase
    {
        #region ---- Variables ----
        private readonly ICategoryService _categoryService;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="categoryService"></param>
        public CategoryController(ILogger<CategoryController> logger,
            ICategoryService categoryService)
            :base(logger)
        {
            _categoryService = categoryService;
        }
        #endregion

        #region ---- Actions ----
        /// <summary>
        /// Get All categories.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<CategoryListResponse>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Response<List<CategoryListResponse>>> GetAll([FromQuery] CategoryListRequest request)
        {
            var result = await _categoryService.GetAll(request);

            return result;
        }

        /// <summary>
        /// Add new Category.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CategoryAddResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Response<CategoryAddResponse>> Add(CategoryAddRequest request)
        {
            var result = await _categoryService.Add(request);

            return result;
        }

        /// <summary>
        /// Edit category.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CategoryEditResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id}")]
        public async Task<Response<CategoryEditResponse>> Edit(Guid id, CategoryEditRequest request)
        {
            var result = await _categoryService.Edit(id, request);

            return result;
        }

        /// <summary>
        /// Delete category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CategoryDeteteResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id}")]
        public async Task<Response<CategoryDeteteResponse>> Delete(Guid id)
        {
            var result = await _categoryService.Delete(id);

            return result;
        }
        #endregion
    }
}

