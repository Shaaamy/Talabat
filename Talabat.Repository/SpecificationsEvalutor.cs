using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationsEvalutor<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecifications<T> Spec)
        {
            var Query = InputQuery; // _dbContext.Set<T>()
            if (Spec.Criteria is not null) // P=>P.Id==id
            {
                Query = Query.Where(Spec.Criteria); //_dbContext.Set<T>().Where(P=>P.Id==id)
            }
            // P=>P.ProductBrand , P=>P.ProductType
            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            //_dbContext.Set<T>().Where(P=>P.Id==id).Include(P=>P.ProductBrand) 
            //_dbContext.Set<T>().Where(P=>P.Id==id).Include(P=>P.ProductBrand).Include(P=>P.ProductType) 

            return Query;
        }
    }
}
