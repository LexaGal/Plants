using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;
using PlantsWpf.DataGridObjects;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PlantsWpf.ControlsBuilders
{
    public class ControlsBuilder
    {
        public FrameworkElementFactory CreateRemoveSensorButtonTemplate(PlantsArea area,
            BindingList<DataGridSensorView> dataGridSensorViews, Func<PlantsArea, Sensor, bool> removeSensor)
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof (Button));
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
                    }
                })
                );
            return buttonTemplate;
        }

        public FrameworkElementFactory CreateSensorSaveButtonTemplate(PlantsArea area,
            BindingList<DataGridSensorView> dataGridSensorViews,
            BindingList<DataGridServiceScheduleView> dataGridServiceScheduleViews, Func<PlantsArea, Sensor, ServiceSchedule, bool> saveSensor)
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof (Button));
            buttonTemplate.SetValue(ContentControl.ContentProperty, "Ok");
            buttonTemplate.AddHandler(
                ButtonBase.ClickEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridSensorView dataGridSensorView = ((FrameworkElement) o).DataContext as DataGridSensorView;

                    if (dataGridSensorView != null)
                    {
                        try
                        {
                            if (dataGridSensorView.Sensor != null)
                            {
                                dataGridSensorView.UpdateSource();
                                saveSensor(area, dataGridSensorView.Sensor, null);
                            }
                            else
                            {
                                CustomParameter customParameter =
                                    new CustomParameter(Guid.NewGuid(), Convert.ToInt32(dataGridSensorView.Optimal),
                                        Convert.ToInt32(dataGridSensorView.Min), Convert.ToInt32(dataGridSensorView.Max),
                                        dataGridSensorView.Measurable);

                                CustomSensor sensor =
                                    new CustomSensor(Guid.NewGuid(), area,
                                        TimeSpan.Parse(dataGridSensorView.Timeout), customParameter, 0);
                                
                                dataGridSensorView.Sensor = sensor;
                                dataGridSensorView.UpdateSource();
                                dataGridSensorView.UpdateView();

                                ServiceState serviceState = 
                                    new ServiceState(sensor.MeasurableType, true);
                                
                                area.PlantsAreaServicesStates.AddServiceState(serviceState);

                                ServiceSchedule serviceSchedule = 
                                    new ServiceSchedule(Guid.NewGuid(), area.Id,
                                        serviceState.ServiceName, new TimeSpan(0, 0, 10), new TimeSpan(0, 1, 0),
                                        new List<MeasurableParameter> { sensor.MeasurableParameter });
                                
                                area.ServicesSchedulesStates.AddServiceSchedule(serviceSchedule);

                                dataGridServiceScheduleViews.Add(new DataGridServiceScheduleView(serviceSchedule));
                                
                                saveSensor(area, sensor, serviceSchedule);
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(@"Wrong sensor data");
                            return;
                        }
                        MessageBox.Show(@"Sensor data saved");
                    }
                })
                );
            return buttonTemplate;
        }

        public FrameworkElementFactory CreateServiceScheduleSaveButtonTemplate(PlantsArea area,
            BindingList<DataGridServiceScheduleView> dataGridServiceScheduleViews, Func<PlantsArea, 
            ServiceSchedule, bool> saveServiceSchedule)
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof(Button));
            buttonTemplate.SetValue(ContentControl.ContentProperty, "Ok");
            buttonTemplate.AddHandler(
                ButtonBase.ClickEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridServiceScheduleView view = ((FrameworkElement) o).DataContext as DataGridServiceScheduleView;

                    if (view != null)
                    {
                        ServiceSchedule serviceSchedule =
                            area.ServicesSchedulesStates.ServicesSchedules.FirstOrDefault(
                                s => s.ServiceState.ToString() == view.ServiceName);

                        TimeSpan servicingSpan;
                        TimeSpan servicingPauseSpan;

                        try
                        {
                            servicingSpan = TimeSpan.Parse(view.ServicingSpan);
                            servicingPauseSpan = TimeSpan.Parse(view.ServicingPauseSpan);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(@"Wrong schedule data");
                            return;
                        }

                        if (serviceSchedule != null)
                        {
                            serviceSchedule.ServicingSpan = servicingSpan;
                            serviceSchedule.ServicingPauseSpan = servicingPauseSpan;
                        }
                        else
                        {
                            MeasurableParameter measurableParameter =
                                area.Plant.MeasurableParameters.SingleOrDefault(
                                    p => p.MeasurableType == view.Parameters);

                            serviceSchedule = new ServiceSchedule(area.Id, view.ServiceName,
                                servicingSpan, servicingPauseSpan, new List<MeasurableParameter> {measurableParameter});
                        }
                        saveServiceSchedule(area, serviceSchedule);
                        
                        MessageBox.Show(@"Schedule data saved");
                        view.IsModified = false.ToString();
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
                Margin = new Thickness(145, 10, 0, 0),
                Content = "Sensors",
                Width = 70,
                Height = 30
            };
            Button addSensorButton = new Button
            {
                Width = 40,
                Height = 30,
                Content = "Add",
                Margin = new Thickness(215, -30, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Visibility = Visibility.Collapsed
            };
            Button closeSensorsToAddButton = new Button
            {
                Width = 40,
                Height = 30,
                Content = "Close",
                Margin = new Thickness(255, -30, 0, 0),
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
                            TimeSpan timeSpan = TimeSpan.Parse(dataGridSensorToAddView.Timeout);

                            if (timeSpan.TotalSeconds <= 0)
                            {
                                throw new FormatException();
                            }

                            sensor.MeasuringTimeout = timeSpan;

                            dataGridSensorToAddViews.Remove(dataGridSensorToAddView);
                        }

                        //if custom sensor
                        else
                        {
                            if (dataGridSensorViews.Any(s => s.Measurable == dataGridSensorToAddView.Measurable))
                            {
                                MessageBox.Show(String.Format("Sensor with measurable '{0}' already exists",
                                    dataGridSensorToAddView.Measurable));
                                continue;
                            }

                            TimeSpan timeout = TimeSpan.Parse(dataGridSensorToAddView.Timeout);
                            CustomParameter customParameter =
                                new CustomParameter(Guid.NewGuid(), Convert.ToInt32(dataGridSensorToAddView.Optimal),
                                    Convert.ToInt32(dataGridSensorToAddView.Min),
                                    Convert.ToInt32(dataGridSensorToAddView.Max), dataGridSensorToAddView.Measurable);
                            sensor = new CustomSensor(Guid.NewGuid(), area, timeout, customParameter, 0);
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

        public Button CreateRemovePlantsAreaButton(Func<PlantsArea, bool> removePlantsArea, PlantsArea area)
        {
            Button removePlantsAreaButton = new Button
            {
                Margin = new Thickness(0, -50, 25, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Content = "X",
                Width = 25,
                Height = 30
            };

            removePlantsAreaButton.Click += (sender, args) =>
            {
                removePlantsArea(area);
            };
            return removePlantsAreaButton;
        }
    }
}