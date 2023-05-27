using AutoMapper;
using Avon.Application.DTOs.SliderDTOs;
using Avon.Application.DTOs;
using Avon.Application.Storages;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.DTOs.CommentDTOs;
using Avon.Application.HelperManager;
using System.Security.Cryptography.Xml;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IStorage _storage;

        public CommentsController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null, bool? isAccepted = null)
        {
            var entities = await _unitOfWork.RepositoryComment.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (isAccepted == true)
                entities = entities.Where(x => x.IsAccept);
            if (isAccepted == false)
                entities = entities.Where(x => !x.IsAccept);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord) || x.Message.Contains(searchWord));
            }

            var list = PagenatedListDto<Comment>.Save(entities, page, pageSize);

            return Ok(list);
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CommentCreateDto objectDto)
        {
            if (!ModelState.IsValid) return BadRequest(objectDto);

            var check = await _unitOfWork.RepositoryProduct.IsAny(x => x.Id == objectDto.ProductId);
            if (check == false) throw new ProductNotFoundException();

            var user = await _unitOfWork.RepositoryUser.GetAsync(x => x.Id == objectDto.AppUserId);
            if (user == null) throw new UserNotFoundException();

            var entity = _mapper.Map<Comment>(objectDto);
            entity.AppUser = user;

            await _unitOfWork.RepositoryComment.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Ok(entity);
        }

        [HttpPost]
        [Route("Manage/Accept")]
        public async Task<IActionResult> Accept(int id)
        {
            var comment = await _unitOfWork.RepositoryComment.GetAsync(x => x.Id == id);
            if (comment.IsDeleted == true) return BadRequest("Comment Deleted");
            comment.IsAccept = true;
            await _unitOfWork.CommitAsync();
            
            return Ok();
        }

        [HttpPost]
        [Route("Manage/Reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var comment = await _unitOfWork.RepositoryComment.GetAsync(x => x.Id == id);
            if (comment.IsDeleted == true) return BadRequest("Comment Deleted");
            comment.IsAccept = true;
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryComment.GetAsync(x => x.Id == id);
            if (entity == null) throw new CommentNotFoundException();

            entity.IsDeleted = true;
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositoryComment.GetAsync(x => x.Id == id);
            if (entity == null) throw new CommentNotFoundException();

            entity.IsDeleted = false;
            await _unitOfWork.CommitAsync();

            return Ok();
        }


        [HttpGet]
        [Route("Manage/Get")]
        public async Task<IActionResult> Get(int id)
        {
            var entityObject = await _unitOfWork.RepositoryComment.GetAsync(x => x.Id == id);
            if (entityObject == null) throw new CommentNotFoundException();
            return Ok(entityObject);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(int productId)
        {
            var entityObject = await _unitOfWork.RepositoryProduct.GetAsync(x => x.Id == productId,false,"Comments");
            if (entityObject == null) throw new ProductNotFoundException();
            return Ok(entityObject.Comments);
        }


    }
}
