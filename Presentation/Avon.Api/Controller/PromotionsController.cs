using AutoMapper;
using Avon.Application.DTOs.FeatureDTOs;
using Avon.Application.DTOs;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.DTOs.PromotionDTOs;
using Avon.Application.HelperManager;
using Avon.Application.Storages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Cryptography.Xml;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.Exceptions;
using Avon.Application.Abstractions.File;
using System.Collections.Generic;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IStorage _storage;
        readonly IFileService _fileService;
        static string _storagePath = "/uploads/promotions/";

        public PromotionsController(IMapper mapper, IUnitOfWork unitOfWork, IStorage storage, IFileService fileService)
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
            var entities = await _unitOfWork.RepositoryPromoiton.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Image.Contains(searchWord));
            }

            var list = PagenatedListDto<Promotion>.Save(entities, page, pageSize);
            list.ForEach(x =>
            {
                x.Image = _storage.GetUrl(_storagePath, x.Image);
            });

            return Ok(list);
        }


        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create([FromForm] PromotionCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            objectDto.FormFiles.ForEach(formFile =>
            {
                _fileService.CheckFileType(formFile, ContentTypeManager.ImageContentTypes);
            });

            var imagesInfo = await _storage.UploadRangeAsync(_storagePath, objectDto.FormFiles);
            List<Promotion> promotions = new();

            foreach (var imageInfo in imagesInfo)
            {
                var entity = new Promotion()
                {
                    Image = imageInfo.fileName,
                    Key = objectDto.Key
                };

                await _unitOfWork.RepositoryPromoiton.InsertAsync(entity);
                await _unitOfWork.CommitAsync();
                promotions.Add(entity);
            }

            promotions.ForEach(x =>
            {
                x.Image = _storage.GetUrl(_storagePath, x.Image);
            });

            return Ok(promotions);
        }


        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var entityObject = await _unitOfWork.RepositoryPromoiton.GetAsync(x => x.Id == id);
            if (entityObject == null) throw new PromotionNotFoundException();
            entityObject.Image = _storage.GetUrl(_storagePath, entityObject.Image);

            return Ok(entityObject);
        }

        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit([FromForm] PromotionEditDto objectDto)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (objectDto.FormFile != null)
            {
                _fileService.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);
            }

            var promotion = await _unitOfWork.RepositoryPromoiton.GetAsync(x => x.Id == objectDto.Id);

            if (objectDto.FormFile != null)
            {
                //todo ImageURLS
                //todo hasFile 
                var check = await _storage.DeleteAsync(_storagePath, promotion.Image);

                if (check == false) return BadRequest("Image Couldn't find");
                else
                {
                    var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.FormFile);
                    promotion.Image = imageInfo.fileName;
                }

            }

            promotion.Key = objectDto.Key;  

            await _unitOfWork.CommitAsync();

            promotion.Image = _storage.GetUrl(_storagePath, promotion.Image);

            return Ok(promotion);
        }


        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryPromoiton.GetAsync(x => x.Id == id);

            if (entity == null) throw new PromotionNotFoundException();

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(string? key = null)
        {
            var promotions = await _unitOfWork.RepositoryPromoiton.GetAllAsync(x => !x.IsDeleted);

            if (key != null)
                promotions = promotions.Where(x => x.Key.Equals(key));

            var list = promotions.ToList();
          
            list.ForEach(x =>
            {
                x.Image = _storage.GetUrl(_storagePath, x.Image);
            });

            return Ok();
        }


    }
}