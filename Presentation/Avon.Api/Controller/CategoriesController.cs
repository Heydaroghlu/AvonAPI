using AutoMapper;
using Avon.Application.Abstractions.File;
using Avon.Application.DTOs;
using Avon.Application.DTOs.CategoryDTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.HelperManager;
using Avon.Application.Storages;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Avon.Infrastructure.Services;
using Avon.Persistence.Contexts;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IStorage _storage;
        readonly IFileService _fileService;
        static string _storagePath = "/uploads/categories/";

        public CategoriesController(IMapper mapper, IUnitOfWork unitOfWork, IStorage storage, IFileService fileService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _storage = storage;
            _fileService = fileService;
        }
        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {
            var entities = await _unitOfWork.RepositoryCategory.GetAllAsync(x => true, false, "SubCategories");
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord));
            }

            var list = PagenatedListDto<Category>.Save(entities, page, pageSize);
            list.ForEach(x =>
            {
                x.Image = _storage.GetUrl(_storagePath, x.Image);
            });

            return Ok(list);
        }

        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existEntity = await _unitOfWork.RepositoryCategory.GetAsync(x => x.Name.ToLower().Equals(objectDto.Name.ToLower()), false);
            if (existEntity != null) return BadRequest($"{objectDto.Name} has been already created");

            var entity = _mapper.Map<Category>(objectDto);
            string fileName = null;

            if (objectDto.ImageFile != null)
            {
                _fileService.CheckFileType(objectDto.ImageFile, ContentTypeManager.ImageContentTypes);
                var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.ImageFile);
                //todo image root
                entity.Image = imageInfo.fileName;
                fileName = imageInfo.fileName;
            }

            await _unitOfWork.RepositoryCategory.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            if (objectDto.ImageFile != null)
            {
                entity.Image = _storage.GetUrl(_storagePath, fileName);
            }

            return Ok(entity);
        }

        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _unitOfWork.RepositoryCategory.GetAsync(x => x.Id == id);
            if (category == null) throw new CategoryNotFoundException();
            category.Image = _storage.GetUrl(_storagePath, category.Image);
            return Ok(category);
        }

        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit([FromForm] CategoryEditDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existObject = await _unitOfWork.RepositoryCategory.GetAsync(x => x.Id == objectDto.Id);
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
            //existObject.DisCount = objectDto.DisCount;

            await _unitOfWork.CommitAsync();

            existObject.Image = storageUrl;
            //todo editde image i bos qoymag isteyende image i sile bilmelidi
            return Ok(existObject);
        }

        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryCategory.GetAsync(x => x.Id == id, true, "SubCategories");

            if (entity == null) return NotFound();

            entity.IsDeleted = true;
            entity.SubCategories.ForEach(x => x.IsDeleted = true);


            await _unitOfWork.CommitAsync();
            return Ok();
        }

        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositoryCategory.GetAsync(x => x.Id == id, true, "SubCategories");

            if (entity == null) return NotFound();

            entity.IsDeleted = false;
            entity.SubCategories.ForEach(x => x.IsDeleted = false);

            await _unitOfWork.CommitAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _unitOfWork.RepositoryCategory.GetAllAsync(x => true, false, "SubCategories");
            var returnDto = _mapper.Map<List<CategoryReturnDto>>(entities);
            returnDto.ForEach(x =>
            {
                x.Image = HttpService.StorageUrl(_storagePath, x.Image);
            });
            return Ok(returnDto);
        }

        //todo Admin Get All zirt pirt

    }
}

    