using AutoMapper;
using Online_Marketplace.DAL.Entities;
using Online_Marketplace.DAL.Entities.Models;
using Online_Marketplace.Shared.DTOs;

namespace Online_Marketplace.BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>();

            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductCreateDto>();

           
            CreateMap<Category, CreateCategoryDto>();
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<Category, CategoryWithProductsDto>();
            CreateMap<CategoryWithProductsDto, Category>();


            CreateMap<Product, ProductsDto>();
            CreateMap<ProductsDto, Product>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Cart, CartItemDto>();

            CreateMap<CartItemDto , Cart >();
            CreateMap<Cart, CartItemDto >();

          
            CreateMap<Product, ProductViewDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImagePath));


            CreateMap<ProductReviews, ReviewDto>();
            CreateMap<ReviewDto, ProductReviews>();


            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();

            CreateMap<OrderItem, OrderItemDto>();

        }
    }
}
