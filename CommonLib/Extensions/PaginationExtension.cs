using System;
using CommonLib.DTOs.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace CommonLib.Extensions
{
    public static class PaginationExtension
    {
        /// <summary>
        /// Paging and return data paged & total page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="paginationFilter"></param>
        /// <returns></returns>
        public static async Task<(IEnumerable<T>, int)> Paging<T>(this IQueryable<T> query,
            PaginationFilter paginationFilter)
            where T : class
        {
            var dataPaged = await query.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToListAsync();

            return (dataPaged, await query.CountAsync());
        }
    }
}

