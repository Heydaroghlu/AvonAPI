using AutoMapper;
using Avon.Application.DTOs.SubCategoryDTOs;
using Avon.Application.DTOs;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.DTOs.DeliveryAddressDTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAddressesController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;

        public DeliveryAddressesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {
            var entities = await _unitOfWork.RepositoryDeliveryAddresses.GetAllAsync(x => true, false,"Orders");
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord));
            }

            var list = PagenatedListDto<DeliveryAdress>.Save(entities, page, pageSize);

            return Ok(list);
        }

        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create(DeliveryAddressCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existEntity = await _unitOfWork.RepositoryDeliveryAddresses.GetAsync(x => x.Name.ToLower().Equals(objectDto.Name.ToLower()), false);
            if (existEntity != null) return BadRequest($"{objectDto.Name} has been already created");

            var entity = _mapper.Map<DeliveryAdress>(objectDto);

            await _unitOfWork.RepositoryDeliveryAddresses.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Ok(entity);
        }


        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit(int id, DeliveryAddressEditDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existObject = await _unitOfWork.RepositoryDeliveryAddresses.GetAsync(x => x.Id == id);

            if (existObject == null) throw new DeliveryAdressNotFoundException();

            existObject.Name = objectDto.Name;
            
            await _unitOfWork.CommitAsync();

            return Ok(existObject);
        }

        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryDeliveryAddresses.GetAsync(x => x.Id == id, true);

            if (entity == null) throw new DeliveryAdressNotFoundException();

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();
            return Ok();
        }


        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var entityObject = await _unitOfWork.RepositoryDeliveryAddresses.GetAsync(x => x.Id == id);
            if (entityObject == null) throw new DeliveryAdressNotFoundException();
            return Ok(entityObject);
        }
    
        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositoryDeliveryAddresses.GetAsync(x => x.Id == id, true);

            if (entity == null) throw new DeliveryAdressNotFoundException();

            entity.IsDeleted = false;

            await _unitOfWork.CommitAsync();
            return Ok();
        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var deliveryAddressQuery = await _unitOfWork.RepositoryDeliveryAddresses.GetAllAsync(x => !x.IsDeleted,false);

            var deliveryAddress = deliveryAddressQuery.ToList();

            if(deliveryAddress == null) throw new DeliveryAdressNotFoundException();

            return Ok(deliveryAddress);
        }

    }
}
