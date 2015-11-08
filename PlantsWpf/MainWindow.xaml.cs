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
using PlantingLib.MeasuringsProviders;
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
        
            SetWeatherBox();
            WindowState = WindowState.Maximized;
            
            ParameterServicesInfo.SetBaseParameters();

            Initialize();

            _beginDateTime = DateTime.Now;

            Weather.SetWeather(WeatherTypesEnum.Warm);

            SetPlantsGrid(1);

            SystemTimer.Start(SendMessagesHandler, new TimeSpan(0, 0, 0, 0, 1000));
        }

        private void SetWeatherBox()
        {
            foreach (string name in Enum.GetNames(typeof(WeatherTypesEnum)))
            {
                WeatherBox.Items.Add(name);
            }
            WeatherBox.Text = WeatherBox.Items[0].ToString();

            WeatherBox.SelectionChanged += WeatherBox_OnSelectionChanged;
        }

        private void SendMessagesHandler(object sender, ElapsedEventArgs args)
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
                    _sensorsCollection.Sensors.ForEach(s => s.Function.ResetFunction(s.MeasurableParameter.Optimal));
                }

                SystemTimer.CurrentTimeSpan = timeSpan;
                _sensorsMeasuringsProvider.SendMessages(timeSpan);
            }
        }

        //private void SetServicesSchedules(IServiceScheduleMappingRepository serviceScheduleMappingRepository)
        //{   
        //    foreach (PlantsArea area in _plantsAreas.Areas)
        //    {
        //        ServiceScheduleMapping serviceScheduleMapping1 = new ServiceScheduleMapping(Guid.NewGuid(), area.Id,
        //            ServiceStateEnum.Nutrienting.ToString(), 3, 15,
        //            String.Format("{0},{1}", area.Plant.Nutrient.Id, area.Plant.SoilPh.Id));

        //        ServiceScheduleMapping serviceScheduleMapping2 = new ServiceScheduleMapping(Guid.NewGuid(), area.Id,
        //            ServiceStateEnum.Watering.ToString(), 2, 10,
        //            String.Format("{0},{1}", area.Plant.Humidity.Id, area.Plant.Temperature.Id));

        //        serviceScheduleMappingRepository.Save(serviceScheduleMapping1, serviceScheduleMapping1.Id);
        //        serviceScheduleMappingRepository.Save(serviceScheduleMapping2, serviceScheduleMapping2.Id);
        //    }
        //}

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

            List<PlantsAreaMapping> plantsAreaMappings = plantsAreaRepository.GetAll();

            _plantsAreas = new PlantsAreas();

            plantsAreaMappings.ForEach(p => _plantsAreas.AddPlantsArea(_dbMapper.RestorePlantArea(p)));
            
            _sensorsCollection = new SensorsCollection();

            foreach (PlantsArea area in _plantsAreas.Areas)
            {
                foreach (SensorMapping sensorMapping in sensorRepository.GetAll(sm => sm.PlantsAreaId == area.Id))
                {
                    Sensor sensor = _dbMapper.RestoreSensor(sensorMapping, area);
                    _sensorsCollection.AddSensor(sensor);
                    sensor.IsOn = true;
                }
            }
            
            foreach (PlantsArea area in _plantsAreas.Areas)
            {
                //if custom sensor
                foreach (Sensor source in area.Sensors.Where(s => s.IsCustom))
                {
                    ServiceState serviceState = new ServiceState(source.MeasurableType, true);
                    area.PlantServicesStates.AddServiceState(serviceState);
                }
            }

            _sensorsMeasuringsProvider = new SensorsMeasuringsProvider(_sensorsCollection);

            _observer = new Observer(_sensorsMeasuringsProvider, _plantsAreas);

            _serviceProvider = new ServiceProvider(_observer, _plantsAreas);

            _dbModifier = new DbModifier(_plantsAreas, _sensorsCollection, measurableParameterRepository,
                plantRepository, sensorRepository, plantsAreaRepository, serviceScheduleMappingRepository);
        }

        private void SetPlantsGrid(int numberInRow)
        {
            const int sizeHorizontal = 1330;
            const int sizeVertical = 260;
            try
            {
                PlantsGrid.Children.Clear();
                int marginLeft = 10;
                int marginTop = 10;

                for (int index = 0; index < _plantsAreas.Areas.Count; index++)
                {
                    PlantsArea area = _plantsAreas.Areas[index];

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
            DataGridsBuilder dataGridsBuilder = new DataGridsBuilder();
            ControlsBuilder controlsBuilder = new ControlsBuilder();

            StackPanel plantAreaPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 1330,
                Height = 250,
                CanVerticallyScroll = true
            };

            plantAreaPanel.Children.Add(new Label
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = area.ToString()
            });

            BindingList<DataGridSensorView> dataGridSensorViews = new BindingList<DataGridSensorView>(
                area.Sensors.OrderBy(s => s.IsCustom).ToList().ConvertAll(s => new DataGridSensorView(s)))
                    {
                        AllowNew = true,
                        AllowEdit = true,
                        AllowRemove = true
                    };

            BindingList<DataGridServiceScheduleView> dataGridServiceScheduleViews =
                new BindingList<DataGridServiceScheduleView>(
                    area.ServicesSchedulesStates.ServicesSchedules.ToList()
                        .ConvertAll(s => new DataGridServiceScheduleView(s)))
                        {
                            RaiseListChangedEvents = true,
                            AllowNew = true,
                            AllowRemove = true,
                            AllowEdit = true
                        };

            FrameworkElementFactory removeSensorButtonTemplate = controlsBuilder.CreateRemoveSensorButtonTemplate(area,
                dataGridSensorViews, dataGridServiceScheduleViews, RemoveSensor);

            FrameworkElementFactory sensorSaveButtonTemplate = controlsBuilder.CreateSensorSaveButtonTemplate(area,
                dataGridSensorViews, dataGridServiceScheduleViews, SaveSensor);

            FrameworkElementFactory onOffSensorButtonTemplate = controlsBuilder.CreateOnOffSensorButtonTemplate();
            
            DataGrid sensorViewsDataGrid = dataGridsBuilder.CreateSensorsDataGrid(area, dataGridSensorViews,
                removeSensorButtonTemplate, sensorSaveButtonTemplate, onOffSensorButtonTemplate);

            DataGrid serviceStatesDataGrid = dataGridsBuilder.CreateServiceSystemsDataGrid(area);

            FrameworkElementFactory serviceScheduleSaveButtonTemplate =
                controlsBuilder.CreateServiceScheduleSaveButtonTemplate(area,
                    dataGridServiceScheduleViews, SaveServiceSchedule);
            
            FrameworkElementFactory onOffServiceScheduleButtonTemplate =
                controlsBuilder.CreateOnOffServiceScheduleButtonTemplate();

            DataGrid serviceSchedulesDataGrid = dataGridsBuilder.CreateServicesSchedulesDataGrid(area,
                dataGridServiceScheduleViews, serviceScheduleSaveButtonTemplate, onOffServiceScheduleButtonTemplate);

            Button removePlantsAreaButton = controlsBuilder.CreateRemovePlantsAreaButton(RemovePlantsArea, area);

            plantAreaPanel.Children.Add(removePlantsAreaButton);
            plantAreaPanel.Children.Add(sensorViewsDataGrid);
            plantAreaPanel.Children.Add(serviceStatesDataGrid);
            plantAreaPanel.Children.Add(serviceSchedulesDataGrid);

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
                Background = Brushes.LightSteelBlue,
                BorderThickness = new Thickness(2),
                Width = plantAreaPanel.Width,
                Height = plantAreaPanel.Height,
                Margin = new Thickness(marginLeft, marginTop, 0, 0),
                Child = scrollViewer
            };
            return border;
        }

        private bool SaveServiceSchedule(PlantsArea area, ServiceSchedule serviceSchedule)
        {
            return _dbModifier.SaveServiceSchedule(area, serviceSchedule);
        }

        private bool SaveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            return _dbModifier.SaveSensor(area, sensor, serviceSchedule);
        }

        private bool AddPlantsArea(PlantsArea plantsArea)
        {
            if (_dbModifier.AddPlantsArea(plantsArea))
            {
                SetPlantsGrid(1);
                MessageBox.Show(String.Format("{0}\narea added", plantsArea));
                return true;
            }
            return false;
        }

        private bool RemoveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            if (_dbModifier.RemoveSensor(area, sensor, serviceSchedule))
            {
                MessageBox.Show(String.Format("'{0}': sensor removed", sensor.MeasurableType));
                return true;
            }
            return false;
        }

        private bool RemovePlantsArea(PlantsArea area)
        {
            if (_dbModifier.RemovePlantsArea(area))
            {
                SetPlantsGrid(1);
                MessageBox.Show(String.Format("{0}\narea removed", area));
                return true;
            }
            return false;
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            SystemTimer.Enable();
            _serviceProvider.StartServices();

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
            _serviceProvider.StopServices();

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
            AddPlantsArea(plantsArea);
        }
    }
}
