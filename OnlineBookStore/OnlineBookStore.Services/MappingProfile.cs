using AutoMapper;
using OnlineBookStore.Models;

namespace OnlineBookStore.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();
            CreateMap<CartItem, CartItemDTO>().ReverseMap();
        }
    }
}
