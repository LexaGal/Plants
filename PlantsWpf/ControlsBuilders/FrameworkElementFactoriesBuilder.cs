using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;
using PlantsWpf.ObjectsViews;
using Binding = System.Windows.Data.Binding;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using CheckBox = System.Windows.Controls.CheckBox;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PlantsWpf.ControlsBuilders
{
    public class FrameworkElementFactoriesBuilder : IControlsRefresher
    {
        public event EventHandler RefreshControl;

        public virtual void OnRefreshControls()
        {
            EventHandler handler = RefreshControl;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public FrameworkElementFactory CreateOnOffSensorButtonTemplate()
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof(CheckBox));
            buttonTemplate.SetValue(UIElement.ClipToBoundsProperty, true);
            buttonTemplate.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsOffByUser")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });

            buttonTemplate.AddHandler(
                ToggleButton.CheckedEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridSensorView dataGridSensorView = ((FrameworkElement)o).DataContext as DataGridSensorView;
                    if (dataGridSensorView != null)
                    {
                        dataGridSensorView.IsOffByUser = true.ToString();
                    }
                }));

            buttonTemplate.AddHandler(
                ToggleButton.UncheckedEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridSensorView dataGridSensorView = ((FrameworkElement)o).DataContext as DataGridSensorView;
                    if (dataGridSensorView != null)
                    {
                        dataGridSensorView.IsOffByUser = false.ToString();
                    }
                }));

            var vb = new FrameworkElementFactory(typeof(Viewbox));
            vb.AppendChild(buttonTemplate);
            return vb;
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
                    DataGridServiceScheduleView dataGridServiceScheduleView =
                        ((FrameworkElement)o).DataContext as DataGridServiceScheduleView;
                    if (dataGridServiceScheduleView != null)
                    {
                        dataGridServiceScheduleView.IsOn = true.ToString();
                    }
                }));
            buttonTemplate.AddHandler(
                ToggleButton.UncheckedEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridServiceScheduleView dataGridServiceScheduleView =
                        ((FrameworkElement)o).DataContext as DataGridServiceScheduleView;
                    if (dataGridServiceScheduleView != null)
                    {
                        dataGridServiceScheduleView.IsOn = false.ToString();
                    }
                }));
            var vb = new FrameworkElementFactory(typeof(Viewbox));
            vb.AppendChild(buttonTemplate);
            return vb;
        }

        public FrameworkElementFactory CreateRemoveSensorButtonTemplate(PlantsArea area,
            BindingList<DataGridSensorView> dataGridSensorViews,
            BindingList<DataGridServiceScheduleView> dataGridServiceScheduleViews,
            Func<PlantsArea, Sensor, ServiceSchedule, bool> removeSensor)
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof(Button));
            buttonTemplate.SetValue(ContentControl.ContentProperty, "X");
            buttonTemplate.AddHandler(
                ButtonBase.ClickEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridSensorView dataGridSensorView = ((FrameworkElement)o).DataContext as DataGridSensorView;
                    if (dataGridSensorView != null)
                    {
                        if (dataGridSensorViews.Count(s => s.Measurable == dataGridSensorView.Measurable) == 0)
                        {
                            MessageBox.Show(
                                $"'{dataGridSensorView.Measurable}': sensor with such measurable does not exist");
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
                    OnRefreshControls();
                })
                );
            return buttonTemplate;
        }

        public FrameworkElementFactory CreateSensorSaveButtonTemplate(PlantsArea area,
            BindingList<DataGridSensorView> dataGridSensorViews,
            BindingList<DataGridServiceScheduleView> dataGridServiceScheduleViews,
            Func<PlantsArea, Sensor, ServiceSchedule, bool> saveSensor)
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof(Button));
            buttonTemplate.SetValue(ContentControl.ContentProperty, "Save");
            buttonTemplate.AddHandler(
                ButtonBase.ClickEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridSensorView dataGridSensorView = ((FrameworkElement)o).DataContext as DataGridSensorView;

                    if (dataGridSensorView != null)
                    {
                        try
                        {
                            ServiceState serviceState;
                            ServiceSchedule serviceSchedule;

                            if (dataGridSensorView.Sensor != null)
                            {
                                if (!dataGridSensorView.Sensor.MeasurableParameter.HasValidParameters())
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

                                        MessageBox.Show(
                                            $"Sensor with measurable '{dataGridSensorView.Measurable}' updated");

                                        OnRefreshControls();

                                        return;
                                    }
                                }
                                saveSensor(area, dataGridSensorView.Sensor, null);

                                dataGridSensorView.IsModified = false.ToString();

                                MessageBox.Show($"'{dataGridSensorView.Measurable}': sensor data saved");

                                return;
                            }

                            if (dataGridSensorViews.Count(s => s.Measurable == dataGridSensorView.Measurable) != 1)
                            {
                                MessageBox.Show(
                                    $"Sensor with measurable '{dataGridSensorView.Measurable}' already exists");
                                return;
                            }

                            CustomParameter customParameter =
                                new CustomParameter(Guid.NewGuid(), Convert.ToInt32(dataGridSensorView.Optimal),
                                    Convert.ToInt32(dataGridSensorView.Min),
                                    Convert.ToInt32(dataGridSensorView.Max),
                                    dataGridSensorView.Measurable);

                            CustomSensor sensor =
                                new CustomSensor(Guid.NewGuid(), area,
                                    TimeSpan.Parse(dataGridSensorView.Timeout), customParameter);

                            dataGridSensorView.Sensor = sensor;

                            serviceState = new ServiceState(sensor.MeasurableType, true);

                            area.PlantServicesStates.AddServiceState(serviceState);

                            serviceSchedule = new ServiceSchedule(Guid.NewGuid(), area.Id,
                                serviceState.ServiceName, new TimeSpan(0, 0, 10), new TimeSpan(0, 1, 0),
                                new List<MeasurableParameter> { sensor.MeasurableParameter });

                            area.ServicesSchedulesStates.AddServiceSchedule(serviceSchedule);

                            dataGridServiceScheduleViews.Add(new DataGridServiceScheduleView(serviceSchedule));

                            saveSensor(area, sensor, serviceSchedule);

                            dataGridSensorView.IsModified = false.ToString();

                            MessageBox.Show($"'{dataGridSensorView.Measurable}': sensor data saved");

                            OnRefreshControls();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"'{dataGridSensorView.Measurable}': wrong sensor data");
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
            buttonTemplate.SetValue(ContentControl.ContentProperty, "Save");
            buttonTemplate.AddHandler(
                ButtonBase.ClickEvent,
                new RoutedEventHandler((o, e) =>
                {
                    DataGridServiceScheduleView dataGridServiceScheduleView =
                        ((FrameworkElement)o).DataContext as DataGridServiceScheduleView;

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
                            MessageBox.Show($"'{dataGridServiceScheduleView.ServiceName}': wrong schedule data");
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

                            serviceSchedule = new ServiceSchedule(Guid.NewGuid(), area.Id,
                                dataGridServiceScheduleView.ServiceName,
                                servicingSpan, servicingPauseSpan, new List<MeasurableParameter> { measurableParameter });
                        }
                        saveServiceSchedule(area, serviceSchedule);

                        MessageBox.Show($"'{dataGridServiceScheduleView.ServiceName}': schedule data saved");
                        dataGridServiceScheduleView.IsModified = false.ToString();
                    }
                })
                );
            return buttonTemplate;
        }

        public Button CreateRemovePlantsAreaButton(Func<PlantsArea, bool> removePlantsArea, PlantsArea area)
        {
            Button removePlantsAreaButton = new Button
            {
                Margin = new Thickness(0, -2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Content = "X",
                Width = 24,
                Height = 24
            };

            removePlantsAreaButton.Click += (sender, args) =>
            {
                MessageBoxResult result =
                    System.Windows.MessageBox.Show("All plant's area data will be lost. Delete?",
                        "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.OK)
                {
                    removePlantsArea(area);
                }
            };
            return removePlantsAreaButton;
        }
    }
}