using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using PlantingLib.Messenging;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using PlantsWpf.Annotations;

namespace PlantsWpf.DataGridObjects
{
    public class DataGridSensorView : INotifyPropertyChanged
    {
        public string Optimal { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Value { get; set; }
        public string NumberOfTimes { get; set; }
        public string IsCritical { get; set; }
        public string MeasurableType { get; set; }

        public Sensor Sensor { get; private set; }

        public DataGridSensorView(Sensor sensor)
        {
            Sensor = sensor;
            Sensor.NewMeasuring += GetNewMeasuring;
            UpdateState(Sensor);
        }

        public void UpdateState(Sensor s)
        {
            MeasurableType = s.MeasurableType.ToString();
            Optimal =
                s.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == s.MeasurableType)
                    .Optimal.ToString();
            Min =
                s.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == s.MeasurableType)
                    .Min.ToString();
            Max =
                s.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == s.MeasurableType)
                    .Max.ToString();
            Value = s.Function.CurrentFunctionValue.ToString("F2");
            NumberOfTimes = s.NumberOfTimes.ToString();
            IsCritical = s.Function.CurrentFunctionValue >
                         s.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == s.MeasurableType).Max ||
                         s.Function.CurrentFunctionValue <
                         s.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == s.MeasurableType).Min
                ? "(!)"
                : String.Empty;
            OnPropertyChanged("Value");
            OnPropertyChanged("NumberOfTimes");
            OnPropertyChanged("IsCritical");
        }

        private void GetNewMeasuring(object sender, EventArgs eventArgs)
        {
            MessengingEventArgs<Sensor> messengingEventArgs = eventArgs as MessengingEventArgs<Sensor>;
            if (messengingEventArgs != null)
            {
                UpdateState(messengingEventArgs.Object);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                 PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
