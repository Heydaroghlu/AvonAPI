using Avon.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Abstractions.User
{
    public interface IUserService
    {   
        Task<AppUser> GetUser(string appUserId);
    }
}
