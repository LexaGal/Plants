﻿using System;
using System.Collections.Generic;
using System.Linq;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;

namespace PlantingLib.Plants
{
    public class PlantsArea
    {
        public Guid Id { get; private set; }
        public int Number { get; }

        public Plant Plant { get; }
        
        public List<Sensor> Sensors { get; private set; }
        public PlantServicesStates PlantServicesStates { get; }
        public ServicesSchedulesStates ServicesSchedulesStates { get; private set; }

        public PlantsArea(Guid id, Plant plant, int number)
        {
            Id = id;
            Plant = plant;
            Number = number;

            ServicesSchedulesStates = new ServicesSchedulesStates();

            PlantServicesStates = new PlantServicesStates();
            ParameterServicesInfo.ParametersServices
                .SelectMany(l => l.ServiceStates)
                .Distinct(new ServiceStateEqualityComparer())
                .Where(s => !s.IsCustom)
                .ToList()
                .ForEach(s => PlantServicesStates.AddServiceState(s.Clone() as ServiceState));
            
            Sensors = new List<Sensor>();
        }

        public bool AddSensor(Sensor sensor)
        {
            if (Sensors == null)
            {
                Sensors = new List<Sensor>();
            }
            if (Sensors.Any(s => sensor != null && s.Id == sensor.Id))
            {
                Sensor old = Sensors.First(s => s.Id == sensor.Id);
                old = sensor;
                return true;
            }
            Sensors.Add(sensor);
            sensor.SetPlantsArea(this);
            return true;
        }

        public bool RemoveSensor(Sensor sensor)
        {
            if (Sensors != null)
            {
                if (Sensors.All(s => sensor != null && s.Id != sensor.Id))
                {
                    return false;
                }
                Sensors.Remove(sensor);
                sensor.SetPlantsArea(null);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Plant's name: {Plant.Name}\nPlant's number: {Number}";
        }
    }
}