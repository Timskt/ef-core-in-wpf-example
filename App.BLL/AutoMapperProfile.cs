using App.BLL.DTO;
using AutoMapper;

namespace App.BLL
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Address, DAL.DTO.Address>().ReverseMap();
        }
    }
}