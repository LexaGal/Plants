﻿using System;
using Database.DatabaseStructure.Repository.Abstract;
using SensorMapping = Database.MappingTypes.SensorMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class SensorMappingRepository : Repository<SensorMapping>, ISensorMappingRepository
    {
        public override bool Edit(Guid id, SensorMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}