using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Observation;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using PlantingLib.ServiceSystems;
using PlantingLib.Timers;
using PlantingLib.WeatherTypes;
using PlantsWpf.ArgsForEvents;
using PlantsWpf.ControlsBuilders;
using PlantsWpf.DataGridObjects;
using PlantsWpf.SavingData;

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
        private DbModifier _dbModifier;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            foreach (string name in Enum.GetNames(typeof (WeatherTypesEnum)))
            {
                WeatherBox.Items.Add(name);
            }
            WeatherBox.Text = WeatherBox.Items[0].ToString();

            WeatherBox.SelectionChanged += WeatherBox_OnSelectionChanged;

            Pause.IsEnabled = false;
            Start.IsEnabled = false;
            SetPlantsGrid();

            SystemTimer.Start(Send, new TimeSpan(0, 0, 0, 0, 1000));
            Start.IsEnabled = false;
            Pause.IsEnabled = true;
            WindowState = WindowState.Maximized;
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

            //_sensorsCollection.AddSensor(_dbMapper.RestoreSensor(sensorMappings[1]));
            //_sensorsCollection = new SensorsCollection();
            
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

            _dbModifier = new DbModifier(_plantsAreas, _sensorsCollection);
        }

        private void SetPlantsGrid()
        {
            try
            {
                PlantsGrid.Children.Clear();
                
                int marginLeft = 10;
                int marginTop = 10;
                
                for (int index = 0; index < _plantsAreas.AllPlantsAreas.Count; index++)
                {
                    PlantsArea area = _plantsAreas.AllPlantsAreas[index];

                    Border borderedPlantAreaPanel = CreateBorderedPlantAreaPanel(area, marginLeft, marginTop);

                    PlantsGrid.Children.Add(borderedPlantAreaPanel);

                    marginLeft += 335;

                    if ((index + 1)%4 == 0)
                    {
                        marginLeft = 10;
                        marginTop += 250;
                    }
                }
            }
            catch (InvalidOperationException)
            {}
        }

        private Border CreateBorderedPlantAreaPanel(PlantsArea area, int marginLeft, int marginTop)
        {
            DataGrid sensorsToAddDataGrid = new DataGrid();
            DataGridsBuilder dataGridsBuilder = new DataGridsBuilder();
            ControlsBuilder controlsBuilder = new ControlsBuilder();

            StackPanel plantAreaPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Vertical,
                Width = 333,
                Height = 240,
                CanVerticallyScroll = true
            };

            plantAreaPanel.Children.Add(new Label
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Content = string.Format("{0} (plant id: {1})", area.Plant.Name, area.Plant.Id)
            });
            
            BindingList<DataGridSensorView> dataGridSensorViews = new BindingList<DataGridSensorView>(
                area.Sensors.ToList().ConvertAll(s => new DataGridSensorView(s)));
            BindingList<PlantsAreaServiceState> plantsAreaServiceStates =
                new BindingList<PlantsAreaServiceState> {area.PlantsAreaServiceState};
            ObservableCollection<DataGridSensorToAddView> dataGridSensorToAddViews = new ObservableCollection
                <DataGridSensorToAddView>(area.FindSensorsToAdd().ConvertAll(s => new DataGridSensorToAddView(s)));
          
            StackPanel buttonsPanel = controlsBuilder.CreateButtonsPanel(area, plantAreaPanel, sensorsToAddDataGrid,
                dataGridSensorToAddViews, SaveSensor, dataGridSensorViews);

            FrameworkElementFactory buttonTemplate = controlsBuilder.CreateButtonTemplate(area, dataGridSensorViews,
                RemoveSensor, dataGridSensorToAddViews, buttonsPanel.Children[0] as Button);
          
            DataGrid sensorsDataGrid = dataGridsBuilder.CreateSensorsDataGrid(area, dataGridSensorViews, buttonTemplate);
            DataGrid serviceSystemsDataGrid = dataGridsBuilder.CreateServiceSystemsDataGrid(area, plantsAreaServiceStates);
           
            plantAreaPanel.Children.Add(sensorsDataGrid);
            plantAreaPanel.Children.Add(serviceSystemsDataGrid);
            plantAreaPanel.Children.Add(buttonsPanel);

            ScrollViewer scrollViewer = new ScrollViewer
            {
                Height = plantAreaPanel.Height,
                CanContentScroll = true,
                Content = plantAreaPanel,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            
            Border border = new Border
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                BorderBrush = Brushes.Black,
                Background = Brushes.LightGoldenrodYellow,
                BorderThickness = new Thickness(1),
                Width = plantAreaPanel.Width,
                Height = plantAreaPanel.Height,
                Margin = new Thickness(marginLeft, marginTop, 0, 0),
                Child = scrollViewer
            };
            return border;
        }

        private void SaveSensor(PlantsArea area, Sensor sensor)
        {
            _dbModifier.SaveSensor(area, sensor);
        }

        private void RemoveSensor(PlantsArea area, Sensor sensor)
        {
            _dbModifier.RemoveSensor(area, sensor);
        }

        private void SavePlantsArea(PlantsArea plantsArea)
        {
            _dbModifier.SavePlantsArea(plantsArea);
            SetPlantsGrid();
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            SystemTimer.Enable();
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
            Start.IsEnabled = true;
            Pause.IsEnabled = false;
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
