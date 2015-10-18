using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using PlantsWpf.DataGridObjects;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PlantsWpf.ControlsBuilders
{
    public class ControlsBuilder
    {
        public FrameworkElementFactory CreateButtonTemplate(PlantsArea area,
            BindingList<DataGridSensorView> dataGridSensorViews, Action<PlantsArea, Sensor> removeSensor,
            ObservableCollection<DataGridSensorToAddView> dataGridSensorToAddViews, Button sensorsToAddButton)
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof(Button));
            buttonTemplate.SetValue(ContentControl.ContentProperty, "X");
            buttonTemplate.AddHandler(
                ButtonBase.ClickEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridSensorView view = ((FrameworkElement) o).DataContext as DataGridSensorView;
                    if (view != null)
                    {
                        removeSensor(area, view.Sensor);
                        dataGridSensorViews.Remove(view);
                        
                        dataGridSensorToAddViews = new ObservableCollection
                            <DataGridSensorToAddView>(
                            area.FindSensorsToAdd().ConvertAll(s => new DataGridSensorToAddView(s)));

                        sensorsToAddButton.Visibility = dataGridSensorViews.Count != 4 ? Visibility.Visible : Visibility.Hidden;
                    }
                })
                );
            return buttonTemplate;
        }

        public StackPanel CreateButtonsPanel(PlantsArea area, StackPanel plantAreaPanel, 
            DataGrid sensorsToAddDataGrid, ObservableCollection<DataGridSensorToAddView> dataGridSensorToAddViews,
            Action<PlantsArea, Sensor> saveSensor, BindingList<DataGridSensorView> dataGridSensorViews) 
        {
            Button sensorsToAddButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 10, 0, 0),
                Content = "Sensors",
                Width = 70,
                Height = 30,
                Visibility = dataGridSensorViews.Count != 4 ? Visibility.Visible : Visibility.Hidden
            };
            Button addSensorButton = new Button
            {
                Width = 40,
                Height = 30,
                Content = "Add",
                Margin = new Thickness(80, -30, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Visibility = Visibility.Hidden
            };
            Button closeSensorsToAddButton = new Button
            {
                Width = 40,
                Height = 30,
                Content = "Close",
                Margin = new Thickness(120, -30, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Visibility = Visibility.Hidden
            };

            DataGridsBuilder dataGridsBuilder = new DataGridsBuilder();

            sensorsToAddButton.Click += (sender, args) =>
            {
                dataGridSensorToAddViews = new ObservableCollection
                    <DataGridSensorToAddView>(
                    area.FindSensorsToAdd().ConvertAll(s => new DataGridSensorToAddView(s)));
                sensorsToAddDataGrid = dataGridsBuilder.CreateSensorsToAddDataGrid(area, dataGridSensorToAddViews);
                plantAreaPanel.Children.Add(sensorsToAddDataGrid);
                sensorsToAddButton.IsEnabled = false;
                addSensorButton.Visibility = Visibility.Visible;
                closeSensorsToAddButton.Visibility = Visibility.Visible;
            };

            addSensorButton.Click += (o, e) =>
            {
                try
                {
                    foreach (DataGridSensorToAddView dataGridSensor in
                            dataGridSensorToAddViews.Where(d => d.Add == "yes").ToList())
                    {
                        Sensor sensor = area.FindSensorsToAdd().SingleOrDefault(s =>
                            s.MeasurableType.ToString() == dataGridSensor.MeasurableType);

                        if (sensor != null)
                        {
                            int i = Convert.ToInt32(dataGridSensor.Timeout);

                            if (i <= 0)
                            {
                                throw new FormatException();
                            }

                            sensor.MeasuringTimeout = new TimeSpan(0, 0,
                                Convert.ToInt32(dataGridSensor.Timeout));
                            saveSensor(area, sensor);
                        }
                        dataGridSensorToAddViews.Remove(dataGridSensor);
                        if (dataGridSensorToAddViews.Count == 0)
                        {
                            sensorsToAddButton.Visibility = Visibility.Hidden;
                            sensorsToAddButton.IsEnabled = true;
                            addSensorButton.Visibility = Visibility.Hidden;
                            closeSensorsToAddButton.Visibility = Visibility.Hidden;
                            plantAreaPanel.Children.Remove(sensorsToAddDataGrid);
                        }

                        dataGridSensorViews.Add(new DataGridSensorView(sensor));
                    }

                }
                catch (FormatException)
                {
                    MessageBox.Show(@"Please, fill in 'Timeout' field with numeric value > 0!");
                }
            };

            closeSensorsToAddButton.Click += (sender, args) =>
            {
                closeSensorsToAddButton.Visibility = Visibility.Hidden;
                addSensorButton.Visibility = Visibility.Hidden;
                sensorsToAddButton.IsEnabled = true;
                plantAreaPanel.Children.Remove(sensorsToAddDataGrid);
            };

            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(sensorsToAddButton);
            stackPanel.Children.Add(addSensorButton);
            stackPanel.Children.Add(closeSensorsToAddButton);
            
            return stackPanel;
        }
    }
}
