using AutoMapper;
using Avon.Application.Abstractions.File;
using Avon.Application.DTOs.SliderDTOs;
using Avon.Application.DTOs;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.HelperManager;
using Avon.Application.Storages;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.DTOs.NewDTOs;
using Microsoft.EntityFrameworkCore;
using Avon.Infrastructure.Services;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IStorage _storage;
        readonly IFileService _fileService;
        static string _storagePath = "/uploads/news/";


        public NewsController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IFileService fileService)
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
            var entities = await _unitOfWork.RepositoryNews.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord));
            }

            var list = PagenatedListDto<News>.Save(entities, page, pageSize);
            list.ForEach(x =>
            {
                x.PosterImage = HttpService.StorageUrl(_storagePath, x.PosterImage);
            });

            return Ok(list);
        }


        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create([FromForm] NewsCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(objectDto);

            _fileService.CheckFileType(objectDto.PosterFile, ContentTypeManager.ImageContentTypes);

            var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.PosterFile);
            string fileName = imageInfo.fileName;

            var entity = _mapper.Map<News>(objectDto);
            entity.PosterImage = fileName;

            await _unitOfWork.RepositoryNews.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            objectDto.TagIds.ForEach(async id =>
            {
                await _unitOfWork.RepositoryNewsTeg.InsertAsync(new NewsTeg()
                {
                    NewsId = entity.Id,
                    TegId = id
                });
                await _unitOfWork.CommitAsync();
            });

            entity.PosterImage = HttpService.StorageUrl(_storagePath, imageInfo.fileName);

            return Ok(entity);
        }

        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> ManageGet(int id)
        {
            var entityObject = await _unitOfWork.RepositoryNews.GetAsync(x => x.Id == id);
            if (entityObject == null) throw new NewsNotFoundException();
            entityObject.PosterImage = HttpService.StorageUrl(_storagePath, entityObject.PosterImage);
            return Ok(entityObject);
        }

        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit([FromForm] NewsEditDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(objectDto);

            var existObject = await _unitOfWork.RepositoryNews.GetAsync(x => x.Id == objectDto.Id,true, "NewsTegs");
            if (existObject == null) throw new NewsNotFoundException();

            var newsCategory = _unitOfWork.RepositoryNewsCategory.GetAsync(x => x.Id == objectDto.NewsCategoryId);
            if(newsCategory == null) throw new NewCategoryNotFoundException();

            string storageUrl = null;

            if (objectDto.PosterFile != null)
            {
                _fileService.CheckFileType(objectDto.PosterFile, ContentTypeManager.ImageContentTypes);

                var check = _storage.HasFile(_storagePath, existObject.PosterImage);
                //todo change path
                if (check == true)
                {
                    await _storage.DeleteAsync(_storagePath, existObject.PosterImage);
                    var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.PosterFile);
                    existObject.PosterImage = imageInfo.fileName;
                    storageUrl = _storage.GetUrl(_storagePath, imageInfo.fileName);
                }
                else
                {
                    var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.PosterFile);
                    existObject.PosterImage = imageInfo.fileName;
                    storageUrl = _storage.GetUrl(_storagePath, imageInfo.fileName);
                }
            }
            else
            {
                storageUrl = _storage.GetUrl(_storagePath, existObject.PosterImage);
            }

            existObject.Name = objectDto.Name;  
            existObject.Content = objectDto.Content;
            existObject.NewsCategoryId = objectDto.NewsCategoryId;

            await _unitOfWork.RepositoryNewsTeg.RemoveRange(x => x.NewsId == existObject.Id);
            await _unitOfWork.CommitAsync();

            objectDto.TagIds.ForEach(async id =>
            {
                await _unitOfWork.RepositoryNewsTeg.InsertAsync(new NewsTeg()
                {
                    NewsId = existObject.Id,
                    TegId = id
                });
                await _unitOfWork.CommitAsync();
            });

            existObject.PosterImage = storageUrl;

            return Ok(existObject);
        }


        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryNewsCategory.GetAsync(x => x.Id == id);
            if (entity == null) throw new NewsNotFoundException();

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositoryNews.GetAsync(x => x.Id == id);

            if (entity == null) throw new NewsNotFoundException();

            entity.IsDeleted = false;

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var news = await _unitOfWork.RepositoryNews.GetAllAsync(x => !x.IsDeleted && x.Id == id,false,"NewsTags", "NewsCategory");
            return Ok(news);
        }

    }
}
