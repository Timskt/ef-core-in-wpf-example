using App.DAL.DTO;
using AutoMapper;
using Base.Mapper;

namespace App.DAL.EF.Mappers
{
    public class AddressMapper : BaseMapper<Address, Domain.Address>
    {
        public AddressMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}