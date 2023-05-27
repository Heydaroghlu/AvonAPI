using AutoMapper;
using Avon.Application.DTOs.CategoryDTOs;
using Avon.Application.DTOs;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.DTOs.SubCategoryDTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.HelperManager;
using Avon.Application.Abstractions.File;
using Avon.Application.Storages;
using CloudinaryDotNet.Actions;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCatigoriesController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IFileService _fileService;
        readonly IStorage _storage;
        static string _storagePath = "/uploads/subCategories/";

        public SubCatigoriesController(IMapper mapper, IUnitOfWork unitOfWork, IFileService fileService, IStorage storage)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _storage = storage;
        }

        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {
            var entities = await _unitOfWork.RepositorySubCategory.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord));
            }

            var list = PagenatedListDto<SubCategory>.Save(entities, page, pageSize);

            list.ForEach(x =>
            {
                x.Image = _storage.GetUrl(_storagePath, x.Image);
            });

            return Ok(list);
        }

        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create([FromForm] SubCategoryCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existEntity = await _unitOfWork.RepositorySubCategory.GetAsync(x => x.Name.ToLower().Equals(objectDto.Name.ToLower()), false);
            if (existEntity != null) return BadRequest($"{objectDto.Name} has been already created");

            var entity = _mapper.Map<SubCategory>(objectDto);
            string fileName = null;

            if (objectDto.ImageFile != null)
            {
                _fileService.CheckFileType(objectDto.ImageFile, ContentTypeManager.ImageContentTypes);
                var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.ImageFile);
                //todo image root
                entity.Image = imageInfo.fileName;
            }

            await _unitOfWork.RepositorySubCategory.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            if (fileName != null)
                entity.Image = _storage.GetUrl(_storagePath, fileName);

            return Ok(entity);
        }



        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var entityObject = await _unitOfWork.RepositorySubCategory.GetAsync(x => x.Id == id);
            if (entityObject == null) throw new SubCategoryNotFoundException();
            entityObject.Image = _storage.GetUrl(_storagePath, entityObject.Image);
            return Ok(entityObject);
        }


        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit([FromForm] SubCategoryEditDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existObject = await _unitOfWork.RepositorySubCategory.GetAsync(x => x.Id == objectDto.Id);

            if (existObject == null) return NotFound();

            string storageUrl = null;

            if (objectDto.ImageFile != null)
            {
                _fileService.CheckFileType(objectDto.ImageFile, ContentTypeManager.ImageContentTypes);

                var check = _storage.HasFile(_storagePath, existObject.Image);
                if (check == true)
                {
                    await _storage.DeleteAsync(_storagePath, existObject.Image);
                    var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.ImageFile);
                    existObject.Image = imageInfo.fileName;
                    storageUrl = _storage.GetUrl(_storagePath, imageInfo.fileName);
                }
                else
                {
                    var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.ImageFile);
                    existObject.Image = imageInfo.fileName;
                    storageUrl = _storage.GetUrl(_storagePath, imageInfo.fileName);
                }
            }
            else
            {
                storageUrl = _storage.GetUrl(_storagePath, existObject.Image);
            }


            existObject.Name = objectDto.Name;
            existObject.Icon = objectDto.Icon;
            existObject.ColorName = objectDto.ColorName;
            existObject.ColorCode = objectDto.ColorCode;
            existObject.CategoryId = objectDto.CategoryId;

            await _unitOfWork.CommitAsync();

            existObject.Image = storageUrl;

            return Ok(existObject);
        }

        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositorySubCategory.GetAsync(x => x.Id == id, true);

            if (entity == null) return NotFound();

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();
            return Ok();
        }

        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositorySubCategory.GetAsync(x => x.Id == id);

            if (entity == null) throw new SubCategoryNotFoundException();

            entity.IsDeleted = false;

            await _unitOfWork.CommitAsync();

            return Ok();
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _unitOfWork.RepositorySubCategory.GetAllAsync(x => x.IsDeleted == false, false, "ProductSubCategories.Product");
            var returnDto = _mapper.Map<List<SubCategoryReturnDto>>(entities);

            returnDto.ForEach(x =>
            {
                if (x.Image != null) x.Image = _storage.GetUrl(_storagePath, x.Image);
            });

            return Ok(returnDto);
        }

    }
}
