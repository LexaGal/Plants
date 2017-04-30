using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherUtil
{
    public class WeatherModel
    {
        public IEnumerable<string> AreasIds { get; set; }
        public IEnumerable<WeatherItem> WeatherItems { get; set; }
    }
}
