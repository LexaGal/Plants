using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using PlantingLib.Plants;
using PlantingLib.Properties;
using PlantsWpf.ObjectsViews;

namespace PlantsWpf.ControlsBuilders
{
    public class DataGridsBuilder
    {
        public DataGrid CreateServiceSystemsDataGrid(PlantsArea area)
        {
            var dataGrid = new DataGrid
            {
                Margin = new Thickness(20, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanUserAddRows = false,
                CanUserResizeColumns = false,
                AutoGenerateColumns = false,
                ColumnWidth = DataGridLength.Auto
            };

            var columnServiceName = new DataGridTextColumn
            {
                Header = "Service",
                Binding = new Binding("ServiceName")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true
            };
            var columnIsOn = new DataGridTextColumn
            {
                Header = "On",
                Binding = new Binding("IsOn")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                CellStyle = new Style
                {
                    TargetType = typeof(DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("IsOn")
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            },
                            Value = Resources.IsScheduledSign,
                            Setters =
                            {
                                new Setter(Control.BackgroundProperty,
                                    (SolidColorBrush) MainWindow.ResourceDictionary["ServiceBackground"])
                            }
                        }
                    }
                },
                IsReadOnly = true
            };

            var columnIsScheduled = new DataGridTextColumn
            {
                Header = "Scheduled",
                Binding = new Binding("IsScheduled")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                CellStyle = new Style
                {
                    TargetType = typeof(DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("IsScheduled")
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            },
                            Value = Resources.IsScheduledSign,
                            Setters =
                            {
                                new Setter(Control.BackgroundProperty,
                                    (SolidColorBrush) MainWindow.ResourceDictionary["ScheduleBackground"])
                            }
                        }
                    }
                },
                IsReadOnly = true
            };
            dataGrid.Columns.Add(columnServiceName);
            dataGrid.Columns.Add(columnIsOn);
            dataGrid.Columns.Add(columnIsScheduled);
            dataGrid.ItemsSource = area.PlantServicesStates.ServicesStates;
            return dataGrid;
        }

        public DataGrid CreateSensorsDataGrid(PlantsArea area, BindingList<DataGridSensorView> dataGridSensorViews,
            FrameworkElementFactory removeSensorButtonTemplate, FrameworkElementFactory sensorSaveButtonTemplate,
            FrameworkElementFactory onOffSensorButtonTemplate)
        {
            var dataGrid = new DataGrid
            {
                Margin = new Thickness(10, 10, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanUserAddRows = true,
                CanUserDeleteRows = false,
                CanUserResizeColumns = false,
                AutoGenerateColumns = false,
                ColumnWidth = DataGridLength.Auto
            };

            var style = new Style(typeof(DataGridCell))
            {
                Setters =
                {
                    new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center)
                }
            };
            var measurableType = new DataGridTextColumn
            {
                Header = "Measurable",
                Binding = new Binding("Measurable")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true
                },
                CellStyle = new Style(typeof(TextBox))
                {
                    TargetType = typeof(DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("IsCustom"),
                            Value = false.ToString(),
                            Setters =
                            {
                                new Setter(UIElement.IsEnabledProperty, false)
                            }
                        }
                    }
                }
            };
            var timeout = new DataGridTextColumn
            {
                Header = "Timeout",
                Binding = new Binding("Timeout")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true
                },
                CellStyle = style
            };
            var optimal = new DataGridTextColumn
            {
                Header = "Optimal",
                Binding = new Binding("Optimal")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true
                },
                CellStyle = style
            };
            var min = new DataGridTextColumn
            {
                Header = "Min",
                Binding = new Binding("Min")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true
                },
                CellStyle = style
            };
            var max = new DataGridTextColumn
            {
                Header = "Max",
                Binding = new Binding("Max")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true
                },
                CellStyle = style
            };
            var value = new DataGridTextColumn
            {
                Header = "Value",
                Binding = new Binding("Value")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true,
                CellStyle = style
            };

            var numberOfTimes = new DataGridTextColumn
            {
                Header = "Messages",
                Binding = new Binding("N")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true,
                CellStyle = style
            };
            var remove = new DataGridTemplateColumn
            {
                Header = "X",
                CellTemplate = new DataTemplate
                {
                    VisualTree = removeSensorButtonTemplate
                },
                CellStyle = new Style(typeof(Button))
                {
                    TargetType = typeof(DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("IsCustom"),
                            Value = false.ToString(),
                            Setters =
                            {
                                new Setter(UIElement.IsEnabledProperty, false)
                            }
                        }
                    }
                }
            };
            var save = new DataGridTemplateColumn
            {
                Header = "Save",
                CellTemplate = new DataTemplate
                {
                    VisualTree = sensorSaveButtonTemplate
                },
                CellStyle = new Style(typeof(Button))
                {
                    TargetType = typeof(DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("IsModified")
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            },
                            Value = false.ToString(),
                            Setters =
                            {
                                new Setter(UIElement.IsEnabledProperty, false)
                            }
                        },
                        new DataTrigger
                        {
                            Binding = new Binding("IsModified")
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            },
                            Value = true.ToString(),
                            Setters =
                            {
                                new Setter(UIElement.IsEnabledProperty, true)
                            }
                        }
                    }
                }
            };

            var onOff = new DataGridTemplateColumn
            {
                Header = "Off",
                CellTemplate = new DataTemplate
                {
                    VisualTree = onOffSensorButtonTemplate
                }
                ,
                CellStyle = style
            };
            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(measurableType);
            dataGrid.Columns.Add(timeout);
            dataGrid.Columns.Add(optimal);
            dataGrid.Columns.Add(min);
            dataGrid.Columns.Add(max);
            dataGrid.Columns.Add(value);
            dataGrid.Columns.Add(numberOfTimes);
            dataGrid.Columns.Add(save);
            dataGrid.Columns.Add(onOff);
            dataGrid.Columns.Add(remove);

            dataGrid.RowStyle = new Style
            {
                TargetType = typeof(DataGridRow),
                Triggers =
                {
                    new DataTrigger
                    {
                        Binding = new Binding("IsOffByUser")
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        },
                        Value = true.ToString(),
                        Setters =
                        {
                            new Setter(Control.BackgroundProperty,
                                (SolidColorBrush) MainWindow.ResourceDictionary["SensorOrScheduleIsOff"])
                        }
                    },
                    new DataTrigger
                    {
                        Binding = new Binding("IsOffByUser")
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        },
                        Value = false.ToString(),
                        Setters =
                        {
                            new Setter(Control.BackgroundProperty,
                                (SolidColorBrush) MainWindow.ResourceDictionary["Main"])
                        }
                    },
                    new DataTrigger
                    {
                        Binding = new Binding("IsCritical")
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        },
                        Value = true.ToString(),
                        Setters =
                        {
                            new Setter(Control.BackgroundProperty,
                                (SolidColorBrush) MainWindow.ResourceDictionary["CriticalBackground"])
                        }
                    }
                }
            };

            dataGrid.ItemsSource = dataGridSensorViews;
            return dataGrid;
        }

        public DataGrid CreateServicesSchedulesDataGrid(PlantsArea area,
            BindingList<DataGridServiceScheduleView> serviceScheduleViews,
            FrameworkElementFactory serviceScheduleSaveButtonTemplate, FrameworkElementFactory onOffSensorButtonTemplate)
        {
            var dataGrid = new DataGrid
            {
                Margin = new Thickness(20, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                CanUserResizeColumns = false,
                AutoGenerateColumns = false,
                ColumnWidth = DataGridLength.Auto
            };

            var serviceName = new DataGridTextColumn
            {
                Header = "Service",
                Binding = new Binding("ServiceName")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true
            };

            var parameters = new DataGridTextColumn
            {
                Header = "Parameters",
                Binding = new Binding("Parameters")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                },
                IsReadOnly = true
            };
            var servicingSpan = new DataGridTextColumn
            {
                Header = "Duration",
                Binding = new Binding("ServicingSpan")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true
                }
            };
            var servicingPauseSpan = new DataGridTextColumn
            {
                Header = "Pause",
                Binding = new Binding("ServicingPauseSpan")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true
                }
            };

            var save = new DataGridTemplateColumn
            {
                Header = "Save",
                CellTemplate = new DataTemplate
                {
                    VisualTree = serviceScheduleSaveButtonTemplate
                },
                CellStyle = new Style(typeof(Button))
                {
                    TargetType = typeof(DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("IsModified")
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            },
                            Value = false.ToString(),
                            Setters =
                            {
                                new Setter(UIElement.IsEnabledProperty, false)
                            }
                        },
                        new DataTrigger
                        {
                            Binding = new Binding("IsModified")
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            },
                            Value = true.ToString(),
                            Setters =
                            {
                                new Setter(UIElement.IsEnabledProperty, true)
                            }
                        }
                    }
                }
            };
            var onOff = new DataGridTemplateColumn
            {
                Header = "On",
                CellTemplate = new DataTemplate
                {
                    VisualTree = onOffSensorButtonTemplate
                }
            };

            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(serviceName);
            dataGrid.Columns.Add(parameters);
            dataGrid.Columns.Add(servicingSpan);
            dataGrid.Columns.Add(servicingPauseSpan);
            dataGrid.Columns.Add(save);
            dataGrid.Columns.Add(onOff);

            dataGrid.RowStyle = new Style
            {
                TargetType = typeof(DataGridRow),
                Triggers =
                {
                    new DataTrigger
                    {
                        Binding = new Binding("IsOn")
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        },
                        Value = false.ToString(),
                        Setters =
                        {
                            new Setter(Control.BackgroundProperty,
                                (SolidColorBrush) MainWindow.ResourceDictionary["SensorOrScheduleIsOff"])
                        }
                    },
                    new DataTrigger
                    {
                        Binding = new Binding("IsOn")
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        },
                        Value = true.ToString(),
                        Setters =
                        {
                            new Setter(Control.BackgroundProperty,
                                (SolidColorBrush) MainWindow.ResourceDictionary["Main"])
                        }
                    }
                }
            };

            dataGrid.ItemsSource = serviceScheduleViews;
            return dataGrid;
        }
    }
}