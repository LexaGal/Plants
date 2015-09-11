using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planting.PlantRequirements
{
    public class MeasurableParameter
    {
        public MeasurableParameter(int optimal, int min, int max)
        {
            Optimal = optimal;
            Min = min;
            Max = max;
        }

        public int Optimal { get; private set; }
        public int Min { get; private set; }
        public int Max { get; private set; }
        public MeasurableTypesEnum Type { get; set; }

        public int RandomValue 
        {
            get
            {
                Random random = new Random();
                return random.Next(Min - random.Next(1, (Optimal-Min)/2 + 1),
                    Max + random.Next(1, (Max-Optimal)/2 + 1));
            }
        }
    }
}
