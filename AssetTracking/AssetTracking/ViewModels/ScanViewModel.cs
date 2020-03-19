using RestSharp;
using AssetTracking.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using System.IO;
using SQLite;
using SocketMobile.Capture;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Geolocator.Abstractions;
using AssetTracking.Helpers;

namespace AssetTracking.ViewModels
{
    public class ScanViewModel : INotifyPropertyChanged
    {
        bool stationLocked = true;
        bool scanComplete = false;
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "assetTracking.db3");
        public Command ClearScanCommand { get; }
        public Command SaveScanCommand { get; }
        public Command UpdateHistoryCommand { get; }
        public Command GetBackgroundFilesCommand { get; }
        public Command ShowStationListCommand { get; }
        public Command ShowActionListCommand { get; }
        public Command UploadScansToServerCommand { get; }       
        public Command GetCurrentLocationCommand { get; }
        public Command ShowMapCommand { get; }
        public Command SetMapPinsCommand { get; }
        public Command ClearLocalDataBaseCommand { get; }
        public Command ValidateScanCommand { get; }
        public Command UpdateElapsedTimeCommand { get; }
        public ScanViewModel()
        {
            SaveScanCommand = new Command(SaveScan);
            ClearScanCommand = new Command(ClearScan);

            ClearLocalDataBaseCommand = new Command(ClearLocalDatabase);
            ValidateScanCommand = new Command(ValidateScan);
            UpdateHistoryCommand = new Command(async () => await GetScans(itemValue),() => !IsBusy);
            GetBackgroundFilesCommand = new Command(async () => await GetBackgroundFiles(), () => !IsBusy);
            UploadScansToServerCommand = new Command(async () => await UploadScansToServer(), () => !IsBusy);
            GetCurrentLocationCommand = new Command(async () => await GetCurrentLocation(), () => !IsBusy);
            UpdateElapsedTimeCommand = new Command(UpdateTimer);
            ShowMapCommand = new Command(async() => await ShowMap(), () => IsGPSEnabled);
            SetMapPinsCommand = new Command(SetMapPins);
            ShowStationListCommand = new Command(ShowStationList);
            ShowActionListCommand = new Command(ShowActionList);
            InitializeLists();
            SetDefaultSelections();
        }

       
        void InitializeLists()
        {
         //   var db = DependencyService.Get<IDatabaseConnection>().DbConnection();
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Station>();
            StationList = db.Table<Station>().OrderBy(i => i.Id).ToList();
            db.CreateTable<ActionItem>();
            ActionItemsList = db.Table<ActionItem>().OrderBy(i => i.Id).ToList();
            db.Close();
        }

        void SetDefaultSelections()
        {
        //    OperatorValue = UserSettings.OperatorId;
        //    ModeValue = UserSettings.ModeId;
        //    PlantValue = UserSettings.PlantId;
        //    DeptValue = UserSettings.DeptId;
        //    UseGPS = UserSettings.UseGPS == "True";
        //    WorkOffline = UserSettings.WorkOffline == "True";
            StationValue = UserSettings.StationId;
            ActionValue = UserSettings.ActionItemId;
            itemValue = "";
        }

