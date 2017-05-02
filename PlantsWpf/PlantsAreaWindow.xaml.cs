using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;
using PlantsWpf.ArgsForEvents;

namespace PlantsWpf
{
    /// <summary>
    ///     Interaction logic for Window.xaml
    /// </summary>
    public partial class PlantsAreaWindow
    {
        public PlantsAreaWindow()
        {
            InitializeComponent();

            foreach (var name in  Enum.GetNames(typeof(PlantNameEnum)))
                PlantNameBox.Items.Add(name);
            PlantNameBox.Text = PlantNameBox.Items[0].ToString();
        }

        public event EventHandler<PlantsAreaEventArgs> PlantsAreaEvent;

        public IEnumerable<Visual> GetChildrenOfVisual(Visual myVisual)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(myVisual); i++)
            {
                var childVisual = (Visual) VisualTreeHelper.GetChild(myVisual, i);
                yield return childVisual;
            }
        }

        private PlantsArea GetPlantsArea()
        {
            try
            {
                var plantName = (PlantNameEnum) Enum.Parse(typeof(PlantNameEnum),
                    PlantNameBox.SelectionBoxItem.ToString());
                var number = Convert.ToInt32(Number.Text);

                var optimalT = Convert.ToInt32(OptimalTemperature.Text);
                var minT = Convert.ToInt32(MinTemperature.Text);
                var maxT = Convert.ToInt32(MaxTemperature.Text);
                var temperature = new Temperature(Guid.NewGuid(), optimalT, minT, maxT);

                if (!temperature.HasValidParameters())
                    throw new ApplicationException(
                        "Fields for Temperature are not valid - numeric values >= 0 and <= 100!");

                var optimalH = Convert.ToInt32(OptimalHumidity.Text);
                var minH = Convert.ToInt32(MinHumidity.Text);
                var maxH = Convert.ToInt32(MaxHumidity.Text);
                var humidity = new Humidity(Guid.NewGuid(), optimalH, minH, maxH);

                if (!humidity.HasValidParameters())
                    throw new ApplicationException("Fields for Humidity are not valid - numeric values >= 0 and <= 100!");

                var optimalS = Convert.ToInt32(OptimalSoilPh.Text);
                var minS = Convert.ToInt32(MinSoilPh.Text);
                var maxS = Convert.ToInt32(MaxSoilPh.Text);
                var soilPh = new SoilPh(Guid.NewGuid(), optimalS, minS, maxS);

                if (!soilPh.HasValidParameters())
                    throw new ApplicationException("Fields for SoilPh are not valid - numeric values >= 0 and <= 100!");

                var optimalN = Convert.ToInt32(OptimalNutrient.Text);
                var minN = Convert.ToInt32(MinNutrient.Text);
                var maxN = Convert.ToInt32(MaxNutrient.Text);
                var nutrient = new Nutrient(Guid.NewGuid(), optimalN, minN, maxN);

                if (!nutrient.HasValidParameters())
                    throw new ApplicationException("Fields for Nutrient are not valid - numeric values >= 0 and <= 100!");

                var plant = new Plant(Guid.NewGuid(), temperature, humidity, soilPh, nutrient, plantName);

                var plantsArea = new PlantsArea(Guid.NewGuid(), default(Guid), plant, number);

                try
                {
                    var temperatureTimeout = TimeSpan.Parse(TemperatureTimeout.Text);
                    var humidityTimeout = TimeSpan.Parse(HumidityTimeout.Text);
                    var soilPhTimeout = TimeSpan.Parse(SoilPhTimeout.Text);
                    var nutrientTimeout = TimeSpan.Parse(NutrientTimeout.Text);

                    Sensor ts = new TemperatureSensor(Guid.NewGuid(), plantsArea, temperatureTimeout, temperature);
                    if ((TemperatureCheckBox.IsChecked != null) && !(bool) TemperatureCheckBox.IsChecked)
                        ts.IsOffByUser = true;

                    Sensor hs = new HumiditySensor(Guid.NewGuid(), plantsArea, humidityTimeout, humidity);
                    if ((HumidityCheckBox.IsChecked != null) && !(bool) HumidityCheckBox.IsChecked)
                        hs.IsOffByUser = true;

                    Sensor ss = new SoilPhSensor(Guid.NewGuid(), plantsArea, soilPhTimeout, soilPh);
                    if ((SoilPhCheckBox.IsChecked != null) && !(bool) SoilPhCheckBox.IsChecked) ss.IsOffByUser = true;

                    Sensor ns = new NutrientSensor(Guid.NewGuid(), plantsArea, nutrientTimeout, nutrient);
                    if ((NutrientCheckBox.IsChecked != null) && !(bool) NutrientCheckBox.IsChecked)
                        ns.IsOffByUser = true;
                }
                catch (Exception)
                {
                    MessageBox.Show(@"Please, fill in all timeouts with TimeSpan values >= 0!");
                    return null;
                }

                plantsArea.ServicesSchedulesStates.AddServiceSchedule(new ServiceSchedule(Guid.NewGuid(), plantsArea.Id,
                    ServiceStateEnum.Nutrienting.ToString(), new TimeSpan(0, 0, 10), new TimeSpan(0, 1, 0),
                    new List<MeasurableParameter> {plantsArea.Plant.Nutrient, plantsArea.Plant.SoilPh}));

                plantsArea.ServicesSchedulesStates.AddServiceSchedule(new ServiceSchedule(Guid.NewGuid(), plantsArea.Id,
                    ServiceStateEnum.Watering.ToString(), new TimeSpan(0, 0, 10), new TimeSpan(0, 1, 0),
                    new List<MeasurableParameter> {plantsArea.Plant.Humidity, plantsArea.Plant.Temperature}));

                return plantsArea;
            }
            catch (ApplicationException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var plantsArea = GetPlantsArea();
            if (plantsArea != null)
                PlantsAreaEvent?.Invoke(this, new PlantsAreaEventArgs(plantsArea));
        }

        private void ClearAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var visual in GetChildrenOfVisual(RootGrid))
            {
                if (visual is TextBox)
                    (visual as TextBox).Text = string.Empty;
                if (visual is ComboBox)
                    (visual as ComboBox).Text = (visual as ComboBox).Items[0].ToString();
                if (visual is CheckBox)
                    (visual as CheckBox).IsChecked = false;
            }
        }
    }
}