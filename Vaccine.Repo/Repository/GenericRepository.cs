using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using Vaccine.Repo.Entities;
//using Vaccine.Repo.Models;

namespace Vaccine.Repo.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        
        internal VaccineDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(VaccineDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }


        public IQueryable<TEntity> GetQueryable()
        {
            return dbSet;
        } 

        // Updated Get method with pagination
        public virtual IEnumerable<TEntity> Get(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "",
    int? pageIndex = null,
    int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;

            // Apply filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include related entities
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // Apply ordering if provided
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implement pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Default page size of 10

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return query.ToList();
        }

        // New Search method
        public virtual IEnumerable<TEntity> Search(
    Expression<Func<TEntity, bool>> searchExpression,
    string includeProperties = "",
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    int? pageIndex = null,
    int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (searchExpression != null)
            {
                query = query.Where(searchExpression);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            // Switching to in-memory execution for custom method
            var result = query.AsEnumerable();

            return result;
        }

        public virtual TEntity GetByID(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (entityToDelete != null)
            {
                if (entityToDelete.GetType().GetProperty("Delete")?.PropertyType == typeof(int))
                {
                    // Perform soft delete
                    SoftDelete(entityToDelete);
                }
                else
                {
                    // Perform hard delete
                    Delete(entityToDelete);
                }
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void SoftDelete(TEntity entityToDelete)
        {
            var propertyInfo = entityToDelete.GetType().GetProperty("Delete");
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(int))
            {
                propertyInfo.SetValue(entityToDelete, 0);
                context.Entry(entityToDelete).State = EntityState.Modified;
            }
            else
            {
                throw new InvalidOperationException("The entity does not have a 'Delete' property of type int.");
            }
        }



        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.Count();
        }

        // Method to remove Vietnamese accents
        public static string RemoveVietnameseAccents(string text)
        {
            string[] VietnameseSigns = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    text = text.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return text;
        }

        // Method to normalize expression
        private Expression<Func<TEntity, bool>> NormalizeExpression(Expression<Func<TEntity, bool>> expression)
        {
            var parameter = expression.Parameters[0];
            var body = expression.Body;

            var visitor = new NormalizeVisitor();
            var newBody = visitor.Visit(body);

            return Expression.Lambda<Func<TEntity, bool>>(newBody, parameter);
        }

        private class NormalizeVisitor : ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member.MemberType == System.Reflection.MemberTypes.Property)
                {
                    var newNode = Expression.Call(
                        typeof(GenericRepository<TEntity>).GetMethod(nameof(RemoveVietnameseAccents), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static),
                        node);

                    return newNode;
                }

                return base.VisitMember(node);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                if (node.Type == typeof(string))
                {
                    var value = (string)node.Value;
                    var newValue = RemoveVietnameseAccents(value);

                    return Expression.Constant(newValue, typeof(string));
                }

                return base.VisitConstant(node);
            }
        }
    }
}
