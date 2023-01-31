using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Mappers;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF
{
    public class AppUnitOfWork : BaseUnitOfWork<ExampleDbContext>, IAppUnitOfWork
    {
        private readonly IMapper _mapper;

        private IAddressRepository _address;

        public AppUnitOfWork(ExampleDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public virtual IAddressRepository Addresses =>
            _address ?? (_address = new AddressRepository(UowDbContext, new AddressMapper(_mapper)));
    }
}