using AutoMapper;
using Avon.Application.DTOs.TagDTOs;
using Avon.Application.DTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.DTOs.VFeatureDTOs;

namespace Avon.Api.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class VFeaturesController : ControllerBase
	{
		readonly IMapper _mapper;
		readonly IUnitOfWork _unitOfWork;


		public VFeaturesController(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		[Route("Manage/GetAll")]
		public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
		{
			var entities = await _unitOfWork.RepositoryVFeature.GetAllAsync(x => true, false);
			int pageSize = 10;

			if (isDeleted == true)
				entities = entities.Where(x => x.IsDeleted);
			if (isDeleted == false)
				entities = entities.Where(x => !x.IsDeleted);

			if (string.IsNullOrWhiteSpace(searchWord) == false)
			{
				entities = entities.Where(x => x.Name.Contains(searchWord));
			}

			var list = PagenatedListDto<VFeature>.Save(entities, page, pageSize);

			return Ok(list);
		}

		[HttpPost]
		[Route("Manage/Create")]
		public async Task<IActionResult> Create(CreateVFeatureDto objectDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var existEntity = await _unitOfWork.RepositoryTeg.GetAsync(x => x.Name.ToLower().Equals(objectDto.Name.ToLower()), false);
			if (existEntity != null) return BadRequest($"{objectDto.Name} has been already created");

			var entity = _mapper.Map<VFeature>(objectDto);

			await _unitOfWork.RepositoryVFeature.InsertAsync(entity);
			await _unitOfWork.CommitAsync();

			return Ok(entity);
		}

		[HttpPost]
		[Route("Manage/Edit")]
		public async Task<IActionResult> Edit(int id, EditVFeatureDto objectDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var existObject = await _unitOfWork.RepositoryVFeature.GetAsync(x => x.Id == id);

			if (existObject == null) return BadRequest("Variant Feature not found");

			existObject.Name = objectDto.Name;
			existObject.Variable=objectDto.Variable;

			await _unitOfWork.CommitAsync();

			return Ok(existObject);
		}


		[HttpDelete]
		[Route("Manage/Delete")]
		public async Task<IActionResult> Delete(int id)
		{
			var entity = await _unitOfWork.RepositoryVFeature.GetAsync(x => x.Id == id, true);

			if (entity == null) return BadRequest("Variant Feature not found");

			entity.IsDeleted = true;

			await _unitOfWork.CommitAsync();
			return Ok();
		}

		[HttpGet]
		[Route("Manage/Get")]
		public async Task<IActionResult> Get(int id)
		{
			var entityObject = await _unitOfWork.RepositoryVFeature.GetAsync(x => x.Id == id);
			if (entityObject == null) return BadRequest("Variant Feature not found");
			return Ok(entityObject);
		}
		[HttpPost]
		[Route("Manage/Recover")]
		public async Task<IActionResult> Recover(int id)
		{
			var entity = await _unitOfWork.RepositoryVFeature.GetAsync(x => x.Id == id, true);

			if (entity == null) return BadRequest("Variant Feature not found");

			entity.IsDeleted = false;

			await _unitOfWork.CommitAsync();
			return Ok();
		}

		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			var entities = await _unitOfWork.RepositoryVFeature.GetAllAsync(x => !x.IsDeleted, false);
			var returnDto = _mapper.Map<List<ReturnVFeatureDto>>(entities);
			return Ok(returnDto);
		}
	}
}
