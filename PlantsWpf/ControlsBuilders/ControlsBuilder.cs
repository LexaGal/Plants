using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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
        public FrameworkElementFactory CreateOnOffSensorButtonTemplate()
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof (CheckBox));
            buttonTemplate.SetValue(UIElement.ClipToBoundsProperty, true);
            buttonTemplate.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsOffByUser")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });

            buttonTemplate.AddHandler(
                ToggleButton.CheckedEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridSensorView dataGridSensorView = ((FrameworkElement) o).DataContext as DataGridSensorView;
                    if (dataGridSensorView != null)
                    {
                        dataGridSensorView.IsOffByUser = true.ToString();
                    }
                }));

            buttonTemplate.AddHandler(
                ToggleButton.UncheckedEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridSensorView dataGridSensorView = ((FrameworkElement) o).DataContext as DataGridSensorView;
                    if (dataGridSensorView != null)
                    {
                        dataGridSensorView.IsOffByUser = false.ToString();
                    }
                }));
            
            return buttonTemplate;
        }

        public FrameworkElementFactory CreateOnOffServiceScheduleButtonTemplate()
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof(CheckBox));
            buttonTemplate.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsOn")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });

            buttonTemplate.AddHandler(
                ToggleButton.CheckedEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridServiceScheduleView dataGridServiceScheduleView = ((FrameworkElement)o).DataContext as DataGridServiceScheduleView;
                    if (dataGridServiceScheduleView != null)
                    {
                        dataGridServiceScheduleView.IsOn = true.ToString();
                    }
                }));
            buttonTemplate.AddHandler(
                ToggleButton.UncheckedEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridServiceScheduleView dataGridServiceScheduleView = ((FrameworkElement)o).DataContext as DataGridServiceScheduleView;
                    if (dataGridServiceScheduleView != null)
                    {
                        dataGridServiceScheduleView.IsOn = false.ToString();
                    }
                }));
            return buttonTemplate;
        }

        public FrameworkElementFactory CreateRemoveSensorButtonTemplate(PlantsArea area,
            BindingList<DataGridSensorView> dataGridSensorViews, 
            BindingList<DataGridServiceScheduleView> dataGridServiceScheduleViews,
            Func<PlantsArea, Sensor, ServiceSchedule, bool> removeSensor)
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof (Button));
            buttonTemplate.SetValue(ContentControl.ContentProperty, "X");
            buttonTemplate.AddHandler(
                ButtonBase.ClickEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridSensorView dataGridSensorView = ((FrameworkElement) o).DataContext as DataGridSensorView;
                    if (dataGridSensorView != null)
                    {
                        if (dataGridSensorViews.Count(s => s.Measurable == dataGridSensorView.Measurable) == 0)
                        {
                            MessageBox.Show(String.Format("'{0}': sensor with such measurable does not exist",
                                dataGridSensorView.Measurable));
                            return;
                        }

                        ServiceState serviceState = area.PlantServicesStates.ServicesStates.SingleOrDefault(
                            state => state.IsFor(dataGridSensorView.Measurable));

                        if (serviceState != null)
                        {
                            DataGridServiceScheduleView dataGridServiceScheduleView =
                                dataGridServiceScheduleViews.SingleOrDefault(
                                    s => s.ServiceName == serviceState.ServiceName);

                            ServiceSchedule serviceSchedule =
                                area.ServicesSchedulesStates.ServicesSchedules.SingleOrDefault(
                                    schedule => schedule.ServiceName == serviceState.ServiceName);

                            removeSensor(area, dataGridSensorView.Sensor, serviceSchedule);

                            dataGridSensorViews.Remove(dataGridSensorView);
                            dataGridServiceScheduleViews.Remove(dataGridServiceScheduleView);
                        }
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
                            ServiceState serviceState;
                            ServiceSchedule serviceSchedule;

                            if (dataGridSensorView.Sensor != null)
                            {
                                if (!dataGridSensorView.Sensor.MeasurableParameter.IsValid())
                                {
                                    throw new Exception();
                                }

                                if (dataGridSensorView.MeasurableIsModified)
                                {

                                    string oldMeasurable = dataGridSensorView.Sensor.MeasurableType;

                                    dataGridSensorView.Sensor.MeasurableParameter.MeasurableType =
                                        dataGridSensorView.Measurable;

                                    serviceState = dataGridSensorView.Sensor.PlantsArea
                                        .PlantServicesStates.GetServiceState(state => state.IsFor(oldMeasurable));

                                    if (serviceState != null)
                                    {
                                        serviceSchedule = dataGridSensorView.Sensor.PlantsArea
                                            .ServicesSchedulesStates.GetServiceSchedule(
                                                schedule => schedule.IsFor(serviceState.ServiceName));

                                        serviceState.ServiceName = dataGridSensorView.Measurable;

                                        if (serviceSchedule != null)
                                        {
                                            serviceSchedule.ServiceName = serviceState.ServiceName;
                                        }

                                        saveSensor(area, dataGridSensorView.Sensor, serviceSchedule);

                                        dataGridSensorView.MeasurableIsModified = false;
                                        dataGridSensorView.IsModified = false.ToString();

                                        MessageBox.Show(String.Format("Sensor with measurable '{0}' updated",
                                            dataGridSensorView.Measurable));

                                        
                                        return;
                                    }
                                }
                                saveSensor(area, dataGridSensorView.Sensor, null);
                                
                                dataGridSensorView.IsModified = false.ToString();
                                
                                MessageBox.Show(String.Format("'{0}': sensor data saved", dataGridSensorView.Measurable));
                                
                                return;
                            }

                            if (dataGridSensorViews.Count(s => s.Measurable == dataGridSensorView.Measurable) != 1)
                            {
                                MessageBox.Show(String.Format("Sensor with measurable '{0}' already exists",
                                    dataGridSensorView.Measurable));
                                return;
                            }

                            CustomParameter customParameter =
                                new CustomParameter(Guid.NewGuid(), Convert.ToInt32(dataGridSensorView.Optimal),
                                    Convert.ToInt32(dataGridSensorView.Min),
                                    Convert.ToInt32(dataGridSensorView.Max),
                                    dataGridSensorView.Measurable);

                            CustomSensor sensor =
                                new CustomSensor(Guid.NewGuid(), area,
                                    TimeSpan.Parse(dataGridSensorView.Timeout), customParameter, 0);

                            dataGridSensorView.Sensor = sensor;

                            serviceState = new ServiceState(sensor.MeasurableType, true);

                            area.PlantServicesStates.AddServiceState(serviceState);

                            serviceSchedule = new ServiceSchedule(Guid.NewGuid(), area.Id,
                                    serviceState.ServiceName, new TimeSpan(0, 0, 10), new TimeSpan(0, 1, 0),
                                    new List<MeasurableParameter> {sensor.MeasurableParameter});

                            area.ServicesSchedulesStates.AddServiceSchedule(serviceSchedule);

                            dataGridServiceScheduleViews.Add(new DataGridServiceScheduleView(serviceSchedule));

                            saveSensor(area, sensor, serviceSchedule);

                            dataGridSensorView.IsModified = false.ToString();

                            MessageBox.Show(String.Format("'{0}': sensor data saved", dataGridSensorView.Measurable));
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(String.Format("'{0}': wrong sensor data", dataGridSensorView.Measurable));
                        }
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
                    DataGridServiceScheduleView dataGridServiceScheduleView = ((FrameworkElement) o).DataContext as DataGridServiceScheduleView;

                    if (dataGridServiceScheduleView != null)
                    {
                        ServiceSchedule serviceSchedule =
                            area.ServicesSchedulesStates.ServicesSchedules.FirstOrDefault(
                                s => s.ServiceName.ToString() == dataGridServiceScheduleView.ServiceName);

                        TimeSpan servicingSpan;
                        TimeSpan servicingPauseSpan;

                        try
                        {
                            servicingSpan = TimeSpan.Parse(dataGridServiceScheduleView.ServicingSpan);
                            servicingPauseSpan = TimeSpan.Parse(dataGridServiceScheduleView.ServicingPauseSpan);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(String.Format("'{0}': wrong schedule data",
                                    dataGridServiceScheduleView.ServiceName));
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
                                    p => p.MeasurableType == dataGridServiceScheduleView.Parameters);

                            serviceSchedule = new ServiceSchedule(Guid.NewGuid(), area.Id, dataGridServiceScheduleView.ServiceName,
                                servicingSpan, servicingPauseSpan, new List<MeasurableParameter> {measurableParameter});
                        }
                        saveServiceSchedule(area, serviceSchedule);
                        
                        MessageBox.Show(String.Format("'{0}': schedule data saved", dataGridServiceScheduleView.ServiceName));
                        dataGridServiceScheduleView.IsModified = false.ToString();
                    }
                })
                );
            return buttonTemplate;
        }
        
        //public StackPanel CreateButtonsPanel(PlantsArea area, StackPanel plantAreaPanel, 
        //    DataGrid sensorsToAddDataGrid, BindingList<DataGridSensorToAddView> dataGridSensorToAddViews,
        //    Action<PlantsArea, Sensor> saveSensor, BindingList<DataGridSensorView> dataGridSensorViews) 
        //{
        //    Button sensorsToAddButton = new Button
        //    {
        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        Margin = new Thickness(145, 10, 0, 0),
        //        Content = "Sensors",
        //        Width = 70,
        //        Height = 30
        //    };
        //    Button addSensorButton = new Button
        //    {
        //        Width = 40,
        //        Height = 30,
        //        Content = "Add",
        //        Margin = new Thickness(215, -30, 0, 0),
        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        Visibility = Visibility.Collapsed
        //    };
        //    Button closeSensorsToAddButton = new Button
        //    {
        //        Width = 40,
        //        Height = 30,
        //        Content = "Close",
        //        Margin = new Thickness(255, -30, 0, 0),
        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        Visibility = Visibility.Collapsed
        //    };

        //    DataGridsBuilder dataGridsBuilder = new DataGridsBuilder();

        //    sensorsToAddButton.Click += (sender, args) =>
        //    {
        //        dataGridSensorToAddViews = new BindingList<DataGridSensorToAddView>(
        //            area.FindMainSensorsToAdd().ConvertAll(s => new DataGridSensorToAddView(s))) {AllowNew = true};

        //        sensorsToAddDataGrid = dataGridsBuilder.CreateSensorsToAddDataGrid(area, dataGridSensorToAddViews);
        //        plantAreaPanel.Children.Add(sensorsToAddDataGrid);
        //        sensorsToAddButton.IsEnabled = false;
        //        addSensorButton.Visibility = Visibility.Visible;
        //        closeSensorsToAddButton.Visibility = Visibility.Visible;
        //    };

        //    addSensorButton.Click += (o, e) =>
        //    {
        //        try
        //        {
        //            foreach (DataGridSensorToAddView dataGridSensorToAddView in
        //                    dataGridSensorToAddViews.Where(d => d.Add == "yes").ToList())
        //            {
        //                Sensor sensor = area.FindMainSensorsToAdd().SingleOrDefault(s =>
        //                    s.MeasurableType == dataGridSensorToAddView.Measurable);

        //                if (sensor != null)
        //                {
        //                    TimeSpan timeSpan = TimeSpan.Parse(dataGridSensorToAddView.Timeout);

        //                    if (timeSpan.TotalSeconds <= 0)
        //                    {
        //                        throw new FormatException();
        //                    }

        //                    sensor.MeasuringTimeout = timeSpan;

        //                    dataGridSensorToAddViews.Remove(dataGridSensorToAddView);
        //                }

        //                //if custom sensor
        //                else
        //                {
        //                    if (dataGridSensorViews.Any(s => s.Measurable == dataGridSensorToAddView.Measurable))
        //                    {
        //                        MessageBox.Show(String.Format("Sensor with measurable '{0}' already exists",
        //                            dataGridSensorToAddView.Measurable));
        //                        continue;
        //                    }

        //                    TimeSpan timeout = TimeSpan.Parse(dataGridSensorToAddView.Timeout);
        //                    CustomParameter customParameter =
        //                        new CustomParameter(Guid.NewGuid(), Convert.ToInt32(dataGridSensorToAddView.Optimal),
        //                            Convert.ToInt32(dataGridSensorToAddView.Min),
        //                            Convert.ToInt32(dataGridSensorToAddView.Max), dataGridSensorToAddView.Measurable);
        //                    sensor = new CustomSensor(Guid.NewGuid(), area, timeout, customParameter, 0);
        //                }

        //                saveSensor(area, sensor);

        //                dataGridSensorViews.Add(new DataGridSensorView(sensor));
        //            }

        //        }
        //        catch (FormatException)
        //        {
        //            MessageBox.Show(@"Please, fill in fields with numeric values > 0!");
        //        }
        //    };

        //    closeSensorsToAddButton.Click += (sender, args) =>
        //    {
        //        closeSensorsToAddButton.Visibility = Visibility.Collapsed;
        //        addSensorButton.Visibility = Visibility.Collapsed;
        //        sensorsToAddButton.IsEnabled = true;
        //        plantAreaPanel.Children.Remove(sensorsToAddDataGrid);
        //    };

        //    StackPanel stackPanel = new StackPanel();
        //    stackPanel.Children.Add(sensorsToAddButton);
        //    stackPanel.Children.Add(addSensorButton);
        //    stackPanel.Children.Add(closeSensorsToAddButton);
            
        //    return stackPanel;
        //}

        public Button CreateRemovePlantsAreaButton(Func<PlantsArea, bool> removePlantsArea, PlantsArea area)
        {
            Button removePlantsAreaButton = new Button
            {
                Margin = new Thickness(0, 0, 0, 0),
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