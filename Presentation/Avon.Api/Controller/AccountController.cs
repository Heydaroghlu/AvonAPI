using AutoMapper;
using Avon.Application.Abstractions.Email;
using Avon.Application.Abstractions.File;
using Avon.Application.Abstractions.Token;
using Avon.Application.DTOs.UserDTOs;
using Avon.Application.Exceptions;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Avon.Application.HelperManager;
using Avon.Application.Storages;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Avon.Infrastructure.Email;
using Avon.Infrastructure.Services;
using CloudinaryDotNet.Actions;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace Avon.Api.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ITokenHandler _tokenHandler;
		private readonly IEmailService _emailService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFileService _fileService;
		private readonly IMapper _mapper;
		private readonly IStorage _storage;
        static string _storagePath = "/uploads/accounts/";


        public AccountController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IEmailService emailService, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IFileService fileService, IStorage storage, IMapper mapper)
        {
            _roleManager = roleManager;
            _emailService = emailService;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _storage = storage;
            _mapper = mapper;
        }
        [HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDTO login)
		{
			AppUser user = await CheckLogin(login.UserName, login.Password);

			if (user == null)
			{
				return StatusCode(401, "Ad və ya şifrə yanlışdır !");
			}
			var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
			if (!result.Succeeded)
			{
				return StatusCode(401, "Ad və ya şifrə yanlışdır !");
			}
			string token = _tokenHandler.CreateAccessToken(user, 60);
			return Ok(token);
		}

		[HttpGet("createrole")]
		public async Task<IActionResult> CreateRoles()
		{
			var role1 = new IdentityRole("Member");
			var role2 = new IdentityRole("Admin");

			await _roleManager.CreateAsync(role1);
			await _roleManager.CreateAsync(role2);
			return Ok();
		}
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromForm] RegisterDTO register)
		{
			AppUser exist = await _userManager.FindByNameAsync(register.Email);
			if (exist != null)
			{
				return BadRequest(register);
			}
			if (register.Password != register.RepeatPassword)
			{
				return BadRequest();
			}
			if (!RegexManager.CheckMailRegex(register.Email))
			{
				return StatusCode(400, "Email is problem");
			}
			if (!RegexManager.CheckPhoneRegex(register.Phone))
			{
				return BadRequest("Phone problem");
			}
            AppUser user = new()
            {
                Name = register.Name,
                Surname = register.Surname,
                Email = register.Email,
                PhoneNumber = register.Phone,
                UserName = register.Email,
            };

            string fileName = null;
			if (register.ImageFile != null)
			{
				_fileService.CheckFileType(register.ImageFile, ContentTypeManager.ImageContentTypes);
				var imageInfo = await _storage.UploadAsync(_storagePath, register.ImageFile);
				fileName = imageInfo.fileName;
				user.ProfileImage = fileName;
			}
			
			if (register.IdForReferal == "1")
			{
				user.IsFirst = true;

			}
			else
			{
				AppUser referalUser = await _userManager.FindByIdAsync(register.IdForReferal);
				if (referalUser == null)
				{
					return BadRequest("Bu referalda istifadeci yoxdur");
				}
				user.IdForReferal = register.IdForReferal;
			}
			var result = await _userManager.CreateAsync(user, register.Password);
			var addToRoleResult = await _userManager.AddToRoleAsync(user, "Member");
			if (!addToRoleResult.Succeeded)
			{
				return BadRequest(addToRoleResult.Errors);
			}
			if (result.Succeeded)
			{
				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { token = code, email = user.Email });
				await _emailService.SendEmail(user.UserName, "Hesab Təsdiqləmə", $"Hesabınızı təsdiqləmək üçün <a href='{callbackUrl}'>bu linkə daxil olun</a>");
				if (fileName != null)
					user.ProfileImage = _storage.GetUrl(_storagePath, fileName);

                return Ok("Qeydiyyatdan uğrula tamamlandı. Zəhmət olmasa hesabınızı təsdiqləmək üçün emailinizə daxil olun.");
			}

			return BadRequest(result.Errors);
		}

		[HttpPost("passwordreset")]
		public async Task<IActionResult> PasswordResetAsync(string userName)
		{
			AppUser user = await _userManager.FindByNameAsync(userName);
			if (user != null)
			{
				string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

				resetToken = resetToken.UrlEncode();
				await _emailService.SendPasswordResetMailAsync(user.Email, userName, resetToken);
				return Ok();
			}
			return StatusCode(StatusCodes.Status400BadRequest, new { message = "Daxil etdiyiniz istifadəçi tapılmadı." });
		}

		[HttpPost("verifyresettoken")]
		public async Task<IActionResult> VerifyResetTokenAsync(string resetToken, string email)
		{
			AppUser user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return StatusCode(StatusCodes.Status400BadRequest, new { message = "token və ya email düzgen daxil olunamyıb" });


			}
			resetToken = resetToken.UrlDecode();

			var verifyUserResult = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
			if (verifyUserResult)
			{
				return StatusCode(StatusCodes.Status200OK, new { result = verifyUserResult });
			}
			return StatusCode(StatusCodes.Status400BadRequest, new { result = verifyUserResult });

		}

		[HttpPost("updatepassword")]
		public async Task<IActionResult> UpdatePassword(string userName, string resetToken, string newPassword)
		{
			AppUser user = await _userManager.FindByNameAsync(userName);
			resetToken = resetToken.UrlDecode();
			IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
			if (result.Succeeded)
			{
				var data=await _userManager.UpdateSecurityStampAsync(user);
				return StatusCode(StatusCodes.Status200OK, new {result=data.Succeeded });
			}
			else throw new PasswordChangeFailedException();
		}
		[HttpGet]
		[Route("MyAccount")]
        #region myIsa
        //public async Task<IActionResult> MyAccount(string id)
        //{

        //    var data1 = await _unitOfWork.RepositoryUser.GetAllAsync(x => x.IsAdmin == false, false, "Orders");
        //    var data2 = await _unitOfWork.RepositorReferalTable.GetAllAsync(x => x.IsDeleted == false, false);
        //    List<AppUser> users = data1.ToList();
        //    List<ReferalTable> positions = data2.ToList();
        //    foreach (var user in users)
        //    {
        //        int count = 0;
        //        double totalSale = 0;
        //        user.ReferalUsers = new List<AppUser>(); // ReferalUsers özelliğini başlat
        //        foreach (var referal in users)
        //        {
        //            count = 0;
        //            totalSale = 0;
        //            if (referal.IdForReferal == user.Id)
        //            {
        //                user.ReferalUsers.Add(referal);
        //                List<Order> orders = new List<Order>();
        //                count++;
        //                foreach (var item in user.Orders)
        //                {
        //                    string dateMonth = item.CreatedAt.Month.ToString();
        //                    string dateYear = item.CreatedAt.Year.ToString();

        //                    if (dateMonth == DateTime.Now.Month.ToString() && dateYear == DateTime.Now.Year.ToString())
        //                    {
        //                        orders.Add(item);
        //                        totalSale += Convert.ToDouble(item.TotalAmount);
        //                    }
        //                }
        //                referal.Orders = orders;
        //                if (orders.Count > 0)
        //                {
        //                    user.ReferalCount++;

        //                }
        //            }
        //        }
        //        user.ReferalCount += count;

        //        foreach (var item in positions)
        //        {
        //            if (user.ReferalCount == item.UserCount)
        //            {
        //                user.Position = item.Name;
        //                user.Gain = item.Percent + item.SecondPercent + item.ThirdPercent;
        //            }
        //        }
        //        count = 0;
        //    }

        //    return Ok(users.Where(x => x.Id == id));
        //}
        #endregion
        public async Task<IActionResult> MyAccount(string id)
        {
            var data1 = await _unitOfWork.RepositoryUser.GetAllAsync(x => x.IsAdmin == false, false, "Orders");
            var data2 = await _unitOfWork.RepositorReferalTable.GetAllAsync(x => x.IsDeleted == false, false);
            List<AppUser> users = data1.ToList();
            List<ReferalTable> positions = data2.ToList();
            double totalSale = 0;
            foreach (var user in users.OrderBy(x => x.CreatedTime))
            {
                int count = 0;
                user.ReferalUsers = new List<AppUser>(); // ReferalUsers özelliğini başlat
                foreach (var referal in users.OrderBy(x => x.CreatedTime).Where(x => x.Id != user.Id))
                {
                    if (referal.IdForReferal == user.Id)
                    {
                        for (int i = referal.Orders.Count - 1; i >= 0; i--)
                        {
                            var item = referal.Orders[i];
                            string dateMonth = item.CreatedAt.Month.ToString();
                            string dateYear = item.CreatedAt.Year.ToString();

                            if (dateMonth != DateTime.Now.Month.ToString() || dateYear != DateTime.Now.Year.ToString())
                            {
                                referal.Orders.Remove(item);


                            }
                            else
                            {
                                referal.FirstAmount += Convert.ToDouble(item.TotalAmount);
                            }

                        }
                        referal.TotalAmount += referal.FirstAmount + totalSale;
                        referal.OrderCountForMounth = referal.Orders.Count();
                        totalSale += referal.FirstAmount;
                        if (referal.OrderCountForMounth > 0)
                        {
                            user.ReferalUsers.Add(referal);
                        }
                        user.ReferalCount += referal.ReferalUsers.Count();
                    }
                    referal.TotalAmount = 0;
                    foreach (var item in positions)
                    {
                        if (!referal.IsFirst && referal.ReferalCount >= item.UserCount && referal.TotalAmount >= item.MustSale)
                        {
                            referal.Position = item.Name;
                            referal.Gain = item.Percent + item.SecondPercent + item.ThirdPercent;
                        }
                    }
                }
                totalSale = 0;

                if (user.IsFirst == true)
                {
                    user.OrderCountForMounth = user.Orders.Count();
                    if (user.OrderCountForMounth > 0)
                    {
                        foreach (var item in user.Orders)
                        {
                            user.FirstAmount = Convert.ToDouble(item.TotalAmount);
                        }
                        user.ReferalCount += count;
                    }
                    user.TotalAmount += totalSale + user.FirstAmount;

                    totalSale = 0;



                }

                foreach (var item in positions)
                {
                    if (user.FirstAmount > item.FIrstAmount && user.ReferalCount >= item.UserCount && user.TotalAmount >= item.MustSale)
                    {
                        user.Position = item.Name;
                        user.Gain = item.Percent + item.SecondPercent + item.ThirdPercent;

                    }


                }

                count = 0;
            }
            List<UserReport> userReports = _mapper.Map<List<UserReport>>(users);
            return Ok(userReports.Where(x => x.Id == id));
        }
        [HttpGet]
		[Route("AllFirstUser")]
        #region isa
        //public async Task<IActionResult> BaseUsers(string? mouth, string? year)
        //{
        //	var data1 = await _unitOfWork.RepositoryUser.GetAllAsync(x => x.IsAdmin == false, false, "Orders");
        //	var data2 = await _unitOfWork.RepositorReferalTable.GetAllAsync(x => x.IsDeleted == false, false);
        //	List<AppUser> users = data1.ToList();
        //	List<ReferalTable> positions = data2.ToList();
        //	double totalSale = 0;
        //	foreach (var user in users.OrderByDescending(x => x.CreatedTime))
        //	{
        //		int count = 0;
        //		user.ReferalUsers = new List<AppUser>(); // ReferalUsers özelliğini başlat
        //		foreach (var referal in users.OrderByDescending(x => x.CreatedTime).Where(x => x.Id != user.Id))
        //		{
        //			count = 0;
        //			if (referal.IdForReferal == user.Id)
        //			{
        //				count++;

        //				for (int i = referal.Orders.Count - 1; i >= 0; i--)
        //				{
        //					var item = referal.Orders[i];
        //					string dateMonth = item.CreatedAt.Month.ToString();
        //					string dateYear = item.CreatedAt.Year.ToString();

        //					if (dateMonth != DateTime.Now.Month.ToString() || dateYear != DateTime.Now.Year.ToString())
        //					{
        //						referal.Orders.Remove(item);

        //					}
        //				}
        //				referal.TotalAmount += referal.FirstAmount + totalSale;
        //				totalSale += referal.FirstAmount;
        //				user.ReferalUsers.Add(referal);
        //				/*  if(user.IsFirst==true)
        //                        {
        //                            user.TotalAmount += user.FirstAmount;
        //                        }*/
        //				if (referal.Orders.Count > 0)
        //				{
        //					user.ReferalCount += count;
        //				}
        //			}

        //		}
        //		if (user.IsFirst == true)
        //		{
        //			user.TotalAmount += totalSale + user.FirstAmount;
        //		}


        //		foreach (var item in positions)
        //		{
        //			if (user.ReferalCount > item.UserCount && user.TotalAmount > item.MustSale)
        //			{
        //				user.Position = item.Name;
        //				user.Gain = item.Percent + item.SecondPercent + item.ThirdPercent;
        //			}
        //		}
        //		count = 0;
        //	}

        //	return Ok(users.Where(x => x.IsFirst));
        //}


        #endregion
        public async Task<IActionResult> BaseUsers(string? mouth, string? year)
        {
            var data1 = await _unitOfWork.RepositoryUser.GetAllAsync(x => x.IsAdmin == false, false, "Orders");
            var data2 = await _unitOfWork.RepositorReferalTable.GetAllAsync(x => x.IsDeleted == false, false);
            List<AppUser> users = data1.ToList();
            List<ReferalTable> positions = data2.ToList();
            double totalSale = 0;
            foreach (var user in users.OrderByDescending(x => x.CreatedTime))
            {
                int count = 0;
                user.ReferalUsers = new List<AppUser>(); // ReferalUsers özelliğini başlat
                foreach (var referal in users.OrderByDescending(x => x.CreatedTime).Where(x => x.Id != user.Id))
                {
                    if (referal.IdForReferal == user.Id)
                    {
                        for (int i = referal.Orders.Count - 1; i >= 0; i--)
                        {
                            var item = referal.Orders[i];
                            string dateMonth = item.CreatedAt.Month.ToString();
                            string dateYear = item.CreatedAt.Year.ToString();

                            if (dateMonth != DateTime.Now.Month.ToString() || dateYear != DateTime.Now.Year.ToString())
                            {
                                referal.Orders.Remove(item);


                            }
                            else
                            {
                                referal.FirstAmount += Convert.ToDouble(item.TotalAmount);
                            }

                        }
                        referal.TotalAmount += referal.FirstAmount + totalSale;
                        referal.OrderCountForMounth = referal.Orders.Count();
                        totalSale += referal.FirstAmount;
                        if (referal.OrderCountForMounth > 0)
                        {
                            user.ReferalUsers.Add(referal);
                        }
                        user.ReferalCount += user.ReferalUsers.Count() + referal.ReferalUsers.Count();

                    }
                    foreach (var item in positions)
                    {
                        if (!referal.IsFirst && referal.ReferalCount >= item.UserCount && referal.TotalAmount >= item.MustSale)
                        {
                            referal.Position = item.Name;
                            referal.Gain = item.Percent + item.SecondPercent + item.ThirdPercent;
                        }
                    }
                }
                if (user.ReferalUsers.Count > 0 && user.IsFirst == false)
                {
                    user.TotalAmount = 0;

                }
                if (user.IsFirst == true)
                {
                    user.OrderCountForMounth = user.Orders.Count();
                    if (user.OrderCountForMounth > 0)
                    {
                        foreach (var item in user.Orders)
                        {
                            user.FirstAmount = Convert.ToDouble(item.TotalAmount);
                        }
                        user.ReferalCount += count;
                    }
                    user.TotalAmount += totalSale + user.FirstAmount;

                    totalSale = 0;



                }

                foreach (var item in positions)
                {
                    if (user.FirstAmount > item.FIrstAmount && user.ReferalCount >= item.UserCount && user.TotalAmount >= item.MustSale)
                    {
                        user.Position = item.Name;
                        user.Gain = item.Percent + item.SecondPercent + item.ThirdPercent;

                    }


                }


                count = 0;
            }
            List<UserReport> userReports = _mapper.Map<List<UserReport>>(users);
            foreach (var item in userReports)
            {
                foreach (var referal in item.ReferalUsers)
                {
                    if (referal.Position == "Bas Kod")
                    {
                        item.Position = "Lider";
                    }
                }
            }
            return Ok(userReports.Where(x => x.IsFirst));
        }

        [HttpGet("UnblockAccount")]
		public async Task<IActionResult> UnblockAccount(string email)
		{
			var userManager = HttpContext.RequestServices.GetService<UserManager<AppUser>>();
			var user = await userManager.FindByNameAsync(email);

			if (user != null)
			{
				var isLockedOut = await userManager.IsLockedOutAsync(user);

				if (isLockedOut)
				{
					var result = await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
					if (result.Succeeded)
					{
						return StatusCode(StatusCodes.Status200OK, new { result=result });
					}
					else
					{

						return StatusCode(StatusCodes.Status400BadRequest, new { result = result });
					}
				}
				else
				{

					return StatusCode(StatusCodes.Status200OK, new { result = isLockedOut });

				}
			}

			return StatusCode(StatusCodes.Status400BadRequest, new { result = "Istifadəçi tapılamdı" });

		}

		[HttpGet("ConfirmEmail")]
		public async Task<IActionResult> ConfirmEmail(string token, string email)
		{
			var user = await _userManager.FindByNameAsync(email);
			if (user != null)
			{
				var result = await _userManager.ConfirmEmailAsync(user, token);
				if (result.Succeeded)
				{
					return StatusCode(StatusCodes.Status200OK, new { Status = "Success", Message = "Email sent successfully" });
				}
			}
			return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "This user does not exist" });
		}

		private async Task<AppUser> CheckLogin(string username, string password)
		{
			var userManager = HttpContext.RequestServices.GetService<UserManager<AppUser>>();
			var user = await userManager.FindByNameAsync(username);

			if (user != null)
			{
				var result = await userManager.CheckPasswordAsync(user, password);
				if (result)
				{
					await userManager.ResetAccessFailedCountAsync(user);
					return user;
				}
				else
				{
					await userManager.AccessFailedAsync(user);

					if (await userManager.GetAccessFailedCountAsync(user) >= 3)
					{
						await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
					}
				}
			}

			return null;
		}

		[HttpGet("Profile")]
		public async Task<IActionResult> Profile(string id)
		{
			var appUser = await _userManager.FindByIdAsync(id);
			if (appUser == null) throw new UserNotFoundException();
			appUser.ProfileImage = _storage.GetUrl(_storagePath, appUser.ProfileImage);
            return Ok(appUser);
		}

		[HttpPost("UpdateProfile")]
		public async Task<IActionResult> UpdateProfile([FromForm] UserUpdateProfileDto userUpdateProfileDto)
		{
			var appUser = await _userManager.FindByIdAsync(userUpdateProfileDto.Id);
			if (appUser == null) throw new UserNotFoundException();

			if (!RegexManager.CheckMailRegex(userUpdateProfileDto.Email))
			{
				return BadRequest(userUpdateProfileDto);
			}
			if (!RegexManager.CheckPhoneRegex(userUpdateProfileDto.Phone))
			{
				return BadRequest(userUpdateProfileDto);
			}
			
			string fileName = null;

			if (userUpdateProfileDto.FormFile != null)
			{
				_fileService.CheckFileType(userUpdateProfileDto.FormFile, ContentTypeManager.ImageContentTypes);
				var imageInfo = await _storage.UploadAsync(_storagePath, userUpdateProfileDto.FormFile);
				appUser.ProfileImage = imageInfo.fileName;
				fileName = imageInfo.fileName;
            }

            appUser.Name = userUpdateProfileDto.Name;
			appUser.Email = userUpdateProfileDto.Email;
			appUser.otherAddress = userUpdateProfileDto.Address;
			appUser.Surname = userUpdateProfileDto.Surname;
			appUser.PhoneNumber = userUpdateProfileDto.Phone;

			await _unitOfWork.CommitAsync();
			if (fileName != null)
				appUser.ProfileImage = _storage.GetUrl(_storagePath, fileName);
            return Ok();
		}
	}
}
