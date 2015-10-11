using System;
using System.Linq;
using System.Timers;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Timers;

namespace PlantingLib.ServiceSystems
{
    public abstract class ServiceSystem
    {
        public PlantsArea PlantsArea { get; private set; }
        public MeasurableTypeEnum MeasurableType { get; private set; }
        public double ParameterValue { get; private set; }
        private Timer _timer;
        private Func<ServiceMessage, ServiceMessage> _func;
        private int _nIntervals;
        
        protected ServiceSystem(MeasurableTypeEnum measurableType, double parameterValue, PlantsArea plantsArea)
        {
            PlantsArea = plantsArea;
            MeasurableType = measurableType;
            ParameterValue = parameterValue;
        }

        public void SetSensorsState(bool state)
        {
            switch (MeasurableType)
            {
                case MeasurableTypeEnum.Humidity:
                    PlantsArea.PlantsAreaServiceState.Watering = (!state).ToString();
                    break;
                case MeasurableTypeEnum.Temperature:
                    if (ParameterValue < PlantsArea.Plant.Temperature.Optimal)
                    {
                        PlantsArea.PlantsAreaServiceState.Warming = (!state).ToString();
                        break;
                    }
                    PlantsArea.PlantsAreaServiceState.Cooling = (!state).ToString();
                    break;
                case MeasurableTypeEnum.SoilPh:
                    PlantsArea.PlantsAreaServiceState.Nutrienting = (!state).ToString();
                    break;
                case MeasurableTypeEnum.Nutrient:
                    PlantsArea.PlantsAreaServiceState.Nutrienting = (!state).ToString();
                    break;
            }
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

        public void ServiceTimer_Tick(object sender, ElapsedEventArgs e)
        {
            if (SystemTimer.IsEnabled)
            {
                if (_nIntervals >= (int) ComputeTimeForService().TotalSeconds)
                {
                    ServiceMessage serviceMessage = new ServiceMessage(PlantsArea.Id, MeasurableType,
                        PlantsArea.Plant.GetMeasurableParameter(MeasurableType).Optimal, ComputeTimeForService());

                    _func(serviceMessage);

                    ResetSensorsFunctions();

                    SetSensorsState(true);
                    
                    _timer.Enabled = false;
                    _timer.AutoReset = false;

                    _nIntervals = 0;
                    return;
                }

                _nIntervals++;
                _timer.AutoReset = true;
                _timer.Enabled = true;
            }
            else
            {
                _timer.AutoReset = true;
                _timer.Enabled = true;
            }
        }

        public void StartService(Func<ServiceMessage, ServiceMessage> func)
        {
            SetSensorsState(false);

            _nIntervals = 0;
            _func = func;
            
            _timer = new Timer(1000);
            _timer.Elapsed += ServiceTimer_Tick;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
    }
}