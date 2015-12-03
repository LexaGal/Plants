using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Threading;
using PlantsWpf.DbDataAccessors;
using PlantsWpf.ObjectsViews;

namespace PlantsWpf.ControlsBuilders
{
    public class PlantAreaMenuBuilder
    {
        private readonly StackPanel _plantAreaSensorsPanel;
        private readonly StackPanel _plantAreaChartsPanel;
        private readonly Menu _menu;
        private readonly ChartDescriptor _chartDescriptor;
        private readonly DbMeasuringMessagesRetriever _dbMeasuringMessagesRetriever;
        private List<Chart> _charts;
        private bool _refreshLastMin;
        private bool _autorefresh;

        private Chart Chart => _charts?.Single(ch => ch.Title.ToString() == _chartDescriptor.MeasurableType);

        private IEnumerable<KeyValuePair<DateTime, double>> Statistics
            => _dbMeasuringMessagesRetriever.RetrieveMessagesStatistics(_chartDescriptor);

        private void SetChartDescriptor(string title, DateTime dateTimeFrom, DateTime dateTimeTo, bool refreshAll)
        {
            _chartDescriptor.MeasurableType = title;
            _chartDescriptor.DateTimeFrom = dateTimeFrom;
            _chartDescriptor.DateTimeTo = dateTimeTo;
            _chartDescriptor.RefreshAll = refreshAll;
        }

        private void RefreshControls(object sender, EventArgs eventArgs)
        {
            RebuildMenu();
        }

        public PlantAreaMenuBuilder(StackPanel plantAreaSensorsPanel, StackPanel plantAreaChartsPanel,
            Menu menu, IControlsRefresher controlsRefresher, DbMeasuringMessagesRetriever dbMeasuringMessagesRetriever,
            ChartDescriptor chartDescriptor)
        {
            controlsRefresher.RefreshControl += RefreshControls;
            _plantAreaSensorsPanel = plantAreaSensorsPanel;
            _plantAreaChartsPanel = plantAreaChartsPanel;
            _menu = menu;
            _chartDescriptor = chartDescriptor;
            _dbMeasuringMessagesRetriever = dbMeasuringMessagesRetriever;
            _refreshLastMin = false;
            _autorefresh = false;
        }

        public void RebuildMenu()
        {
            _menu.Items.Clear();
            _menu.HorizontalAlignment = HorizontalAlignment.Left;
            _menu.VerticalAlignment = VerticalAlignment.Top;

            _charts = _plantAreaChartsPanel.Children.OfType<Chart>().ToList();

            BackgroundWorker worker = new BackgroundWorker();
            AreaSeries areaSeries = Chart.Series[0] as AreaSeries;
            IEnumerable<KeyValuePair<DateTime, double>> statistics = new List<KeyValuePair<DateTime, double>>();

            MenuItem menuItemCharts = new MenuItem
            {
                Header = "Charts"
            };
            _menu.Items.Add(menuItemCharts);

            foreach (Chart chart in _charts)
            {
                MenuItem menuItemChart = new MenuItem
                {
                    Header = chart.Title
                };
                menuItemChart.Click += (sender, args) =>
                {
                    _plantAreaSensorsPanel.Visibility = Visibility.Collapsed;
                    _plantAreaChartsPanel.Visibility = Visibility.Visible;

                    _charts.ForEach(c => c.Visibility = Visibility.Collapsed);
                    chart.Visibility = Visibility.Visible;

                    _chartDescriptor.MeasurableType = chart.Title.ToString();
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
                Width = 60
            };

            Action backgroundWork = delegate
            {
                refreshButton.IsEnabled = false;
                refreshButton.Content = "Refreshing";

                worker.DoWork += delegate
                {
                    statistics = Statistics;
                };

                worker.RunWorkerCompleted += delegate
                {
                    refreshButton.IsEnabled = true;
                    if (areaSeries != null)
                    {
                        areaSeries.ItemsSource = statistics;
                        refreshButton.Content = "Refresh";
                    }
                };
                if (!worker.IsBusy)
                {
                    worker.RunWorkerAsync();
                }
            };

            refreshButton.Click += delegate
            {
                if (_refreshLastMin == false)
                {
                    SetChartDescriptor(Chart.Title.ToString(), _chartDescriptor.DateTimeFrom,
                        _chartDescriptor.DateTimeTo,
                        true);
                }
                areaSeries = Chart.Series[0] as AreaSeries;
                Chart.Dispatcher.BeginInvoke(DispatcherPriority.Background, backgroundWork);
            };

            Label refreshLastMinLabel = new Label { Content = "Refresh last min" };
            CheckBox refreshLastMinCheckBox = new CheckBox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };
            refreshLastMinCheckBox.Checked += delegate
            {
                SetChartDescriptor(Chart.Title.ToString(), DateTime.Now.Subtract(new TimeSpan(0, 1, 0)), DateTime.Now,
                    false);

                _refreshLastMin = true;
            };
            refreshLastMinCheckBox.Unchecked += delegate
            {
                _refreshLastMin = false;
            };

            Label autorefreshLabel = new Label {Content = "Autorefresh" };
            CheckBox autorefreshCheckBox = new CheckBox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };
            autorefreshCheckBox.Checked += delegate
            {
                _refreshLastMin = true;
                _autorefresh = true;

                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Tick += delegate
                {
                    SetChartDescriptor(Chart.Title.ToString(), DateTime.Now.Subtract(new TimeSpan(0, 1, 0)),
                        DateTime.Now,
                        false);

                    if (_autorefresh == false)
                    {
                        return;
                    }
                    Chart.Dispatcher.BeginInvoke(DispatcherPriority.Background, backgroundWork);
                };
                dispatcherTimer.Start();

            };
            autorefreshCheckBox.Unchecked += delegate
            {
                _autorefresh = false;
            };
            DockPanel buttonsDockPanel = new DockPanel();
            buttonsDockPanel.Children.Add(refreshButton);
            buttonsDockPanel.Children.Add(refreshLastMinLabel);
            buttonsDockPanel.Children.Add(refreshLastMinCheckBox);
            buttonsDockPanel.Children.Add(autorefreshLabel);
            buttonsDockPanel.Children.Add(autorefreshCheckBox);
            _plantAreaChartsPanel.Children.Add(buttonsDockPanel);
        }
    }
}