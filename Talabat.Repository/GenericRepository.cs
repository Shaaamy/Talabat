﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Witout Spec
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IEnumerable<T>)await _dbContext.Products.Include(P => P.ProductBrand)
                                                .Include(P => P.ProductType)
                                                .ToListAsync();
            }
            else
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {

            }
            return await _dbContext.Set<T>().FindAsync(id);
        }

        #endregion


        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).FirstOrDefaultAsync();
        }


        private IQueryable<T> ApplySpecifications(ISpecifications<T> Spec)
        {
            return SpecificationsEvalutor<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }
    }
}