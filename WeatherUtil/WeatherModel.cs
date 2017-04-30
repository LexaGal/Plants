using System.Collections.Generic;

namespace WeatherUtil
{
    public class WeatherModel
    {
        public IEnumerable<string> AreasIds { get; set; }
        public IEnumerable<WeatherItem> WeatherItems { get; set; }
    }
}
