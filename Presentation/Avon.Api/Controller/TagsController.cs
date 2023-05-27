using AutoMapper;
using Avon.Application.DTOs.FeatureDTOs;
using Avon.Application.DTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Avon.Persistence.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.DTOs.TagDTOs;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;


        public TagsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {
            var entities = await _unitOfWork.RepositoryTeg.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord));
            }

            var list = PagenatedListDto<Teg>.Save(entities, page, pageSize);

            return Ok(list);
        }

        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create(TagCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existEntity = await _unitOfWork.RepositoryTeg.GetAsync(x => x.Name.ToLower().Equals(objectDto.Name.ToLower()), false);
            if (existEntity != null) return BadRequest($"{objectDto.Name} has been already created");

            var entity = _mapper.Map<Teg>(objectDto);

            await _unitOfWork.RepositoryTeg.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Ok(entity);
        }

        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit(int id, TagEditDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existObject = await _unitOfWork.RepositoryTeg.GetAsync(x => x.Id == id);

            if (existObject == null) throw new TagNotFoundException();

            existObject.Name = objectDto.Name;

            await _unitOfWork.CommitAsync();

            return Ok(existObject);
        }


        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryTeg.GetAsync(x => x.Id == id, true);

            if (entity == null) throw new TagNotFoundException();

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();
            return Ok();
        }

        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var entityObject = await _unitOfWork.RepositoryTeg.GetAsync(x => x.Id == id);
            if (entityObject == null) throw new TagNotFoundException();
            return Ok(entityObject);
        }
        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositoryTeg.GetAsync(x => x.Id == id, true);

            if (entity == null) throw new TagNotFoundException();

            entity.IsDeleted = false;

            await _unitOfWork.CommitAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _unitOfWork.RepositoryTeg.GetAllAsync(x => !x.IsDeleted, false);
            var returnDto = _mapper.Map<List<TagReturnDto>>(entities);
            return Ok(returnDto);
        }

    }
}
