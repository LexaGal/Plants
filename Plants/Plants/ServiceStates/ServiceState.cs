    using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
    using PlantingLib.Properties;
    //using PlantsWpf.Annotations;
    using static System.String;

namespace PlantingLib.Plants.ServiceStates
{
    public class ServiceState : INotifyPropertyChanged, ICloneable
    {  
        public ServiceState(string serviceName, bool isCustom)
        {
            IsCustom = isCustom;
            ServiceName = serviceName;
            IsRunning = false;
        }

        public bool IsCustom { get; }

        public string ServiceName
        {
            get
            {
                return _serviceName;
            }
            set
            {
                _serviceName = IsCustom ? $"*{value}*" : value;
                OnPropertyChanged();
            }
        }

        public bool IsRunning { get; private set; }

        public string IsOn
        {
            get { return IsRunning ? Properties.Resources.IsScheduledSign : Empty; }
            set
            {
                IsRunning = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }

        private bool _isScheduled;
        private string _serviceName;

        public string IsScheduled
        {
            get { return _isScheduled ? Properties.Resources.IsScheduledSign : Empty; }
            set
            {
                _isScheduled = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }

        public bool IsFor(string measurableType)
        {
            return ServiceName == $"*{measurableType}*";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public object Clone()
        {
            return new ServiceState(ServiceName, IsCustom);
        }
    }
}