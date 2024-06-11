using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Repositories.Payment
{
    public interface IPaymentRepository
    {
        Subscription GetSubscriptionById(string subscriptionId);
        Task AddSubscriptionAsync(Subscription subscription);
        void UpdateSubscription(Subscription subscription);
        User GetUserById(int userId);
        Subscription GetSubscriptionByUserId(int id);
        Task DeleteSubscriptionAsync(Subscription subscription);
    }
}
