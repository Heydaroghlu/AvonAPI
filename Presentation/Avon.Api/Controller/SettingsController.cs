using AutoMapper;
using Avon.Application.DTOs.PromotionDTOs;
using Avon.Application.DTOs;
using Avon.Application.Storages;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.DTOs.SliderDTOs;
using Avon.Application.HelperManager;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.Abstractions.File;
using Avon.Application.DTOs.SettingDTOs;
using CloudinaryDotNet.Actions;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        readonly IFileService _fileService;
        static string _storagePath = "/uploads/settings/";

        public SettingsController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IFileService fileService)
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
            var entities = await _unitOfWork.RepositorySetting.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Key.Contains(searchWord));
            }

            var list = PagenatedListDto<Setting>.Save(entities, page, pageSize);

            list.ForEach(x =>
            {
                if(x.Key.Contains("image"))
                    x.Value = _storage.GetUrl(_storagePath, x.Value);
            });

            return Ok(list);
        }



        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var setting = await _unitOfWork.RepositorySetting.GetAsync(x => x.Id == id);
            if (setting == null) throw new SettingNotFoundException();
            setting.Value = _storage.GetUrl(_storagePath, setting.Value);
            return Ok(setting);
        }

        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit([FromForm]SettingEditDto objectDto, int id)
        {
            var existObject = await _unitOfWork.RepositorySetting.GetAsync(x => x.Id == id);

            if (existObject == null) throw new SettingNotFoundException();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool containsCheck = existObject.Key.Contains("image");

            string storageUrl = null;

            if (containsCheck)
            {
                if (objectDto.FormFile != null)
                {
                    _fileService.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);
                    var check = _storage.HasFile(_storagePath, existObject.Value);

                    if (check == true)
                    {
                        await _storage.DeleteAsync(_storagePath, existObject.Value);
                        var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.FormFile);
                        existObject.Value = imageInfo.fileName;
                        storageUrl = _storage.GetUrl(_storagePath, imageInfo.fileName);
                    }
                    else
                    {
                        var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.FormFile);
                        existObject.Value = imageInfo.fileName;
                        storageUrl = _storage.GetUrl(_storagePath, imageInfo.fileName);
                    }
                }
            }
            else
            {
                existObject.Value = objectDto.Value;
            }

            await _unitOfWork.CommitAsync();

            if (existObject.Key.Contains("image"))
                existObject.Value = storageUrl;

            return Ok(existObject);

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(string key)
        {
            var setting = await _unitOfWork.RepositorySetting.GetAsync(x => x.Key.Equals(key));

            if(setting == null) throw new SettingNotFoundException();

            if (setting.Key.Contains("image"))
                setting.Value = _storage.GetUrl(_storagePath, setting.Value);

            return Ok(setting);
        }

    }
}
