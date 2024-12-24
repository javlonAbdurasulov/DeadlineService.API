using DeadlineService.Domain.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace DeadlineService.Application.Services
{
    public  class PaginationService<T>
    {
        public  async Task<PaginatedResponse<T>> GetPaginatedAsyncData(IQueryable<T> items, PaginationParametrs paginationParametrs)
        {

            int totalItems = await items.CountAsync();
            int totalPages = (int)Math.Ceiling((totalItems) / (double)paginationParametrs.PageSize);

            var paginatedData = await items.Skip((paginationParametrs.PageNumber - 1) * paginationParametrs.PageSize).
                Take(paginationParametrs.PageSize).
                ToListAsync();

            return new PaginatedResponse<T>
            {
                PageNumber = paginationParametrs.PageNumber,
                PageSize = paginationParametrs.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Data = paginatedData
            };
        }


    }
}
