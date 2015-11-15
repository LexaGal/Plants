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
        private readonly DockPanel _plantAreaChartsPanel;
        private readonly Menu _menu;
        private readonly DbMeasuringMessagesRetriever _dbMeasuringMessagesRetriever;

        private void RefreshControls(object sender, EventArgs eventArgs)
        {
            RebuildMenu();
        }

        public PlantAreaMenuBuilder(PlantsArea area, StackPanel plantAreaSensorsPanel, DockPanel plantAreaChartsPanel, Menu menu, IControlsRefresher controlsRefresher)
        {
            controlsRefresher.RefreshControl += RefreshControls;
            _area = area;
            _plantAreaSensorsPanel = plantAreaSensorsPanel;
            _plantAreaChartsPanel = plantAreaChartsPanel;
            _menu = menu;
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
            menuItemCharts.Click += (sender, args) =>
            {
                _plantAreaSensorsPanel.Visibility = Visibility.Collapsed;
                _plantAreaChartsPanel.Visibility = Visibility.Visible;

                ChartDescriptor chartDescriptor = new ChartDescriptor(_area.Id,
                    _area.Plant.Temperature.MeasurableType, 30, DateTime.Now.Subtract(new TimeSpan(0, 0, 30)),
                    DateTime.Now, false);

                IEnumerable<KeyValuePair<DateTime, double>> statistics =
                    _dbMeasuringMessagesRetriever.RetrieveMessagesStatistics(chartDescriptor);

                AreaSeries areaSeries = charts[0].Series[0] as AreaSeries;
                if (areaSeries != null)
                {
                    areaSeries.ItemsSource = statistics;
                }
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

                    ChartDescriptor chartDescriptor = new ChartDescriptor(_area.Id,
                        chart.Title.ToString(), 30, DateTime.Now.Subtract(new TimeSpan(0, 0, 30)),
                        DateTime.Now, false);

                    IEnumerable<KeyValuePair<DateTime, double>> statistics =
                        _dbMeasuringMessagesRetriever.RetrieveMessagesStatistics(chartDescriptor);

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
        }
    }
}