using AutoMapper;
using Avon.Application.Abstractions.User;
using Avon.Application.DTOs.HelperDTOs;
using Avon.Application.Exceptions;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListsController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IUserService _userService;

        public WishListsController(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        [HttpPost]
        [Route("AddWishList")]
        public async Task<IActionResult> AddWishList(int productId, string appUserId)
        {
            //var userAndProduct = await GetUserAndProduct(productId, appUserId);
            var userAndProduct = await GetDto.GetUserAndProduct(productId, appUserId, _unitOfWork);

            var wishListItem = await _unitOfWork.RepositoryWishList.GetAsync(x => x.ProductId == productId && x.AppUserId == appUserId);

            if (wishListItem == null)
            {
                wishListItem = new()
                {
                    ProductId = productId,
                    AppUserId = appUserId
                };
                await _unitOfWork.RepositoryWishList.InsertAsync(wishListItem);
            }
            else
            {
                throw new WishListExistExcetion("You did add this product");
            }

            await _unitOfWork.CommitAsync();

            return Ok(wishListItem);
        }

        [HttpPost]
        [Route("RemoveWishList")]
        public async Task<IActionResult> RemoveWishList(int productId, string appUserId)
        {
            var userAndProduct = await GetDto.GetUserAndProduct(productId, appUserId, _unitOfWork);

            //var userAndProduct = await GetUserAndProduct(productId, appUserId);

            var wishListItem = await _unitOfWork.RepositoryWishList.GetAsync(x => x.ProductId == productId && x.AppUserId == appUserId);

            if (wishListItem == null) return NotFound("Couldn't find any wihhList element");

            await _unitOfWork.RepositoryWishList.Remove(x=>x.Id == wishListItem.Id);
            await _unitOfWork.CommitAsync();

            return Ok();
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(string appUserId)
        {
            var appUser = await _userService.GetUser(appUserId);
            if (appUser == null) throw new UserNotFoundException();

            var wishLists = await _unitOfWork.RepositoryWishList.GetAllAsync(x => true,false);

            return Ok(wishLists.ToList());
        }

    }
}
