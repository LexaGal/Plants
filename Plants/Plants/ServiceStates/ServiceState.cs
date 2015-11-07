    using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlantsWpf.Annotations;

namespace PlantingLib.Plants.ServiceStates
{
    public class ServiceState : INotifyPropertyChanged, ICloneable
    {  
        public ServiceState(string serviceName, bool isCustom)
        {
            IsCustom = isCustom;
            ServiceName = IsCustom ? String.Format("*{0}*", serviceName) : serviceName;
            IsRunning = false;
        }

        public bool IsCustom { get; private set; }
        public string ServiceName { get; private set; }
        public bool IsRunning { get; private set; }

        public string IsOn
        {
            get { return IsRunning ? "✔" : String.Empty; }
            set
            {
                IsRunning = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }

        private bool _isScheduled;
        
        public string IsScheduled
        {
            get { return _isScheduled ? "✔" : String.Empty; }
            set
            {
                _isScheduled = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }

        public bool IsFor(string measurableType)
        {
            return ServiceName == String.Format("*{0}*", measurableType);
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

        public object Clone()
        {
            return new ServiceState(ServiceName, IsCustom);
        }
    }
}