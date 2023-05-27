using Avon.Application.Abstractions.User;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Persistence.Concrets.User
{
    public class UserService : IUserService
    {

        readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> GetUser(string appUserId)
        {
            return await _userManager.FindByIdAsync(appUserId);
        }
    }
}
