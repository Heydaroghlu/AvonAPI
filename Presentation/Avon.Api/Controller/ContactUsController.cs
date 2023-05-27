using AutoMapper;
using Avon.Application.Abstractions.Email;
using Avon.Application.DTOs;
using Avon.Application.DTOs.ContactUsDTOs;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IEmailService _emailService;

        public ContactUsController(IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {
            var entities = await _unitOfWork.RepositoryContactUs.GetAllAsync(x => true, false);
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord) || x.Email.Contains(searchWord));
            }

            var list = PagenatedListDto<ContactUs>.Save(entities, page, pageSize);

            return Ok(list);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ContactUsCreateDto contactDto) {
            if (!ModelState.IsValid) return BadRequest();
            var entity=_mapper.Map<ContactUs>(contactDto);
            await _unitOfWork.RepositoryContactUs.InsertAsync(entity);
            await _unitOfWork.CommitAsync();
            return Ok(entity);
        }

        [HttpGet("Message")]
        public async Task<IActionResult> Message(int id) {
            var message=await _unitOfWork.RepositoryContactUs.GetAsync(x=>x.Id==id);
            if (message == null) return BadRequest();
            
            return Ok(message);
        }

        [HttpPost("AcceptMessage")]
        public async Task<IActionResult> AcceptMessage(AcceptContactMessageDto messageDto) {
            var message = await _unitOfWork.RepositoryContactUs.GetAsync(x => x.Id == messageDto.Id);
            if (message == null) return BadRequest();

            await _emailService.SendEmail(message.Email, $"Avon.", $"Salam {message.Name}.\n {messageDto.Message}");
            message.Status = true;
            await _unitOfWork.CommitAsync();
            return Ok("Messaj Getdi");
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryContactUs.GetAsync(x => x.Id == id);

            if (entity == null) return BadRequest();

            await _unitOfWork.RepositoryContactUs.Remove(id);
            return Ok();
        }
    }
}
