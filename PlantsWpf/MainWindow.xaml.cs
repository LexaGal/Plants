using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using MongoDbServer;
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
using PlantsWpf.DbDataAccessors;
using PlantsWpf.ObjectsViews;
using Server;
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
        private MessagesListener _messagesListener;
        private PlantsAreas _plantsAreas;
        private SensorsMeasuringsProvider _sensorsMeasuringsProvider;
        private ServiceProvider _serviceProvider;
        private DbMapper _dbMapper;
        private DateTime _beginDateTime;
        private DbDataModifier _dbDataModifier;
        public static ResourceDictionary ResourceDictionary;

        public User _user;

        public MainWindow()
        {
            InitializeComponent();
            ResourceDictionary = Application.LoadComponent(
                    new Uri("/PlantsWpf;component/ResDictionary.xaml",
                        UriKind.RelativeOrAbsolute)) as ResourceDictionary;

            SetWeatherBox();

            ParameterServicesInfo.SetBaseParameters();

            _beginDateTime = DateTime.Now;

            Weather.SetWeather(WeatherTypesEnum.Warm);
        }

        public void StartMainProcess()
        {
            Initialize();

            WindowState = WindowState.Maximized;

            SetPlantsGrid(1);

            SystemTimer.Start(SendMessagesHandler, new TimeSpan(0, 0, 0, 0, 1000));

            //ServerScheduler.Start();

            //BsonClassMapsSetter.SetMongoSensorMap();
            //BsonClassMapsSetter.SetMongoPlantsArea();
            //MongoServerScheduler.Start();
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

        public void Initialize()
        {
            IPlantMappingRepository plantRepository = new PlantMappingRepository();
            IPlantsAreaMappingRepository plantsAreaRepository = new PlantsAreaMappingRepository();
            IMeasurableParameterMappingRepository measurableParameterRepository =
                new MeasurableParameterMappingRepository();
            ISensorMappingRepository sensorRepository = new SensorMappingRepository();
            IServiceScheduleMappingRepository serviceScheduleMappingRepository = new ServiceScheduleMappingRepository();

            _dbMapper = new DbMapper(plantRepository,
                measurableParameterRepository, serviceScheduleMappingRepository);

            List<PlantsAreaMapping> plantsAreaMappings = plantsAreaRepository.GetAll(mapping => mapping.UserId == _user.Id);

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

            _messagesListener = new MessagesListener(_observer);

            _serviceProvider = new ServiceProvider(_observer, _plantsAreas);

            _dbDataModifier = new DbDataModifier(_plantsAreas, _sensorsCollection, measurableParameterRepository,
                plantRepository, sensorRepository, plantsAreaRepository, serviceScheduleMappingRepository);
        }

        private void SetPlantsGrid(int numberInRow)
        {
            const int sizeHorizontal = 1330;
            const int sizeVertical = 310;
            try
            {
                PlantsGrid.Children.Clear();
                int marginLeft = 10;
                int marginTop = 10;

                for (int index = 0; index < _plantsAreas.Areas.Count; index++)
                {
                    PlantsArea area = _plantsAreas.Areas[index];

                    Border borderedPlantAreaPanel = CreateFullPlantAreaPanel(area, marginLeft, marginTop);
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

        private Border CreateFullPlantAreaPanel(PlantsArea area, int marginLeft, int marginTop)
        {
            DataGridsBuilder dataGridsBuilder = new DataGridsBuilder();
            FrameworkElementFactoriesBuilder frameworkElementFactoriesBuilder = new FrameworkElementFactoriesBuilder();
            
            StackPanel plantAreaSensorsPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 1330,
                Height = 300,
                CanVerticallyScroll = true,
                CanHorizontallyScroll = true,
             };

            plantAreaSensorsPanel.Children.Add(new Label
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

            FrameworkElementFactory removeSensorButtonTemplate = frameworkElementFactoriesBuilder.CreateRemoveSensorButtonTemplate(area,
                dataGridSensorViews, dataGridServiceScheduleViews, RemoveSensor);

            FrameworkElementFactory sensorSaveButtonTemplate = frameworkElementFactoriesBuilder.CreateSensorSaveButtonTemplate(area,
                dataGridSensorViews, dataGridServiceScheduleViews, SaveSensor);

            FrameworkElementFactory onOffSensorButtonTemplate = frameworkElementFactoriesBuilder.CreateOnOffSensorButtonTemplate();
            
            DataGrid sensorViewsDataGrid = dataGridsBuilder.CreateSensorsDataGrid(area, dataGridSensorViews,
                removeSensorButtonTemplate, sensorSaveButtonTemplate, onOffSensorButtonTemplate);

            DataGrid serviceStatesDataGrid = dataGridsBuilder.CreateServiceSystemsDataGrid(area);

            FrameworkElementFactory serviceScheduleSaveButtonTemplate =
                frameworkElementFactoriesBuilder.CreateServiceScheduleSaveButtonTemplate(area,
                    dataGridServiceScheduleViews, SaveServiceSchedule);
            
            FrameworkElementFactory onOffServiceScheduleButtonTemplate =
                frameworkElementFactoriesBuilder.CreateOnOffServiceScheduleButtonTemplate();

            DataGrid serviceSchedulesDataGrid = dataGridsBuilder.CreateServicesSchedulesDataGrid(area,
                dataGridServiceScheduleViews, serviceScheduleSaveButtonTemplate, onOffServiceScheduleButtonTemplate);

            Button removePlantsAreaButton = frameworkElementFactoriesBuilder.CreateRemovePlantsAreaButton(RemovePlantsArea, area);

            plantAreaSensorsPanel.Children.Add(sensorViewsDataGrid);
            plantAreaSensorsPanel.Children.Add(serviceStatesDataGrid);
            plantAreaSensorsPanel.Children.Add(serviceSchedulesDataGrid);

            StackPanel plantAreaChartsPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Visibility = Visibility.Collapsed
            };
            
            ChartDescriptor chartDescriptor = new ChartDescriptor(area.Id, area.Plant.MeasurableParameters.First().MeasurableType, 30,
                        DateTime.Now.Subtract(new TimeSpan(0, 0, 30)), DateTime.Now);
            
            PlantAreaChartsPanelBuilder plantAreaChartsPanelBuilder = new PlantAreaChartsPanelBuilder(area.Plant.MeasurableParameters,
                frameworkElementFactoriesBuilder, plantAreaChartsPanel, chartDescriptor);
            plantAreaChartsPanelBuilder.RebuildChartsPanel();
            
            Menu menu = new Menu();

            DbMeasuringMessagesRetriever dbMeasuringMessagesRetriever = new DbMeasuringMessagesRetriever(new MeasuringMessageMappingRepository(), _observer.MessagesDictionary);

            PlantAreaMenuBuilder plantAreaMenuBuilder = new PlantAreaMenuBuilder(plantAreaSensorsPanel,
                plantAreaChartsPanel, menu, frameworkElementFactoriesBuilder, dbMeasuringMessagesRetriever,
                chartDescriptor);

            plantAreaMenuBuilder.RebuildMenu();
            
            DockPanel plantAreaFullPanel = new DockPanel();

            plantAreaFullPanel.Children.Add(menu);
            plantAreaFullPanel.Children.Add(removePlantsAreaButton);
            plantAreaFullPanel.Children.Add(plantAreaSensorsPanel);
            plantAreaFullPanel.Children.Add(plantAreaChartsPanel);
            
            ScrollViewer scrollViewer = new ScrollViewer
            {
                Height = plantAreaSensorsPanel.Height,
                CanContentScroll = true,
                Content = plantAreaFullPanel,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            Border border = new Border
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                BorderBrush = Brushes.Black,
                Background = (LinearGradientBrush)ResourceDictionary["PlantsAreaBackground"],
                BorderThickness = new Thickness(2),
                Width = plantAreaSensorsPanel.Width,
                Height = plantAreaSensorsPanel.Height,
                Margin = new Thickness(marginLeft, marginTop, 0, 0),
                Child = scrollViewer
            };
            return border;
        }

        private bool SaveServiceSchedule(PlantsArea area, ServiceSchedule serviceSchedule)
        {
            return _dbDataModifier.SaveServiceSchedule(area, serviceSchedule);
        }

        private bool SaveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            return _dbDataModifier.SaveSensor(area, sensor, serviceSchedule);
        }

        private bool AddPlantsArea(PlantsArea plantsArea)
        {
            if (_dbDataModifier.AddPlantsArea(plantsArea))
            {
                SetPlantsGrid(1);
                MessageBox.Show($"{plantsArea}\narea added");
                return true;
            }
            return false;
        }

        private bool RemoveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            if (_dbDataModifier.RemoveSensor(area, sensor, serviceSchedule))
            {
                MessageBox.Show($"'{sensor.MeasurableType}': sensor removed");
                return true;
            }
            return false;
        }

        private bool RemovePlantsArea(PlantsArea area)
        {
            if (_dbDataModifier.RemovePlantsArea(area))
            {
                SetPlantsGrid(1);
                MessageBox.Show($"{area}\narea removed");
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

        private User GetUser(string fn, string ln, string pass)
        {
            IUserRepository userRepository = new UserRepository();
            return userRepository.GetUser(fn, ln, Encrypt(pass));
        }

        private void CreateUserAccount()
        {
            IUserRepository userRepository = new UserRepository();
            userRepository.Save(_user, _user.Id);
        }

        private string Encrypt(string text)
        {
            using (HashAlgorithm md5 = new SHA1CryptoServiceProvider())
            {
                var data = Encoding.ASCII.GetBytes(text);
                var result = md5.ComputeHash(data, 0, data.Count());
                return Convert.ToBase64String(result);
            }
        }
        
        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            string fn = FirstName.Text;
            string ln = LastName.Text;
            string pass = Password.Password;

            if (CreateAccount.IsChecked != null && !(bool) CreateAccount.IsChecked)
            {
                _user = GetUser(fn, ln, pass);
            }
            else
            {
                string em = Email.Text;
                EmailAddressAttribute addressAttribute = new EmailAddressAttribute();
                if (!addressAttribute.IsValid(Email.Text))
                {
                    MessageBox.Show(@"Email is wrong");
                    return;
                }
                string cpass = ConfirmPassword.Password;
                if (pass != cpass)
                {
                    MessageBox.Show(@"Passwords do not match");
                    return;
                }
                _user = new User(fn, ln, em, Encrypt(pass));
                CreateUserAccount();
            }
            StartMainProcess();
            LoginLabel.Content = $"You are logged in as {fn} {ln}";
        }

        private void CreateAccount_OnChecked(object sender, RoutedEventArgs e)
        {
            EmailLabel.Visibility = Visibility.Visible;
            ConfirmPasswordLabel.Visibility = Visibility.Visible;
            Email.Visibility = Visibility.Visible;
            ConfirmPassword.Visibility = Visibility.Visible;
        }

        private void CreateAccount_OnUnchecked(object sender, RoutedEventArgs e)
        {
            EmailLabel.Visibility = Visibility.Collapsed;
            ConfirmPasswordLabel.Visibility = Visibility.Collapsed;
            Email.Visibility = Visibility.Collapsed;
            ConfirmPassword.Visibility = Visibility.Collapsed;
        }
    }
}
