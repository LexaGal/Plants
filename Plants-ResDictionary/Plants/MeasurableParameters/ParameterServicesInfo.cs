using System.Collections.Generic;
using System.Linq;
using PlantingLib.Plants.ServiceStates;

namespace PlantingLib.MeasurableParameters
{
    public static class ParameterServicesInfo
    {
        public static List<ParameterServices> ParametersServices;

        public static ParameterServices GetParameterInfo(string mt)
        {
            return ParametersServices.SingleOrDefault(p => p.MeasurableType == mt);
        }

        public static void SetBaseParameters()
        {
            ParametersServices = new List<ParameterServices>
            {
                new ParameterServices(ParameterEnum.Nutrient.ToString(),
                    new List<ServiceState> {new ServiceState(ServiceStateEnum.Nutrienting.ToString(), false)}),
                new ParameterServices(ParameterEnum.SoilPh.ToString(),
                    new List<ServiceState> {new ServiceState(ServiceStateEnum.Nutrienting.ToString(), false)}),
                new ParameterServices(ParameterEnum.Humidity.ToString(),
                    new List<ServiceState> {new ServiceState(ServiceStateEnum.Watering.ToString(), false)}),
                new ParameterServices(ParameterEnum.Temperature.ToString(),
                    new List<ServiceState>
                    {
                        new ServiceState(ServiceStateEnum.Warming.ToString(), false),
                        new ServiceState(ServiceStateEnum.Cooling.ToString(), false)
                    })
            };
        }
    }
}