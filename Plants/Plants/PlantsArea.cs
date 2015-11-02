using System;
using System.Collections.Generic;
using System.Linq;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;

namespace PlantingLib.Plants
{
    public class PlantsArea
    {
        public Guid Id { get; private set; }

        public Plant Plant { get; private set; }
        public IList<Sensor> Sensors { get; private set; }
        public int Number { get; private set; }
        public PlantsAreaServiceState PlantsAreaServiceState { get; set; }
        public ServicesSchedulesState ServicesSchedulesState { get; set; }

        public PlantsArea(Plant plant, int number)
        {
            Id = Guid.NewGuid();
            Plant = plant;
            Number = number;
            PlantsAreaServiceState = new PlantsAreaServiceState();
            MeasurableParametersInfo.ParametersInfo
                .SelectMany(l => l.ServiceStates)
                .Distinct(new ServiceStateEqualityComparer())
                .Where(s => !s.IsCustom)
                .ToList()
                .ForEach(s => PlantsAreaServiceState.AddServiceState(s.Clone() as ServiceState));
            Sensors = new List<Sensor>();
        }

        public PlantsArea(Guid id, Plant plant, int number)
        {
            Id = id;
            Plant = plant;
            Number = number;
            PlantsAreaServiceState = new PlantsAreaServiceState();
            MeasurableParametersInfo.ParametersInfo
                .SelectMany(l => l.ServiceStates)
                .Distinct(new ServiceStateEqualityComparer())
                .Where(s => !s.IsCustom)
                .ToList()
                .ForEach(s => PlantsAreaServiceState.AddServiceState(s.Clone() as ServiceState));
            Sensors = new List<Sensor>();
        }

        public bool AddSensor(Sensor sensor)
        {
            if (Sensors == null)
            {
                Sensors = new List<Sensor>();
            }
            if (Sensors.Any(s => s.Id == sensor.Id))
            {
                return false;
            }
            Sensors.Add(sensor);
            sensor.SetPlantsArea(this);
            return true;
        }

        public bool RemoveSensor(Sensor sensor)
        {
            if (Sensors.All(s => s.Id != sensor.Id))
            {
                return false;
            }
            Sensors.Remove(sensor);
            sensor.SetPlantsArea(null);
            return true;
        }

        public List<Sensor> FindMainSensorsToAdd()
        {
            List<Sensor> sensors = new List<Sensor>();
            if (Sensors.All(sensor => sensor.MeasurableType != ParameterEnum.Temperature.ToString()))
            {
                sensors.Add(new TemperatureSensor(null, new TimeSpan(0, 0, 1), Plant.Temperature, 0));
            }
            if (Sensors.All(sensor => sensor.MeasurableType != ParameterEnum.Humidity.ToString()))
            {
                sensors.Add(new HumiditySensor(null, new TimeSpan(0, 0, 1), Plant.Humidity, 0));
            }
            if (Sensors.All(sensor => sensor.MeasurableType != ParameterEnum.SoilPh.ToString()))
            {
                sensors.Add(new SoilPhSensor(null, new TimeSpan(0, 0, 1), Plant.SoilPh, 0));
            }
            if (Sensors.All(sensor => sensor.MeasurableType != ParameterEnum.Nutrient.ToString()))
            {
                sensors.Add(new NutrientSensor(null, new TimeSpan(0, 0, 1), Plant.Nutrient, 0));
            }
            return sensors;
        }

        public override string ToString()
        {
            return String.Format("Plant's name: {0}\nPlant's id: {1}\nPlant's number: {2}", Plant.Name, Plant.Id, Number);
        }
    }
}
