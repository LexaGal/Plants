using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using Binding = System.Windows.Data.Binding;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using DataGrid = System.Windows.Controls.DataGrid;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.Forms.MessageBox;
using TextBox = System.Windows.Controls.TextBox;
using TextBoxBase = System.Windows.Controls.Primitives.TextBoxBase;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace PlantsWpf.DataGridBuilders
{
    public class DataGridBuilder
    {
        public DataGrid CreateServiceSystemsDataGrid(PlantsArea area, EventHandler<DataGridRowEventArgs> dataGridRowAction)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(0, 10, 0, 0),
                Width = 113,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
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
                Header = "State",
                Binding = new Binding("State")
            };

            dataGrid.Columns.Add(action);
            dataGrid.Columns.Add(state);

            dataGrid.Items.Add(new
            {
                Action = "Watering",
                State = area.IsBeingWatering.ToString()
            });
            dataGrid.Items.Add(new
            {
                Action = "Nutrienting",
                State = area.IsBeingNutrienting.ToString()
            });
            dataGrid.Items.Add(new
            {
                Action = "Warming",
                State = area.IsBeingWarming.ToString()
            });
            dataGrid.Items.Add(new
            {
                Action = "Cooling",
                State = area.IsBeingCooling.ToString()
            });

            return dataGrid;
        }

        public DataGrid CreateSensorsDataGrid(PlantsArea area, EventHandler<DataGridRowEventArgs> dataGridRowAction)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(0, 10, 0, 0),
                Width = 307,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            dataGrid.LoadingRow += dataGridRowAction;

            // Create Columns
            DataGridTextColumn measurableType = new DataGridTextColumn
            {
                Header = "Measurable type",
                Binding = new Binding("MeasurableType"),
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
            DataGridTextColumn value = new DataGridTextColumn
            {
                Header = "Value",
                Binding = new Binding("Value")
            };
            DataGridTextColumn critical = new DataGridTextColumn
            {
                Header = "Critical",
                Binding = new Binding("Critical")
            };

            dataGrid.Columns.Add(measurableType);
            dataGrid.Columns.Add(optimal);
            dataGrid.Columns.Add(min);
            dataGrid.Columns.Add(max);
            dataGrid.Columns.Add(value);
            dataGrid.Columns.Add(critical);

            if (area.Sensors.Any(s => s.MeasurableType == MeasurableTypeEnum.Humidity))
            {
                HumiditySensor humiditySensor =
                    area.Sensors.First(s => s.MeasurableType == MeasurableTypeEnum.Humidity) as HumiditySensor;

                if (humiditySensor != null)
                {
                    dataGrid.Items.Add(new
                    {
                        area.Plant.Humidity.MeasurableType,
                        area.Plant.Humidity.Optimal,
                        area.Plant.Humidity.Min,
                        area.Plant.Humidity.Max,
                        Value = humiditySensor.Function.CurrentFunctionValue.ToString("F2"),
                        Critical = humiditySensor.Function.CurrentFunctionValue > area.Plant.Humidity.Max ||
                        humiditySensor.Function.CurrentFunctionValue < area.Plant.Humidity.Min ? "Yes" : "No"
                    });
                }
            }
            if (area.Sensors.Any(s => s.MeasurableType == MeasurableTypeEnum.Temperature))
            {
                TemperatureSensor temperatureSensor =
                    area.Sensors.First(s => s.MeasurableType == MeasurableTypeEnum.Temperature) as TemperatureSensor;
                if (temperatureSensor != null)
                {
                    dataGrid.Items.Add(new
                    {
                        area.Plant.Temperature.MeasurableType,
                        area.Plant.Temperature.Optimal,
                        area.Plant.Temperature.Min,
                        area.Plant.Temperature.Max,
                        Value = temperatureSensor.Function.CurrentFunctionValue.ToString("F2"),
                        Critical = temperatureSensor.Function.CurrentFunctionValue > area.Plant.Temperature.Max ||
                            temperatureSensor.Function.CurrentFunctionValue < area.Plant.Temperature.Min ? "Yes" : "No"
                    });
                }
            }
            if (area.Sensors.Any(s => s.MeasurableType == MeasurableTypeEnum.SoilPh))
            {
                SoilPhSensor soilPhSensor =
                    area.Sensors.First(s => s.MeasurableType == MeasurableTypeEnum.SoilPh) as SoilPhSensor;

                if (soilPhSensor != null)
                {
                    dataGrid.Items.Add(new
                    {
                        area.Plant.SoilPh.MeasurableType,
                        area.Plant.SoilPh.Optimal,
                        area.Plant.SoilPh.Min,
                        area.Plant.SoilPh.Max,
                        Value = soilPhSensor.Function.CurrentFunctionValue.ToString("F2"),
                        Critical = soilPhSensor.Function.CurrentFunctionValue > area.Plant.SoilPh.Max ||
                        soilPhSensor.Function.CurrentFunctionValue < area.Plant.SoilPh.Min ? "Yes" : "No"
                    });
                }
            }
            if (area.Sensors.Any(s => s.MeasurableType == MeasurableTypeEnum.Nutrient))
            {
                NutrientSensor nutrientSensor =
                    area.Sensors.First(s => s.MeasurableType == MeasurableTypeEnum.Nutrient) as NutrientSensor;

                if (nutrientSensor != null)
                {
                    dataGrid.Items.Add(new
                    {
                        area.Plant.Nutrient.MeasurableType,
                        area.Plant.Nutrient.Optimal,
                        area.Plant.Nutrient.Min,
                        area.Plant.Nutrient.Max,
                        Value = nutrientSensor.Function.CurrentFunctionValue.ToString("F2"),
                        Critical = nutrientSensor.Function.CurrentFunctionValue > area.Plant.Nutrient.Max ||
                        nutrientSensor.Function.CurrentFunctionValue < area.Plant.Nutrient.Min ? "Yes" : "No"
                    });
                }
            }
            return dataGrid;
        }

        public DataGrid CreateTurnedOffSensorsDataGrid(PlantsArea area, Action<PlantsArea, Sensor> action)
        {
            DataGrid dataGrid = new DataGrid
            {
                Margin = new Thickness(0, 10, 0, 0),
                Width = 307,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
           
            // Create Columns
            DataGridTextColumn measurableType = new DataGridTextColumn
            {
                Header = "Measurable type",
                Binding = new Binding("MeasurableType"),
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
            
            //
            FrameworkElementFactory textBoxTemplate = new FrameworkElementFactory(typeof(TextBox));
            textBoxTemplate.SetBinding(ContentControl.ContentProperty, new Binding("Timeout"));
            DataGridTemplateColumn textBoxTemplateColumn = new DataGridTemplateColumn()
            {
                Header = "Timeout",
                CellTemplate = new DataTemplate { VisualTree = textBoxTemplate }
            };
            //
            
            //
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory(typeof(Button));
            buttonTemplate.SetBinding(ContentControl.ContentProperty, new Binding("Add"));
            DataGridTemplateColumn dataGridTemplateColumn = new DataGridTemplateColumn()
            {
                Header = "Add",
                CellTemplate = new DataTemplate {VisualTree = buttonTemplate}
            };
            //
 
            dataGrid.Columns.Add(measurableType);
            dataGrid.Columns.Add(optimal);
            dataGrid.Columns.Add(min);
            dataGrid.Columns.Add(max);
            dataGrid.Columns.Add(textBoxTemplateColumn);
            dataGrid.Columns.Add(dataGridTemplateColumn);
            
            List<Sensor> sensors = area.FindTurnedOffSensors();

            foreach (Sensor sensor in sensors)
            {
                string timeout = null;
                TextBox textBox = new TextBox
                {
                    Height = 30,
                    Width = 40,
                    Text = ((int)sensor.MeasuringTimeout.TotalSeconds).ToString()
                };
                textBox.TextChanged += (sender, args) => { timeout = textBox.Text; };
                Button addSensorButton = new Button
                {
                    Name = "AddSensor",
                    Content = "+"
                };

                buttonTemplate.AddHandler(
                    ButtonBase.ClickEvent,
                    new RoutedEventHandler((o, e) =>
                    {
                        try
                        {if (timeout == null) throw new FormatException(tgtguy777uy6y);
                            sensor.SetMeasuringTimeout(new TimeSpan(0, 0, Convert.ToInt32(timeout)));
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show(@"Please, fill in field with numeric value!");
                            return;
                        }
                        action(area, sensor);
                    })
                );

                dataGrid.Items.Add(new
                {
                    sensor.MeasurableType,
                    sensor.MeasurableParameter.Optimal,
                    sensor.MeasurableParameter.Min,
                    sensor.MeasurableParameter.Max,
                    textBox,
                    addSensorButton
                });
            }

            return dataGrid;
        }
    }
}
