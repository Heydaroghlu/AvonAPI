using AutoMapper;
using Avon.Application.DTOs.BrandDTOs;
using Avon.Application.DTOs.CategoryDTOs;
using Avon.Application.DTOs.ColorDTOs;
using Avon.Application.DTOs.CommentDTOs;
using Avon.Application.DTOs.ContactUsDTOs;
using Avon.Application.DTOs.DeliveryAddressDTOs;
using Avon.Application.DTOs.FeatureDTOs;
using Avon.Application.DTOs.NewCategoryDTOs;
using Avon.Application.DTOs.NewDTOs;
using Avon.Application.DTOs.OrderDTOs;
using Avon.Application.DTOs.ProductDTOs;
using Avon.Application.DTOs.PromotionDTOs;
using Avon.Application.DTOs.SizeDTOs;
using Avon.Application.DTOs.SliderDTOs;
using Avon.Application.DTOs.SubCategoryDTOs;
using Avon.Application.DTOs.TagDTOs;
using Avon.Application.DTOs.UserDTOs;
using Avon.Application.DTOs.VFeatureDTOs;
using Avon.Application.DTOs.WishListDTOs;
using Avon.Domain.Entities;

namespace Avon.Application.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, CategoryReturnDto>();
            CreateMap<SubCategoryCreateDto, SubCategory>();
            CreateMap<SubCategory, SubCategoryReturnDto>();


            CreateMap<Feature, FeatureReturnDto>();
            CreateMap<FeatureCreateDto, Feature>();

            CreateMap<Promotion, PromotionReturnDto>();
            CreateMap<PromotionCreateDto, Promotion>();

            CreateMap<Slider, SliderReturnDto>();
            CreateMap<SliderCreateDto, Slider>();

            CreateMap<ContactUs, ContactUsReturnDto>();
            CreateMap<ContactUsCreateDto, ContactUs>();

            CreateMap<DeliveryAddressCreateDto, DeliveryAdress>();
            CreateMap<WishList, WishListReturnDto>();


            CreateMap<ProductCreateDto, Product>();
			CreateMap<ProductEditDto, Product>();

			CreateMap<OrderCreateDto, Order>();
            CreateMap<OrderCreateDto, Order>();
            CreateMap<CommentCreateDto, Comment>();

            CreateMap<BrandCreateDto, Brand>();
            CreateMap<NewsCategoryCreateDto, NewsCategory>();
            CreateMap<NewsCategoryRetunDto, NewsCategory>();

            CreateMap<NewsCreateDto, News>();
            CreateMap<TagCreateDto, Teg>();
            CreateMap<Teg, TagReturnDto>();

            CreateMap<ColorCreateDTO, Color>();
            CreateMap<SizeCreateDTO, Size>();
            CreateMap<AppUser, UserReport>();

			CreateMap<CreateVFeatureDto, VFeature>();
			CreateMap<EditVFeatureDto, VFeature>();
			CreateMap<VFeature, ReturnVFeatureDto>();

		}

    }
}
