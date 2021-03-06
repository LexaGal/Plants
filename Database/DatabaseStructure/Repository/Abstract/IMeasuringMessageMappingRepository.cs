﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Database.MappingTypes;

namespace Database.DatabaseStructure.Repository.Abstract
{
    public interface IMeasuringMessageMappingRepository : IRepository<MeasuringMessageMapping>
    {
        int SaveMany(List<MeasuringMessageMapping> measuringMessageMappings);
        int DeleteMany(int n = 1000, Expression<Func<MeasuringMessageMapping, bool>> func = null);
    }
}