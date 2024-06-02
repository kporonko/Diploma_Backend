using Diploma.Backend.Application.Repositories.Payment;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.PayPal.Repositories.impl
{
    public class PayPalRepository : IPaymentRepository
    {
        private readonly ApplicationContext _dbContext;

        public PayPalRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Subscription GetSubscriptionById(string subscriptionId)
        {
            return _dbContext.Subscriptions.FirstOrDefault(x => x.SubscriptionId == subscriptionId);
        }

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            _dbContext.Subscriptions.Add(subscription);
            await _dbContext.SaveChangesAsync();
        }

        public void UpdateSubscription(Subscription subscription)
        {
            _dbContext.Subscriptions.Update(subscription);
            _dbContext.SaveChanges();
        }

        public User GetUserById(int userId)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == userId);
        }

        public Subscription GetSubscriptionByUserId(int id)
        {
            return _dbContext.Subscriptions.FirstOrDefault(s => s.UserId == id);
        }
    }
}
