using System;
using _2.ExpenseManagement.Api.Contants;
using _2.ExpenseManagement.Api.DTOs.ExpenseTypes;
using _2.ExpenseManagement.Api.UoW;
using CommonLib.DTOs.ResponseModel;
using CommonLib.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace _2.ExpenseManagement.Api.Services.ExpenseTypes
{
    public class ExpenseTypeService : BaseService, IExpenseTypeService
    {
        #region ---- Variables ----
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region ---- Constructors ----
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="stringLocalizer"></param>
        public ExpenseTypeService(IUnitOfWork unitOfWork,
            IStringLocalizer<ExpenseTypeService> stringLocalizer)
            : base(stringLocalizer)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region ---- Public methods ----
        /// <summary>
        /// Get all expense type.
        /// </summary>
        /// <returns></returns>
        public async Task<Response<ExpenseTypeListResponse>> GetAll()
        {
            var response = await _unitOfWork.EntityTypeRepository
                .Find(x => x.Type == EntityTypeConst.ExpenseType)
                .Select(x => new ExpenseTypeData
                {
                    ID = x.ID,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return ToResponse(new ExpenseTypeListResponse
            {
                ExpenseTypes = response
            });

        }
        #endregion
    }
}

