using Microsoft.EntityFrameworkCore;
using SalesFlow.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SalesFlow.DataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyGlobalQueryFilter(this ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes().Where(x => typeof(BaseEntity).IsAssignableFrom(x.ClrType));

            foreach (var entityType in entityTypes)
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
}