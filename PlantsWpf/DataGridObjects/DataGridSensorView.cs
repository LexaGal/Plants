using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using PlantsWpf.Annotations;

namespace PlantsWpf.DataGridObjects
{
    public class DataGridSensorView : INotifyPropertyChanged
    {
        public string Optimal { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string MeasurableType { get; set; }
        public string Value { get; set; }
        
        private string _numberOfTimes;
        private string _isCritical;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotNull]
        public string NumberOfTimes
        {
            get { return _numberOfTimes; }
            set
            {
                _numberOfTimes = value;
                OnPropertyChanged("NumberOfTimes");
            }
        }
        
        [NotNull]
        public string IsCritical
        {
            get { return _isCritical; }
            set
            {
                _isCritical = value;
                OnPropertyChanged("IsCritical");
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual bool OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return true;
        }
    }
}
