using System;
using AutoMapper;

namespace Core.AutoMapper
{
    public static class MapperProvider
    {
        private static IMapper _mapper;

        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    throw new NotSupportedException("Mapper has not been initialized");

                }
                return _mapper;
            }
        }

        /// <summary>
        /// Initializes mapper
        /// </summary>
        /// <param name="mapper">Instance of mapper</param>
        public static void SetMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
