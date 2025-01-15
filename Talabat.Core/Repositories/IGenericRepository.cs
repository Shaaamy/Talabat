using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Spec
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);


        #endregion

        #region With Spec
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<T> GetByIdWithSpecAsync(ISpecifications<T> Spec); 
        Task<int> GetCountOfSpecAsync(ISpecifications<T> Spec);
        Task AddAsync(T item);
        #endregion

    }
}
