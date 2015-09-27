using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using PlantingLib.MeasurableParameters;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Messenging;
using PlantingLib.Observation;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using PlantingLib.ServiceSystems;
using PlantingLib.Timers;
using PlantingLib.WeatherTypes;
using PlantsWpf.DataGridBuilders;

namespace PlantsWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private SensorsCollection _sensorsCollection;
        private Observer _observer;
        private PlantsAreas _plantsAreas;
        private SensorsMeasuringsProvider _sensorsMeasuringsProvider;
        private ServiceProvider _serviceProvider;
        private DbMapper _dbMapper;
        private DateTime _beginDateTime;
        private DispatcherTimer _dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();

            WeatherBox.Items.Add(WeatherTypesEnum.Warm);
            WeatherBox.Items.Add(WeatherTypesEnum.Cold);
            WeatherBox.Items.Add(WeatherTypesEnum.Hot);
            WeatherBox.Items.Add(WeatherTypesEnum.Rainy);

            WeatherBox.SelectionChanged += WeatherBox_OnSelectionChanged;
        }

        private void Send(object sender, ElapsedEventArgs args)
        {
            if (_sensorsMeasuringsProvider != null)
            {
                TimeSpan timeSpan = args.SignalTime.Subtract(_beginDateTime);

                if (timeSpan.TotalSeconds > SystemTimer.RestartTimeSpan.TotalSeconds)
                {
                    _beginDateTime = _beginDateTime.Add(SystemTimer.RestartTimeSpan);

                    timeSpan = new TimeSpan(0, 0, (int) (timeSpan.TotalSeconds%SystemTimer.RestartTimeSpan.TotalSeconds));

                    //restarting timer and reseting all functions values to base values (new day after night sleep)
                    SystemTimer.Restart();
                    _sensorsCollection.AllSensors.ToList().ForEach(s => s.Function.ResetFunction());
                }

                SystemTimer.CurrentTimeSpan = timeSpan;
                _sensorsMeasuringsProvider.SendMessages(timeSpan);
            }
        }

        public void Initialize()
        {
            IPlantMappingRepository plantRepository = new PlantMappingRepository();
            IPlantsAreaMappingRepository plantsAreaRepository = new PlantsAreaMappingRepository();
            IMeasurableParameterMappingRepository measurableParameterRepository =
                new MeasurableParameterMappingRepository();
            ISensorMappingRepository sensorRepository = new SensorMappingRepository();

            _dbMapper = new DbMapper(plantRepository, plantsAreaRepository,
                measurableParameterRepository);

            List<SensorMapping> sensorMappings = sensorRepository.GetAll().ToList();
            _sensorsCollection = new SensorsCollection();
            sensorMappings.ForEach(m => _sensorsCollection.AddSensor(_dbMapper.RestoreSensor(m)));

            _plantsAreas = new PlantsAreas();

            _sensorsCollection.AllSensors.ToList().ForEach(s =>
            {
                _plantsAreas.AddPlantsArea(s.PlantsArea);
            });

            foreach (var area in _plantsAreas.AllPlantsAreas)
            {
                foreach (var sensor in _sensorsCollection.AllSensors)
                {
                    if (sensor.PlantsArea.Id == area.Id)
                    {
                        area.AddSensor(sensor);
                    }
                }
            }

            _sensorsMeasuringsProvider = new SensorsMeasuringsProvider(_sensorsCollection);

            _plantsAreas =
                new PlantsAreas(_plantsAreas.AllPlantsAreas.Distinct(new PlantsAreaEqualityComparer()).ToList());

            _observer = new Observer(_sensorsMeasuringsProvider, _plantsAreas);

            _serviceProvider = new ServiceProvider(_observer, _plantsAreas);

            _beginDateTime = DateTime.Now;

            Weather.SetWeather(WeatherTypesEnum.Warm);
        }

        private void SetPlantsGrid()
        {
            PlantsGrid.Children.Clear();
            int n = _plantsAreas.AllPlantsAreas.Count;
            int marginLeft = 20;
            int marginTop = 50;

            for (int index = 0; index < _plantsAreas.AllPlantsAreas.Count; index++)
            {
                PlantsArea area = _plantsAreas.AllPlantsAreas[index];
                GroupBox groupBox = new GroupBox
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Name = string.Format("{0}Area", area.Plant.Name),
                    Header = string.Format("{0} area", area.Plant.Name),
                    Width = 325,
                    Height = 250,
                    Margin = new Thickness(marginLeft, marginTop, 0, 0)
                };

                marginLeft += 325;

                if ((index + 1)%4 == 0)
                {
                    marginLeft = 0;
                    marginTop += 250;
                }

                DataGridBuilder builder = new DataGridBuilder();
                StackPanel stackPanel = new StackPanel();
                stackPanel.Children.Add(builder.CreateSensorsDataGrid(area, DataGrid_LoadingRow));
                stackPanel.Children.Add(builder.CreateServiceSystemsDataGrid(area, DataGrid_LoadingRow));
                stackPanel.CanHorizontallyScroll = true;
                groupBox.Content = stackPanel;

                PlantsGrid.Children.Add(groupBox);
            }
        }

        private void SetDispatcherTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        private void WeatherBox_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            WeatherTypesEnum weatherTypesEnum =
                (WeatherTypesEnum)
                    Enum.Parse(typeof (WeatherTypesEnum), selectionChangedEventArgs.AddedItems[0].ToString());
            Weather.SetWeather(weatherTypesEnum);
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            SetDispatcherTimer();
            SystemTimer.Start(Send, new TimeSpan(0, 0, 0, 0, 1000));
            Start.IsEnabled = false;
            Pause.IsEnabled = true;
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            SystemTimer.Enable();
            _dispatcherTimer.IsEnabled = true;
            Start.IsEnabled = false;
            Pause.IsEnabled = true;
        }

        private void Pause_OnClick(object sender, RoutedEventArgs e)
        {
            SystemTimer.Disable();
            _dispatcherTimer.IsEnabled = false;
            Start.IsEnabled = true;
            Pause.IsEnabled = false;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            SetPlantsGrid();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            string row = e.Row.Item.ToString();

            if (row.Contains("Yes"))
            {
                e.Row.Background = new SolidColorBrush(Colors.Red);
            }
            if (row.Contains("True"))
            {
                e.Row.Background = new SolidColorBrush(Colors.LawnGreen);
            }
        }
    }
}
