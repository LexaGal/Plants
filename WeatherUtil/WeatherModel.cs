using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherUtil
{
    public class WeatherModel
    {
        public string UserId { get; set; }
        public IEnumerable<string> AreasIds { get; set; }
        public IEnumerable<WeatherItem> WeatherItems { get; set; }

        public static string WeatherItemsToString(IEnumerable<WeatherItem> wi)
        {
            var builder = new StringBuilder().AppendLine("Weather Info");
            var list = wi.ToList();
            if (list.Any())
            {
                list.Take(7).ToList().ForEach(i => { builder.AppendLine($"{i.Name}: {i.Value} {i.Ext}"); });
            }
            else
            {
                builder.AppendLine("Default");
            }
            return builder.ToString();
        }
    }
}