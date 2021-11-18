using apiProject.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            //orderRate
            CreateMap<IEnumerable<OrderItem>, OrderRate>().ForMember(dto => dto.OrderItems, opt => opt.MapFrom(src => src));
            CreateMap<OrderDetails, OrderRate>().ForMember(dto => dto.OrderDetails, opt => opt.MapFrom(src => src));
            CreateMap<Rate, OrderRate>().ForMember(dto => dto.Rate, opt => opt.MapFrom(src => src));
            CreateMap<IEnumerable<Item>, OrderRate>().ForMember(dto => dto.Items, opt => opt.MapFrom(src => src));
            CreateMap<IEnumerable<ItemFile>, OrderRate>().ForMember(dto => dto.ItemCovers, opt => opt.MapFrom(src => src));
            //ItemList
            CreateMap<IEnumerable<Item>, ItemList>().ForMember(dto => dto.Items, opt => opt.MapFrom(src => src));
            CreateMap<Paginate, ItemList>().ForMember(dto => dto.Paginate, opt => opt.MapFrom(src => src));
            //ItemDetails
            CreateMap<IEnumerable<ItemFile>, ItemDetails>().ForMember(dto => dto.ItemFiles, opt => opt.MapFrom(src => src));
            CreateMap<Item, ItemDetails>().ForMember(dto => dto.Item, opt => opt.MapFrom(src => src));
            //ShoppingCart
            CreateMap<IEnumerable<ShoppingCartItem>, ShoppingCart>().ForMember(dto => dto.ShoppingCartItems, opt => opt.MapFrom(src => src));
            CreateMap<IEnumerable<ShoppingCartItem>, ShoppingCart>().ForMember(dto => dto.TotalCost, opt => opt.MapFrom(src => src.Sum(i=>i.Item.Price)));
            //UserInfor
            CreateMap<User, UserInfor>().ForMember(dto => dto.UserName, opt => opt.MapFrom(u => u.UserName));
            CreateMap<User, UserInfor>().ForMember(dto => dto.Password, opt => opt.MapFrom(u => u.Password));
            CreateMap<User, UserInfor>().ForMember(dto => dto.IpAddress, opt => opt.MapFrom(u => Convert.ToBase64String(u.IpAddress)));
            //User
            CreateMap<UserInfor, User> ().ForMember(dto => dto.UserName, opt => opt.MapFrom(u => u.UserName));
            CreateMap<UserInfor, User>().ForMember(dto => dto.Password, opt => opt.MapFrom(u => u.Password));
            CreateMap<UserInfor, User>().ForMember(dto => dto.IpAddress, opt => opt.MapFrom(u => u.IpAddress));
        }
    }
}
