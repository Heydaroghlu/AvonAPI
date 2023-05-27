using AutoMapper;
using Avon.Application.DTOs.VariantDTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Avon.Api.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class VariantsController : ControllerBase
	{
		IUnitOfWork _unitOfWork;
		IMapper _mapper;

		public VariantsController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpPost("Manage/Create")]
		public async Task<IActionResult> Create(CreateVariantDto objectDto) {
			if (!ModelState.IsValid) return BadRequest(objectDto);
			Variant variant = _mapper.Map<Variant>(objectDto);
			if (objectDto.selectFeatureIds!=null)
			{
				foreach (var item in objectDto.selectFeatureIds)
				{
					VariantFeature variantFeature = new()
					{
						VFeatureId = item,
						Variant = variant
					};
					await _unitOfWork.RepositoryVariantFeature.InsertAsync(variantFeature);

				}
			}
			await _unitOfWork.RepositoryVariant.InsertAsync(variant);
			await _unitOfWork.CommitAsync();
			return Ok(new { variant = variant });

		}

		[HttpPost("Manage/Edit")]
		public async Task<IActionResult> Edit(EditVariantDto objectDto)
		{
			var existVariant = await _unitOfWork.RepositoryVariant.GetAsync(x=>x.Id==objectDto.Id,true,"VariantFeatures");

			if (existVariant != null) return BadRequest("Variant not found");

			if (!ModelState.IsValid) return BadRequest(objectDto);

			existVariant = _mapper.Map<Variant>(objectDto);

			await _unitOfWork.RepositoryVariantFeature.RemoveRange(x => x.VariantId == existVariant.Id);

			if (objectDto.selectFeatureIds!=null)
			{
				foreach (var item in objectDto.selectFeatureIds)
				{
					await _unitOfWork.RepositoryVariantFeature.InsertAsync(new VariantFeature
					{
						VFeatureId=item,
						Variant=existVariant
					});
					await _unitOfWork.CommitAsync();
				}
				return Ok(objectDto);
			}
			return BadRequest(ModelState);
		}

		[HttpDelete]
		[Route("Manage/Delete")]
		public async Task<IActionResult> Delete(int id)
		{
			var product = await _unitOfWork.RepositoryVariant.GetAsync(x => x.Id == id);

			if (product == null) return BadRequest();

			product.IsDeleted = true;

			await _unitOfWork.CommitAsync();

			return Ok();
		}

		[HttpPost]
		[Route("Manage/Recover")]
		public async Task<IActionResult> Recover(int id)
		{
			var entity = await _unitOfWork.RepositoryVariant.GetAsync(x => x.Id == id);

			if (entity == null) return BadRequest();

			entity.IsDeleted = false;

			await _unitOfWork.CommitAsync();

			return Ok();
		}


	}
}
