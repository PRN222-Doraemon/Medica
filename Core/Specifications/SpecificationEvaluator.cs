﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications
{
    public class SpecificationEvaluator<T> where T : class
    {
        /// <summary>
        /// Receives a queryable set of entities and chains expression 
        /// available in the given specification.
        /// </summary>
        /// <param name="inputQuery"></param>
        /// <param name="spec"></param>
        /// <returns>A queryable set of entities</returns>
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = spec.CustomIncludes.Aggregate(query, (current, include) => include(current));

            return query;
        }
    }
}
