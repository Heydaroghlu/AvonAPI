using AutoMapper;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.HelperDTOs
{
    public class GetDto
    {
        public static async Task<(Product, AppUser)> GetUserAndProduct(int productId, string appUserId, IUnitOfWork _unitOfWork)
        {
            var product = await _unitOfWork.RepositoryProduct.GetAsync(x => x.Id == productId && !x.IsDeleted);
            if (product == null) throw new ProductNotFoundException();

            var appUser = await _unitOfWork.RepositoryUser.GetAsync(x => x.Id == appUserId && !x.IsBlocked);
            if (appUser == null) throw new UserNotFoundException();

            return new(product, appUser);
        }
    }
}
