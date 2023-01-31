using App.DAL.DTO;
using AutoMapper;

namespace App.DAL.EF
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Address, Domain.Address>().ReverseMap();
        }
    }
}