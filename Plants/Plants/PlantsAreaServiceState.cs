using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlantsWpf.Annotations;

namespace PlantingLib.Plants
{
    public class PlantsAreaServiceState : INotifyPropertyChanged
    {
        private bool _nutrienting;
        private bool _warming;
        private bool _cooling;
        private bool _watering;

        public string Watering
        {
            get { return _watering ? "✔" : String.Empty; }
            set
            {
                _watering = Convert.ToBoolean(value);
                OnPropertyChanged("Watering");
            }
        }

        public string Nutrienting
        {
            get { return _nutrienting ? "✔" : String.Empty; }
            set
            {
                _nutrienting = Convert.ToBoolean(value);
                OnPropertyChanged("Nutrienting");
            }
        }

        public string Warming
        {
            get { return _warming ? "✔" : String.Empty; }
            set
            {
                _warming = Convert.ToBoolean(value);
                OnPropertyChanged("Warming");
            }
        }

        public string Cooling
        {
            get { return _cooling ? "✔" : String.Empty; }
            set
            {
                _cooling = Convert.ToBoolean(value);
                OnPropertyChanged("Cooling");
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