using Arch.Domain.Adapters.Helper;
using AutoMapper;

namespace BookApi.Helper
{
    public class MapperHelper<T, TT, TF>
    {
        private readonly IMapper _mapper;

        public MapperHelper(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// metadata Model para Entidade
        /// </summary>
        /// <param name="metadata">Model</param>
        /// <returns>Entidade</returns>
        public async Task<Metadata<T, TF>> FromModel(Metadata<TT, TF> metadata)
        {
            return _mapper.Map<Metadata<TT, TF>, Metadata<T, TF>>(metadata);
        }

        public T FromModel(TT obj)
        {
            return _mapper.Map<TT, T>(obj);
        }

        public async Task<Metadata<TT, TF>> ToModel(Metadata<T, TF> metadata)
        {
            return _mapper.Map<Metadata<T, TF>, Metadata<TT, TF>>(metadata);
        }

        public TT ToModel(T obj)
        {
            return _mapper.Map<T, TT>(obj);
        }
    }
}
