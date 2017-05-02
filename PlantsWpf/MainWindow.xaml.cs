using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using AspNet.Identity.MySQL.Repository.Concrete;
using AspNet.Identity.MySQL.WebApiModels;
using AzureQueuing;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using MongoDbServer;
using MongoDbServer.BsonClassMaps;
using MongoDbServer.MongoDocs;
using NLog;
using ObservationUtil;
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
using WeatherUtil;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PlantsWpf
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IReciever
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static ResourceDictionary ResourceDictionary;

        private readonly Dictionary<Guid, Label> _areaWeatherInfoLabels = new Dictionary<Guid, Label>();        
        private Dictionary<Guid, IEnumerable<WeatherItem>> _areaWeatherModels =
            new Dictionary<Guid, IEnumerable<WeatherItem>>();

        private DateTime _beginDateTime;
        private DbMapper _dbMapper;
        private MongoDbAccessor _mongoDbAccessor;
        private MySqlDbDataModifier _mySqlDbDataModifier;
        private Observer _observer;
        private PlantsAreas _plantsAreas;
        private SensorsCollection _sensorsCollection;
        private SensorsMeasuringsProvider _sensorsMeasuringsProvider;
        private ServiceProvider _serviceProvider;
        private ApplicationUser _user;

        //private DbDataModifier _dbDataModifier;
        //private User _oldUser;
        //private MongoMessagesListener _mongoMessagesListener;

        public MainWindow()
        {
            //

            // Retrieve storage account from connection string.
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            //    CloudConfigurationManager.GetSetting("StorageConnectionString"));

            //// Create the queue client.
            //CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            //// Retrieve a reference to a container.
            //CloudQueue queue = queueClient.GetQueueReference("myqueue");

            //var mes = queue.GetMessage();

            //

            InitializeComponent();

            Logginglabel.SetBinding(VisibilityProperty, new Binding
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            Logginglabel.SetBinding(ContentProperty, new Binding
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            Logginglabel.SetValue(ContentProperty, "You are being logged in. Please, wait...");
            Logginglabel.SetValue(VisibilityProperty, Visibility.Hidden);

            ResourceDictionary = Application.LoadComponent(
                new Uri("/PlantsWpf;component/ResDictionary.xaml",
                    UriKind.RelativeOrAbsolute)) as ResourceDictionary;

            SetWeatherBox();

            ParameterServicesInfo.SetBaseParameters();

            _beginDateTime = DateTime.Now;

            Weather.SetWeather(WeatherTypesEnum.Warm);

            SetBsonClassMaps();
        }

        public void Initialize()
        {
            //IPlantMappingRepository plantRepository = new PlantMappingRepository();
            //IPlantsAreaMappingRepository plantsAreaRepository = new PlantsAreaMappingRepository();
            //IMeasurableParameterMappingRepository measurableParameterRepository =
            //    new MeasurableParameterMappingRepository();
            //ISensorMappingRepository sensorRepository = new SensorMappingRepository();
            //IServiceScheduleMappingRepository serviceScheduleMappingRepository = new ServiceScheduleMappingRepository();

            var sqlMeasurableParameterMappingRepository =
                new MySqlMeasurableParameterMappingRepository();
            var sqlPlantMappingRepository = new MySqlPlantMappingRepository();
            var sqlPlantsAreaMappingRepository = new MySqlPlantsAreaMappingRepository();
            var sqlSensorMappingRepository = new MySqlSensorMappingRepository();
            var sqlServiceScheduleMappingRepository =
                new MySqlServiceScheduleMappingRepository();

            _dbMapper = new DbMapper(sqlPlantMappingRepository,
                sqlMeasurableParameterMappingRepository, sqlServiceScheduleMappingRepository);

            _mongoDbAccessor = new MongoDbAccessor();

            var plantsAreaMappings = new List<PlantsAreaMapping>();

            if (_user != null)
                plantsAreaMappings =
                    sqlPlantsAreaMappingRepository.GetAll(mapping => mapping.UserId == new Guid(_user.Id));

            _plantsAreas = new PlantsAreas();

            plantsAreaMappings.ForEach(p => _plantsAreas.AddPlantsArea(_dbMapper.RestorePlantArea(p)));

            _sensorsCollection = new SensorsCollection();

            foreach (var area in _plantsAreas.Areas)
                foreach (
                    var sensorMapping in sqlSensorMappingRepository.GetAll(sm => sm.PlantsAreaId == area.Id))
                {
                    var sensor = _dbMapper.RestoreSensor(sensorMapping, area);
                    if (sensor != null)
                    {
                        _sensorsCollection.AddSensor(sensor);
                        sensor.IsOn = true;
                    }
                }

            foreach (var area in _plantsAreas.Areas)
                foreach (var source in area.Sensors.Where(s => s.IsCustom))
                {
                    var serviceState = new ServiceState(source.MeasurableType, true);
                    area.PlantServicesStates.AddServiceState(serviceState);
                }

            _sensorsMeasuringsProvider = new SensorsMeasuringsProvider(_sensorsCollection);

            _observer = new Observer(_sensorsMeasuringsProvider, _plantsAreas);

            //_mongoMessagesListener = new MongoMessagesListener(_observer);

            _serviceProvider = new ServiceProvider(_observer, _plantsAreas);

            //_dbDataModifier = new DbDataModifier(_plantsAreas, _sensorsCollection, measurableParameterRepository,
            //    plantRepository, sensorRepository, plantsAreaRepository, serviceScheduleMappingRepository);

            _mySqlDbDataModifier = new MySqlDbDataModifier(sqlMeasurableParameterMappingRepository,
                sqlPlantMappingRepository, sqlPlantsAreaMappingRepository, sqlSensorMappingRepository,
                sqlServiceScheduleMappingRepository, new MySqlMeasuringMessageMappingRepository(), _sensorsCollection,
                _plantsAreas);
            //new MySqlMeasurableParameterMappingRepository(),
            //               new MySqlPlantMappingRepository(), new MySqlPlantsAreaMappingRepository(),
            //               new MySqlSensorMappingRepository(), new MySqlServiceScheduleMappingRepository(), new MySqlMeasuringMessageMappingRepository(), new SensorsCollection(), new PlantsAreas());

            _areaWeatherModels = new Dictionary<Guid, IEnumerable<WeatherItem>>();
            _plantsAreas.Areas.ForEach(a => _areaWeatherModels.Add(a.Id, new List<WeatherItem>()));
            // => _areaWeatherInfoLabels.Add(a.Id, tem>()));
        }

        public void StartMainProcess()
        {
            Initialize();

            WindowState = WindowState.Maximized;

            SetPlantsGrid(1);

            LoginButton.IsEnabled = true;
            AddArea.IsEnabled = true;
            WeatherBox.IsEnabled = true;

            if (!SystemTimer.IsEnabled)
            {
                SystemTimer.Start(SendMessagesHandler, new TimeSpan(0, 0, 0, 0, 1000));
                //DatabaseCleanerScheduler.Start();
                MongoServerScheduler.Start();
            }
        }

        public void StartQueueWorker()
        {
            var queueWorker = new MessageQueueWorker("StorageConnectionString", "myqueue", "poison-myqueue", 1, 1);
            queueWorker.MessageSending += RecieveMessage;
            queueWorker.Start();
        }

        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            try
            {
                var messengingEventArgs =
                    eventArgs as MessengingEventArgs<WeatherModel>;
                if (messengingEventArgs != null)
                {
                    var weatherModel = messengingEventArgs.Object;

                    if (!(Guid.Parse(weatherModel.UserId) == Guid.Parse(_user.Id)))
                        return;

                    var guids = weatherModel.AreasIds.Select(Guid.Parse).ToList();

                    var areas = new List<string>();

                    var items = weatherModel.WeatherItems.ToList();
                    var content = WeatherModel.WeatherItemsToString(items);

                    guids.ForEach(g =>
                    {
                        if (_areaWeatherModels.ContainsKey(g))
                        {
                            _areaWeatherModels[g] = weatherModel.WeatherItems;
                            // _areaWeatherModels[g];//Custom

                            Dispatcher.Invoke(() => { _areaWeatherInfoLabels[g].Content = content; });

                            var param = items.Single(i => i.Name == "Temperature").Value;
                            var area = _plantsAreas.Areas.Single(a => a.Id == g);
                            var paramSensor = area.Sensors.Single(s => s.GetType() == typeof(TemperatureSensor));
                            paramSensor.Function.SetWeatherValue(double.Parse(param));

                            param = items.Single(i => i.Name == "Humidity").Value;
                            paramSensor = area.Sensors.Single(s => s.GetType() == typeof(HumiditySensor));
                            paramSensor.Function.SetWeatherValue(double.Parse(param));

                            areas.Add(area.Plant.Name.ToString());
                        }
                    });
                    var city = items.Single(i => i.Name == "Location").Value;
                    MessageBox.Show($"{content}From city '{city}'\nWas applied to {string.Join(", ", areas)}");
                    //Weather ''\n
                }
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
            }
        }
        
        public void PlantsAreaWindow_GetPlantsArea(object sender, PlantsAreaEventArgs e)
        {
            var plantsArea = e.PlantsArea;
            plantsArea.UserId = new Guid(_user.Id);
            AddPlantsArea(plantsArea);
        }

        private void SetBsonClassMaps()
        {
            BsonClassMapsSetter.SetMongoSensorMap();
            BsonClassMapsSetter.SetMongoPlantsArea();
            BsonClassMapsSetter.SetMongoUserMap();
            BsonClassMapsSetter.SetMongoMessageMap();
            BsonClassMapsSetter.SetMongoNotificationMap();
        }

        private void SetWeatherBox()
        {
            foreach (var name in Enum.GetNames(typeof(WeatherTypesEnum)))
                WeatherBox.Items.Add(name);
            WeatherBox.Text = WeatherBox.Items[0].ToString();

            WeatherBox.SelectionChanged += WeatherBox_OnSelectionChanged;
        }

        private void SendMessagesHandler(object sender, ElapsedEventArgs args)
        {
            if (_sensorsMeasuringsProvider != null)
            {
                var timeSpan = args.SignalTime.Subtract(_beginDateTime);

                if (timeSpan.TotalSeconds > SystemTimer.RestartTimeSpan.TotalSeconds)
                {
                    _beginDateTime = _beginDateTime.Add(SystemTimer.RestartTimeSpan);

                    timeSpan = new TimeSpan(0, 0, (int) (timeSpan.TotalSeconds%SystemTimer.RestartTimeSpan.TotalSeconds));

                    //restarting timer and reseting all functions values to base values (new day after night sleep)
                    SystemTimer.Restart();
                    _sensorsCollection.Sensors.ForEach(s => s.Function.SetCurrentValue(s.MeasurableParameter.Optimal));
                }

                SystemTimer.CurrentTimeSpan = timeSpan;
                _sensorsMeasuringsProvider.SendMessages(timeSpan);
            }
        }
        
        private void SetPlantsGrid(int numberInRow)
        {
            const int sizeHorizontal = 1600;
            const int sizeVertical = 310;
            try
            {
                PlantsGrid.Children.Clear();
                var marginLeft = 10;
                var marginTop = 10;

                for (var index = 0; index < _plantsAreas.Areas.Count; index++)
                {
                    var area = _plantsAreas.Areas[index];

                    var borderedPlantAreaPanel = CreateFullPlantAreaPanel(area, marginLeft, marginTop);
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
                logger.Error(e.ToString());
            }
        }

        private Border CreateFullPlantAreaPanel(PlantsArea area, int marginLeft, int marginTop)
        {
            var dataGridsBuilder = new DataGridsBuilder();
            var frameworkElementFactoriesBuilder = new FrameworkElementFactoriesBuilder();

            var plantAreaSensorsPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 1550,
                Height = 300,
                CanVerticallyScroll = true,
                CanHorizontallyScroll = true
            };

            plantAreaSensorsPanel.Children.Add(new Label
            {
                Margin = new Thickness(10, 10, 0, 0),
                Name = "AreaInfo",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = area.ToString(),
                BorderBrush = Brushes.LawnGreen,
                BorderThickness = new Thickness(1)
            });

            var weatherInfoLabel = new Label
            {
                Margin = new Thickness(10, 10, 0, 0),
                Name = "WeatherInfo",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = WeatherModel.WeatherItemsToString(_areaWeatherModels[area.Id]),
                BorderBrush = Brushes.LawnGreen,
                BorderThickness = new Thickness(1)
            };

            _areaWeatherInfoLabels.Add(area.Id, weatherInfoLabel);

            plantAreaSensorsPanel.Children.Add(weatherInfoLabel);

            var dataGridSensorViews = new BindingList<DataGridSensorView>(
                area.Sensors.OrderBy(s => s.IsCustom).ToList().ConvertAll(s => new DataGridSensorView(s)))
            {
                AllowNew = true,
                AllowEdit = true,
                AllowRemove = true
            };

            var dataGridServiceScheduleViews =
                new BindingList<DataGridServiceScheduleView>(
                    area.ServicesSchedulesStates.ServicesSchedules.ToList()
                        .ConvertAll(s => new DataGridServiceScheduleView(s)))
                {
                    RaiseListChangedEvents = true,
                    AllowNew = true,
                    AllowRemove = true,
                    AllowEdit = true
                };

            var removeSensorButtonTemplate =
                frameworkElementFactoriesBuilder.CreateRemoveSensorButtonTemplate(area,
                    dataGridSensorViews, dataGridServiceScheduleViews, RemoveSensor);

            var sensorSaveButtonTemplate =
                frameworkElementFactoriesBuilder.CreateSensorSaveButtonTemplate(area,
                    dataGridSensorViews, dataGridServiceScheduleViews, SaveSensor);

            var onOffSensorButtonTemplate =
                frameworkElementFactoriesBuilder.CreateOnOffSensorButtonTemplate();

            var sensorViewsDataGrid = dataGridsBuilder.CreateSensorsDataGrid(area, dataGridSensorViews,
                removeSensorButtonTemplate, sensorSaveButtonTemplate, onOffSensorButtonTemplate);

            var serviceStatesDataGrid = dataGridsBuilder.CreateServiceSystemsDataGrid(area);

            var serviceScheduleSaveButtonTemplate =
                frameworkElementFactoriesBuilder.CreateServiceScheduleSaveButtonTemplate(area,
                    dataGridServiceScheduleViews, SaveServiceSchedule);

            var onOffServiceScheduleButtonTemplate =
                frameworkElementFactoriesBuilder.CreateOnOffServiceScheduleButtonTemplate();

            var serviceSchedulesDataGrid = dataGridsBuilder.CreateServicesSchedulesDataGrid(area,
                dataGridServiceScheduleViews, serviceScheduleSaveButtonTemplate, onOffServiceScheduleButtonTemplate);

            var removePlantsAreaButton =
                frameworkElementFactoriesBuilder.CreateRemovePlantsAreaButton(RemovePlantsArea, area);

            plantAreaSensorsPanel.Children.Add(sensorViewsDataGrid);
            plantAreaSensorsPanel.Children.Add(serviceStatesDataGrid);
            plantAreaSensorsPanel.Children.Add(serviceSchedulesDataGrid);

            var plantAreaChartsPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Visibility = Visibility.Collapsed
            };

            var chartDescriptor = new ChartDescriptor(area.Id,
                area.Plant.MeasurableParameters.First().MeasurableType, 30,
                DateTime.Now.Subtract(new TimeSpan(0, 0, 30)), DateTime.Now);

            var plantAreaChartsPanelBuilder =
                new PlantAreaChartsPanelBuilder(area.Plant.MeasurableParameters,
                    frameworkElementFactoriesBuilder, plantAreaChartsPanel, chartDescriptor);
            plantAreaChartsPanelBuilder.RebuildChartsPanel();

            var menu = new Menu();

            var dbMeasuringMessagesRetriever =
                new DbMeasuringMessagesRetriever(new MySqlMeasuringMessageMappingRepository(),
                    _observer.MessagesDictionary);

            var plantAreaMenuBuilder = new PlantAreaMenuBuilder(plantAreaSensorsPanel,
                plantAreaChartsPanel, menu, frameworkElementFactoriesBuilder, dbMeasuringMessagesRetriever,
                chartDescriptor);

            plantAreaMenuBuilder.RebuildMenu();

            var plantAreaFullPanel = new DockPanel();

            plantAreaFullPanel.Children.Add(menu);
            plantAreaFullPanel.Children.Add(removePlantsAreaButton);
            plantAreaFullPanel.Children.Add(plantAreaSensorsPanel);
            plantAreaFullPanel.Children.Add(plantAreaChartsPanel);

            var scrollViewer = new ScrollViewer
            {
                Height = plantAreaSensorsPanel.Height,
                CanContentScroll = true,
                Content = plantAreaFullPanel,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            var border = new Border
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                BorderBrush = Brushes.Black,
                Background = (LinearGradientBrush) ResourceDictionary["PlantsAreaBackground"],
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
            return _mySqlDbDataModifier.SaveServiceSchedule(area, serviceSchedule);
        }

        private bool SaveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            if (_mySqlDbDataModifier.SaveSensor(area, sensor, serviceSchedule))
            {
                _mongoDbAccessor.SaveMongoSensor(new MongoSensor(sensor));
                _mongoDbAccessor.SaveMongoPlantsArea(new MongoPlantsArea(sensor.PlantsArea));
                _mongoDbAccessor.AddMongoNotification(new MongoNotification(sensor.PlantsAreaId.ToString(),
                    $"'{sensor.MeasurableType}' sensor added/updated.", _user.Id));

                return true;
            }
            return false;
        }

        private bool AddPlantsArea(PlantsArea plantsArea)
        {
            if (_mySqlDbDataModifier.AddPlantsArea(plantsArea))
            {
                _areaWeatherInfoLabels.Clear();
                _areaWeatherModels.Add(plantsArea.Id, new List<WeatherItem>());

                SetPlantsGrid(1);
                MessageBox.Show($"{plantsArea}\nArea added.");

                _mongoDbAccessor.SaveMongoPlantsArea(new MongoPlantsArea(plantsArea));
                plantsArea.Sensors.ForEach(sensor => _mongoDbAccessor.SaveMongoSensor(new MongoSensor(sensor)));
                _mongoDbAccessor.AddMongoNotification(new MongoNotification(plantsArea.Id.ToString(),
                    $"{plantsArea}\nArea added.", _user.Id));

                return true;
            }
            return false;
        }

        private bool RemoveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            _mongoDbAccessor.DeleteMongoSensor(new MongoSensor(sensor));
            _mongoDbAccessor.AddMongoNotification(new MongoNotification(sensor.PlantsAreaId.ToString(),
                $"'{sensor.MeasurableType}' sensor removed.", _user.Id));

            if (_mySqlDbDataModifier.RemoveSensor(area, sensor, serviceSchedule))
            {
                MessageBox.Show($"'{sensor.MeasurableType}' sensor removed.");

                _mongoDbAccessor.SaveMongoPlantsArea(new MongoPlantsArea(area));

                return true;
            }
            return false;
        }

        private bool RemovePlantsArea(PlantsArea plantsArea)
        {
            plantsArea.Sensors.ForEach(sensor => _mongoDbAccessor.DeleteMongoSensor(new MongoSensor(sensor)));
            _mongoDbAccessor.DeleteMongoPlantsArea(new MongoPlantsArea(plantsArea));
            _mongoDbAccessor.AddMongoNotification(new MongoNotification(plantsArea.Id.ToString(),
                $"{plantsArea}\nArea removed.", _user.Id));

            if (_mySqlDbDataModifier.RemovePlantsArea(plantsArea))
            {
                _areaWeatherInfoLabels.Clear();
                _areaWeatherModels.Remove(plantsArea.Id);

                SetPlantsGrid(1);
                MessageBox.Show($"{plantsArea}\nArea removed.");

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
            var weatherTypesEnum =
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
            var secondWindow = new PlantsAreaWindow();
            secondWindow.PlantsAreaEvent += PlantsAreaWindow_GetPlantsArea;
            secondWindow.Show();
        }
        
        //MS Sql
        private User GetUser(string fn, string ln, string pass)
        {
            IUserRepository userRepository = new UserRepository();
            return userRepository.GetUser(fn, ln, Encrypt(pass));
        }

        //private bool CreateUserAccount()
        //{
        //    IUserRepository userRepository = new UserRepository();
        //    if (userRepository.GetUser(_user.FirstName, _user.LastName, _user.PasswordHash) != null)
        //    {
        //        return false;
        //    }
        //    userRepository.Save(_user, _user.Id);
        //    return true;
        //}

        //MS Sql

        private string Encrypt(string password)
        {
            var sha256 = SHA256.Create();
            var data = Encoding.UTF8.GetBytes(password);
            var result = sha256.ComputeHash(data);

            var hashString = result.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
            return hashString;
        }

        private void CreateAccount_OnChecked(object sender, RoutedEventArgs e)
        {
            RegisterGrid.Visibility = Visibility.Visible;
        }

        private void CreateAccount_OnUnchecked(object sender, RoutedEventArgs e)
        {
            RegisterGrid.Visibility = Visibility.Collapsed;
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mySqlDbDataModifier = new MySqlDbDataModifier();
            //new MySqlMeasurableParameterMappingRepository(),
            //    new MySqlPlantMappingRepository(), new MySqlPlantsAreaMappingRepository(),
            //    new MySqlSensorMappingRepository(), new MySqlServiceScheduleMappingRepository(), new MySqlMeasuringMessageMappingRepository(), new SensorsCollection(), new PlantsAreas());

            //MeasurableParameterMapping mpm = new MeasurableParameterMapping(Guid.NewGuid(), 10, 9, 11,
            //    ParameterEnum.Temperature.ToString());
            //_mySqlDbDataModifier.SqlMeasurableParameterMappingRepository.Save(mpm, Guid.Empty);
            //var mpm1 = _mySqlDbDataModifier.SqlMeasurableParameterMappingRepository.Get(mpm.Id);
            //mpm1.Optimal = 23;
            //_mySqlDbDataModifier.SqlMeasurableParameterMappingRepository.Save(mpm1, Guid.Empty);
            //_mySqlDbDataModifier.SqlMeasurableParameterMappingRepository.Delete(mpm1.Id);

            //PlantMapping plantMapping = new PlantMapping(Guid.NewGuid(), new Guid("a2948b2a-921f-4c7c-806d-a5724e7302cb"),
            //    new Guid("3555C74B-E887-4685-A627-005017305139"), new Guid("FDCF0612-1077-45D6-B294-272504C4E51E"),
            //    new Guid("F8867AAB-1B2C-4629-BB51-04E61B714222"),
            //    "Apple", null);

            //_mySqlDbDataModifier.SqlPlantMappingRepository.Save(plantMapping, Guid.Empty);
            //var mpm1 = _mySqlDbDataModifier.SqlPlantMappingRepository.Get(plantMapping.Id);
            //mpm1.Name = "Pear";
            //_mySqlDbDataModifier.SqlPlantMappingRepository.Save(mpm1, Guid.Empty);
            //_mySqlDbDataModifier.SqlPlantMappingRepository.Delete(mpm1.Id);

            //PlantsAreaMapping mpm = new PlantsAreaMapping(Guid.NewGuid(), new Guid("aafdb697-fd67-48e8-9ea8-7f2afe5989d0"), 10, new Guid("9adc5dca-94bc-4c35-afb5-42b06eedf989"));
            //_mySqlDbDataModifier.SqlPlantsAreaMappingRepository.Save(mpm, Guid.Empty);
            //var mpm1 = _mySqlDbDataModifier.SqlPlantsAreaMappingRepository.Get(mpm.Id);
            //mpm1.Number = 23;
            //_mySqlDbDataModifier.SqlPlantsAreaMappingRepository.Save(mpm1, Guid.Empty);

            //SensorMapping mpm = new SensorMapping(Guid.NewGuid(), new Guid("48664c4e-8300-4a2c-a7d5-3829005b1d7e"), 3,
            //    new Guid("a2948b2a-921f-4c7c-806d-a5724e7302cb"), ParameterEnum.Temperature.ToString());
            //_mySqlDbDataModifier.SqlSensorMappingRepository.Save(mpm, Guid.Empty);
            //mpm = new SensorMapping(Guid.NewGuid(), new Guid("48664c4e-8300-4a2c-a7d5-3829005b1d7e"), 3,
            //    new Guid("3555C74B-E887-4685-A627-005017305139"), ParameterEnum.Humidity.ToString());
            //_mySqlDbDataModifier.SqlSensorMappingRepository.Save(mpm, Guid.Empty);
            //mpm = new SensorMapping(Guid.NewGuid(), new Guid("48664c4e-8300-4a2c-a7d5-3829005b1d7e"), 3,
            //    new Guid("FDCF0612-1077-45D6-B294-272504C4E51E"), ParameterEnum.Nutrient.ToString());
            //_mySqlDbDataModifier.SqlSensorMappingRepository.Save(mpm, Guid.Empty);
            //mpm = new SensorMapping(Guid.NewGuid(), new Guid("48664c4e-8300-4a2c-a7d5-3829005b1d7e"), 3,
            //    new Guid("F8867AAB-1B2C-4629-BB51-04E61B714222"), ParameterEnum.SoilPh.ToString());
            //_mySqlDbDataModifier.SqlSensorMappingRepository.Save(mpm, Guid.Empty);
            //var mpm1 = _mySqlDbDataModifier.SqlSensorMappingRepository.Get(new Guid("74e4da76-a799-48b4-b75e-65e5f3fc2cd8"));
            //mpm1.MeasuringTimeout = 25;
            //_mySqlDbDataModifier.SqlSensorMappingRepository.Save(mpm1, Guid.Empty);

            //ServiceScheduleMapping mpm = new ServiceScheduleMapping(Guid.NewGuid(),
            //    new Guid("48664c4e-8300-4a2c-a7d5-3829005b1d7e"), ServiceStateEnum.Watering.ToString(), 3, 2,
            //        "a2948b2a-921f-4c7c-806d-a5724e7302cb,3555C74B-E887-4685-A627-005017305139");
            //_mySqlDbDataModifier.SqlServiceScheduleMappingRepository.Save(mpm, Guid.Empty);

            //mpm = new ServiceScheduleMapping(Guid.NewGuid(), new Guid("2e664c4e-8300-4a2c-a7d5-3829005b1d7e"), ServiceStateEnum.Nutrienting.ToString(), 3, 2,
            //        "F8867AAB-1B2C-4629-BB51-04E61B714222,FDCF0612-1077-45D6-B294-272504C4E51E");
            //_mySqlDbDataModifier.SqlServiceScheduleMappingRepository.Save(mpm, Guid.Empty);

            //mpm = new ServiceScheduleMapping(Guid.NewGuid(), new Guid("7be64c4e-8300-4a2c-a7d5-3829005b1d7e"), ServiceStateEnum.Warming.ToString(), 3, 2,
            //        "F8867AAB-1B2C-4629-BB51-04E61B714222");
            //_mySqlDbDataModifier.SqlServiceScheduleMappingRepository.Save(mpm, Guid.Empty);

            //mpm = new ServiceScheduleMapping(Guid.NewGuid(), new Guid("9ce64c4e-8300-4a2c-a7d5-3829005b1d7e"), ServiceStateEnum.Cooling.ToString(), 3, 2,
            //        "F8867AAB-1B2C-4629-BB51-04E61B714222");
            //_mySqlDbDataModifier.SqlServiceScheduleMappingRepository.Save(mpm, Guid.Empty);

            //var mpm1 = _mySqlDbDataModifier.SqlServiceScheduleMappingRepository.Get(new Guid("0824a75a-f91e-4f57-84a8-1f9bf45a995f"));
            //mpm1.ServicingPauseSpan = 5;
            //_mySqlDbDataModifier.SqlServiceScheduleMappingRepository.Save(mpm1, Guid.Empty);

            //var mm = new MeasuringMessageMapping(Guid.NewGuid(), DateTime.Now, MessageTypeEnum.UsualInfo.ToString(),
            //    ParameterEnum.Nutrient.ToString(), new Guid("48664c4e-8300-4a2c-a7d5-3829005b1d7e"), 12.66);
            //_mySqlDbDataModifier.SqlMeasuringMessageMappingRepository.Save(mm, Guid.Empty);

            //_mySqlDbDataModifier.SqlMeasuringMessageMappingRepository.Delete(mm.Id);

            //IUserRepository userRepository = new UserRepository();
            //foreach (var user in userRepository.GetAll())
            //{
            //    _mongoDbAccessor = new MongoDbAccessor();
            //    _mongoDbAccessor.AddMongoUser(new MongoUser(user));
            //}

            Logginglabel.Visibility = Visibility.Visible;
            LoginButton.IsEnabled = false;

            var firstName = FirstName.Text;
            var lastName = LastName.Text;
            var email = Email.Text;
            var password = Password.Password;

            HttpResponseMessage response;
            if ((CreateAccount.IsChecked != null) && !(bool)CreateAccount.IsChecked)
            {
                Logginglabel.Content = @"You are being logged in. Please, wait...";

                //_oldUser = GetUser(firstName, lastName, password);
                //return;

                //if (_user == null)
                //{
                //    Logginglabel.Content = @"User with such credentials does not exist";
                //    LoginButton.IsEnabled = true;
                //    return;
                //}

                var loginViewModel = new LoginViewModel
                {
                    Email = email,
                    Password = password,
                    RememberMe = true
                };

                response = await _mySqlDbDataModifier.LoginUser(loginViewModel); //.Start();//ContinueWith(); //.Result;

                if (!response.IsSuccessStatusCode)
                {
                    Logginglabel.Content = response.ReasonPhrase;
                    LoginButton.IsEnabled = true;

                    //_user = (response.Content as ObjectContent)?.Value as ApplicationUser;
                    return;
                }
                _user = response.Content.ReadAsAsync<ApplicationUser>().Result;
            }
            else
            {
                Logginglabel.Content = "You are being registered. Please, wait...";
                var confirmPassword = ConfirmPassword.Password;
                //EmailAddressAttribute addressAttribute = new EmailAddressAttribute();
                //if (!addressAttribute.IsValid(Email.Text))
                //{
                //    Logginglabel.Content = @"Email is wrong";
                //    LoginButton.IsEnabled = true;
                //    return;
                //}
                //if (password != confirmPassword)
                //{
                //    Logginglabel.Content = @"Passwords do not match";
                //    LoginButton.IsEnabled = true;
                //    return;
                //}

                string username = $"{firstName} {lastName}";

                var registerViewModel = new RegisterViewModel
                {
                    Name = username,
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword
                };

                response = await _mySqlDbDataModifier.RegisterUser(registerViewModel);

                if (!response.IsSuccessStatusCode)
                {
                    Logginglabel.Content = response.ReasonPhrase;
                    LoginButton.IsEnabled = true;

                    //_user = (response.Content as ObjectContent)?.Value as ApplicationUser;
                    return;
                }
                _user = response.Content.ReadAsAsync<ApplicationUser>().Result;


                //for MS Sql and Mongo DBs
                //_user = new User(firstName, lastName, email, Encrypt(password));
                //if (CreateUserAccount())
                //{

                _mongoDbAccessor = new MongoDbAccessor();
                _mongoDbAccessor.AddMongoUser(
                    new MongoUser(new User(_user.UserName.Split(' ')[0], _user.UserName.Split(' ')[1], _user.Email,
                        Encrypt(Password.Password))));
                //}
                //else
                //{
                //    Logginglabel.Content = @"User with such credentials already exists";
                //    LoginButton.IsEnabled = true;
                //    return;
                //}
            }

            //if (_oldUser != null) 
            if (_user != null)
            {
                StartMainProcess();
                LoginNameLabel.Content = $"You are logged in as {_user.UserName}";
                LoginNameLabel.Background = Brushes.Wheat;
                Logginglabel.Visibility = Visibility.Hidden;
            }
            LogIn.IsExpanded = false;

            StartQueueWorker();
        }    
    }
}