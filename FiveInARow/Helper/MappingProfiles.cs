using AutoMapper;
using FiveInARow.Dto;
using FiveInARow.Models;

namespace FiveInARow.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<GameRecord, GameRecordDto>();
            CreateMap<GameRecordDto, GameRecord>();
        }
    }
}