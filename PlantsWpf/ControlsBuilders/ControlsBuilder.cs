using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;
using PlantsWpf.DataGridObjects;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PlantsWpf.ControlsBuilders
{
    public class ControlsBuilder
    {
        public FrameworkElementFactory CreateButtonTemplate(PlantsArea area,
            BindingList<DataGridSensorView> dataGridSensorViews, Action<PlantsArea, Sensor> removeSensor,
            BindingList<DataGridSensorToAddView> dataGridSensorToAddViews)
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

                        dataGridSensorToAddViews = new BindingList<DataGridSensorToAddView>(
                            area.FindMainSensorsToAdd().ConvertAll(s => new DataGridSensorToAddView(s)))
                        {
                            AllowNew = true
                        };

                        removeSensor(area, view.Sensor);
                    }
                })
                );
            return buttonTemplate;
        }

        public StackPanel CreateButtonsPanel(PlantsArea area, StackPanel plantAreaPanel, 
            DataGrid sensorsToAddDataGrid, BindingList<DataGridSensorToAddView> dataGridSensorToAddViews,
            Action<PlantsArea, Sensor> saveSensor, BindingList<DataGridSensorView> dataGridSensorViews) 
        {
            Button sensorsToAddButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 10, 0, 0),
                Content = "Sensors",
                Width = 70,
                Height = 30
            };
            Button addSensorButton = new Button
            {
                Width = 40,
                Height = 30,
                Content = "Add",
                Margin = new Thickness(70, -30, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Visibility = Visibility.Collapsed
            };
            Button closeSensorsToAddButton = new Button
            {
                Width = 40,
                Height = 30,
                Content = "Close",
                Margin = new Thickness(110, -30, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Visibility = Visibility.Collapsed
            };

            DataGridsBuilder dataGridsBuilder = new DataGridsBuilder();

            sensorsToAddButton.Click += (sender, args) =>
            {
                dataGridSensorToAddViews = new BindingList<DataGridSensorToAddView>(
                    area.FindMainSensorsToAdd().ConvertAll(s => new DataGridSensorToAddView(s))) {AllowNew = true};

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
                    foreach (DataGridSensorToAddView dataGridSensorToAddView in
                            dataGridSensorToAddViews.Where(d => d.Add == "yes").ToList())
                    {
                        Sensor sensor = area.FindMainSensorsToAdd().SingleOrDefault(s =>
                            s.MeasurableType == dataGridSensorToAddView.Measurable);

                        if (sensor != null)
                        {
                            int i = Convert.ToInt32(dataGridSensorToAddView.Timeout);

                            if (i <= 0)
                            {
                                throw new FormatException();
                            }

                            sensor.MeasuringTimeout = new TimeSpan(0, 0,
                                Convert.ToInt32(dataGridSensorToAddView.Timeout));

                            dataGridSensorToAddViews.Remove(dataGridSensorToAddView);
                        }

                        //if custom sensor
                        else
                        {
                            TimeSpan timeout = new TimeSpan(0, 0, Convert.ToInt32(dataGridSensorToAddView.Timeout));
                            CustomParameter customParameter =
                                new CustomParameter(Convert.ToInt32(dataGridSensorToAddView.Optimal),
                                    Convert.ToInt32(dataGridSensorToAddView.Min),
                                    Convert.ToInt32(dataGridSensorToAddView.Max), dataGridSensorToAddView.Measurable);
                            sensor = new CustomSensor(area, timeout, customParameter, 0);
                        }

                        saveSensor(area, sensor);
                        
                        dataGridSensorViews.Add(new DataGridSensorView(sensor));
                    }

                }
                catch (FormatException)
                {
                    MessageBox.Show(@"Please, fill in fields with numeric values > 0!");
                }
            };

            closeSensorsToAddButton.Click += (sender, args) =>
            {
                closeSensorsToAddButton.Visibility = Visibility.Collapsed;
                addSensorButton.Visibility = Visibility.Collapsed;
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
