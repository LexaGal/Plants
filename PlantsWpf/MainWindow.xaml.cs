using System;
using System.Collections.Generic;
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
using PlantingLib.MeasurableParameters;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Observation;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;
using PlantingLib.ServiceSystems;
using PlantingLib.Timers;
using PlantingLib.WeatherTypes;
using PlantsWpf.ArgsForEvents;
using PlantsWpf.ControlsBuilders;
using PlantsWpf.DataGridObjects;
using PlantsWpf.SavingData;
using MessageBox = System.Windows.Forms.MessageBox;

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
            
            MeasurableParametersInfo.SetBaseParameters();

            Initialize();
            
            foreach (string name in Enum.GetNames(typeof (WeatherTypesEnum)))
            {
                WeatherBox.Items.Add(name);
            }
            WeatherBox.Text = WeatherBox.Items[0].ToString();

            WeatherBox.SelectionChanged += WeatherBox_OnSelectionChanged;

            Pause.IsEnabled = false;
            Start.IsEnabled = false;
            SetPlantsGrid(3);

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
                    _sensorsCollection.AllSensors.ToList().ForEach(s => s.Function.ResetFunction(s.MeasurableParameter.Optimal));
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
            IServiceScheduleMappingRepository serviceScheduleMappingRepository = new ServiceScheduleMappingRepository();
            
            _dbMapper = new DbMapper(plantRepository, plantsAreaRepository,
                measurableParameterRepository, serviceScheduleMappingRepository);

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
                //if custom sensor
                foreach (Sensor source in area.Sensors.Where(s => s.IsCustom))
                {
                    ServiceState serviceState = new ServiceState(source.MeasurableType, true);
                    area.PlantsAreaServiceState.AddServiceState(serviceState);

                    if (MeasurableParametersInfo.GetParameterInfo(source.MeasurableType) == null)
                    {
                        MeasurableParametersInfo.ParametersInfo.Add(new ParameterInfo(source.MeasurableType,
                            new List<ServiceState> { serviceState }));
                    }
                }
            }

            _sensorsMeasuringsProvider = new SensorsMeasuringsProvider(_sensorsCollection);

            _plantsAreas =
                new PlantsAreas(_plantsAreas.AllPlantsAreas.Distinct(new PlantsAreaEqualityComparer()).ToList());

            //foreach (var area in _plantsAreas.AllPlantsAreas)
            //{
            //    ServiceScheduleMapping serviceScheduleMapping1 = new ServiceScheduleMapping(Guid.NewGuid(), area.Id,
            //        ServiceStateEnum.Nutrienting.ToString(), 3, 15,
            //        String.Format("{0},{1}", area.Plant.Nutrient.Id, area.Plant.SoilPh.Id));

            //    ServiceScheduleMapping serviceScheduleMapping2 = new ServiceScheduleMapping(Guid.NewGuid(), area.Id,
            //        ServiceStateEnum.Watering.ToString(), 2, 10,
            //        String.Format("{0},{1}", area.Plant.Humidity.Id, area.Plant.Temperature.Id));

            //    serviceScheduleMappingRepository.Add(serviceScheduleMapping1);
            //    serviceScheduleMappingRepository.Add(serviceScheduleMapping2);
            //}

            _observer = new Observer(_sensorsMeasuringsProvider, _plantsAreas);

            _serviceProvider = new ServiceProvider(_observer, _plantsAreas);

            _beginDateTime = DateTime.Now;

            Weather.SetWeather(WeatherTypesEnum.Warm);

            _dbModifier = new DbModifier(_plantsAreas, _sensorsCollection, measurableParameterRepository,
                plantRepository, sensorRepository, plantsAreaRepository, serviceScheduleMappingRepository);
        }

        private void SetPlantsGrid(int numberInRow)
        {
            const int sizeHorizontal = 1352;
            const int sizeVertical = 410;
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

                    marginLeft += sizeHorizontal/numberInRow;
                    if ((index + 1)%numberInRow == 0)
                    {
                        marginLeft = 10;
                        marginTop += sizeVertical;
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show(e.StackTrace);
            }
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
                Width = 440,
                Height = 400,
                CanVerticallyScroll = true
            };

            plantAreaPanel.Children.Add(new Label
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = area.ToString()
            });

            BindingList<DataGridSensorView> dataGridSensorViews = new BindingList<DataGridSensorView>(
                area.Sensors.ToList().ConvertAll(s => new DataGridSensorView(s)));

            BindingList<DataGridSensorToAddView> dataGridSensorToAddViews =
                new BindingList<DataGridSensorToAddView>(
                    area.FindMainSensorsToAdd().ConvertAll(s => new DataGridSensorToAddView(s))) {AllowNew = true};

            BindingList<DataGridServiceScheduleView> dataGridServiceScheduleViews = new BindingList<DataGridServiceScheduleView>(
                area.ServicesSchedulesState.ServicesSchedules.ToList().ConvertAll(s => new DataGridServiceScheduleView(s))) {AllowNew = true, AllowEdit = true};

            StackPanel buttonsPanel = controlsBuilder.CreateButtonsPanel(area, plantAreaPanel, sensorsToAddDataGrid,
                dataGridSensorToAddViews, SaveSensor, dataGridSensorViews);

            FrameworkElementFactory removeSensorsButtonTemplate = controlsBuilder.CreateRemoveSensorButtonTemplate(area,
                dataGridSensorViews, RemoveSensor, dataGridSensorToAddViews);

            DataGrid sensorViewsDataGrid = dataGridsBuilder.CreateSensorsDataGrid(area, dataGridSensorViews,
                removeSensorsButtonTemplate);
            
            DataGrid serviceStatesDataGrid = dataGridsBuilder.CreateServiceSystemsDataGrid(area);

            FrameworkElementFactory serviceScheduleSetUpButtonTemplate = controlsBuilder.CreateServiceScheduleSetUpButtonTemplate(area,
                dataGridServiceScheduleViews, SaveServiceSchedule);

            DataGrid serviceSchedulesDataGrid = dataGridsBuilder.CreateServicesSchedulesDataGrid(area,
                dataGridServiceScheduleViews, serviceScheduleSetUpButtonTemplate);
            
            Button removePlantsAreaButton = controlsBuilder.CreateRemovePlantsAreaButton(RemovePlantsArea, area);

            plantAreaPanel.Children.Add(removePlantsAreaButton);
            plantAreaPanel.Children.Add(sensorViewsDataGrid);
            plantAreaPanel.Children.Add(serviceStatesDataGrid);
            plantAreaPanel.Children.Add(serviceSchedulesDataGrid);
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
                Background = Brushes.LightBlue,
                BorderThickness = new Thickness(3),
                Width = plantAreaPanel.Width,
                Height = plantAreaPanel.Height,
                Margin = new Thickness(marginLeft, marginTop, 0, 0),
                Child = scrollViewer
            };
            return border;
        }

        private void SaveServiceSchedule(PlantsArea area, ServiceSchedule serviceSchedule)
        {
            _dbModifier.SaveServiceSchedule(area, serviceSchedule);
        }

        private void SaveSensor(PlantsArea area, Sensor sensor)
        {
            _dbModifier.SaveAddedSensor(area, sensor);
        }

        private void RemoveSensor(PlantsArea area, Sensor sensor)
        {
            _dbModifier.RemoveSensor(area, sensor);
        }
        
        private void RemovePlantsArea(PlantsArea area)
        {
            _dbModifier.RemovePlantsArea(area);
            SetPlantsGrid(3);
        }
        
        private void SavePlantsArea(PlantsArea plantsArea)
        {
            _dbModifier.SaveAddedPlantsArea(plantsArea);
            SetPlantsGrid(3);
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
