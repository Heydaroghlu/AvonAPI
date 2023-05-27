using Avon.Application.Repositories;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Avon.Domain.Entitity;
using Avon.Persistence.Contexts;
using Avon.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {

        readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            RepositoryUser = new Repository<AppUser>(_context);
            RepositoryCategory = new Repository<Category>(_context);
            RepositorySubCategory = new Repository<SubCategory>(_context);
            RepositoryBasketItem = new Repository<BasketItem>(_context);
            RepositoryProduct = new Repository<Product>(_context);
            RepositoryWishList = new Repository<WishList>(_context);
            RepositoryFeature = new Repository<Feature>(_context);
            RepositorySlider = new Repository<Slider>(_context);
            RepositoryComment = new Repository<Comment>(_context);
            RepositorySetting = new Repository<Setting>(_context);
            RepositoryContactUs = new Repository<ContactUs>(_context);
            RepositorReferalTable=new Repository<ReferalTable>(_context);
            RepositoryOrders = new Repository<Order>(_context);
            RepositoryDeliveryAddresses = new Repository<DeliveryAdress>(_context);
            RepositoryImages = new Repository<ProductImages>(_context);
            RepositoryProductSubCategory = new Repository<ProductSubCategory>(_context);
            RepositoryColors=new Repository<Color>(_context);
            RepositorySizes=new Repository<Size>(_context);
            RepositoryImageFor=new Repository<ImageFor>(_context);
            RepositoryCategoryColor = new Repository<CategoryColor>(_context);
            RepositoryTeg = new Repository<Teg>(_context);
            RepositoryNewsCategory = new Repository<NewsCategory>(_context);
            RepositoryNews = new Repository<News>(_context);
            RepositoryBrand = new Repository<Brand>(_context);
            RepositoryNewsTeg = new Repository<NewsTeg>(_context);
            RepositoryProductTeg = new Repository<ProductTeg>(_context);
            RepositoryPromoiton = new Repository<Promotion>(_context);
			RepositoryVariant = new Repository<Variant>(_context);
			RepositoryVFeature = new Repository<VFeature>(_context);
			RepositoryVariantFeature = new Repository<VariantFeature>(_context);
		}

        public IRepository<AppUser> RepositoryUser { get; set; }
        public IRepository<Category> RepositoryCategory { get; set; }

        public IRepository<SubCategory> RepositorySubCategory { get; set; }
        public IRepository<BasketItem> RepositoryBasketItem { get; set; }
        public IRepository<Product> RepositoryProduct { get; set; }
        public IRepository<WishList> RepositoryWishList { get; set; }
        public IRepository<Feature> RepositoryFeature { get; set; }
        public IRepository<Promotion> RepositoryPromoiton { get; set; }

        public IRepository<Slider> RepositorySlider { get; set; }
        public IRepository<Comment> RepositoryComment { get; set; }
        public IRepository<Setting> RepositorySetting { get; set; }
        public IRepository<ContactUs> RepositoryContactUs { get; set; }
        public IRepository<ReferalTable> RepositorReferalTable { get ; set ; }
        public IRepository<Order> RepositoryOrders { get; set; }
        public IRepository<DeliveryAdress> RepositoryDeliveryAddresses { get; set; }
        public IRepository<ProductImages> RepositoryImages { get; set; }
        public IRepository<ProductSubCategory> RepositoryProductSubCategory { get; set; }
        public IRepository<Color> RepositoryColors { get ; set ; }
        public IRepository<Size> RepositorySizes { get ; set ; }
        public IRepository<ImageFor> RepositoryImageFor { get ; set ; }
        public IRepository<CategoryColor> RepositoryCategoryColor { get; set; }

        public IRepository<Teg> RepositoryTeg { get; set; }
        public IRepository<NewsCategory> RepositoryNewsCategory { get; set; }
        public IRepository<News> RepositoryNews { get; set; }
        public IRepository<Brand> RepositoryBrand { get; set; }
        public IRepository<NewsTeg> RepositoryNewsTeg { get; set; }
        public IRepository<ProductTeg> RepositoryProductTeg { get; set; }
		public IRepository<Variant> RepositoryVariant { get; set; }
		public IRepository<VariantFeature> RepositoryVariantFeature { get; set; }
		public IRepository<VFeature> RepositoryVFeature { get; set; }

		public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
