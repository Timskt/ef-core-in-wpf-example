using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories
{
    public class AddressRepository : BaseEntityRepository<Address, Domain.Address, ExampleDbContext>, IAddressRepository
    {
        public AddressRepository(ExampleDbContext dbContext, IMapper<Address, Domain.Address> mapper) : base(dbContext,
            mapper)
        {
        }
    }
}