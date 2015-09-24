using System;
using System.Linq;
using System.Timers;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;

namespace PlantingLib.ServiceSystems
{
    public abstract class ServiceSystem
    {
        public PlantsArea PlantsArea { get; private set; }
        public MeasurableTypesEnum MeasurableType { get; private set; }
        public double ParameterValue { get; private set; }

        protected ServiceSystem(MeasurableTypesEnum measurableType, double parameterValue, PlantsArea plantsArea)
        {
            PlantsArea = plantsArea;
            MeasurableType = measurableType;
            ParameterValue = parameterValue;
        }

        public void SetSensorsState(bool state)
        {
            PlantsArea.Sensors
                .Where(s => s.MeasurableParameter.MeasurableType == MeasurableType)
                .ToList()
                .ForEach(s => s.IsOn = state);
        }

        public void ResetSensorsFunctions()
        {
            PlantsArea.Sensors
                .Where(s => s.MeasurableParameter.MeasurableType == MeasurableType)
                .ToList()
                .ForEach(s => s.Function.ResetFunction());

        }

        public abstract TimeSpan ComputeTimeForService();

        public void StartService(TimeSpan timeSpan, Func<ServiceMessage, ServiceMessage> func)
        {
            SetSensorsState(false);

            Timer timer = new Timer(ComputeTimeForService().TotalMilliseconds);

            timer.Elapsed += (sender, args) =>
            {
                ServiceMessage serviceMessage = new ServiceMessage(PlantsArea.Id, MeasurableType,
                    PlantsArea.Plant.GetMeasurableParameter(MeasurableType).Optimal, timeSpan);

                func(serviceMessage);

                ResetSensorsFunctions();

                timer.Stop();

                SetSensorsState(true);
            };
            timer.Start();
        }
    }
}