using AutoMapper;
using Base.Contracts.Mapper;

namespace Base.Mapper
{
    public abstract class BaseMapper<TOut, TIn> : IMapper<TOut, TIn>
    {
        public readonly IMapper Mapper;

        protected BaseMapper(IMapper mapper)
        {
            Mapper = mapper;
        }

        public TOut Map(TIn entity)
        {
            return Mapper.Map<TOut>(entity);
        }

        public TIn Map(TOut entity)
        {
            return Mapper.Map<TIn>(entity);
        }
    }
}