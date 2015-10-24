using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using PlantingLib.Messenging;
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

        public void UpdateState(Sensor sensor)
        {
            MeasurableType = sensor.MeasurableType;
            Optimal =
                sensor.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == sensor.MeasurableType)
                    .Optimal.ToString();
            Min =
                sensor.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == sensor.MeasurableType)
                    .Min.ToString();
            Max =
                sensor.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == sensor.MeasurableType)
                    .Max.ToString();
            Value = sensor.Function.CurrentFunctionValue.ToString("F2");
            NumberOfTimes = sensor.NumberOfTimes.ToString();
            IsCritical = sensor.Function.CurrentFunctionValue >
                         sensor.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == sensor.MeasurableType).Max ||
                         sensor.Function.CurrentFunctionValue <
                         sensor.PlantsArea.Plant.MeasurableParameters.First(p => p.MeasurableType == sensor.MeasurableType).Min
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
