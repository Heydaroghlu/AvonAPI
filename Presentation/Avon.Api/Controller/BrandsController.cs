using AutoMapper;
using Avon.Application.Abstractions.File;
using Avon.Application.DTOs;
using Avon.Application.DTOs.BrandDTOs;
using Avon.Application.DTOs.SliderDTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.HelperManager;
using Avon.Application.Storages;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Avon.Infrastructure.Services;
using Avon.Persistence.UnitOfWorks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IStorage _storage;
        readonly IFileService _fileService;
        static string _storagePath = "/uploads/brands/";
        public BrandsController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
            _fileService = fileService;
        }


        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {

            var entities = await _unitOfWork.RepositoryBrand.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord));
            }

            var list = PagenatedListDto<Brand>.Save(entities, page, pageSize);
            list.ForEach(x =>
            {
                x.Image = _storage.GetUrl(_storagePath,x.Image);
            });
            
            return Ok(list);
        }


        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create([FromForm] BrandCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(objectDto);

            if(objectDto.ImageFile != null)
                _fileService.CheckFileType(objectDto.ImageFile, ContentTypeManager.ImageContentTypes);

            var entity = _mapper.Map<Brand>(objectDto);
            string fileName = null;

            if(objectDto.ImageFile != null)
            {   
                var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.ImageFile);
                fileName = imageInfo.fileName;
                entity.Image = fileName;
            }

            await _unitOfWork.RepositoryBrand.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            entity.Image = _storage.GetUrl(_storagePath, fileName);

            return Ok(entity);
        }

        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var brand = await _unitOfWork.RepositoryBrand.GetAsync(x => x.Id == id);
            if (brand == null) throw new BrandNotFoundException();
            brand.Image = _storage.GetUrl(_storagePath, brand.Image);
            return Ok(brand);   
        }

        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit([FromForm] BrandEditDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(objectDto);

            var existObject = await _unitOfWork.RepositoryBrand.GetAsync(x => x.Id == objectDto.Id);
            if (existObject == null) throw new BrandNotFoundException();

            //existObject.Image = storageUrl;
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
                storageUrl = _storage.GetUrl(_storagePath,existObject.Image);
            }

            existObject.Name = objectDto.Name;

            await _unitOfWork.CommitAsync();

            existObject.Image = storageUrl;

            return Ok(existObject);
        }

        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryBrand.GetAsync(x => x.Id == id);

            if (entity == null) throw new BrandNotFoundException();

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositoryBrand.GetAsync(x => x.Id == id);

            if (entity == null) throw new BrandNotFoundException();

            entity.IsDeleted = false;

            await _unitOfWork.CommitAsync();

            return Ok();
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _unitOfWork.RepositoryBrand.GetAllAsync(x => !x.IsDeleted,false);

            List<Brand> newList = new List<Brand>();

            brands.ToList().ForEach(x =>
            {
                x.Image = _storage.GetUrl(_storagePath, x.Image);
                newList.Add(x);
            });
            return Ok(newList);
        }
    }
}
