using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using PlantingLib.Plants;
using PlantsWpf.Converters;
using PlantsWpf.DataGridObjects;
using Binding = System.Windows.Data.Binding;
using Color = System.Drawing.Color;
using DataGrid = System.Windows.Controls.DataGrid;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace PlantsWpf.ControlsBuilders
{
    public class DataGridsBuilder
    {
        public DataGrid CreateServiceSystemsDataGrid(PlantsArea area)
        {
            DataGrid dataGrid = new DataGrid
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

            DataGridTextColumn columnServiceName = new DataGridTextColumn
            {
                Header = "Service",
                Binding = new Binding("ServiceName")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true
            };
            DataGridTextColumn columnIsOn = new DataGridTextColumn
            {
                Header = "On",
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
                            Setters =
                            {
                                new Setter(Control.BackgroundProperty, Brushes.LawnGreen)
                            }
                        }
                    }
                },
                IsReadOnly = true
            };

            DataGridTextColumn columnIsScheduled = new DataGridTextColumn
            {
                Header = "Scheduled",
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
                            Value = "✔",
                            Setters =
                            {
                                new Setter(Control.BackgroundProperty, Brushes.Yellow)
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
            DataGrid dataGrid = new DataGrid
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

            DataGridTextColumn measurableType = new DataGridTextColumn
            {
                Header = "Measurable",
                Binding = new Binding("Measurable")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                },

                CellStyle = new Style(typeof (TextBox))
                {
                    TargetType = typeof (DataGridCell),
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
            DataGridTextColumn timeout = new DataGridTextColumn
            {
                Header = "Timeout",
                Binding = new Binding("Timeout")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                }
            };
            DataGridTextColumn optimal = new DataGridTextColumn
            {
                Header = "Optimal",
                Binding = new Binding("Optimal")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                }
            };
            DataGridTextColumn min = new DataGridTextColumn
            {
                Header = "Min",
                Binding = new Binding("Min")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                }
            };
            DataGridTextColumn max = new DataGridTextColumn
            {
                Header = "Max",
                Binding = new Binding("Max")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                }
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
            DataGridTemplateColumn remove = new DataGridTemplateColumn
            {
                Header = "X",
                CellTemplate = new DataTemplate
                {
                    VisualTree = removeSensorButtonTemplate
                },
                CellStyle = new Style(typeof (Button))
                {
                    TargetType = typeof (DataGridCell),
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
            DataGridTemplateColumn save = new DataGridTemplateColumn
            {
                Header = "Ok",
                CellTemplate = new DataTemplate
                {
                    VisualTree = sensorSaveButtonTemplate
                },
                CellStyle = new Style(typeof (Button))
                {
                    TargetType = typeof (DataGridCell),
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
                                new Setter(UIElement.IsEnabledProperty, true),
                            }
                        }
                    }
                }
            };

            DataGridTemplateColumn onOff = new DataGridTemplateColumn
            {
                Header = "Off",
                CellTemplate = new DataTemplate
                {
                    VisualTree = onOffSensorButtonTemplate,
                }   
            };
            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(measurableType);
            dataGrid.Columns.Add(timeout);
            dataGrid.Columns.Add(optimal);
            dataGrid.Columns.Add(min);
            dataGrid.Columns.Add(max);
            dataGrid.Columns.Add(value);
            dataGrid.Columns.Add(numberOfTimes);
            //dataGrid.Columns.Add(isCritical);
            dataGrid.Columns.Add(save);
            dataGrid.Columns.Add(onOff);
            dataGrid.Columns.Add(remove);

            dataGrid.RowStyle = new Style
            {
                TargetType = typeof (DataGridRow),
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
                            new Setter(Control.BackgroundProperty, Brushes.BurlyWood),
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
                            new Setter(Control.BackgroundProperty, Brushes.White),
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
                            new Setter(Control.BackgroundProperty, Brushes.Red),
                        }
                    },
                }
            };
            
            dataGrid.ItemsSource = dataGridSensorViews;
            return dataGrid;
        }

        //public DataGrid CreateSensorsToAddDataGrid(PlantsArea area,
        //    BindingList<DataGridSensorToAddView> dataGridSensorsToAddViews)
        //{
        //    DataGrid dataGrid = new DataGrid
        //    {
        //        Margin = new Thickness(0, 10, 0, 0),
        //        Width = 307,
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        VerticalAlignment = VerticalAlignment.Bottom,
        //        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
        //        CanUserAddRows = true,
        //        CanUserResizeColumns = false,
        //        AutoGenerateColumns = false
        //    };

        //    DataGridTextColumn measurableType = new DataGridTextColumn
        //    {
        //        Header = "Measurable",
        //        Width = 77,
        //        Binding = new Binding("Measurable")

        //    };
        //    DataGridTextColumn optimal = new DataGridTextColumn
        //    {
        //        Header = "Optimal",
        //        Binding = new Binding("Optimal")

        //    };
        //    DataGridTextColumn min = new DataGridTextColumn
        //    {
        //        Header = "Min",
        //        Binding = new Binding("Min")

        //    };
        //    DataGridTextColumn max = new DataGridTextColumn
        //    {
        //        Header = "Max",
        //        Binding = new Binding("Max")

        //    };
        //    DataGridTextColumn timeout = new DataGridTextColumn
        //    {
        //        Header = "Timeout",
        //        Binding = new Binding("Timeout")
        //        {
        //            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //        }
        //    };
        //    DataGridCheckBoxColumn add = new DataGridCheckBoxColumn
        //    {
        //        Header = "Add",
        //        Binding =
        //            new Binding("Add")
        //            {
        //                Converter = new StringToBoolConverter(),
        //                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //            }
        //    };

        //    dataGrid.Columns.Clear();
        //    dataGrid.Columns.Add(measurableType);
        //    dataGrid.Columns.Add(optimal);
        //    dataGrid.Columns.Add(min);
        //    dataGrid.Columns.Add(max);
        //    dataGrid.Columns.Add(timeout);
        //    dataGrid.Columns.Add(add);

        //    dataGrid.ItemsSource = dataGridSensorsToAddViews;
        //    return dataGrid;
        //}

        public DataGrid CreateServicesSchedulesDataGrid(PlantsArea area,
            BindingList<DataGridServiceScheduleView> serviceScheduleViews,
            FrameworkElementFactory serviceScheduleSaveButtonTemplate, FrameworkElementFactory onOffSensorButtonTemplate)
        {
            DataGrid dataGrid = new DataGrid
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

            DataGridTextColumn serviceName = new DataGridTextColumn
            {
                Header = "Service",
                Binding = new Binding("ServiceName")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true
            };

            DataGridTextColumn parameters = new DataGridTextColumn
            {
                Header = "Parameters",
                Binding = new Binding("Parameters")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                },
                IsReadOnly = true
            };
            DataGridTextColumn servicingSpan = new DataGridTextColumn
            {
                Header = "Duration",
                Binding = new Binding("ServicingSpan")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                }
            };
            DataGridTextColumn servicingPauseSpan = new DataGridTextColumn
            {
                Header = "Pause",
                Binding = new Binding("ServicingPauseSpan")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    Mode = BindingMode.TwoWay,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                }
            };

            DataGridTemplateColumn save = new DataGridTemplateColumn
            {
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
            DataGridTemplateColumn onOff = new DataGridTemplateColumn
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

            dataGrid.RowStyle = new Style()
            {
                TargetType = typeof (DataGridRow),
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
                            new Setter(Control.BackgroundProperty, Brushes.BurlyWood),
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
                            new Setter(Control.BackgroundProperty, Brushes.White),
                        }
                    }
                }
            };

            dataGrid.ItemsSource = serviceScheduleViews;
            return dataGrid;
        }

    }
}
