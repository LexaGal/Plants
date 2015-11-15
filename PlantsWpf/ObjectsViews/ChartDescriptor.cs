using System;

namespace PlantsWpf.ObjectsViews
{
    public class ChartDescriptor
    {
        public Guid PlantsAreaId { get; private set; }
        public string MeasurableType { get; set; }
        public int Number { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public bool OnlyCritical { get; set; }

        public ChartDescriptor(Guid plantsAreaId, string measurableType, int number, DateTime dateTimeFrom, DateTime dateTimeTo, bool onlyCritical)
        {
            PlantsAreaId = plantsAreaId;
            MeasurableType = measurableType;
            Number = number;
            DateTimeFrom = dateTimeFrom;
            DateTimeTo = dateTimeTo;
            OnlyCritical = onlyCritical;
        }
    }
}