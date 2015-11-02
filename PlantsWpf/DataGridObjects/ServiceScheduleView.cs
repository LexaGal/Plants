using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PlantingLib.Plants.ServicesScheduling;
using PlantsWpf.Annotations;

namespace PlantsWpf.DataGridObjects
{
    public class DataGridServiceScheduleView : INotifyPropertyChanged
    {
        public DataGridServiceScheduleView()
        {
        }

        public string ServiceName { get; set; }
        public string Parameters { get; set; }
        private string _servicingSpan;
        private string _servicingPauseSpan;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotNull]
        public string ServicingSpan
        {
            get { return _servicingSpan; }
            set
            {
                _servicingSpan = value;
                OnPropertyChanged("ServicingSpan");
            }
        }

        [NotNull]
        public string ServicingPauseSpan
        {
            get { return _servicingPauseSpan; }
            set
            {
                _servicingPauseSpan = value;
                OnPropertyChanged("ServicingPauseSpan");
            }
        }

        public DataGridServiceScheduleView(ServiceSchedule serviceSchedule)
        {
            ServiceName = serviceSchedule.ServiceState.ToString();

            StringBuilder builder = new StringBuilder();
            serviceSchedule.MeasurableParameters.ToList()
                .ForEach(m => builder.Append(String.Format("{0}, ", m.MeasurableType)));
            builder.Remove(builder.Length - 2, 2);

            Parameters = builder.ToString();

            ServicingSpan = serviceSchedule.ServicingSpan.ToString();
            
            ServicingPauseSpan = serviceSchedule.ServicingPauseSpan.ToString();
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}