        async Task GetCurrentLocation()
        {
            if (UserSettings.UseGPS == "True")
            {
                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 20;
                    var position = await locator.GetPositionAsync(timeout: TimeSpan.FromMilliseconds(10000));
                    CurrentPosition = position;
                    var loc = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                    PinCollection.Add(new Pin { Position = loc, Type = PinType.Generic, Label = "You are Here!" });

                    DisplayMessage = $"You are at {position.Latitude} Latitude and {position.Longitude} Longitude";
                }
                catch (Exception ex)
                {
                    DisplayMessage = ex.Message.ToString();
                }
            }
            else
            {
                DisplayMessage = "GPS is Disabled";
            }

        }
        async Task ShowMap()
        {
            await GetCurrentLocation();
            await App.Current.MainPage.Navigation.PushAsync(new MapDisplayPage(_currentPosition.Latitude, _currentPosition.Longitude, _pinCollection), false);

        }
        #region Properties   
        private string saveResult = "Scan or Pick";
        public string SaveResult
        {
            get
            {
                return saveResult;
            }
            set
            {
                saveResult = value;
                OnPropertyChanged(nameof(SaveResult));
            }
        }
        private Scan lastScan = new Scan();
        public Scan LastScan
        {
            get
            {
                return lastScan;
            }
            set
            {
                lastScan = value;
                OnPropertyChanged(nameof(LastScan));
            }
        }

        DateTime scanTime;
        public DateTime ScanTime
        {
            get { return scanTime; }
            set
            {
                scanTime = value;
                OnPropertyChanged(nameof(ScanTime));
            }
        }
        private TimeSpan elapsedTime;
        public TimeSpan ElapsedTime
        {
            get
            {
           //     TimeSpan span = DateTime.Now - DateTime.Parse(lastScan);
           //     return span.TotalMinutes.ToString() + " Minutes";
                return elapsedTime;
            }
            set
            {
                elapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
                OnPropertyChanged(nameof(ElapsedTimeDisplay));

            }
        }
        private string elapsedTimeDisplay;
        public string ElapsedTimeDisplay
        {
            get
            {
           //     return elapsedTimeDisplay;
          //  }
          //  set
          //  {
                if (UserSettings.ShiftStart == "")
                {
                    elapsedTimeDisplay = "Timer Off";
                }
                else if (UserSettings.ShiftEnd != "")
                {
                    elapsedTimeDisplay = "Clocked Out";

                }
                else if (elapsedTime.TotalSeconds < 60)
                {
                    elapsedTimeDisplay = elapsedTime.Seconds.ToString() + " Seconds";
                }
                else if (elapsedTime.TotalSeconds < 300)
                {
                    elapsedTimeDisplay = elapsedTime.Minutes + " Min " + elapsedTime.Seconds.ToString() + " Sec";
                }
                else
                {
                    elapsedTimeDisplay = (elapsedTime.Minutes / 60).ToString("F1") + " Hour(s)";
                }
                return elapsedTimeDisplay;
           //     OnPropertyChanged(nameof(ElapsedTimeDisplay));
            }
        }

        private Plugin.Geolocator.Abstractions.Position _currentPosition = new Plugin.Geolocator.Abstractions.Position(42.5742501, -83.1970502);
        public Plugin.Geolocator.Abstractions.Position CurrentPosition
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                _currentPosition = value;
                OnPropertyChanged(nameof(CurrentPosition));
            }
        }
        private ObservableCollection<Xamarin.Forms.Maps.Pin> _pinCollection = new ObservableCollection<Xamarin.Forms.Maps.Pin>();
        public ObservableCollection<Xamarin.Forms.Maps.Pin> PinCollection
        {
            get
            {
              
                return _pinCollection;
            }
            set
            {
                _pinCollection = value;
                OnPropertyChanged(nameof(PinCollection));
            }
        }

        private List<ActionItem> actionItems = new List<ActionItem>();
        public List<ActionItem> ActionItemsList
        {
            get
            {
                return actionItems;
            }
            set
            {
                actionItems = value;
                OnPropertyChanged(nameof(ActionItemsList));
            }

        }
        ActionItem actionPicked ;
        public ActionItem ActionPicked
        {
            get
            {
                return actionPicked;
            }
            set
            {
                actionPicked = value;
             
                if (actionPicked != null)
                {
                    actionValue = actionPicked.Id.ToString();
                    UserSettings.ActionItemId = actionValue;


               //     PostIfScanComplete();
                }
                OnPropertyChanged(nameof(ActionValue));
                OnPropertyChanged(nameof(ActionPicked));
                OnPropertyChanged(nameof(DisplayMessage));
            }
        }
        private string actionValue = UserSettings.ActionItemId;
        public string ActionValue
        {
            get
            {
                return actionValue;
            }
            set
            {
                actionValue = value;
                if (actionValue !="")
                {
                     actionPicked = ActionItemsList.Find(i => i.Id == Int32.Parse(actionValue));
                     OnPropertyChanged(nameof(ActionPicked));
                }
                OnPropertyChanged(nameof(ActionValue));
                OnPropertyChanged(nameof(ActionItemsList));
           
                PostIfScanComplete();
            }
        }

        private string itemValue = "";
        public string ItemValue
        {
            get
            {
                return itemValue;
            }
            set
            {
                itemValue = value;
                OnPropertyChanged(nameof(ItemValue));
                PostIfScanComplete();
              
            }
        }
        private List<Station> stationList = new List<Station>();
        public List<Station> StationList
        {
            get
            {
                return stationList;
            }
            set
            {
                stationList = value;
                stationListVisible = false;
                OnPropertyChanged(nameof(StationListVisible));
                OnPropertyChanged(nameof(StationList));
            }
        }

        Station stationPicked ;
        public Station StationPicked
        {
            get
            {
                return stationPicked;
            }
            set
            {
                stationPicked = value;
                if (stationPicked != null)
                // Limit Actions to those for selected Station
                {
                    var db = DependencyService.Get<IDatabaseConnection>().DbConnection();
              //      var db = new SQLiteConnection(dbPath);
                    ActionItemsList = db.Table<ActionItem>().Where(a => a.DeptId == stationPicked.DeptId).ToList();
                    db.Close();
                    stationValue = stationPicked.Id.ToString();
                    UserSettings.StationId = stationValue;
                }
             //   PostIfScanComplete();
                OnPropertyChanged(nameof(StationPicked));
                OnPropertyChanged(nameof(IsScanComplete));
                OnPropertyChanged(nameof(DisplayMessage));


            }
        }
        private string stationValue = UserSettings.StationId;
        public string StationValue
        {
            get
            {
                return stationValue;
            }
            set
            {
                stationValue = value;
                if (stationValue != "")
                {
                     stationPicked = StationList.Find(s => s.Id == Int32.Parse(stationValue));
                //     StationPicked = StationList[index];
                     OnPropertyChanged(nameof(StationPicked));
            
                }
                OnPropertyChanged(nameof(StationValue));
            }
        }

        Operator operatorPicked = new Operator();
        public Operator OperatorPicked
        {
            get
            {
                return operatorPicked;
            }
            set
            {
                operatorPicked = value;
                OnPropertyChanged(nameof(OperatorPicked));
            }
        }
        private List<Operator> operatorList = new List<Operator>();
        public List<Operator> OperatorList
        {
            get
            {
                return operatorList;
            }
            set
            {
                operatorList = value;
                OnPropertyChanged(nameof(OperatorList));
            }
        }

        private List<Scan> scansList;
        public List<Scan> ScansList
        {
            get
            {
             //   var db = new SQLiteConnection(dbPath);
             //   var scansList = db.Table<Scan>().OrderByDescending(s => s.Id).ToList();
                return scansList;
            }
            set
            {
                scansList = value;
                OnPropertyChanged("ScansList");
            }
        }
        private string itemPicked = "";
        public string ItemPicked
        {
            get { return itemPicked; }
            set
            {
                itemPicked = value;
                if (itemPicked != "")
                {
                    itemValue = itemPicked;
                    OnPropertyChanged(nameof(ItemValue));
           //        PostIfScanComplete();
                }
                OnPropertyChanged(nameof(ItemPicked));
             // OnPropertyChanged(nameof(ScanComplete));
             // OnPropertyChanged(nameof(DisplayMessage));
              
            }
        }

        private bool isSaved = false;
        public bool IsSaved
        {
            get
            {
                return isSaved;
            }
            set
            {
                isSaved = value;
                OnPropertyChanged(nameof(IsSaved));
            }
        }
        private bool isValidScan = true;
        public bool IsValidScan
        {
            get
            {
                return isValidScan;
            }
            set
            {
                // If Station and Action are selected, Validate that they are valid
                isValidScan = value;
                OnPropertyChanged(nameof(IsValidScan));
                OnPropertyChanged(nameof(DisplayMessage));
            }
         
        }

        bool isOnline = false;
        public bool IsOnline
        {
            get
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    isOnline = true;
                }
                else
                {
                    isOnline = false;
                }
                return isOnline;
            }
            set
            {
                isOnline = value;
                OnPropertyChanged(nameof(IsOnline));
            }
        }

        bool actionLocked = true;
        public bool ActionLocked
        {
            get
            {
                return actionLocked;
            }
            set
            {
                actionLocked = value;
                OnPropertyChanged(nameof(ActionLocked));
            }
        }

        public bool StationLocked
        {
            get
            {
                return stationLocked;
            }
            set
            {
                stationLocked = value;
                OnPropertyChanged(nameof(StationLocked));
            }
        }
        public bool IsScanComplete
        {
            get
            {
                scanComplete = false;
                if (stationValue != "" )
                {
                    if (actionValue != "")
                    {
                        if (actionPicked != null)
                        {
                            if (actionPicked.ItemRequired == false)
                            {
                                scanComplete = true;
                            }
                            { 
                            scanComplete = itemValue.Length >= 6 ? true : false;
                            }
                        }
                    }

                }
                return scanComplete;
            }
            set
            {
                scanComplete = value;
                OnPropertyChanged(nameof(IsScanComplete));
            }
        }
        private string displayMessage;
        public string DisplayMessage
        {
            get
            {
                return displayMessage + $"{actionPicked} {stationPicked} {itemPicked}";
            }
            set
            {
                displayMessage = value;
                OnPropertyChanged(nameof(DisplayMessage));
            }

        }
        private string scannedData = "";
        public string ScannedData
        {
            get
            {
                return scannedData;
            }
            set
            {
                scannedData = value;

                OnPropertyChanged(nameof(ScannedData));

                // Parse Scan / Set appropriate Value
                var length = scannedData.Length;
                // Determine Scan Type from Prefix of Scan
                if (scannedData.StartsWith("A"))
                {
                    actionListVisible = false;
                    ActionValue = scannedData.Substring(1);
                    OnPropertyChanged(nameof(ActionPicked));
                    OnPropertyChanged(nameof(ActionValue));
                  //  OnPropertyChanged(nameof(ActionListVisible));
                }

                else if (scannedData.StartsWith("S"))
                {
                //    stationPicked.Id = Int32.Parse(scannedData.Substring(1));
                //    stationListVisible = false;
                    StationValue = scannedData.Substring(1);
                    OnPropertyChanged(nameof(StationPicked));
                    OnPropertyChanged(nameof(StationListVisible));
                    OnPropertyChanged(nameof(StationValue));
                }
                else
                {
                    itemValue = scannedData;
              //      OnPropertyChanged(nameof(ItemPicked));
                    OnPropertyChanged(nameof(ItemValue));
                }
                ValidateScan();
             //   SaveScan();
            }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                UploadScansToServerCommand.ChangeCanExecute();
            }
        }
        private bool isGPSEnabled;
        public bool IsGPSEnabled
        {
            get
            {
                return UserSettings.UseGPS == "True";
            }
            set
            {
                isGPSEnabled = value;
                OnPropertyChanged(nameof(IsGPSEnabled));
                ShowMapCommand.ChangeCanExecute();
            }
        }

        private bool isSoundEnabled;
        public bool IsSoundEnabled
        {
            get
            {
                return UserSettings.SoundEnabled == "True";
            }
            set
            {
                isSoundEnabled = value;
                OnPropertyChanged(nameof(IsSoundEnabled));
            }
        }




        private bool actionListVisible;
        public bool ActionListVisible
        {
            get
            {
                return actionListVisible;
            }
            set
            {
                actionListVisible = value;
                OnPropertyChanged(nameof(ActionListVisible));
            }
        }
        private bool stationListVisible;
        public bool StationListVisible
        {
            get
            {
                return stationListVisible;
            }
            set
            {
                stationListVisible = value;
                OnPropertyChanged(nameof(StationListVisible));
            }
        }

        private bool triggerErrorMessage = false;
        public bool TriggerErrorMessage
        {
            get
            {
                return triggerErrorMessage;
            }
            set
            {
                triggerErrorMessage = value;
                OnPropertyChanged(nameof(TriggerErrorMessage));
            }
        }


        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #region Tasks
        void SaveScan()
        {
            if (IsValidScan == true && IsScanComplete == true)
            {
                //   This operation will save current Scan to local database
                var db = DependencyService.Get<IDatabaseConnection>().DbConnection();

//                var db = new SQLiteConnection(dbPath);
                db.CreateTable<Scan>();
                var maxPK = db.Table<Scan>().OrderByDescending(s => s.Id).FirstOrDefault();
                var newScan = new Scan();
                newScan.Id = (maxPK == null ? 1 : maxPK.Id + 1);
                newScan.ScanTime = DateTime.Now;
                newScan.ActionItemId = Int32.Parse(ActionValue);
                newScan.StationId = Int32.Parse(StationValue);
                //   newScan.StationId = StationPicked.Id;
                //   newScan.ActionItemId = ActionPicked.Id;
                newScan.Latitude = CurrentPosition.Latitude;
                newScan.Longitude = CurrentPosition.Longitude;
                newScan.OperatorId = Int32.Parse(UserSettings.OperatorId);
                newScan.Item = ItemValue;
                newScan.Station = StationPicked.Code;
                newScan.Action = ActionPicked.Code;
                newScan.Operator = OperatorPicked.Initials;
                db.Insert(newScan);
                db.Close();
                IsSaved = true;
                SaveResult = "Scan Saved";
                LastScan = newScan;
                ClearScan();
            }
            else
            {
                TriggerErrorMessage = true;
                DisplayMessage = "Incomplete Scan";
            }
// Once Saved it will display Message that scan was saved
//  DisplayMessage = "Scan Saved";
         //  scanComplete = false;
            OnPropertyChanged("DisplayMessage");
        }
        void GetScansLocal()
        {
            var db = DependencyService.Get<IDatabaseConnection>().DbConnection();

//            var db = new SQLiteConnection(dbPath);

            ScansList = db.Table<Scan>().OrderByDescending(s => s.Id).ToList();
            foreach (var s in ScansList)
            {
                var loc = new Xamarin.Forms.Maps.Position(latitude: s.Latitude, longitude: s.Longitude);
                _pinCollection.Add(new Pin { Position = loc, Type = PinType.Generic, Label = s.ToString() });
            }



        }
        async Task GetScans(string item)
        {
            IsBusy = true;
            await Task.Delay(1000);
            // Upload and Scans in Cache
            await UploadScansToServer();
            // Retrieve current Scans
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("https://assettrackingwebapi.azurewebsites.net/scan/getbyitem/"+item);
            ScansList = JsonConvert.DeserializeObject<List<Scan>>(response);
            // For each scan in Items Collection / Update Pin Collection to be Displayed on Map
            SetMapPins();

            IsBusy = false;

        }

        void ShowActionList()
        {
            ActionListVisible = true;
            
        }
        void ShowStationList()
        {
            StationListVisible = true;
        }


        void ClearScan()
        {
            if (actionLocked != true) ActionPicked = null;
            if (stationLocked != true) StationPicked = null;
            ItemPicked = "";
            ItemValue = "";

            OnPropertyChanged(nameof(ItemValue));
            OnPropertyChanged(nameof(ItemPicked));
         //   IsScanComplete = false;
        }
        void SetMapPins()
        {
            foreach (var s in ScansList)
            {
                if (s.Latitude != 0 && s.Longitude !=0)
                {
                    var loc = new Xamarin.Forms.Maps.Position(latitude: s.Latitude, longitude: s.Longitude);
                    _pinCollection.Add(new Pin { Position = loc, Type = PinType.Generic, Label = s.ToString() });
                }
                OnPropertyChanged(nameof(PinCollection));
            }
        }
        void PostIfScanComplete()
        {
            
            if (IsScanComplete)
            {
                SaveScan();
            }

        }

        void ClearLocalDatabase()
        {
            var db = DependencyService.Get<IDatabaseConnection>().DbConnection();

       //     var db = new SQLiteConnection(dbPath);

            // Download Actions and Save to Local DB
            IsBusy = true;
            db.DeleteAll<Scan>();
            IsBusy = false;
        }
        void ValidateScan()
        {
            // If Station and Action are selected, Validate that they are valid

            int deptId;
            if (actionValue != "" && stationValue != "")
            {
                var db = DependencyService.Get<IDatabaseConnection>().DbConnection();

//                var db = new SQLiteConnection(dbPath);
                var staval = Int32.Parse(stationValue);

                var sta = db.Table<Station>().Where(i => i.Id == staval).FirstOrDefault();
                if (sta == null)
                {
                    TriggerErrorMessage = true;
                    DisplayMessage = "Invalid Station";
                    isValidScan = false;
                }
                deptId = sta.DeptId;
                var actval = Int32.Parse(actionValue);
                var act = db.Table<ActionItem>().Where(a => a.Id == actval && a.DeptId == sta.DeptId).Count();
                if (act == 0)
                {
                    displayMessage = "Invalid Action";
                    isValidScan = false;
                    triggerErrorMessage = true;
                    OnPropertyChanged("IsValidScan");
                    OnPropertyChanged("TriggerErrorMessage");
          //          OnPropertyChanged("DisplayMessage");
                }
                isValidScan = true ;
            }
            isValidScan = true ;
        }
     void UpdateTimer()
     {
            ElapsedTime = DateTime.Now - LastScan.ScanTime;
     }
    // moved to device view model
    async Task GetBackgroundFiles()
        {
            if (IsOnline)
            {
                HttpClient client = new HttpClient();
                var db = DependencyService.Get<IDatabaseConnection>().DbConnection();
           //     var db = new SQLiteConnection(dbPath);
                // Download Actions and Save to Local DB
                IsBusy = true;

            //    db.DeleteAll<Scan>();

                db.CreateTable<ActionItem>();
                var getactionresponse = await client.GetStringAsync("https://assettrackingwebapi.azurewebsites.net/scan/getactions");
                var actions = JsonConvert.DeserializeObject<List<ActionItem>>(getactionresponse);
                db.DeleteAll<ActionItem>();
                foreach (var act in actions)
                {
                    var newAction = new ActionItem();
                    newAction.Id = act.Id;
                    newAction.Code = act.Code;
                    newAction.SortCode = act.SortCode;
                    newAction.Description = act.Description;
                    newAction.DeptId = act.DeptId;
                    newAction.ItemRequired = act.ItemRequired;
                    db.Insert(newAction);
                }
                ActionItemsList = db.Table<ActionItem>().ToList();
         
                //         stationList.Add(new Station() { Id = 1, DeptId = 1, LastScanId = 0, Description = "Dept A", Code = "A" });
                //         stationList.Add(new Station() { Id = 2, DeptId = 1, LastScanId = 0, Description = "Dept B", Code = "B" });
                //         stationList.Add(new Station() { Id = 3, DeptId = 2, LastScanId = 0, Description = "Dept C", Code = "C" });
                //         stationList.Add(new Station() { Id = 4, DeptId = 2, LastScanId = 0, Description = "Dept D", Code = "D" });


                db.CreateTable<Station>();
                // Download Stations and Save to Local DB
                var getstationresponse = await client.GetStringAsync("https://assettrackingwebapi.azurewebsites.net/scan/getstations");
                var stations = JsonConvert.DeserializeObject<List<Station>>(getstationresponse);
                db.DeleteAll<Station>();
                foreach (var sta in stations)
                {
                    var newStation = new Station();
                    newStation.Id = sta.Id;
                    newStation.Code = sta.Code;
                    newStation.DeptId = sta.DeptId;
                    newStation.Description = sta.Description;
                    newStation.LastScanId = sta.LastScanId;
                    db.Insert(newStation);
                }
                StationList = db.Table<Station>().ToList();
             
                // Download Operators and Save to Local DB
                db.CreateTable<Operator>();

                var getoperatorresponse = await client.GetStringAsync("https://assettrackingwebapi.azurewebsites.net/scan/getoperators");
                var operators = JsonConvert.DeserializeObject<List<Operator>>(getoperatorresponse);
                db.DeleteAll<Operator>();
                foreach (var oper in operators)
                {
                    var newOperator = new Operator();
                    newOperator.Id = oper.Id;
                    newOperator.Initials = oper.Initials;
                    newOperator.FirstName = oper.FirstName;
                    newOperator.LastName = oper.LastName;
                    newOperator.PIN = oper.PIN;
                    db.Insert(newOperator);
                }
                OperatorList = db.Table<Operator>().ToList();

                //// Download Depts and Save to Local DB
                ///
                db.CreateTable<Dept>();
                var getdeptresponse = await client.GetStringAsync("https://assettrackingwebapi.azurewebsites.net/scan/getdepts");
                var depts = JsonConvert.DeserializeObject<List<Dept>>(getdeptresponse);
                db.DeleteAll<Dept>();
                foreach (var dept in depts)
                {
                    var newDept = new Dept();
                    newDept.Id = dept.Id;
                    newDept.Code = dept.Code;
                    newDept.PlantId = dept.PlantId;
                    newDept.Description = dept.Description;
                    db.Insert(newDept);
                }
                // Download Plants and Save to Local DB
                db.CreateTable<Plant>();
                var getplantresponse = await client.GetStringAsync("https://assettrackingwebapi.azurewebsites.net/scan/getplants");
                var plants = JsonConvert.DeserializeObject<List<Plant>>(getplantresponse);
                db.DeleteAll<Plant>();
                foreach (var plant in plants)
                {
                    var newPlant = new Plant();
                    newPlant.Id = plant.Id;
                    newPlant.Code = plant.Code;
                    newPlant.CompanyName = plant.CompanyName;
                    newPlant.Address = plant.Address;
                    newPlant.City = plant.City;
                    newPlant.State = plant.State;
                    newPlant.ZipCode = plant.ZipCode;
                    newPlant.Telephone = plant.Telephone;
                    db.Insert(newPlant);
                }
                db.Close();
        //        PlantList = db.Table<Plant>().ToList();
                IsBusy = false;
            }
            {
                DisplayMessage = "Working Offline";
            }
        }
        async Task UploadScansToServer()
        {
      //      if (IsOnline)
            if(true)
            {

                IsBusy = true;
                await Task.Delay(2000);
                var db = DependencyService.Get<IDatabaseConnection>().DbConnection();

                //                var db = new SQLiteConnection(dbPath);
                db.CreateTable<Scan>();
                var list = db.Query<Scan>("Select * from [Scan]");
                HttpClient client = new HttpClient();
                foreach (var s in list)
                {
                    // Connect and Upload to Cloud Server
                    var newscan = new Scan()
                    {
                        ScanTime = s.ScanTime,
                        StationId = s.StationId,
                        Item = s.Item,
                        OperatorId = s.OperatorId,
                        ActionItemId = s.ActionItemId,
                        Latitude = s.Latitude,
                        Longitude = s.Longitude,
                    //    Action = s.Action,
                    //    ActionIsLocked = false,
                    //    Operator = s.Operator,
                    //    Station = s.Station,
                        Processed = false
                    };

                    var rc = new RestSharp.RestClient();
                    var jsonstring = JsonConvert.SerializeObject(newscan, Formatting.None);
                    var request = new RestSharp.RestRequest("https://assettrackingwebapi.azurewebsites.net/scan/savescan", RestSharp.Method.POST);
                    request.AddParameter("application/json; charset=utf-8", jsonstring, ParameterType.RequestBody);
                    request.RequestFormat = DataFormat.Json;
                    var response = rc.Execute(request);


                    //var jsonstring = JsonConvert.SerializeObject(newscan, Formatting.None);
                    //var buffer = System.Text.Encoding.UTF8.GetBytes(jsonstring);
                    //var byteContent = new ByteArrayContent(buffer);
                    //byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    //HttpResponseMessage result = await client.PostAsync("https://assettrackingwebapi.azurewebsites.net/scan/savescan", byteContent);
                    //// Mark Scan as Posted

                }
                db.Query<Scan>("Delete from [scan]");
                db.Close(); 
                IsBusy = false;
            }
            
        }       
    }
    #endregion
}




