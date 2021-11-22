using apiProject.DTO;
using apiProject.Models.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
            //OrderDetails
            CreateMap<ShoppingCartDTO, OrderDetails>()
                .ForMember(dto => dto.OrderTime, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dto => dto.TotalCost, opt => opt.MapFrom(src => src.ShoppingCartItems.Select(s => s.Price * s.Quantity).Sum()))
                .ForMember(dto =>dto.UserName, opt => opt.MapFrom(src=>src.UserName))
                .ForMember(dto=>dto.Status, opt=>opt.MapFrom(src=> OrderStatus.Preparing.ToString()))
                .ForMember(dto => dto.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress));
            CreateMap<OrderDetails, OrderDetailDTO>()
                .ForMember(dto => dto.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dto => dto.OrderTime, opt => opt.MapFrom(src => src.OrderTime))
                .ForMember(dto => dto.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dto => dto.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dto => dto.TotalCost, opt => opt.MapFrom(src => src.TotalCost))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.UserName));
            CreateMap<IEnumerable<ItemDTO>, OrderDetailDTO>()
                .ForMember(dto => dto.items, opt => opt.MapFrom(src => src));

            //OrderList
            CreateMap<IEnumerable<OrderDetailDTO>, OrderList>().ForMember(dto => dto.Orders, opt => opt.MapFrom(src => src));
            CreateMap<Paginate, OrderList>().ForMember(dto => dto.Paginate, opt => opt.MapFrom(src => src));
            //OrderItem
            CreateMap<ShoppingCartItem, OrderItem>()
                .ForMember(dto => dto.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dto => dto.Quantity, opt => opt.MapFrom(src => src.Quantity));
            CreateMap<OrderDetails, OrderItem>()
                .ForMember(dto => dto.OrderId, opt => opt.MapFrom(src => src.OrderId));
            //ItemList
            CreateMap<IEnumerable<ItemDTO>, ItemList>().ForMember(dto => dto.Items, opt => opt.MapFrom(src => src));
            CreateMap<Paginate, ItemList>().ForMember(dto => dto.Paginate, opt => opt.MapFrom(src => src));
            //ItemDetails
            CreateMap<IEnumerable<ItemFile>, ItemDetails>().ForMember(dto => dto.ItemFiles, opt => opt.MapFrom(src => src));
            CreateMap<Item, ItemDetails>().ForMember(dto => dto.Item, opt => opt.MapFrom(src => src));
            //ShoppingCart
            CreateMap<IEnumerable<ShoppingCartItem>, ShoppingCartDTO>()
                .ForMember(dto => dto.ShoppingCartItems, opt => opt.MapFrom(src => src))
                .ForMember(dto => dto.TotalCost, opt => opt.MapFrom(src => src.Sum(i=>i.Price)));
            //UserInfor
            CreateMap<User, UserInfor>()
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(u => u.UserName))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(u => u.Password))
                .ForMember(dto => dto.IpAddress, opt => opt.MapFrom(u => Encoding.ASCII.GetString(u.IpAddress)));
            //User
            CreateMap<IdentityUser, User> ()
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(u => u.UserName))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(u => u.PasswordHash));
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
