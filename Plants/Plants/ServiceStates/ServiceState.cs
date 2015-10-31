﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlantsWpf.Annotations;

namespace PlantingLib.Plants.ServiceStates
{
    public class ServiceState : INotifyPropertyChanged, ICloneable
    {
        private bool _isOn;
        public bool IsCustom { get; private set; }

        public ServiceState(string serviceName, bool isCustom)
        {
            IsCustom = isCustom;
            ServiceName = IsCustom ? String.Format("*{0}*", serviceName) : serviceName;
            _isOn = false;
        }

        public string ServiceName { get; private set; }

        public string IsOn
        {
            get { return _isOn ? "✔" : String.Empty; }
            set
            {
                _isOn = Convert.ToBoolean(value);
                OnPropertyChanged("IsOn");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public object Clone()
        {
            return new ServiceState(ServiceName, IsCustom);
        }
    }
}