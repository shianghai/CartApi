using AutoMapper;
using CartApi.Domain.Entities;
using CartApi.Dtos.Read;
using CartApi.Dtos.Write;
using HotelApi.DTOS.WriteDtos;
using System.Collections.Generic;

namespace CartApi
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<ItemWriteDto, Item>().ReverseMap();
            CreateMap<Item, ItemReadDto>().ReverseMap();

            CreateMap<UserWriteDto, ApiUser>().ReverseMap();

            CreateMap<ItemWriteDto, List<Item>>();

            CreateMap<Item, IEnumerable<Item>>()
                .ConvertUsing(source => new List<Item> { source });

            CreateMap<ItemReadDto, IEnumerable<ItemReadDto>>()
                .ConvertUsing(source => new List<ItemReadDto> { source });

            

        }
    }
}
