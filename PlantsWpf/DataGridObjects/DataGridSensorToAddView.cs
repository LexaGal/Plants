using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using PlantingLib.Sensors;
using PlantsWpf.Annotations;

namespace PlantsWpf.DataGridObjects
{
    public class DataGridSensorToAddView : INotifyPropertyChanged
    {
        public string Optimal { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string MeasurableType { get; set; }
        private string _timeout;
        private string _add;

        public DataGridSensorToAddView(Sensor s)
        {
            _timeout = s.MeasuringTimeout.TotalSeconds.ToString(CultureInfo.InvariantCulture);
            _add = "yes";
            Optimal = s.MeasurableParameter.Optimal.ToString();
            Min = s.MeasurableParameter.Min.ToString();
            Max = s.MeasurableParameter.Max.ToString();
            MeasurableType =
                s.MeasurableType.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotNull]
        public string Timeout
        {
            get { return _timeout; }
            set
            {
                _timeout = value;
                OnPropertyChanged("Timeout");
            }
        }

        [NotNull]
        public string Add
        {
            get { return _add; }
            set
            {
                _add = value; 
                OnPropertyChanged("Add");
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
