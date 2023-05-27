using AutoMapper;
using Avon.Application.Abstractions.User;
using Avon.Application.DTOs.HelperDTOs;
using Avon.Application.Exceptions;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Avon.Domain.Entitity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IUserService _userService;
        public BasketsController(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        [HttpPost]
        [Route("AddBasket")]
        public async Task<IActionResult> AddBasket(int productId, string appUserId)
        {
            //var userAndProduct = await GetUserAndProduct(productId, appUserId);
            var userAndProduct = await GetDto.GetUserAndProduct(productId, appUserId, _unitOfWork);

            var basketItem = await _unitOfWork.RepositoryBasketItem.GetAsync(x => x.ProductId == productId && x.AppUserId == appUserId);

            if (basketItem != null)
            {
                basketItem.ProductCount++;
            }
            else
            {
                basketItem = new()
                {
                    ProductId = productId,
                    AppUserId = appUserId
                };
                await _unitOfWork.RepositoryBasketItem.InsertAsync(basketItem);
            }

            await _unitOfWork.CommitAsync();

            return Ok(basketItem);
        }

        [HttpPost]
        [Route("RemoveBasket")]
        public async Task<IActionResult> RemoveBasket(int productId, string appUserId)
        {
            var userAndProduct = await GetDto.GetUserAndProduct(productId, appUserId, _unitOfWork);

            //var userAndProduct = await GetUserAndProduct(productId, appUserId);

            var basketItem = await _unitOfWork.RepositoryBasketItem.GetAsync(x => x.ProductId == productId && x.AppUserId == appUserId);

            if (basketItem == null) return NotFound("Couldn't find basket element");

            if (basketItem.ProductCount == 1) await _unitOfWork.RepositoryBasketItem.Remove(x => x.Id == basketItem.Id);
            else basketItem.ProductCount--;

            await _unitOfWork.CommitAsync();
            
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(string appUserId)
        {
            var appUser = await _userService.GetUser(appUserId);
            if (appUser == null) throw new UserNotFoundException();

            var basketLists = await _unitOfWork.RepositoryBasketItem.GetAllAsync(x => true, false);

            return Ok(basketLists.ToList());
        }

    }
}
