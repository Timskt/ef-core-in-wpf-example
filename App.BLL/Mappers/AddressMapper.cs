using App.BLL.DTO;
using AutoMapper;
using Base.Mapper;

namespace App.BLL.Mappers
{
    public class AddressMapper : BaseMapper<Address, DAL.DTO.Address>
    {
        public AddressMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}