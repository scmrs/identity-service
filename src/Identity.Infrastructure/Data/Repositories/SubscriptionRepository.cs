﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Application.Data;
using Identity.Application.Data.Repositories;
using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public SubscriptionRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServicePackageSubscription> GetSubscriptionByIdAsync(Guid subscriptionId)
        {
            return await _dbContext.Subscriptions.FindAsync(subscriptionId);
        }

        public async Task<List<ServicePackageSubscription>> GetSubscriptionByUserIdAsync(Guid userId)
        {
            return await _dbContext.Subscriptions
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task AddSubscriptionAsync(ServicePackageSubscription subscription)
        {
            _dbContext.Subscriptions.Add(subscription);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task UpdateSubscriptionAsync(ServicePackageSubscription subscription)
        {
            _dbContext.Subscriptions.Update(subscription);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task DeleteSubscriptionAsync(ServicePackageSubscription subscription)
        {
            _dbContext.Subscriptions.Remove(subscription);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<bool> ExistsSubscriptionByPackageIdAsync(Guid packageId)
        {
            return await _dbContext.Subscriptions.AnyAsync(s => s.PackageId == packageId);
        }
    }
}