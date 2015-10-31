using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using PlantingLib.Plants;
using PlantingLib.Plants.ServiceStates;
using PlantsWpf.Converters;
using PlantsWpf.DataGridObjects;
using Binding = System.Windows.Data.Binding;
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
                Margin = new Thickness(0, 10, 0, 0),
                Width = 113,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                CanUserAddRows = false,
                AutoGenerateColumns = false
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
                Header = "Is On",
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

            dataGrid.Columns.Add(columnServiceName);
            dataGrid.Columns.Add(columnIsOn);
            dataGrid.ItemsSource = area.PlantsAreaServiceState.ServiceStates;
            return dataGrid;
        }

        public DataGrid CreateSensorsDataGrid(PlantsArea area, BindingList<DataGridSensorView> dataGridSensorViews,
            FrameworkElementFactory factory)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(0, 10, 0, 0),
                Width = 320,
                HorizontalAlignment = HorizontalAlignment.Left,
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
                            Value = "(!)",
                            Setters = {new Setter(Control.BackgroundProperty, Brushes.Red)}
                        }
                    }
                }
            };

            DataGridTemplateColumn remove = new DataGridTemplateColumn
            {
                CellTemplate = new DataTemplate
                {
                    VisualTree = factory
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
                HorizontalAlignment = HorizontalAlignment.Left,
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
    }
}
