using Avon.Application.Repositories;
using Avon.Domain.Entities;
using Avon.Domain.Entitity;

namespace Avon.Application.UnitOfWorks
{
    public interface IUnitOfWork
	{
		public IRepository<AppUser> RepositoryUser { get; set; }
		public IRepository<Category> RepositoryCategory { get; set; }
		public IRepository<SubCategory> RepositorySubCategory { get; set; }
		public IRepository<BasketItem> RepositoryBasketItem { get; set; }
		public IRepository<Product> RepositoryProduct { get; set; }
		public IRepository<WishList> RepositoryWishList { get; set; }
		public IRepository<Variant> RepositoryVariant { get; set; }
		public IRepository<VariantFeature> RepositoryVariantFeature { get; set; }
		public IRepository<VFeature> RepositoryVFeature { get; set; }
		public IRepository<Feature> RepositoryFeature { get; set; }
		public IRepository<Promotion> RepositoryPromoiton { get; set; }
		public IRepository<Slider> RepositorySlider { get; set; }
		public IRepository<Comment> RepositoryComment { get; set; }
		public IRepository<Setting> RepositorySetting { get; set; }
        public IRepository<ReferalTable> RepositorReferalTable { get; set; }
        public IRepository<ContactUs> RepositoryContactUs { get; set; }
		public IRepository<Order> RepositoryOrders { get; set; }
		public IRepository<DeliveryAdress> RepositoryDeliveryAddresses { get; set; }
		public IRepository<ProductImages> RepositoryImages { get; set; }
		public IRepository<ProductSubCategory> RepositoryProductSubCategory { get; set; }
        public IRepository<Color> RepositoryColors { get; set; }
        public IRepository<Size> RepositorySizes { get; set; }
        public IRepository<ImageFor> RepositoryImageFor { get; set; }
        public IRepository<Teg> RepositoryTeg { get; set; }
        public IRepository<Brand> RepositoryBrand { get; set; }
        public IRepository<NewsCategory> RepositoryNewsCategory { get; set; }
        public IRepository<News> RepositoryNews { get; set; }

        public IRepository<ProductTeg> RepositoryProductTeg { get; set; }
        public IRepository<NewsTeg> RepositoryNewsTeg { get; set; }

        Task<int> CommitAsync();
	}
}
 