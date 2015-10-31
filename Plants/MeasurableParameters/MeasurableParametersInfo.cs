using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using PlantingLib.Plants.ServiceStates;

namespace PlantingLib.MeasurableParameters
{
    public static class MeasurableParametersInfo
    {
        public static List<ParameterInfo> ParametersInfo;

        public static ParameterInfo GetParameterInfo(string mt)
        {
            return ParametersInfo.SingleOrDefault(p => p.MeasurableType == mt);
        }

        public static void SetBaseParameters()
        {
            ParametersInfo = new List<ParameterInfo>
            {
                new ParameterInfo(ParameterEnum.Nutrient.ToString(),
                    new List<ServiceState> {new ServiceState(ServiceStateEnum.Nutrienting.ToString(), false)}),
                new ParameterInfo(ParameterEnum.SoilPh.ToString(),
                    new List<ServiceState> {new ServiceState(ServiceStateEnum.Nutrienting.ToString(), false)}),
                new ParameterInfo(ParameterEnum.Humidity.ToString(),
                    new List<ServiceState> {new ServiceState(ServiceStateEnum.Watering.ToString(), false)}),
                new ParameterInfo(ParameterEnum.Temperature.ToString(),
                    new List<ServiceState>
                    {
                        new ServiceState(ServiceStateEnum.Warming.ToString(), false),
                        new ServiceState(ServiceStateEnum.Cooling.ToString(), false)
                    })
            };
        }
    }
}