using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PlantingLib.Plants.ServicesScheduling;
using PlantsWpf.Annotations;

namespace PlantsWpf.ObjectsViews
{
    public class DataGridServiceScheduleView : INotifyPropertyChanged
    {
        private readonly ServiceSchedule _serviceSchedule;
        private string _servicingSpan;
        private string _servicingPauseSpan;
        private string _serviceName;
        private string _parameters;
        private string _isModified;
        private string _isOn;

        public string IsOn
        {
            get { return _isOn; }
            set
            {
                if (value == _isOn) return;
                _isOn = value;
                _serviceSchedule.IsOn = Convert.ToBoolean(IsOn);
                OnPropertyChanged();
            }
        }

        public string IsModified
        {
            get { return _isModified; }
            set
            {
                if (value == _isModified) return;
                _isModified = value;
                OnPropertyChanged();
            }
        }

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                _serviceName = value;
                OnPropertyChanged();
            }
        }

        public string Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
                OnPropertyChanged();
            }
        }

        public string ServicingSpan
        {
            get { return _servicingSpan; }
            set
            {
                _servicingSpan = value;
                OnPropertyChanged();
                _serviceSchedule.ServicingSpan = TimeSpan.Parse(_servicingSpan);
                IsModified = true.ToString();
            }
        }

        public string ServicingPauseSpan
        {
            get { return _servicingPauseSpan; }
            set
            {
                _servicingPauseSpan = value;
                OnPropertyChanged();
                _serviceSchedule.ServicingPauseSpan = TimeSpan.Parse(_servicingPauseSpan);
                IsModified = true.ToString();
            }
        }

        public DataGridServiceScheduleView()
        {
        }

        public DataGridServiceScheduleView(ServiceSchedule serviceSchedule)
        {
            _serviceSchedule = serviceSchedule;
            _serviceSchedule.NewServiceName += GetNewServiceName;

            ServiceName = _serviceSchedule.ServiceName;

            Parameters = MeasurableParametersToStringView();

            ServicingSpan = _serviceSchedule.ServicingSpan.ToString();
            ServicingPauseSpan = _serviceSchedule.ServicingPauseSpan.ToString();

            IsOn = true.ToString();

            IsModified = false.ToString();
        }

        private void GetNewServiceName(object sender, EventArgs eventArgs)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            ServiceName = _serviceSchedule.ServiceName;
            Parameters = MeasurableParametersToStringView();
        }

        public string MeasurableParametersToStringView()
        {
            StringBuilder builder = new StringBuilder();
            _serviceSchedule.MeasurableParameters.ToList()
                .ForEach(m => builder.Append($"{m.MeasurableType}, "));
            builder.Remove(builder.Length - 2, 2);

            return builder.ToString();   
        }

    public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}