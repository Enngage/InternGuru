using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Helpers;
using Entity.Base;

namespace Web.Lib.AutoMapper
{
    public static class AutoMapperHelper
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;

                // create maps here

                // Map all entities to themselves to enable deep cloning
                // If Entity is not mapped, it will error our because it would not be possible to
                // create a deep copy of it 
                foreach (var entityType in TypeHelper.GetClassesImplementingInterface(typeof(IEntity)))
                {
                    // map to itself, otherwise deep copy will not be created by automapper
                    cfg.CreateMap(entityType, entityType);
                }
            });

            return new Mapper(config);
        }


    }
}