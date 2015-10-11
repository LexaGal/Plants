using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using PlantsWpf.DataGridObjects;
using Binding = System.Windows.Data.Binding;
using Button = System.Windows.Controls.Button;
using DataGrid = System.Windows.Controls.DataGrid;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.Forms.MessageBox;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace PlantsWpf.DataGridsBuilders
{
    public class DataGridBuilder
    {
        public DataGrid CreateServiceSystemsDataGrid(PlantsArea area,
            BindingList<PlantsAreaServiceState> plantsAreaServiceStates)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(35, 10, 0, 0),
                Width = 245,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
            };

            // Create Columns
            DataGridTextColumn watering = new DataGridTextColumn
            {
                Header = "Watering",
                Binding = new Binding("Watering")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay,
                },
                CellStyle = new Style
                {
                    TargetType = typeof (DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("Watering")
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
            DataGridTextColumn nutrienting = new DataGridTextColumn
            {
                Header = "Nutrienting",
                Binding = new Binding("Nutrienting")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay,
                },
                CellStyle = new Style
                {
                    TargetType = typeof (DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("Nutrienting")
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
            DataGridTextColumn warming = new DataGridTextColumn
            {
                Header = "Warming",
                Binding = new Binding("Warming")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay,
                },
                CellStyle = new Style
                {
                    TargetType = typeof (DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("Warming")
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
            DataGridTextColumn cooling = new DataGridTextColumn
            {
                Header = "Cooling",
                Binding = new Binding("Cooling")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay,
                },
                CellStyle = new Style
                {
                    TargetType = typeof (DataGridCell),
                    Triggers =
                    {
                        new DataTrigger
                        {
                            Binding = new Binding("Cooling")
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

            dataGrid.Columns.Add(watering);
            dataGrid.Columns.Add(nutrienting);
            dataGrid.Columns.Add(warming);
            dataGrid.Columns.Add(cooling);

            dataGrid.ItemsSource = plantsAreaServiceStates;
            return dataGrid;
        }

        public DataGrid CreateSensorsDataGrid(PlantsArea area, BindingList<DataGridSensorView> dataGridSensorViews)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(10, 10, 0, 0),
                Width = 300,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
               };
            
            //Create Columns
            DataGridTextColumn measurableType = new DataGridTextColumn
            {
                Header = "Measurable type",
                Binding = new Binding("MeasurableType"),
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
                    Mode = BindingMode.OneWay,
                },
                IsReadOnly = true
             };
            DataGridTextColumn numberOfTimes = new DataGridTextColumn
            {
                Header = "N",
                Binding = new Binding("NumberOfTimes")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.OneWay
                },
                IsReadOnly = true
            };
            DataGridTextColumn isCritical = new DataGridTextColumn
            {
                Header = "✘",
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
                            Value = "✘",
                            Setters = {new Setter(Control.BackgroundProperty, Brushes.Red)}
                        }
                    }
                }
            };
        
            dataGrid.Columns.Add(measurableType);
            dataGrid.Columns.Add(optimal);
            dataGrid.Columns.Add(min);
            dataGrid.Columns.Add(max);
            dataGrid.Columns.Add(value);
            dataGrid.Columns.Add(numberOfTimes);
            dataGrid.Columns.Add(isCritical);

            dataGrid.ItemsSource = dataGridSensorViews;
            return dataGrid;
        }

        public DataGrid CreateSensorsToAddDataGrid(PlantsArea area, ObservableCollection<DataGridSensorToAddView> dataGridSensorsToAddViews)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(0, 10, 0, 0),
                Width = 307,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                AutoGenerateColumns = false
            };

            // Create Columns
            DataGridTextColumn measurableType = new DataGridTextColumn
            {
                Header = "Measurable type",
                Binding = new Binding("MeasurableType"),
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

            dataGrid.Columns.Add(measurableType);
            dataGrid.Columns.Add(optimal);
            dataGrid.Columns.Add(min);
            dataGrid.Columns.Add(max);
            dataGrid.Columns.Add(timeout);
            dataGrid.Columns.Add(add);
            
            dataGrid.ItemsSource = dataGridSensorsToAddViews;
            return dataGrid;
        }
    }
}
