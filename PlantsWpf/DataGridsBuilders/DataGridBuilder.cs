using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            EventHandler<DataGridRowEventArgs> dataGridRowAction)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(10, 10, 0, 0),
                Width = 95,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
            };
            dataGrid.LoadingRow += dataGridRowAction;

            // Create Columns
            DataGridTextColumn action = new DataGridTextColumn
            {
                Header = "Action",
                Binding = new Binding("Action")
            };
            DataGridTextColumn state = new DataGridTextColumn
            {
                Header = "✔",
                Binding = new Binding("State")
            };

            dataGrid.Columns.Add(action);
            dataGrid.Columns.Add(state);

            dataGrid.Items.Add(new
            {
                Action = "Watering",
                State = area.IsBeingWatering ? "✔" : String.Empty
            });
            dataGrid.Items.Add(new
            {
                Action = "Nutrienting",
                State = area.IsBeingNutrienting ? "✔" : String.Empty
            });
            dataGrid.Items.Add(new
            {
                Action = "Warming",
                State = area.IsBeingWarming ? "✔" : String.Empty
            });
            dataGrid.Items.Add(new
            {
                Action = "Cooling",
                State = area.IsBeingCooling ? "✔" : String.Empty
            });

            return dataGrid;
        }

        public DataGrid CreateSensorsDataGrid(PlantsArea area, EventHandler<DataGridRowEventArgs> dataGridRowAction)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(10, 10, 0, 0),
                Width = 300,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
            };
            dataGrid.LoadingRow += dataGridRowAction;

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
            DataGridTextColumn value = new DataGridTextColumn
            {
                Header = "Value",
                Binding = new Binding("Value") {UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged},
                IsReadOnly = true
            };
            DataGridTextColumn numberOfTimes = new DataGridTextColumn
            {
                Header = "N",
                Binding = new Binding("NumberOfTimes") {UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged},
                IsReadOnly = true
            };
            DataGridTextColumn critical = new DataGridTextColumn
            {
                Header = "✘",
                Binding = new Binding("Critical") {UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged},
                IsReadOnly = true
            };

            dataGrid.Columns.Add(measurableType);
            dataGrid.Columns.Add(optimal);
            dataGrid.Columns.Add(min);
            dataGrid.Columns.Add(max);
            dataGrid.Columns.Add(value);
            dataGrid.Columns.Add(numberOfTimes);
            dataGrid.Columns.Add(critical);

            ObservableCollection<DataGridSensorView> dataGridSensorViews = new ObservableCollection<DataGridSensorView>(
                area.Sensors.ToList().ConvertAll(s => new DataGridSensorView
                {
                    MeasurableType = s.MeasurableType.ToString(),
                    Optimal = area.Plant.Humidity.Optimal.ToString(),
                    Min = area.Plant.Humidity.Min.ToString(),
                    Max = area.Plant.Humidity.Max.ToString(),
                    Value = s.Function.CurrentFunctionValue.ToString("F2"),
                    NumberOfTimes = s.NumberOfTimes.ToString(),
                    IsCritical = s.Function.CurrentFunctionValue > area.Plant.Humidity.Max ||
                                 s.Function.CurrentFunctionValue < area.Plant.Humidity.Min ? "✘" : String.Empty,
                }));

            //if (area.Sensors.Any(s => s.MeasurableType == MeasurableTypeEnum.Humidity))
            //{
            //    HumiditySensor humiditySensor =
            //        area.Sensors.First(s => s.MeasurableType == MeasurableTypeEnum.Humidity) as HumiditySensor;

            //    if (humiditySensor != null)
            //    {
            //        dataGrid.Items.Add(new
            //        {
            //            area.Plant.Humidity.MeasurableType,
            //            area.Plant.Humidity.Optimal,
            //            area.Plant.Humidity.Min,
            //            area.Plant.Humidity.Max,
            //            Value = humiditySensor.Function.CurrentFunctionValue.ToString("F2"),
            //            humiditySensor.NumberOfTimes,
            //            Critical = humiditySensor.Function.CurrentFunctionValue > area.Plant.Humidity.Max ||
            //                       humiditySensor.Function.CurrentFunctionValue < area.Plant.Humidity.Min
            //                ? "✘"
            //                : String.Empty,
            //        });
            //    }
            //}
            //if (area.Sensors.Any(s => s.MeasurableType == MeasurableTypeEnum.Temperature))
            //{
            //    TemperatureSensor temperatureSensor =
            //        area.Sensors.First(s => s.MeasurableType == MeasurableTypeEnum.Temperature) as TemperatureSensor;
            //    if (temperatureSensor != null)
            //    {
            //        dataGrid.Items.Add(new
            //        {
            //            area.Plant.Temperature.MeasurableType,
            //            area.Plant.Temperature.Optimal,
            //            area.Plant.Temperature.Min,
            //            area.Plant.Temperature.Max,
            //            Value = temperatureSensor.Function.CurrentFunctionValue.ToString("F2"),
            //            temperatureSensor.NumberOfTimes,
            //            Critical = temperatureSensor.Function.CurrentFunctionValue > area.Plant.Temperature.Max ||
            //                       temperatureSensor.Function.CurrentFunctionValue < area.Plant.Temperature.Min
            //                ? "✘"
            //                : String.Empty,

            //        });
            //    }
            //}
            //if (area.Sensors.Any(s => s.MeasurableType == MeasurableTypeEnum.SoilPh))
            //{
            //    SoilPhSensor soilPhSensor =
            //        area.Sensors.First(s => s.MeasurableType == MeasurableTypeEnum.SoilPh) as SoilPhSensor;

            //    if (soilPhSensor != null)
            //    {
            //        dataGrid.Items.Add(new
            //        {
            //            area.Plant.SoilPh.MeasurableType,
            //            area.Plant.SoilPh.Optimal,
            //            area.Plant.SoilPh.Min,
            //            area.Plant.SoilPh.Max,
            //            Value = soilPhSensor.Function.CurrentFunctionValue.ToString("F2"),
            //            soilPhSensor.NumberOfTimes,
            //            Critical = soilPhSensor.Function.CurrentFunctionValue > area.Plant.SoilPh.Max ||
            //                       soilPhSensor.Function.CurrentFunctionValue < area.Plant.SoilPh.Min
            //                ? "✘"
            //                : String.Empty,
            //        });
            //    }
            //}
            //if (area.Sensors.Any(s => s.MeasurableType == MeasurableTypeEnum.Nutrient))
            //{
            //    NutrientSensor nutrientSensor =
            //        area.Sensors.First(s => s.MeasurableType == MeasurableTypeEnum.Nutrient) as NutrientSensor;

            //    if (nutrientSensor != null)
            //    {
            //        dataGrid.Items.Add(new
            //        {
            //            area.Plant.Nutrient.MeasurableType,
            //            area.Plant.Nutrient.Optimal,
            //            area.Plant.Nutrient.Min,
            //            area.Plant.Nutrient.Max,
            //            Value = nutrientSensor.Function.CurrentFunctionValue.ToString("F2"),
            //            nutrientSensor.NumberOfTimes,
            //            Critical = nutrientSensor.Function.CurrentFunctionValue > area.Plant.Nutrient.Max ||
            //                       nutrientSensor.Function.CurrentFunctionValue < area.Plant.Nutrient.Min
            //                ? "✘"
            //                : String.Empty,
            //        });
            //    }
            //}

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
