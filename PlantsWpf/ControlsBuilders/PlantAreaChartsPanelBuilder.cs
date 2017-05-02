using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Media;
using PlantingLib.MeasurableParameters;
using PlantsWpf.ObjectsViews;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

namespace PlantsWpf.ControlsBuilders
{
    public class PlantAreaChartsPanelBuilder
    {
        private readonly ChartDescriptor _chartDescriptor;
        private readonly List<MeasurableParameter> _measurableParameters;
        private readonly StackPanel _plantAreaChartsPanel;

        public PlantAreaChartsPanelBuilder(List<MeasurableParameter> measurableParameters,
            IControlsRefresher controlsRefresher, StackPanel plantAreaChartsPanel, ChartDescriptor chartDescriptor)
        {
            controlsRefresher.RefreshControl += RefreshControls;
            _measurableParameters = measurableParameters;
            _plantAreaChartsPanel = plantAreaChartsPanel;
            _chartDescriptor = chartDescriptor;
        }

        private void RefreshControls(object sender, EventArgs eventArgs)
        {
            RebuildChartsPanel();
        }

        public void RebuildChartsPanel()
        {
            _plantAreaChartsPanel.Children.Clear();
            foreach (var measurableParameter in _measurableParameters)
                if (measurableParameter != null)
                {
                    var chart = new Chart
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Width = 1100,
                        Height = 225,
                        BorderBrush = Brushes.Black,
                        Background = (LinearGradientBrush) MainWindow.ResourceDictionary["ChartBackground"],
                        Title = measurableParameter.MeasurableType
                    };

                    var areaSeries = new AreaSeries
                    {
                        IndependentValueBinding = new Binding("Key"),
                        DependentValueBinding = new Binding("Value"),
                        Title = measurableParameter.MeasurableType
                    };
                    chart.Series.Add(areaSeries);
                    _plantAreaChartsPanel.Children.Add(chart);
                }

            var chartDescriptorPanel = CreateChartDescriptorPanel();

            _plantAreaChartsPanel.Children.Add(chartDescriptorPanel);
        }

        public DockPanel CreateChartDescriptorPanel()
        {
            var dateTimeFromLabel = new Label {Content = "DateTime from:"};
            var dateTimePickerFrom = new DateTimePicker
            {
                DataContext = _chartDescriptor.DateTimeFrom,
                Value = _chartDescriptor.DateTimeFrom
            };
            dateTimePickerFrom.SetBinding(InputBase.TextProperty, new Binding("DateTimeFrom")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            dateTimePickerFrom.ValueChanged += delegate
            {
                if (dateTimePickerFrom.Value != null)
                    _chartDescriptor.DateTimeFrom = (DateTime) dateTimePickerFrom.Value;
            };

            var dateTimeToLabel = new Label {Content = "DateTime to:"};
            var dateTimePickerTo = new DateTimePicker
            {
                DataContext = _chartDescriptor.DateTimeTo,
                Value = _chartDescriptor.DateTimeFrom
            };
            dateTimePickerFrom.SetBinding(InputBase.TextProperty, new Binding("DateTimeTo")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            dateTimePickerTo.ValueChanged += delegate
            {
                if (dateTimePickerTo.Value != null)
                    _chartDescriptor.DateTimeTo = (DateTime) dateTimePickerTo.Value;
            };

            var numberTextBoxLabel = new Label {Content = "Number of points"};
            var numberTextBox = new TextBox
            {
                DataContext = _chartDescriptor.Number,
                Text = _chartDescriptor.Number.ToString(),
                Width = 40
            };
            numberTextBox.TextChanged += delegate
            {
                try
                {
                    _chartDescriptor.Number = Convert.ToInt32(numberTextBox.Text);
                }
                catch (Exception)
                {
                    // ignored
                }
            };

            var onlyCriticalCheckBoxLabel = new Label {Content = "Show only critical"};
            var onlyCriticalCheckBox = new CheckBox
            {
                DataContext = _chartDescriptor.OnlyCritical,
                VerticalAlignment = VerticalAlignment.Center
            };
            onlyCriticalCheckBox.Checked += delegate
            {
                if ((onlyCriticalCheckBox.IsChecked != null) && onlyCriticalCheckBox.IsChecked.Value)
                    _chartDescriptor.OnlyCritical = true;
            };
            onlyCriticalCheckBox.Unchecked += delegate
            {
                if ((onlyCriticalCheckBox.IsChecked != null) && !onlyCriticalCheckBox.IsChecked.Value)
                    _chartDescriptor.OnlyCritical = false;
            };

            var chartDescriptorPanel = new DockPanel
            {
                Name = _chartDescriptor.MeasurableType,
                Margin = new Thickness(0, 10, 0, 0)
            };

            chartDescriptorPanel.Children.Add(dateTimeFromLabel);
            chartDescriptorPanel.Children.Add(dateTimePickerFrom);
            chartDescriptorPanel.Children.Add(dateTimeToLabel);
            chartDescriptorPanel.Children.Add(dateTimePickerTo);
            chartDescriptorPanel.Children.Add(numberTextBoxLabel);
            chartDescriptorPanel.Children.Add(numberTextBox);
            chartDescriptorPanel.Children.Add(onlyCriticalCheckBoxLabel);
            chartDescriptorPanel.Children.Add(onlyCriticalCheckBox);

            return chartDescriptorPanel;
        }
    }
}