//using PlantsWpf.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlantingLib.Properties;
using static System.String;

namespace PlantingLib.Plants.ServiceStates
{
    public class ServiceState : INotifyPropertyChanged, ICloneable
    {
        private bool _isScheduled;
        private string _serviceName;

        public ServiceState(string serviceName, bool isCustom)
        {
            IsCustom = isCustom;
            ServiceName = serviceName;
            IsRunning = false;
        }

        public bool IsCustom { get; }

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                _serviceName = IsCustom ? $"*{value}*" : value;
                OnPropertyChanged();
            }
        }

        public bool IsRunning { get; private set; }

        public string IsOn
        {
            get { return IsRunning ? Resources.IsScheduledSign : Empty; }
            set
            {
                IsRunning = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }

        public string IsScheduled
        {
            get { return _isScheduled ? Resources.IsScheduledSign : Empty; }
            set
            {
                _isScheduled = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }

        public object Clone()
        {
            return new ServiceState(ServiceName, IsCustom);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsFor(string measurableType)
        {
            return ServiceName == $"*{measurableType}*";
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}