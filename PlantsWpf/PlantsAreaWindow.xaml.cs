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
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class PlantsAreaWindow
    {
        public event EventHandler<PlantsAreaEventArgs> PlantsAreaEvent;
        
        public IEnumerable<Visual> GetChildrenOfVisual(Visual myVisual)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(myVisual); i++)
            {
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(myVisual, i);
                yield return childVisual;
            }
        }
        
        public PlantsAreaWindow()
        {
            InitializeComponent();
            
            foreach (string name in  Enum.GetNames(typeof (PlantNameEnum)))
            {
                PlantNameBox.Items.Add(name);
            }
            PlantNameBox.Text = PlantNameBox.Items[0].ToString();
        }

        private PlantsArea GetPlantsArea()
        {
            try
            {
                PlantNameEnum plantName = (PlantNameEnum) Enum.Parse(typeof (PlantNameEnum),
                    PlantNameBox.SelectionBoxItem.ToString());
                int number = Convert.ToInt32(Number.Text);

                int optimalT = Convert.ToInt32(OptimalTemperature.Text);
                int minT = Convert.ToInt32(MinTemperature.Text);
                int maxT = Convert.ToInt32(MaxTemperature.Text);
                Temperature temperature = new Temperature(Guid.NewGuid(), optimalT, minT, maxT);

                if (!temperature.IsValid())
                {
                    throw new ApplicationException("Fields for Temperature are not valid - numeric values >= 0 and <= 100!");
                }

                int optimalH = Convert.ToInt32(OptimalHumidity.Text);
                int minH = Convert.ToInt32(MinHumidity.Text);
                int maxH = Convert.ToInt32(MaxHumidity.Text);
                Humidity humidity = new Humidity(Guid.NewGuid(), optimalH, minH, maxH);

                if (!humidity.IsValid())
                {
                    throw new ApplicationException("Fields for Humidity are not valid - numeric values >= 0 and <= 100!");
                }

                int optimalS = Convert.ToInt32(OptimalSoilPh.Text);
                int minS = Convert.ToInt32(MinSoilPh.Text);
                int maxS = Convert.ToInt32(MaxSoilPh.Text);
                SoilPh soilPh = new SoilPh(Guid.NewGuid(), optimalS, minS, maxS);

                if (!soilPh.IsValid())
                {
                    throw new ApplicationException("Fields for SoilPh are not valid - numeric values >= 0 and <= 100!");
                }

                int optimalN = Convert.ToInt32(OptimalNutrient.Text);
                int minN = Convert.ToInt32(MinNutrient.Text);
                int maxN = Convert.ToInt32(MaxNutrient.Text);
                Nutrient nutrient = new Nutrient(Guid.NewGuid(), optimalN, minN, maxN);

                if (!nutrient.IsValid())
                {
                    throw new ApplicationException("Fields for Nutrient are not valid - numeric values >= 0 and <= 100!");
                }

                Plant plant = new Plant(Guid.NewGuid(), temperature, humidity, soilPh, nutrient, plantName);

                PlantsArea plantsArea = new PlantsArea(Guid.NewGuid(), plant, number);

                try
                {
                    TimeSpan temperatureTimeout = TimeSpan.Parse(TemperatureTimeout.Text);
                    TimeSpan humidityTimeout = TimeSpan.Parse(HumidityTimeout.Text);
                    TimeSpan soilPhTimeout = TimeSpan.Parse(SoilPhTimeout.Text);
                    TimeSpan nutrientTimeout = TimeSpan.Parse(NutrientTimeout.Text);

                    Sensor ts = new TemperatureSensor(Guid.NewGuid(), plantsArea, temperatureTimeout, temperature, 0);
                    if (TemperatureCheckBox.IsChecked != null && !(bool) TemperatureCheckBox.IsChecked){ts.IsOn = false;}

                    Sensor hs = new HumiditySensor(Guid.NewGuid(), plantsArea, humidityTimeout, humidity, 0);
                    if (HumidityCheckBox.IsChecked != null && !(bool) HumidityCheckBox.IsChecked){hs.IsOn = false;}

                    Sensor ss = new SoilPhSensor(Guid.NewGuid(), plantsArea, soilPhTimeout, soilPh, 0);
                    if (SoilPhCheckBox.IsChecked != null && !(bool) SoilPhCheckBox.IsChecked){ss.IsOn = false;}

                    Sensor ns = new NutrientSensor(Guid.NewGuid(), plantsArea, nutrientTimeout, nutrient, 0);
                    if (NutrientCheckBox.IsChecked != null && !(bool) NutrientCheckBox.IsChecked){ns.IsOn = false;}

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
                    new List<MeasurableParameter> { plantsArea.Plant.Humidity, plantsArea.Plant.Temperature }));
                
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
            PlantsArea plantsArea = GetPlantsArea();
            if (plantsArea != null && PlantsAreaEvent != null)
            {
                PlantsAreaEvent(this, new PlantsAreaEventArgs(plantsArea));
            }
        }

        private void ClearAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (Visual visual in GetChildrenOfVisual(RootGrid))
            {
                if (visual is TextBox)
                {
                    (visual as TextBox).Text = String.Empty;
                }
                if (visual is ComboBox)
                {
                    (visual as ComboBox).Text = (visual as ComboBox).Items[0].ToString();
                }
                if (visual is CheckBox)
                {
                    (visual as CheckBox).IsChecked = false;
                }
            }
        }
    }
}
