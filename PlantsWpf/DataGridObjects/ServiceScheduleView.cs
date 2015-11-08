﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PlantingLib.Plants.ServicesScheduling;
using PlantsWpf.Annotations;

namespace PlantsWpf.DataGridObjects
{
    public class DataGridServiceScheduleView : INotifyPropertyChanged
    {
        private readonly ServiceSchedule _serviceSchedule;
        private string _servicingSpan;
        private string _servicingPauseSpan;
        private string _serviceName;
        private string _parameters;
        private string _isModified;
        private string _isOn;

        public string IsOn
        {
            get { return _isOn; }
            set
            {
                if (value == _isOn) return;
                _isOn = value;
                try
                {
                    _serviceSchedule.IsOn = Convert.ToBoolean(IsOn);
                }
                catch (Exception )
                {
                    return;
                }
                OnPropertyChanged();
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

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                _serviceName = value;
                OnPropertyChanged();
            }
        }

        public string Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
                OnPropertyChanged();
            }
        }
        
        public string ServicingSpan
        {
            get { return _servicingSpan; }
            set
            {
                _servicingSpan = value;
                OnPropertyChanged();
                try
                {
                    _serviceSchedule.ServicingSpan = TimeSpan.Parse(_servicingSpan);
                }
                catch (Exception)
                {
                    return;
                }
                IsModified = true.ToString();
            }
        }

        public string ServicingPauseSpan
        {
            get { return _servicingPauseSpan; }
            set
            {
                _servicingPauseSpan = value;
                OnPropertyChanged();
                try
                {
                    _serviceSchedule.ServicingPauseSpan = TimeSpan.Parse(_servicingPauseSpan);
                }
                catch (Exception)
                {
                    return;
                }
                IsModified = true.ToString();
            }
        }

        public DataGridServiceScheduleView()
        {
        }

        public DataGridServiceScheduleView(ServiceSchedule serviceSchedule)
        {
            _serviceSchedule = serviceSchedule;
            
            ServiceName = _serviceSchedule.ServiceName;

            StringBuilder builder = new StringBuilder();
            _serviceSchedule.MeasurableParameters.ToList()
                .ForEach(m => builder.Append(String.Format("{0}, ", m.MeasurableType)));
            builder.Remove(builder.Length - 2, 2);

            Parameters = builder.ToString();

            ServicingSpan = _serviceSchedule.ServicingSpan.ToString();
            ServicingPauseSpan = _serviceSchedule.ServicingPauseSpan.ToString();

            IsOn = true.ToString();

            IsModified = false.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}