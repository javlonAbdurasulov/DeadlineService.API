using DeadlineService.Domain.Models.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface IFilterService<TIn,TOut>
    {
        Task<IQueryable<TOut>> GetFilteredAsync(TIn filterModel,List<TOut> listOut=null);
    }
}
