using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Forms;
using System.Windows.Threading;
using PlantsWpf.DbDataAccessors;
using PlantsWpf.ObjectsViews;
using PlantsWpf.PdfDocuments;
using Button = System.Windows.Controls.Button;
using CheckBox = System.Windows.Controls.CheckBox;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Label = System.Windows.Controls.Label;
using Menu = System.Windows.Controls.Menu;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;

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
        private DispatcherTimer _dispatcherTimer;

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

            //_menu.Items.Add(ExportChartToPdfMenuItem());

            Button refreshButton = new Button
            {
                Content = "Refresh",
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 65
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
                if (_refreshLastMin == false && _autorefresh == false)
                {
                    SetChartDescriptor(Chart.Title.ToString(), _chartDescriptor.DateTimeFrom,
                        _chartDescriptor.DateTimeTo,
                        true);
                }
                areaSeries = Chart.Series[0] as AreaSeries;
                Chart.Dispatcher.BeginInvoke(DispatcherPriority.Background, backgroundWork);
            };

            SetDispatcherTimer(backgroundWork);

            Label refreshLastMinLabel = new Label {Content = "Refresh only last min"};
            CheckBox refreshLastMinCheckBox = new CheckBox
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            
            Label autorefreshLabel = new Label {Content = "Auto refresh"};
            CheckBox autorefreshCheckBox = new CheckBox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };

            refreshLastMinCheckBox.Checked += delegate
            {
                SetChartDescriptor(Chart.Title.ToString(), DateTime.Now.Subtract(new TimeSpan(0, 1, 0)), DateTime.Now,
                    false);

                _refreshLastMin = true;
                
                autorefreshCheckBox.IsEnabled = false;
            };
            refreshLastMinCheckBox.Unchecked += delegate
            {
                _refreshLastMin = false;
                
                autorefreshCheckBox.IsEnabled = true;
            };

            autorefreshCheckBox.Checked += delegate
            {
                _autorefresh = true;

                refreshLastMinCheckBox.IsEnabled = false;
                refreshButton.IsEnabled = false;

                _dispatcherTimer.Start();
            };
            autorefreshCheckBox.Unchecked += delegate
            {
                _autorefresh = false;

                refreshLastMinCheckBox.IsEnabled = true;
                refreshButton.IsEnabled = true;

                _dispatcherTimer.Stop();
            };
            DockPanel buttonsDockPanel = new DockPanel();
            buttonsDockPanel.Children.Add(refreshButton);
            buttonsDockPanel.Children.Add(refreshLastMinLabel);
            buttonsDockPanel.Children.Add(refreshLastMinCheckBox);
            buttonsDockPanel.Children.Add(autorefreshLabel);
            buttonsDockPanel.Children.Add(autorefreshCheckBox);
            _plantAreaChartsPanel.Children.Add(buttonsDockPanel);
        }

        public void SetDispatcherTimer(Delegate del)
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Tick += delegate
            {
                SetChartDescriptor(Chart.Title.ToString(), DateTime.Now.Subtract(new TimeSpan(0, 1, 0)),
                    DateTime.Now,
                    false);

                if (_autorefresh == false)
                {
                    return;
                }
                Chart.Dispatcher.BeginInvoke(DispatcherPriority.Background, del);
            };
        }

        public MenuItem ExportChartToPdfMenuItem()
        {
            MenuItem exportChartToPdfMenuItem = new MenuItem
            {
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Header = "To pdf",
            };
            exportChartToPdfMenuItem.Click += delegate 
            {              
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                saveFileDialog.FileOk += (sender, args) =>
                {
                    PdfDocumentCreator creator = new PdfDocumentCreator();
                    creator.CreatePdfDocument(Chart, saveFileDialog.FileName);
                    MessageBox.Show("Pdf document saved!");
                };
                saveFileDialog.ShowDialog();
            };
            return exportChartToPdfMenuItem;
        }
    }
}