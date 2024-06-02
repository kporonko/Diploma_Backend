using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.Repositories.impl
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Subscription> GetSubscriptionByUserIdAsync(int userId)
        {
            return await _context.Subscriptions
                .Include(s => s.User)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
