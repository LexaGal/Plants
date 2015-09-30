using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
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
                TimeSpan growingTime = new TimeSpan(0, 0, Convert.ToInt32(GrowingTime.Text));

                int optimalT = Convert.ToInt32(OptimalTemperature.Text);
                int minT = Convert.ToInt32(MinTemperature.Text);
                int maxT = Convert.ToInt32(MaxTemperature.Text);
                Temperature temperature = new Temperature(optimalT, minT, maxT);

                int optimalH = Convert.ToInt32(OptimalHumidity.Text);
                int minH = Convert.ToInt32(MinHumidity.Text);
                int maxH = Convert.ToInt32(MaxHumidity.Text);
                Humidity humidity = new Humidity(optimalH, minH, maxH);

                int optimalS = Convert.ToInt32(OptimalSoilPh.Text);
                int minS = Convert.ToInt32(MinSoilPh.Text);
                int maxS = Convert.ToInt32(MaxSoilPh.Text);
                SoilPh soilPh = new SoilPh(optimalS, minS, maxS);

                int optimalN = Convert.ToInt32(OptimalNutrient.Text);
                int minN = Convert.ToInt32(MinNutrient.Text);
                int maxN = Convert.ToInt32(MaxNutrient.Text);
                Nutrient nutrient = new Nutrient(optimalN, minN, maxN);

                Plant plant = new Plant(temperature, humidity, soilPh, nutrient,
                    growingTime, TimeSpan.Zero, TimeSpan.Zero, plantName);

                PlantsArea plantsArea = new PlantsArea(plant, number);
                
                if (TemperatureCheckBox.IsChecked != null && (bool) TemperatureCheckBox.IsChecked)
                {
                    plantsArea.AddSensor(new TemperatureSensor(plantsArea, new TimeSpan(0, 0, 4), temperature));
                }
                if (HumidityCheckBox.IsChecked != null && (bool)HumidityCheckBox.IsChecked)
                {
                    plantsArea.AddSensor(new HumiditySensor(plantsArea, new TimeSpan(0, 0, 3), humidity));
                }
                if (SoilPhCheckBox.IsChecked != null && (bool)SoilPhCheckBox.IsChecked)
                {
                    plantsArea.AddSensor(new SoilPhSensor(plantsArea, new TimeSpan(0, 0, 2), soilPh));
                }
                if (NutrientCheckBox.IsChecked != null && (bool)NutrientCheckBox.IsChecked)
                {
                    plantsArea.AddSensor(new NutrientSensor(plantsArea, new TimeSpan(0, 0, 5), nutrient));
                }
                return plantsArea;
            }
            catch (FormatException)
            {
                MessageBox.Show("Please, fill in all fields with numeric values!");
                return null;
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            PlantsArea plantsArea = GetPlantsArea();
            if (PlantsAreaEvent != null)
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
                    PlantNameBox.Text = PlantNameBox.Items[0].ToString();
                }
            }
        }
    }
}
