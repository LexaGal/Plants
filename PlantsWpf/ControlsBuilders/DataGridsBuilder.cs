using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantsWpf.Converters;
using PlantsWpf.DataGridObjects;
using Binding = System.Windows.Data.Binding;
using DataGrid = System.Windows.Controls.DataGrid;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.Forms.MessageBox;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace PlantsWpf.ControlsBuilders
{
    public class DataGridsBuilder
    {
        public DataGrid CreateServiceSystemsDataGrid(PlantsArea area)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(0, 10, 0, 0),
                Width = 183,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanUserAddRows = false,
                AutoGenerateColumns = false
            };

            DataGridTextColumn columnServiceName = new DataGridTextColumn
            {
                Header = "Service",
                Width = 90,
                Binding = new Binding("ServiceName")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true
            };
            DataGridTextColumn columnIsOn = new DataGridTextColumn
            {
                Binding = new Binding("IsOn")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                CellStyle = new Style
                {
                    TargetType = typeof (DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("IsOn")
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            },
                            Value = "✔",
                            Setters = {new Setter(Control.BackgroundProperty, Brushes.LawnGreen)}
                        }
                    }
                },
                IsReadOnly = true
            };

            DataGridTextColumn columnIsScheduled = new DataGridTextColumn
            {
                Width = 70,
                Binding = new Binding("IsScheduled")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                CellStyle = new Style
                {
                    TargetType = typeof (DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("IsScheduled")
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            },
                            Value = "By schedule",
                            Setters = {new Setter(Control.BackgroundProperty, Brushes.Yellow)}
                        }
                    }
                },
                IsReadOnly = true
            };
            dataGrid.Columns.Add(columnServiceName);
            dataGrid.Columns.Add(columnIsOn);
            dataGrid.Columns.Add(columnIsScheduled);
            dataGrid.ItemsSource = area.PlantsAreaServiceState.ServiceStates;
            return dataGrid;
        }

        public DataGrid CreateSensorsDataGrid(PlantsArea area, BindingList<DataGridSensorView> dataGridSensorViews,
            FrameworkElementFactory removeSensorButtonTemplate)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(0, 10, 0, 0),
                Width = 325,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanUserAddRows = false,
                AutoGenerateColumns = false
            };

            DataGridTextColumn measurableType = new DataGridTextColumn
            {
                Header = "Measurable",
                Width = 73,
                Binding = new Binding("Measurable"),
                IsReadOnly = true
            };
            DataGridTextColumn optimal = new DataGridTextColumn
            {
                Header = "Optimal",
                Binding = new Binding("Optimal"),
                IsReadOnly = true
            };
            DataGridTextColumn min = new DataGridTextColumn
            {
                Header = "Min",
                Binding = new Binding("Min"),
                IsReadOnly = true
            };
            DataGridTextColumn max = new DataGridTextColumn
            {
                Header = "Max",
                Binding = new Binding("Max"),
                IsReadOnly = true
            };
            DataGridTextColumn value = new DataGridTextColumn
            {
                Header = "Value",
                Binding = new Binding("Value")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true
            };
            DataGridTextColumn numberOfTimes = new DataGridTextColumn
            {
                Header = "N",
                Binding = new Binding("N")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true
            };
            DataGridTextColumn isCritical = new DataGridTextColumn
            {
                Width = 27,
                Binding = new Binding("IsCritical")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                },
                IsReadOnly = true,
                CellStyle = new Style
                {
                    TargetType = typeof (DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("IsCritical")
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            },
                            Value = "SOS",
                            Setters = {new Setter(Control.BackgroundProperty, Brushes.Red)}
                        }
                    }
                }
            };

            DataGridTemplateColumn remove = new DataGridTemplateColumn
            {
                CellTemplate = new DataTemplate
                {
                    VisualTree = removeSensorButtonTemplate
                }
            };

            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(measurableType);
            dataGrid.Columns.Add(optimal);
            dataGrid.Columns.Add(min);
            dataGrid.Columns.Add(max);
            dataGrid.Columns.Add(value);
            dataGrid.Columns.Add(numberOfTimes);
            dataGrid.Columns.Add(isCritical);
            dataGrid.Columns.Add(remove);

            dataGrid.ItemsSource = dataGridSensorViews;
            return dataGrid;
        }

        public DataGrid CreateSensorsToAddDataGrid(PlantsArea area,
            BindingList<DataGridSensorToAddView> dataGridSensorsToAddViews)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(0, 10, 0, 0),
                Width = 307,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanUserAddRows = true,
                AutoGenerateColumns = false,
            };

            DataGridTextColumn measurableType = new DataGridTextColumn
            {
                Header = "Measurable",
                Width = 77,
                Binding = new Binding("Measurable")

            };
            DataGridTextColumn optimal = new DataGridTextColumn
            {
                Header = "Optimal",
                Binding = new Binding("Optimal")

            };
            DataGridTextColumn min = new DataGridTextColumn
            {
                Header = "Min",
                Binding = new Binding("Min")

            };
            DataGridTextColumn max = new DataGridTextColumn
            {
                Header = "Max",
                Binding = new Binding("Max")

            };
            DataGridTextColumn timeout = new DataGridTextColumn
            {
                Header = "Timeout",
                Binding = new Binding("Timeout")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                }
            };
            DataGridCheckBoxColumn add = new DataGridCheckBoxColumn
            {
                Header = "Add",
                Binding =
                    new Binding("Add")
                    {
                        Converter = new StringToBoolConverter(),
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    }
            };

            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(measurableType);
            dataGrid.Columns.Add(optimal);
            dataGrid.Columns.Add(min);
            dataGrid.Columns.Add(max);
            dataGrid.Columns.Add(timeout);
            dataGrid.Columns.Add(add);

            dataGrid.ItemsSource = dataGridSensorsToAddViews;
            return dataGrid;
        }

        public DataGrid CreateServicesSchedulesDataGrid(PlantsArea area,
            BindingList<DataGridServiceScheduleView> serviceScheduleViews,
            FrameworkElementFactory serviceScheduleSetUpButtonTemplate)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(0, 10, 0, 0),
                Width = 336,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanUserAddRows = true,
                AutoGenerateColumns = false
            };

            DataGridComboBoxColumn serviceName = new DataGridComboBoxColumn
            {
                Header = "Service",
                Width = 77,
                SelectedItemBinding = new Binding("ServiceName"),
                ItemsSource = area.PlantsAreaServiceState.ServiceStates.Select(s => s.ServiceName),
                EditingElementStyle = new Style(typeof (ComboBox))
                {
                    Setters =
                    {
                        new EventSetter
                        {
                            Event = Selector.SelectionChangedEvent,
                            Handler = new SelectionChangedEventHandler((sender, args) =>
                            {
                                ComboBox comboBox = sender as ComboBox;
                                if (comboBox != null)
                                {
                                    if (comboBox.SelectedItem != null)
                                    {
                                        string service = comboBox.SelectedItem.ToString();

                                        ServiceStateEnum parameter;
                                        bool parsed = Enum.TryParse(service, out parameter);

                                        if (!parsed)
                                        {
                                            DataGridServiceScheduleView serviceScheduleView =
                                                serviceScheduleViews.SingleOrDefault(s => s.ServiceName == service);

                                            MeasurableParameter measurableParameter = area.Plant.MeasurableParameters
                                                .SingleOrDefault(
                                                    m => service == String.Format("*{0}*", m.MeasurableType));

                                            if (measurableParameter != null)
                                            {
                                                if (serviceScheduleView == null)
                                                {
                                                    serviceScheduleView = new DataGridServiceScheduleView
                                                    {
                                                        ServiceName = service,
                                                        Parameters = measurableParameter.MeasurableType
                                                    };
                                                    serviceScheduleViews.Remove(serviceScheduleViews.Last());
                                                    serviceScheduleViews.Add(serviceScheduleView);
                                                }
                                            }
                                            return;
                                        }
                                        MessageBox.Show(@"You can add new schedule only for custom service");
                                    }
                                }
                            })
                        }
                    }

                }
            };

            List<string> parametersList = new List<string>(serviceScheduleViews.Select(s => s.Parameters));
            parametersList.AddRange(area.Plant.MeasurableParameters.Select(s => s.MeasurableType));
            DataGridComboBoxColumn parameters = new DataGridComboBoxColumn
            {
                Header = "Parameters",
                SelectedItemBinding = new Binding("Parameters"),
                ItemsSource = parametersList,

            };
            DataGridTextColumn servicingSpan = new DataGridTextColumn
            {
                Header = "Duration",
                Binding = new Binding("ServicingSpan")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                }
            };
            DataGridTextColumn servicingPauseSpan = new DataGridTextColumn
            {
                Header = "Pause",
                Binding =
                    new Binding("ServicingPauseSpan")
                    {
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    }
            };

            DataGridTemplateColumn setUp = new DataGridTemplateColumn
            {
                CellTemplate = new DataTemplate
                {
                    VisualTree = serviceScheduleSetUpButtonTemplate
                }
            };

            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(serviceName);
            dataGrid.Columns.Add(parameters);
            dataGrid.Columns.Add(servicingSpan);
            dataGrid.Columns.Add(servicingPauseSpan);
            dataGrid.Columns.Add(setUp);

            dataGrid.ItemsSource = serviceScheduleViews;
            return dataGrid;
        }

    }
}
