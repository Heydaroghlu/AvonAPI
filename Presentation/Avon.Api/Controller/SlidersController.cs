using AutoMapper;
using Avon.Application.DTOs.PromotionDTOs;
using Avon.Application.DTOs;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.HelperManager;
using Avon.Application.Storages;
using Avon.Application.DTOs.SliderDTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.Abstractions.File;
using CloudinaryDotNet.Actions;
using System.Collections.Generic;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IStorage _storage;
        readonly IFileService _fileService;
        static string _storagePath = "/uploads/sliders/";


        public SlidersController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IFileService fileService)
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
            var entities = await _unitOfWork.RepositorySlider.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Title1.Contains(searchWord) || x.Title2.Contains(searchWord));
            }

            var list = PagenatedListDto<Slider>.Save(entities, page, pageSize);
            
            list.ForEach(x =>
            {
                x.Image = _storage.GetUrl(_storagePath, x.Image);
            });

            return Ok(list);
        }


        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create([FromForm]SliderCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(objectDto);

            _fileService.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);

            var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.FormFile);

            var entity = _mapper.Map<Slider>(objectDto);
            entity.Image = imageInfo.fileName;

            await _unitOfWork.RepositorySlider.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            entity.Image = _storage.GetUrl(_storagePath, imageInfo.fileName);

            return Ok(entity);
        }



        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var entityObject = await _unitOfWork.RepositorySlider.GetAsync(x => x.Id == id);
            if (entityObject == null) throw new SliderNotFoundException();
            entityObject.Image = _storage.GetUrl(_storagePath, entityObject.Image);
            return Ok(entityObject);
        }

        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit([FromForm]SliderEditDto objectDto, int id)
        {
            if (!ModelState.IsValid) return BadRequest(objectDto);

            var existObject = await _unitOfWork.RepositorySlider.GetAsync(x => x.Id == id);
            if (existObject == null) throw new SliderNotFoundException();
            
            string storageUrl = null;

            if (objectDto.FormFile != null)
            {
                _fileService.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);

                var check = _storage.HasFile(_storagePath, existObject.Image);
                if (check == true)
                {
                    await _storage.DeleteAsync(_storagePath, existObject.Image);
                    var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                    storageUrl = _storage.GetUrl(_storagePath, imageInfo.fileName);
                }
                else
                {
                    var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                    storageUrl = _storage.GetUrl(_storagePath, imageInfo.fileName);
                }
            }
                
            existObject.Title1 = objectDto.Title1;
            existObject.Title2 = objectDto.Title2;

            await _unitOfWork.CommitAsync();

            existObject.Image = storageUrl;

            return Ok(existObject);
        }


        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositorySlider.GetAsync(x => x.Id == id);

            if (entity == null) throw new SliderNotFoundException();

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositorySlider.GetAsync(x => x.Id == id);

            if (entity == null) throw new SliderNotFoundException();

            entity.IsDeleted = false;

            await _unitOfWork.CommitAsync();

            return Ok();
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var sliders = await _unitOfWork.RepositorySlider.GetAllAsync(x=>!x.IsDeleted);
            var list = sliders.ToList();
            
            list.ForEach(x =>
            {
                x.Image = _storage.GetUrl(_storagePath, x.Image);
            });

            return Ok(sliders);
        }

    }
}
