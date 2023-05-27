using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        string CreateAccessToken(AppUser user, int minute);
        List<Claim> CreateClaims(AppUser user);
    }
}
