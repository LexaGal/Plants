using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
        private string _measurable;
        private string _isOffByUser;

        public string IsOffByUser
        {
            get { return _isOffByUser; }
            set
            {
                _isOffByUser = value;
                OnPropertyChanged();
                _sensor.IsOffByUser = Convert.ToBoolean(IsOffByUser);
            }
        }

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
                if (!_sensor.MeasurableParameter.IsValid())
                {
                    throw new ArgumentException();
                }
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
               if (!_sensor.MeasurableParameter.IsValid())
               {
                   throw new ArgumentException();
               } 
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
                if (!_sensor.MeasurableParameter.IsValid())
                {
                    throw new ArgumentException();
                }
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

        public string Measurable
        {
            get { return _measurable; }
            set
            {
                if (value == _measurable) return;
                _measurable = value;
                OnPropertyChanged();

                // do not: _sensor.MeasurableParameter.MeasurableType = Measurable;
                // oldMeasurable will overwrite
                if (Regex.Match(Measurable, @"[^a-zA-Z0-9_@]+").Success)
                {
                    throw new FormatException();
                }
                IsModified = true.ToString();
                MeasurableIsModified = true;
            }
        }

        public bool MeasurableIsModified { get; set; }

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

            IsOffByUser = _sensor.IsOffByUser.ToString();

            MeasurableIsModified = false;

            UpdateView(); 
        }

        public void UpdateView()
        {
            Value = _sensor.Function.CurrentFunctionValue.ToString("F2");
            N = _sensor.NumberOfTimes.ToString();
            IsCritical = _sensor.Function.CurrentFunctionValue >
                         _sensor.MeasurableParameter.Max ||
                         _sensor.Function.CurrentFunctionValue <
                         _sensor.MeasurableParameter.Min
                ? true.ToString()
                : false.ToString();
        }

        private void GetNewMeasuring(object sender, EventArgs eventArgs)
        {
            UpdateView();
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
