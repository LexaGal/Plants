using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Media;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;

namespace PlantsWpf.ControlsBuilders
{
    public class PlantAreaChartsPanelBuilder
    {
        private readonly List<MeasurableParameter> _measurableParameters;
        private readonly DockPanel _plantAreaChartsPanel;

        public PlantAreaChartsPanelBuilder(List<MeasurableParameter> measurableParameters,
            IControlsRefresher controlsRefresher, DockPanel plantAreaChartsPanel)
        {
            controlsRefresher.RefreshControl += RefreshControls;
            _measurableParameters = measurableParameters;
            _plantAreaChartsPanel = plantAreaChartsPanel;
        }

        private void RefreshControls(object sender, EventArgs eventArgs)
        {
            RebuildChartsPanel();
        }

        public void RebuildChartsPanel()
        {
            _plantAreaChartsPanel.Children.Clear();
            foreach (MeasurableParameter measurableParameter in _measurableParameters)
            {
                Chart chart = new Chart
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = 1200,
                    Height = 240,
                    Background = Brushes.Beige,
                    Title = measurableParameter.MeasurableType,
                };

                AreaSeries lineSeries = new AreaSeries
                {
                    IndependentValueBinding = new Binding("Key"),
                    DependentValueBinding = new Binding("Value"),
                    Title = measurableParameter.MeasurableType,
                };
                chart.Series.Add(lineSeries);
                _plantAreaChartsPanel.Children.Add(chart);
            }
        }
    }
}