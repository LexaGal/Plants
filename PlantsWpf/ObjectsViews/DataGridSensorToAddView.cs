using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlantingLib.Sensors;
using PlantsWpf.Annotations;

namespace PlantsWpf.ObjectsViews
{
    public class DataGridSensorToAddView : INotifyPropertyChanged
    {
        public string Optimal { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Measurable { get; set; }
        private string _timeout;
        private string _add;

        public DataGridSensorToAddView()
        {
        }

        public DataGridSensorToAddView(Sensor s)
        {
            _timeout = s.MeasuringTimeout.ToString();
            _add = "yes";
            Optimal = s.MeasurableParameter.Optimal.ToString();
            Min = s.MeasurableParameter.Min.ToString();
            Max = s.MeasurableParameter.Max.ToString();
            Measurable = s.MeasurableType;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotNull]
        public string Timeout
        {
            get { return _timeout; }
            set
            {
                _timeout = value;
                OnPropertyChanged();
            }
        }

        [NotNull]
        public string Add
        {
            get { return _add; }
            set
            {
                _add = value; 
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual bool OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return true;
        }
    }
}
