using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using Database.DatabaseStructure.Repository.Concrete;
using PlantingLib.Plants;
using PlantsWpf.DbDataAccessors;
using PlantsWpf.ObjectsViews;

namespace PlantsWpf.ControlsBuilders
{
    public class PlantAreaMenuBuilder
    {
        private readonly PlantsArea _area;
        private readonly StackPanel _plantAreaSensorsPanel;
        private readonly StackPanel _plantAreaChartsPanel;
        private readonly Menu _menu;
        private readonly ChartDescriptor _chartDescriptor;
        private readonly DbMeasuringMessagesRetriever _dbMeasuringMessagesRetriever;

        private void RefreshControls(object sender, EventArgs eventArgs)
        {
            RebuildMenu();
        }

        public PlantAreaMenuBuilder(PlantsArea area, StackPanel plantAreaSensorsPanel, StackPanel plantAreaChartsPanel,
            Menu menu, IControlsRefresher controlsRefresher, ChartDescriptor chartDescriptor)
        {
            controlsRefresher.RefreshControl += RefreshControls;
            _area = area;
            _plantAreaSensorsPanel = plantAreaSensorsPanel;
            _plantAreaChartsPanel = plantAreaChartsPanel;
            _menu = menu;
            _chartDescriptor = chartDescriptor;
            _dbMeasuringMessagesRetriever = new DbMeasuringMessagesRetriever(new MeasuringMessageMappingRepository());
        }

        public void RebuildMenu()
        {
            _menu.Items.Clear();
            _menu.HorizontalAlignment = HorizontalAlignment.Left;
            _menu.VerticalAlignment = VerticalAlignment.Top;

            List<Chart> charts = _plantAreaChartsPanel.Children.OfType<Chart>().ToList();

            MenuItem menuItemCharts = new MenuItem
            {
                Header = "Charts"
            };
            _menu.Items.Add(menuItemCharts);

            foreach (Chart chart in charts)
            {
                MenuItem menuItemChart = new MenuItem
                {
                    Header = chart.Title
                };
                menuItemChart.Click += (sender, args) =>
                {
                    _plantAreaSensorsPanel.Visibility = Visibility.Collapsed;
                    _plantAreaChartsPanel.Visibility = Visibility.Visible;

                    charts.ForEach(c => c.Visibility = Visibility.Collapsed);
                    chart.Visibility = Visibility.Visible;

                    _chartDescriptor.MeasurableType = chart.Title.ToString();

                    IEnumerable<KeyValuePair<DateTime, double>> statistics =
                        _dbMeasuringMessagesRetriever.RetrieveMessagesStatistics(_chartDescriptor);

                    AreaSeries areaSeries = chart.Series[0] as AreaSeries;
                    if (areaSeries != null)
                    {
                        areaSeries.ItemsSource = statistics;
                    }
                };
                menuItemCharts.Items.Add(menuItemChart);
            }

            MenuItem menuItemSensors = new MenuItem
            {
                Header = "Sensors"
            };
            menuItemSensors.Click += (sender, args) =>
            {
                _plantAreaSensorsPanel.Visibility = Visibility.Visible;
                _plantAreaChartsPanel.Visibility = Visibility.Collapsed;
            };
            _menu.Items.Add(menuItemSensors);

            Button refreshButton = new Button
            {
                Content = "Refresh",
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 50
            };
            refreshButton.Click += delegate
            {
                Chart chart = charts.Single(chart1 => chart1.Title.ToString() == _chartDescriptor.MeasurableType);
                _chartDescriptor.MeasurableType = chart.Title.ToString();

                IEnumerable<KeyValuePair<DateTime, double>> statistics =
                    _dbMeasuringMessagesRetriever.RetrieveMessagesStatistics(_chartDescriptor);

                AreaSeries areaSeries = chart.Series[0] as AreaSeries;
                if (areaSeries != null)
                {
                    areaSeries.ItemsSource = statistics;
                }
            };
            _plantAreaChartsPanel.Children.Add(refreshButton);

        }
    }
}