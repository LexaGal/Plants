using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlantsWpf.Annotations;

namespace PlantingLib.Plants.ServiceStates
{
    public class ServiceState : INotifyPropertyChanged
    {
        private bool _isOn;

        public ServiceState(string serviceName, bool isCustom)
        {
            ServiceName = isCustom ? String.Format("*{0}*", serviceName) : serviceName;
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
    }
}