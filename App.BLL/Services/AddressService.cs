using App.BLL.DTO;
using App.BLL.Mappers;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;

namespace App.BLL.Services
{
    public class AddressService : BaseEntityService<Address, DAL.DTO.Address, IAddressRepository>, IAddressService
    {
        public AddressService(IAddressRepository repository, AddressMapper mapper) : base(
            repository, mapper)
        {
        }
    }
}