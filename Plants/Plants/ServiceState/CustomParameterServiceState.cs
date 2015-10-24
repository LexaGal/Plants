using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlantsWpf.Annotations;

namespace PlantingLib.Plants.ServiceState
{
    public class CustomParameterServiceState : INotifyPropertyChanged
    {
        private bool _servicing;

        public string Servicing
        {
            get { return _servicing ? "✔" : String.Empty; }
            set
            {
                _servicing = Convert.ToBoolean(value);
                OnPropertyChanged("Servicing");
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