using App.BLL.Mappers;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        private readonly IMapper _mapper;

        private IAddressService _address;

        public AppBLL(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public virtual IAddressService Addresses =>
            _address ?? (_address = new AddressService(UnitOfWork.Addresses, new AddressMapper(_mapper)));
    }
}