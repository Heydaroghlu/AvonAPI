
using AutoMapper;
using Avon.Application.DTOs;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Avon.Domain.Enums.forOrders;
using Avon.Application.Abstractions.Email;
using Avon.Application.Abstractions.User;
using Avon.Application.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Avon.Domain.Enums.forOrders;
using Avon.Domain.Entitity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IEmailService _emailService;
        readonly IUserService _userService;
        readonly UserManager<AppUser> _userManager;

        public OrdersController(IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService, IUserService userService, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = false, string? orderStat = null, string? deliveryStat = null)
        {
            var entities = await _unitOfWork.RepositoryOrders.GetAllAsync(x => true, false, "OrderItems");
            int pageSize = 10;

            orderStat = orderStat ?? orderStatus.Gözləmədə.ToString();
            deliveryStat = deliveryStat ?? deliveryStatus.Anbarda.ToString();

            if (orderStat.Equals(orderStatus.İmtina)) entities = entities.Where(x => x.Status == orderStatus.İmtina);
            else if (orderStat.Equals(orderStatus.Gözləmədə)) entities = entities.Where(x => x.Status == orderStatus.Gözləmədə);
            else if (orderStat.Equals(orderStatus.Qəbul)) entities = entities.Where(x => x.Status == orderStatus.Qəbul);


            if (deliveryStat.Equals(deliveryStatus.Anbarda)) entities = entities.Where(x => x.DeliveryStatus == deliveryStatus.Anbarda);
            else if (deliveryStat.Equals(deliveryStatus.Kuryerde)) entities = entities.Where(x => x.DeliveryStatus == deliveryStatus.Kuryerde);
            else if (deliveryStat.Equals(deliveryStatus.Catdirildi)) entities = entities.Where(x => x.DeliveryStatus == deliveryStatus.Catdirildi);

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord) || x.Surname.Contains(searchWord));
            }

            var list = PagenatedListDto<Order>.Save(entities, page, pageSize);
            return Ok(list);
        }


        [HttpPost]
        [Route("Manage/ChangeOrderStatus")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, string status, string appUserId)
        {
            var entity = await _unitOfWork.RepositoryOrders.GetAsync(x => x.Id == orderId);
            var appUser = await _userService.GetUser(appUserId);

            if (status.Equals(orderStatus.İmtina)) entity.Status = orderStatus.İmtina;
            else if (status.Equals(orderStatus.Gözləmədə)) entity.Status = orderStatus.Gözləmədə;
            else if (status.Equals(orderStatus.Qəbul)) entity.Status = orderStatus.Qəbul;

            await _unitOfWork.CommitAsync();

            await _emailService.SendEmail(appUser.Email, $"Sifariş statusu", $"Sizin sifarişiniz statusu {status} olaraq dəyişdirildi");

            return Ok(entity);
        }

        [HttpPost]
        [Route("Manage/ChangeDeliveryStatus")]
        public async Task<IActionResult> ChangeDeliveryStatus(int orderId, string status, string appUserId)
        {
            var entity = await _unitOfWork.RepositoryOrders.GetAsync(x => x.Id == orderId);
            var appUser = await _userService.GetUser(appUserId);

            if (status.Equals(deliveryStatus.Anbarda)) entity.DeliveryStatus = deliveryStatus.Anbarda;
            else if (status.Equals(deliveryStatus.Kuryerde)) entity.DeliveryStatus = deliveryStatus.Kuryerde;
            else if (status.Equals(deliveryStatus.Catdirildi)) entity.DeliveryStatus = deliveryStatus.Catdirildi;

            await _unitOfWork.CommitAsync();

            await _emailService.SendEmail(appUser.Email, $"Sifariş statusu", $"Sizin sifarişiniz statusu {status} olaraq dəyişdirildi");

            return Ok(entity);
        }

        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryOrders.GetAsync(x => x.Id == id);
            if (entity == null) return RedirectToAction("NotFound", "Page");

            entity.IsDeleted = true;
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpPost("PostOrder")]
        [Authorize]
        public async Task<IActionResult> Order(OrderCreateDto orderCreateDto)
        {
            AppUser user = null;    
            Order order = _mapper.Map<Order>(orderCreateDto);

            List<CheckOutItemDto> checkOutItemDtos = (await GetCheckOutItems()).ToList();
            if (User.Identity.IsAuthenticated) user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            order.CodePrefix = orderCreateDto.Name[0].ToString().ToUpper() + order.Surname[0].ToString().ToUpper();
            var lastOrder = await (await _unitOfWork.RepositoryOrders.GetAllAsync(x => !x.IsDeleted)).OrderByDescending(x=>x.Id).FirstOrDefaultAsync();
            order.CodeNumber = lastOrder == null ? 1001 : lastOrder.CodeNumber + 1;
         
            if (checkOutItemDtos.Count == 0) return BadRequest();
            if (user == null) return BadRequest();

            order.AppUserId = user?.Id;
            order.Status = orderStatus.Gözləmədə;
            order.OrderItems = new List<OrderItem>();
            order.DeliveryAdressId = orderCreateDto.DeliveryAdressId;
            foreach (var item in checkOutItemDtos)
            {
                OrderItem orderItem = new()
                {
                    ProductID = item.Product.Id,
                    Count = item.Count,
                    CostPrice = item.Product.CostPrice,
                    SalePrice = item.Product.SalePrice,
                    DiscountPrice = item.Product.DiscountPrice,
                    
                };
                (await _unitOfWork.RepositoryProduct.GetAsync(x => x.Id == orderItem.ProductID)).SalePrice += item.Count;
                order.TotalAmount += orderItem.DiscountPrice > 0 ? (orderItem.SalePrice * (1 - orderItem.DiscountPrice / 100)) * orderItem.Count
                    : orderItem.SalePrice * orderItem.Count;
                order.OrderItems.Add(orderItem);
                
            }
            await _unitOfWork.RepositoryOrders.InsertAsync(order);
            if (user != null)
            {
                await _unitOfWork.RepositoryBasketItem.RemoveRange(x => x.AppUserId == user.Id);
                await _unitOfWork.CommitAsync();

            }
            //todo Basketə əlavə olunan məhsul wishlist'dən silinməlidir.
            return Ok("success");
        }
        private async Task<List<CheckOutItemDto>> GetCheckOutItems()
        {
            List<CheckOutItemDto> checkoutItems = new List<CheckOutItemDto>();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            if (user != null && user.IsAdmin == false)
            {
                IQueryable<BasketItem> basketItems = (await _unitOfWork.RepositoryBasketItem.GetAllAsync(x=>x.IsDeleted==false,true,"Product")).Where(x=>x.AppUserId==user.Id);

                foreach (var item in basketItems)
                {
                    CheckOutItemDto checkoutItem = new CheckOutItemDto
                    {
                        Product = item.Product,
                        Count = item.ProductCount
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }
            
            return checkoutItems;
        }
        [HttpGet("TrackOrder")]
        public async Task<IActionResult> TrackOrder(string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Order order = await (await _unitOfWork.RepositoryOrders.GetAllAsync(x=>!x.IsDeleted,false,"OrderItems.Product")).FirstOrDefaultAsync(x=>(x.CodePrefix+x.CodeNumber)==code);            
            return Ok(order);
        }
    }

}
