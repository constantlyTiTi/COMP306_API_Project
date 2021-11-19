using apiProject.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
            CreateMap<IEnumerable<ItemDTO>, ItemList>().ForMember(dto => dto.Items, opt => opt.MapFrom(src => src));
            CreateMap<Paginate, ItemList>().ForMember(dto => dto.Paginate, opt => opt.MapFrom(src => src));
            //ItemDetails
            CreateMap<IEnumerable<ItemFile>, ItemDetails>().ForMember(dto => dto.ItemFiles, opt => opt.MapFrom(src => src));
            CreateMap<Item, ItemDetails>().ForMember(dto => dto.Item, opt => opt.MapFrom(src => src));
            //ShoppingCart
            CreateMap<IEnumerable<ShoppingCartItem>, ShoppingCart>()
                .ForMember(dto => dto.ShoppingCartItems, opt => opt.MapFrom(src => src))
                .ForMember(dto => dto.TotalCost, opt => opt.MapFrom(src => src.Sum(i=>i.Item.Price)));
            //UserInfor
            CreateMap<User, UserInfor>()
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(u => u.UserName))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(u => u.Password))
                .ForMember(dto => dto.IpAddress, opt => opt.MapFrom(u => Encoding.ASCII.GetString(u.IpAddress)));
            //User
            CreateMap<UserInfor, User> ()
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(u => u.UserName))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(u => u.Password))
                .ForMember(dto => dto.IpAddress, opt => opt.MapFrom(u => Encoding.ASCII.GetBytes(u.IpAddress)));
            //ItemDTO to Item
            CreateMap<ItemDTO, Item>()
                .ForMember(dto => dto.ItemId, opt => opt.Ignore())
                .ForMember(dto => dto.ItemName, opt => opt.MapFrom(i => i.ItemName))
                .ForMember(dto => dto.LocationPostalCode, opt => opt.MapFrom(i => i.LocationPostalCode))
                .ForMember(dto => dto.Price, opt => opt.MapFrom(i => i.Price))
                .ForMember(dto => dto.UploadItemDateTime, opt => opt.MapFrom(i => i.UploadItemDateTime))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(i => i.UserName))
                .ForMember(dto => dto.Description, opt => opt.MapFrom(i => i.Description))
                .ForMember(dto => dto.Category, opt => opt.MapFrom(i => i.Category));
            CreateMap<Item, ItemDTO > ()
                .ForMember(dto => dto.ItemId, opt => opt.MapFrom(i => i.ItemId))
                .ForMember(dto => dto.ItemName, opt => opt.MapFrom(i => i.ItemName))
                .ForMember(dto => dto.LocationPostalCode, opt => opt.MapFrom(i => i.LocationPostalCode))
                .ForMember(dto => dto.Price, opt => opt.MapFrom(i => i.Price))
                .ForMember(dto => dto.UploadItemDateTime, opt => opt.MapFrom(i => i.UploadItemDateTime))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(i => i.UserName))
                .ForMember(dto => dto.Description, opt => opt.MapFrom(i => i.Description))
                .ForMember(dto => dto.Category, opt => opt.MapFrom(i => i.Category));
            CreateMap<IEnumerable<ItemFile>, ItemDTO>()
                .ForMember(dto => dto.ItemImagePaths, opt => opt.MapFrom(i => i.Select(f => f.ImgFileKey)))
                .ForMember(dto => dto.CoverImagePath, opt => opt.MapFrom(i => i.Select(f => ResourceUrl.ImgBucket.ToUrl() + f.ImgFileKey).FirstOrDefault()));
        }
    }
}
