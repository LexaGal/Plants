using System;
using System.Timers;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.ServiceSystems
{
    public class TemperatureSystem : ServiceSystem
    {
        public TemperatureSystem(MeasurableTypesEnum measurableType, double parameterValue, PlantsArea plantsArea)
            : base(measurableType, parameterValue, plantsArea)
        {
        }

        public override TimeSpan ComputeTimeForService()
        {
            if (PlantsArea != null)
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, (int) (ParameterValue - PlantsArea.PlantRequirements.Temperature.Optimal));
                return timeSpan;
            }
            return TimeSpan.Zero;
        }

        public override void StartService(TimeSpan timeSpan, Func<TimeSpan, TimeSpan> func)
        {
            Timer timer = new Timer(timeSpan.TotalMilliseconds / 2);
            timer.Elapsed += (sender, args) =>
            {
                func(timeSpan);
                timer.Stop();
            };
            timer.Start();
        }
    };

}
