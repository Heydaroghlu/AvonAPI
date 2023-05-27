using AutoMapper;
using Avon.Application.DTOs.CategoryDTOs;
using Avon.Application.DTOs;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.DTOs.FeatureDTOs;
using Avon.Application.DTOs.ContactUsDTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;


        public FeaturesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {
            var entities = await _unitOfWork.RepositoryFeature.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Title.Contains(searchWord));
            }

            var list = PagenatedListDto<Feature>.Save(entities, page, pageSize);

            return Ok(list);
        }

        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create(FeatureCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existEntity = await _unitOfWork.RepositoryFeature.GetAsync(x => x.Title.ToLower().Equals(objectDto.Title.ToLower()), false);
            if (existEntity != null) return BadRequest($"{objectDto.Title} has been already created");

            var entity = _mapper.Map<Feature>(objectDto);

            await _unitOfWork.RepositoryFeature.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Ok(entity);
        }

        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit(int id, FeatureEditDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existObject = await _unitOfWork.RepositoryFeature.GetAsync(x => x.Id == id);

            if (existObject == null) throw new FeatureNotFoundException();

            existObject.Title = objectDto.Title;
            existObject.Icon = objectDto.Icon;

            await _unitOfWork.CommitAsync();

            return Ok(existObject);
        }


        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryFeature.GetAsync(x => x.Id == id, true);

            if (entity == null) throw new FeatureNotFoundException();

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();
            return Ok();
        }

        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var entityObject = await _unitOfWork.RepositoryFeature.GetAsync(x => x.Id == id);
            if (entityObject == null) throw new FeatureNotFoundException();
            return Ok(entityObject);
        }
        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositoryFeature.GetAsync(x => x.Id == id, true);

            if (entity == null) throw new FeatureNotFoundException();

            entity.IsDeleted = false;

            await _unitOfWork.CommitAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _unitOfWork.RepositoryFeature.GetAllAsync(x => !x.IsDeleted, false);
            var returnDto = _mapper.Map<List<FeatureReturnDto>>(entities);
            return Ok(returnDto);
        }

    }
}
