using AutoMapper;
using Avon.Application.Abstractions.User;
using Avon.Application.DTOs.SliderDTOs;
using Avon.Application.DTOs;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Avon.Application.HelperManager;
using Avon.Application.Storages;
using Avon.Application.Abstractions.File;
using Avon.Application.DTOs.ProductDTOs;
using CloudinaryDotNet.Actions;
using Microsoft.VisualBasic;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace Avon.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IUserService _userService;
        readonly IFileService _fileService;
        readonly IStorage _storage;
        readonly IWebHostEnvironment _env;
        static string _storagePath = "/uploads/products/";

        public ProductsController(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService, IFileService fileService, IStorage storage, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _fileService = fileService;
            _storage = storage;
            _env = env;
        }


        [HttpGet]
        [Route("Manage/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = false)
        {
            var entities = await _unitOfWork.RepositoryProduct.GetAllAsync(x => true, false, "Attribute", "ProductSubCategories", "ProductImages");
            int pageSize = 10;

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted);
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord));
            }

            var list = PagenatedListDto<Product>.Save(entities, page, pageSize);
            list.ForEach(x =>
            {
                x.PosterImage = _storage.GetUrl(_storagePath, x.PosterImage);
            });

            return Ok(list);
        }


        [HttpPost]
        [Route("Manage/Create")]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto objectDto)
        {

            if (!ModelState.IsValid) return BadRequest(objectDto);
            
            _fileService.CheckFileType(objectDto.PosterFile, ContentTypeManager.ImageContentTypes);

            if (objectDto.ImageFiles != null)
            {
                objectDto.ImageFiles.ForEach(file =>
                {
                    _fileService.CheckFileType(file, ContentTypeManager.ImageContentTypes);
                });
            }

            

            if (!await _unitOfWork.RepositoryBrand.IsAny(x => x.Id == objectDto.BrandId))
            {
                throw new BrandNotFoundException();
            }

            if (!await _unitOfWork.RepositoryTeg.IsAny(x => x.Id == objectDto.TegId))
            {
                throw new TagNotFoundException();
            }

            var entity = _mapper.Map<Product>(objectDto);
            string fileName = null;

            var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.PosterFile);
            entity.PosterImage = imageInfo.fileName;
            fileName = imageInfo.fileName;

            await _unitOfWork.RepositoryProduct.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            await _unitOfWork.RepositoryProductTeg.InsertAsync(new ProductTeg
            {
                TegId = objectDto.TegId,
                ProductId = entity.Id
            });

            await _unitOfWork.CommitAsync();


            if (objectDto.ProductSubCategoryIds != null)
            {
                foreach (var item in objectDto.ProductSubCategoryIds)
                {
                    await _unitOfWork.RepositoryProductSubCategory.InsertAsync(new ProductSubCategory
                    {
                        ProductId = entity.Id,
                        SubCategoryId = item
                    });
                }
            }

            List<ProductImages> images = new List<ProductImages>();
            if (objectDto.ImageFiles != null)
            {
                objectDto.ImageFiles.ForEach(async file =>
                {
                    var imageInfo = await _storage.UploadAsync("/uploads/productImages/", file);

                    //var imageInfo = await _storage.UploadAsync("//uploads/products/", file);
                    var image = new ProductImages()
                    {
                        ProductId = entity.Id,
                        Image = imageInfo.fileName,
                        IsPoster = false,
                    };
                    images.Add(image);
                });
            }

            await _unitOfWork.RepositoryImages.InsertRangeAsync(images);
            await _unitOfWork.CommitAsync();

            entity.PosterImage = _storage.GetUrl(_storagePath, fileName);

            return Ok(new { product = entity, images = images });
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _unitOfWork.RepositoryProduct.GetAsync(x => x.Id == id,false, "ProductAttribute", "Brand", "ProductSubCategory", "ProductImages", "ProductTeg");
            if (product == null) throw new ProductNotFoundException();

            product.ProductImages.ForEach(x =>
            {
                x.Image = _storage.GetUrl("/uploads/productImages/", x.Image);
            });
            product.PosterImage = _storage.GetUrl(_storagePath, product.PosterImage);
            return Ok(product);
        }

        [HttpPost]
        [Route("Manage/Edit")]
        public async Task<IActionResult> Edit([FromForm] ProductEditDto objectDto)
        {
            var existEntity = await _unitOfWork.RepositoryProduct.GetAsync(x => x.Id == objectDto.Id,true,"ProductImages");
            if (existEntity == null) throw new ProductNotFoundException();


            var brandCheck = await _unitOfWork.RepositoryBrand.GetAsync(x => x.Id == objectDto.BrandId);
            if (brandCheck == null) throw new BrandNotFoundException();

            if (!ModelState.IsValid) return BadRequest(objectDto);
            
            if(objectDto.PosterFile != null)
                _fileService.CheckFileType(objectDto.PosterFile, ContentTypeManager.ImageContentTypes);

            if (objectDto.ImageFiles != null)
            {
                objectDto.ImageFiles.ForEach(file =>
                {
                    _fileService.CheckFileType(file, ContentTypeManager.ImageContentTypes);
                });
            }

            string storageUrl = null;

            if (objectDto.PosterFile != null)
            {
                var check = await _storage.DeleteAsync(_storagePath, existEntity.PosterImage);
                var imageInfo = await _storage.UploadAsync(_storagePath, objectDto.PosterFile);
                existEntity.PosterImage = imageInfo.fileName;
                storageUrl = _storage.GetUrl(_storagePath, imageInfo.fileName);
            }

            List<ProductImages> images = new List<ProductImages>();
            if (objectDto.ImageFiles != null)
            {
                objectDto.ImageFiles.ForEach(async file =>
                {
                    var imageInfo = await _storage.UploadAsync("/uploads/productImages/", file);
                    var image = new ProductImages()
                    {
                        ProductId = existEntity.Id,
                        Image = imageInfo.fileName,
                        IsPoster = false,
                    };
                    images.Add(image);
                });
            }

            existEntity.ProductImages.ForEach(async x =>
            {
                var check = await _storage.DeleteAsync(_storagePath, x.Image);
            });


            await _unitOfWork.RepositoryProductSubCategory.RemoveRange(x => x.ProductId == existEntity.Id);
            await _unitOfWork.RepositoryProductTeg.RemoveRange(x => x.ProductId == existEntity.Id);

            if (objectDto.ProductSubCategoryIds != null)
            {
                foreach (var item in objectDto.ProductSubCategoryIds)
                {
                    await _unitOfWork.RepositoryProductSubCategory.InsertAsync(new ProductSubCategory
                    {
                        ProductId = existEntity.Id,
                        SubCategoryId = item
                    });
                }
            }

            if (objectDto.ProductTegIds != null)
            {
                foreach (var item in objectDto.ProductTegIds)
                {
                    await _unitOfWork.RepositoryProductTeg.InsertAsync(new ProductTeg
                    {
                        ProductId = existEntity.Id,
                        TegId = item
                    });
                }
            }

            existEntity.AttributeId = objectDto.AttributeId;
            existEntity.BrandId = objectDto.BrandId;    
            existEntity.CostPrice = objectDto.CostPrice;
            existEntity.SalePrice = objectDto.SalePrice;
            existEntity.DiscountPrice = objectDto.DiscountPrice;

            existEntity.Description = objectDto.Description;
            existEntity.inStock = objectDto.inStock;
            existEntity.IsNew = objectDto.IsNew;
            existEntity.PreOrder = objectDto.PreOrder;
            existEntity.StockCount = objectDto.StockCount;
            existEntity.Name = objectDto.Name;

            await _unitOfWork.RepositoryImages.InsertRangeAsync(images);
            await _unitOfWork.CommitAsync();

            existEntity.PosterImage = storageUrl;

            existEntity.ProductImages.ForEach(x =>
            {
                x.Image = _storage.GetUrl("/uploads/productImages/", x.Image);
            });

            return Ok(new { product = existEntity, images = images });
        }
		
        [HttpPost]
		[Route("Manage/DeleteProductImage")]
		public async Task<IActionResult> DeleteProductImage(int id)
        {   
            var productImage = await _unitOfWork.RepositoryImages.GetAsync(x => x.Id == id);

            if (productImage == null) throw new ProductImageNotFoundException();

			await _storage.DeleteAsync("/uploads/productImages/", productImage.Image);

            return Ok();
        }

		[HttpDelete]
		[Route("Manage/Delete")]
		public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.RepositoryProduct.GetAsync(x => x.Id == id);

            if (product == null) throw new ProductNotFoundException();

            product.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositoryProduct.GetAsync(x => x.Id == id);

            if (entity == null) throw new SliderNotFoundException();

            entity.IsDeleted = false;

            await _unitOfWork.CommitAsync();

            return Ok();
        }


        [HttpGet]
		[Route("GetProducts")]
		public async Task<IActionResult> GetProducts(int subCategoryId)
        {
            var products = await _unitOfWork.RepositoryProductSubCategory.GetAllAsync(x => x.SubCategoryId == subCategoryId,false,"Product");

            var list = products.ToList();

            if (list.Count == 0) throw new ProductNotFoundException();

            list.ForEach(x =>
            {
                x.Product.PosterImage = _storage.GetUrl(_storagePath, x.Product.PosterImage);
            });


            return Ok(list);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string searchWord)
        {
            List<Product> products = new();

            if (searchWord.Length >= 3)
            {
                var query = await _unitOfWork.RepositoryProduct.GetAllAsync(x => x.Name.Contains(searchWord) && !x.IsDeleted,false);
                products = query.Take(3).ToList();
            }

            products.ForEach(x =>
            {
                x.PosterImage = _storage.GetUrl(_storagePath, x.PosterImage);
            });


            return Ok(products);
        }


  //      [HttpGet]
		//[Route("GetProduct")]
		//public async Task<IActionResult> GetProduct(int productId)
  //      {
  //          var product = await _unitOfWork.RepositoryProduct.GetAsync(x => x.Id == productId,false,"Attribute","Comments", "ProductSubCategories", "ProductImages","Brand");

  //          if(product == null) throw new ProductNotFoundException();


  //          products.ForEach(x =>
  //          {
  //              x.PosterImage = _storage.GetUrl(_storagePath, x.PosterImage);
  //          });
            
  //          return Ok(product);
  //      }
    }
}
