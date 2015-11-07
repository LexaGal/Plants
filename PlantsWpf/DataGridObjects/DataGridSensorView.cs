using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlantingLib.Messenging;
using PlantingLib.Sensors;
using PlantsWpf.Annotations;

namespace PlantsWpf.DataGridObjects
{
    public class DataGridSensorView : INotifyPropertyChanged
    {
        private Sensor _sensor;
        private string _n;
        private string _value;
        private string _isCritical;
        private string _optimal;
        private string _min;
        private string _max;
        private string _timeout;
        private string _isModified;

        public string Timeout
        {
            get { return _timeout; }
            set
            {
                _timeout = value;
                OnPropertyChanged();
                _sensor.MeasuringTimeout = TimeSpan.Parse(Timeout);
                IsModified = true.ToString();
            }
        }

        public string Optimal
        {
            get { return _optimal; }
            set
            {
                _optimal = value;
                OnPropertyChanged();
                _sensor.MeasurableParameter.Optimal = Convert.ToInt32(Optimal);
                IsModified = true.ToString();
            }
        }
            
        public string Min
        {
            get { return _min; }
            set
            {
                _min = value;
                OnPropertyChanged();
                _sensor.MeasurableParameter.Min = Convert.ToInt32(Min);
                IsModified = true.ToString();
            }
        }

        public string Max
        {
            get { return _max; }
            set
            {
                _max = value;
                OnPropertyChanged();
                _sensor.MeasurableParameter.Max = Convert.ToInt32(Max);
                IsModified = true.ToString();
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public string N
        {
            get { return _n; }
            set
            {
                _n = value;
                OnPropertyChanged();
            }
        }

        public string IsCritical
        {
            get { return _isCritical; }
            set
            {
                _isCritical = value;
                OnPropertyChanged();
            }
        }
        
        public Sensor Sensor
        {
            get { return _sensor; }
            set
            {
                _sensor = value;
                _sensor.NewMeasuring += GetNewMeasuring;
            }
        }

        public string IsModified
        {
            get { return _isModified; }
            set
            {
                if (value == _isModified) return;
                _isModified = value;
                OnPropertyChanged();
            }
        }

        public string Measurable { get; set; }
        public string IsCustom { get; set; }
        
        public DataGridSensorView()
        {
        }

        public DataGridSensorView(Sensor sensor)
        {
            _sensor = sensor;
            _sensor.NewMeasuring += GetNewMeasuring;

            Measurable = _sensor.MeasurableType;
            Optimal = _sensor.MeasurableParameter.Optimal.ToString();
            Min = _sensor.MeasurableParameter.Min.ToString();
            Max = _sensor.MeasurableParameter.Max.ToString();
            Value = _sensor.Function.CurrentFunctionValue.ToString("F2");
            Timeout = _sensor.MeasuringTimeout.ToString();
            IsCustom = _sensor.IsCustom.ToString();
            N = _sensor.NumberOfTimes.ToString();
           
            IsModified = false.ToString();

            UpdateView(); 
        }

        public void UpdateSource()
        {
            if (_sensor != null)
            {   
                _sensor.MeasurableType = Measurable;
                _sensor.MeasurableParameter.Optimal = Convert.ToInt32(Optimal);
                _sensor.MeasurableParameter.Min = Convert.ToInt32(Min);
                _sensor.MeasurableParameter.Max = Convert.ToInt32(Max);
                _sensor.MeasuringTimeout = TimeSpan.Parse(Timeout);
                
                IsModified = false.ToString();
            }
        }

        public void UpdateView()
        {
            Value = _sensor.Function.CurrentFunctionValue.ToString("F2");
            N = _sensor.NumberOfTimes.ToString();
            IsCritical = _sensor.Function.CurrentFunctionValue >
                         _sensor.MeasurableParameter.Max ||
                         _sensor.Function.CurrentFunctionValue <
                         _sensor.MeasurableParameter.Min
                ? "SOS"
                : String.Empty;
        }

        private void GetNewMeasuring(object sender, EventArgs eventArgs)
        {
            MessengingEventArgs<Sensor> messengingEventArgs = eventArgs as MessengingEventArgs<Sensor>;
            if (messengingEventArgs != null)
            {
                UpdateView();
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
