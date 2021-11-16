using apiProject.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
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
            CreateMap<IEnumerable<User>, ItemList>().ForMember(dto => dto.Uploaders, opt => opt.MapFrom(src => src.Select(u=>u.UserName)));
            CreateMap<ItemFile, ItemList>().ForMember(dto => dto.ItemFileCover, opt => opt.MapFrom(src => src));
            //ItemDetails
            CreateMap<IEnumerable<ItemFile>, ItemDetails>().ForMember(dto => dto.ItemFiles, opt => opt.MapFrom(src => src));
            CreateMap<Item, ItemDetails>().ForMember(dto => dto.Item, opt => opt.MapFrom(src => src));
        }
    }
}
