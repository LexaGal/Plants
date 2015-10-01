using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using PlantingLib.MeasurableParameters;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Observation;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using PlantingLib.ServiceSystems;
using PlantingLib.Timers;
using PlantingLib.WeatherTypes;
using PlantsWpf.ArgsForEvents;
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
            foreach (string name in Enum.GetNames(typeof(WeatherTypesEnum)))
            {
                WeatherBox.Items.Add(name);
            }
            WeatherBox.Text = WeatherBox.Items[0].ToString();
            
            WeatherBox.SelectionChanged += WeatherBox_OnSelectionChanged;

            Pause.IsEnabled = false;
            Start.IsEnabled = false;
        }

        private void Send(object sender, ElapsedEventArgs args)
        {
            if (_sensorsMeasuringsProvider != null)
            {
                TimeSpan timeSpan = args.SignalTime.Subtract(_beginDateTime);

                if (timeSpan.TotalSeconds > SystemTimer.RestartTimeSpan.TotalSeconds)
                {
                    _beginDateTime = _beginDateTime.Add(SystemTimer.RestartTimeSpan);

                    timeSpan = new TimeSpan(0, 0, (int)(timeSpan.TotalSeconds % SystemTimer.RestartTimeSpan.TotalSeconds));

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
            
            foreach (PlantsArea area in _plantsAreas.AllPlantsAreas)
            {
                foreach (Sensor sensor in _sensorsCollection.AllSensors)
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
            int marginLeft = 10;
            int marginTop = 10;

            for (int index = 0; index < _plantsAreas.AllPlantsAreas.Count; index++)
            {
                PlantsArea area = _plantsAreas.AllPlantsAreas[index];
                Panel bigPanel = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Orientation = Orientation.Vertical,
                    Name = string.Format("{0}Area", area.Plant.Name),
                    Width = 325,
                    Height = 250,
                    CanVerticallyScroll = true,
                };
                bigPanel.Children.Add(new Label
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Content = area.Plant.Name + " area"
                });
                
                DataGridBuilder builder = new DataGridBuilder();
                bigPanel.Children.Add(builder.CreateSensorsDataGrid(area, DataGrid_LoadingRow));
                bigPanel.Children.Add(builder.CreateServiceSystemsDataGrid(area, DataGrid_LoadingRow));
                
                Button addSensorsButton = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Name = "AddSensors",
                    Content = "+ Sensors",
                    Width = 60,
                    Height = 30
                };
                addSensorsButton.Click += (sender, args) =>
                {
                    DataGridBuilder dataGridBuilder = new DataGridBuilder();
                    DataGrid dataGrid = dataGridBuilder.CreateTurnedOffSensorsDataGrid(area, SaveAddedSensor);
                    bigPanel.Children.Add(dataGrid);
                };

                bigPanel.Children.Add(addSensorsButton);
                
                ScrollViewer scrollViewer = new ScrollViewer
                {
                    Height = bigPanel.Height,
                    CanContentScroll = true,
                    Content = bigPanel,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                };
                Border border = new Border
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    BorderBrush = Brushes.Black,
                    Background = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    Width = bigPanel.Width,
                    Height = bigPanel.Height,
                    Margin = new Thickness(marginLeft, marginTop, 0, 0),
                    Child = scrollViewer
                };

                PlantsGrid.Children.Add(border);

                marginLeft += 335;

                if ((index + 1) % 4 == 0)
                {
                    marginLeft = 10;
                    marginTop += 260;
                }
            }
        }

        private void SaveAddedSensor(PlantsArea area, Sensor sensor)
        {
            SensorMapping sensorMapping = _dbMapper.GetSensorMapping(sensor);
            ISensorMappingRepository sensorMappingRepository = new SensorMappingRepository();
            sensorMappingRepository.Add(sensorMapping);
            _sensorsCollection.AddSensor(sensor);
        }

        private void SetDispatcherTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        private void SavePlantsArea(PlantsArea plantsArea)
        {
            DbMapper dbMapper = new DbMapper();

            IPlantMappingRepository plantMappingRepository = new PlantMappingRepository();
            IPlantsAreaMappingRepository plantsAreaMappingRepository = new PlantsAreaMappingRepository();
            IMeasurableParameterMappingRepository measurableParameterMappingRepository =
                new MeasurableParameterMappingRepository();
            ISensorMappingRepository sensorMappingRepository = new SensorMappingRepository();

            Temperature temperature = plantsArea.Plant.Temperature;
            MeasurableParameterMapping measurableParameterMapping = dbMapper.GetMeasurableParameterMapping(temperature);
            measurableParameterMappingRepository.Add(measurableParameterMapping);

            Humidity humidity = plantsArea.Plant.Humidity;
            measurableParameterMapping = dbMapper.GetMeasurableParameterMapping(humidity);
            measurableParameterMappingRepository.Add(measurableParameterMapping);

            SoilPh soilPh = plantsArea.Plant.SoilPh;
            measurableParameterMapping = dbMapper.GetMeasurableParameterMapping(soilPh);
            measurableParameterMappingRepository.Add(measurableParameterMapping);

            Nutrient nutrient = plantsArea.Plant.Nutrient;
            measurableParameterMapping = dbMapper.GetMeasurableParameterMapping(nutrient);
            measurableParameterMappingRepository.Add(measurableParameterMapping);

            Plant plant = plantsArea.Plant;
            PlantMapping plantMapping = dbMapper.GetPlantMapping(plant);
            plantMappingRepository.Add(plantMapping);
            
            PlantsAreaMapping plantsAreaMapping = dbMapper.GetPlantsAreaMapping(plantsArea);
            plantsAreaMappingRepository.Add(plantsAreaMapping);

            foreach (var sensor in plantsArea.Sensors)
            {
                SensorMapping sensorMapping = dbMapper.GetSensorMapping(sensor);
                sensorMappingRepository.Add(sensorMapping);
                _sensorsCollection.AddSensor(sensor);
            }

            _plantsAreas.AddPlantsArea(plantsArea);
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            SetDispatcherTimer();
            SystemTimer.Start(Send, new TimeSpan(0, 0, 0, 0, 1000));
            Start.IsEnabled = false;
            Pause.IsEnabled = true;
            Load.IsEnabled = false;
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            SystemTimer.Enable();
            _dispatcherTimer.IsEnabled = true;
            Start.IsEnabled = false;
            Pause.IsEnabled = true;
        }

        private void WeatherBox_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            WeatherTypesEnum weatherTypesEnum =
                (WeatherTypesEnum)
                    Enum.Parse(typeof(WeatherTypesEnum), selectionChangedEventArgs.AddedItems[0].ToString());
            Weather.SetWeather(weatherTypesEnum);
        }

        private void Pause_OnClick(object sender, RoutedEventArgs e)
        {
            SystemTimer.Disable();
            _dispatcherTimer.IsEnabled = false;
            Start.IsEnabled = true;
            Pause.IsEnabled = false;
        }

        private void DispatcherTimer_Tick(object sender, System.EventArgs e)
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

        private void AddArea_OnClick(object sender, RoutedEventArgs e)
        {
            PlantsAreaWindow secondWindow = new PlantsAreaWindow();
            secondWindow.PlantsAreaEvent += PlantsAreaWindow_GetPlantsArea;
            secondWindow.Show();
        }

        public void PlantsAreaWindow_GetPlantsArea(object sender, PlantsAreaEventArgs e)
        {
            PlantsArea plantsArea = e.PlantsArea;
            SavePlantsArea(plantsArea);
        }
    }
}
