﻿using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace KitchenDeliverySystem.Infra.Persistence
{
    [ExcludeFromCodeCoverage]
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() =>
            {
                _context.Set<T>().Update(entity);
            });
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() =>
            {
                _context.Set<T>().Remove(entity);
            });
        }
    }
}
