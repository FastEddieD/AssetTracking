using AssetTracking.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using AssetTracking.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace AssetTracking.ViewModels
{
    public class DeviceViewModel :INotifyPropertyChanged 
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "assetTracking.db3");
        public Command DeleteItemCommand { get; }
        public Command AddItemCommand { get; }
        public Command GetPickListItemsCommand { get; }
        public Command ClearPickListItemsCommand { get; }
        public Command StartProgramCommand { get; }
        public Command ShowSetupCommand { get; }
        public Command ServerSyncCommand { get; set; }
        public Command GetBackgroundFilesCommand { get; }
        public Command ClockInCommand { get; }
        public Command ClockOutCommand { get; }
        public DeviceViewModel()
        {

            GetBackgroundFilesCommand = new Command(async () => await GetBackgroundFiles(), () => !IsBusy);

  
            //      GetPickListItemsCommand = new Command(async () => await GetItems(), () => !IsBusy);
            GetPickListItemsCommand = new Command(GetItems);
    //        AddItemCommand = new Command(AddItem(scanValue,1));
            ClearPickListItemsCommand = new Command(ClearItems);
            AddItemCommand = new Command(AddItem);
            ScanType = "";
            ScanValue = "";
            InitializeLists();
            SetDefaultSelections();
            ClockInCommand = new Command(ClockIn);
            ClockOutCommand = new Command(ClockOut);
            // Comment this out
          //  WorkOffline = true;
          //  IsOnline = false;
         //   StartProgramCommand = new Command(StartProgram);
         //   ShowSetupCommand = new Command(ShowSetupOptions);
         //   ServerSyncCommand = new Command(ServerSync);
        }
        void InitializeLists()
        {

            var db = DependencyService.Get<IDatabaseConnection>().DbConnection();
        //             var db = new SQLiteConnection(dbPath);
            db.CreateTable<Operator>();
            OperatorList = db.Table<Operator>().OrderBy(i => i.Id).ToList();
            db.CreateTable<Plant>();
            PlantList = db.Table<Plant>().OrderBy(p => p.Id).ToList();
            db.CreateTable<Dept>();
            DeptList = db.Table<Dept>().OrderBy(d => d.Id).ToList();
            db.Close();

            modeList.Add(new Mode { Id = 1, Code = "Run",Description = "Live Scan Mode"});
            modeList.Add(new Mode { Id = 2, Code = "Inventory", Description = "Inventory" });
            modeList.Add(new Mode { Id = 3, Code = "PickList", Description = "Shopping Cart" });
            modeList.Add(new Mode { Id = 4, Code = "Lookup", Description = "Item Lookup" });
            modeList.Add(new Mode { Id = 5, Code = "Station", Description = "Stations Status" });
            modeList.Add(new Mode { Id = 6, Code = "Database", Description = "Device Status" });
            ModeList = modeList;

            pickList.Add(new PickList { Id = 1, ItemCode = "1234567890", Qty = 1, Description = "Test Scan", ScanTime = DateTime.Now });
            pickList.Add(new PickList { Id = 2, ItemCode = "9876543210", Qty = 2, Description = "Another Item", ScanTime = DateTime.Now });
            pickList.Add(new PickList { Id = 3, ItemCode = "123456", Qty = 1, Description = "A Bin", ScanTime = DateTime.Now });
            PickList = pickList;

            checkoutActionList = new List<string>();
            checkoutActionList.Add("Move");
            checkoutActionList.Add("Ship");
            checkoutActionList.Add("Receive");
            checkoutActionList.Add("Approve");
            checkoutActionList.Add("Reject");
            checkoutActionList.Add("Done");
            CheckoutActionList = checkoutActionList;
        }

        void SetDefaultSelections()
        {
            OperatorValue = UserSettings.OperatorId;
            ModeValue = UserSettings.ModeId;
            PlantValue = UserSettings.PlantId;
            DeptValue = UserSettings.DeptId;
            UseGPS = UserSettings.UseGPS == "True";
            WorkOffline = UserSettings.WorkOffline == "True";
            ShiftStart = UserSettings.ShiftStart;
            ShiftEnd = UserSettings.ShiftEnd;
            IsClockRunning = UserSettings.UseTimer == "True";
        }
        private bool useGPS = false;
        public bool UseGPS
        {
            get
            {
                return UserSettings.UseGPS == "True";
            }
            set
            {
                UserSettings.UseGPS = value.ToString();
                useGPS = value;
                OnPropertyChanged(nameof(UseGPS));
            }
        }
        private bool workOffline = false;
        public bool WorkOffline
        {
            get
            {
                return UserSettings.WorkOffline == "True";
            }
            set
            {
                UserSettings.WorkOffline = value.ToString();
                workOffline = value;
                OnPropertyChanged(nameof(WorkOffline));
            }
        }
        private bool soundEnabled = false;
        public bool SoundEnabled
        {
            get
            {
                return UserSettings.SoundEnabled == "True";
            }
            set
            {
                UserSettings.SoundEnabled = value.ToString();
                soundEnabled = value;
                OnPropertyChanged(nameof(SoundEnabled));
            }
        }


        private bool isBusy = false;
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
                GetPickListItemsCommand.ChangeCanExecute();
            }
        }
        string scanType;
        public string ScanType
        {
            get
            {
                return scanType;
            }
            set
            {
                scanType = value;
                OnPropertyChanged(nameof(ScanType));
            }
        }
        string scanValue;
        public string ScanValue
        {
            get
            {
                return scanValue;
            }
            set
            {
                scanValue = value;
             //   GetPickListItemsCommand.Execute(null);
                OnPropertyChanged(nameof(ScanValue));

            }
        }

        private string displayMessage;
        public string DisplayMessage
        {
            get
            {
                return $"{displayMessage}";
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

                // Parse Scan / Set appropriate Value
                var length = scannedData.Length;

                // Determine Scan Type from Prefix of Scan
                if (scannedData.StartsWith("A"))
                {
                    displayMessage = "Error - You Must scan a Station or Item";
                }
                else if (scannedData.StartsWith("S"))
                {
                    scanType = "S";
                    scanValue = scannedData.Substring(1);
                    displayMessage = "Location " + scanValue;
                }
                else
                {
                    scanType = "I";
                    scanValue = scannedData;
                }
                ScanValue = scanValue;
                ScanType = scanType;
                DisplayMessage = displayMessage;
                AddItem();
                OnPropertyChanged(nameof(ScannedData));
                //   OnPropertyChanged(nameof(ScanType));
                //   OnPropertyChanged(nameof(ScanValue));
                OnPropertyChanged(nameof(DisplayMessage));
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
                if (operatorPicked != null)
                {
                    operatorValue = operatorPicked.Id.ToString();
                    UserSettings.OperatorId = operatorValue;
                }
                OnPropertyChanged(nameof(OperatorPicked));
                OnPropertyChanged(nameof(DisplayMessage));
            }
        }
        private string operatorValue = "";
        public string OperatorValue
        {
            get
            {
                return operatorValue;
            }
            set
            {


                operatorValue = value;

                if (operatorValue != "")
                {

                    OperatorPicked = OperatorList.Find(o => o.Id == Int32.Parse(operatorValue));
                    UserSettings.OperatorId = operatorValue;
               //     Application.Current.Properties["OperatorId"] = operatorValue;
                    OnPropertyChanged(nameof(OperatorPicked));
                }
            
                OnPropertyChanged(nameof(OperatorValue));
            }
        }
        private string shiftStart = "";
        public string ShiftStart
        {
            get
            {
                return shiftStart;
            }
            set
            {


                shiftStart = value;

                if (shiftStart != "")
                {

                    UserSettings.ShiftStart = shiftStart;
                    UserSettings.ShiftEnd = "";
                  
                }
                OnPropertyChanged(nameof(ShiftStart));
                OnPropertyChanged(nameof(ShiftEnd));
            }
        }
        private string shiftEnd = "";
        public string ShiftEnd
        {
            get
            {
                return shiftEnd;
            }
            set
            {


                shiftEnd = value;

                if (shiftEnd != "")
                {

                    UserSettings.ShiftEnd = value;
                }
                OnPropertyChanged(nameof(ShiftEnd));
            }
      
        }
        private bool isClockRunning = false;
        public bool IsClockRunning
        {
            get
            {
                return isClockRunning;
            }
            set
            {
                isClockRunning = value;
                UserSettings.UseTimer = isClockRunning.ToString();
                OnPropertyChanged(nameof(IsClockRunning));
            }
        }


        private List<Dept> deptList = new List<Dept>();
        public List<Dept> DeptList
        {
            get
            {
                return deptList;
            }
            set
            {
                deptList = value;
                OnPropertyChanged(nameof(DeptList));
            }
        }
        Dept deptPicked = new Dept();
        public Dept DeptPicked
        {
            get { return deptPicked; }
            set
            {
                deptPicked = value;
                if (deptPicked != null)
                {
                    deptValue = deptPicked.Id.ToString();
                    UserSettings.DeptId = deptValue;
                }
                OnPropertyChanged(nameof(DeptPicked));
                OnPropertyChanged(nameof(DisplayMessage));
            }
        }
        private string deptValue = "";
        public string DeptValue
        {
            get
            {
                return deptValue;
            }
            set
            {
                deptValue = value;

                if (deptValue != "")
                {
                    DeptPicked = DeptList.Find(d => d.Id == Int32.Parse(deptValue));
                    OnPropertyChanged(nameof(DeptPicked));
                }

                OnPropertyChanged(nameof(DeptValue));
            }
        }

        private List<Mode> modeList = new List<Mode>();
        public List<Mode> ModeList
        {
            get
            {
                return modeList;
            }
            set
            {
                modeList = value;
                OnPropertyChanged(nameof(ModeList));
            }
        }
        Mode modePicked = new Mode();
        public Mode ModePicked
        {
            get { return modePicked; }
            set
            {
                modePicked = value;
                if (modePicked != null)
                {
                    modeValue = modePicked.Id.ToString();
                    UserSettings.ModeId = modeValue;
                }
                OnPropertyChanged(nameof(ModePicked));
                OnPropertyChanged(nameof(ModeValue));
                // OnPropertyChanged(nameof(ScanComplete));
                // OnPropertyChanged(nameof(DisplayMessage));

            }
        }

        private string modeValue = "";
        public string ModeValue
        {
            get
            {
                return modeValue;
            }
            set
            {


                modeValue = value;

                if (modeValue != "")
                {
                    ModePicked = ModeList.Find(m => m.Id == Int32.Parse(modeValue));
                    Application.Current.Properties["ModeId"] = modeValue;
                    OnPropertyChanged(nameof(ModePicked));
                }

                OnPropertyChanged(nameof(ModeValue));
            }
        }

        private List<Plant> plantList = new List<Plant>();
        public List<Plant> PlantList
        {
            get
            {
                return plantList;
            }
            set
            {
                plantList = value;
                OnPropertyChanged(nameof(PlantList));
            }
        }
        Plant plantPicked = new Plant();
        public Plant PlantPicked
        {
            get { return plantPicked; }
            set
            {
                plantPicked = value;
                if (plantPicked != null)
                {
                    plantValue = plantPicked.Id.ToString();
                    UserSettings.PlantId = plantValue;
                }
                OnPropertyChanged(nameof(PlantPicked));
                OnPropertyChanged(nameof(PlantValue));
            }
        }
        private string plantValue = "";
        public string PlantValue
        {
            get
            {
                return plantValue;
            }
            set
            {


                plantValue = value;

                if (plantValue != "")
                {
                    PlantPicked = PlantList.Find(p => p.Id == Int32.Parse(plantValue));
                    OnPropertyChanged(nameof(PlantPicked));
                }

                OnPropertyChanged(nameof(PlantValue));
            }
        }





        private List<PickList> pickList = new List<PickList>();
        public List<PickList> PickList
        {
            get
            {
                return pickList;
            }
            set
            {
                pickList = value;
                OnPropertyChanged(nameof(PickList));
            }
        }

        private List<string> checkoutActionList;
        public List<string> CheckoutActionList
        {
            get
            {
                return checkoutActionList;
            }
            set
            {
                checkoutActionList = value;
                OnPropertyChanged(nameof(CheckoutActionList));
            }
        }
        private string checkoutAction;
        public string CheckoutAction
        {
            get
            {
                return checkoutAction;
            }
            set
            {
                checkoutAction = value;
                CheckoutPicklist();
                OnPropertyChanged(nameof(CheckoutAction));
            }

        }
        bool isOnline = false;
        public bool IsOnline
        {
            get
            {
                if (UserSettings.WorkOffline == "False")
             //   if (CrossConnectivity.Current.IsConnected)
                {
                    isOnline = true;
               //     isOnline = false;
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
           //     stationListVisible = false;
           //     OnPropertyChanged(nameof(StationListVisible));
                OnPropertyChanged(nameof(StationList));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        void GetItems()
        {
            // This will load all items in Market Basket from local database
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<PickList>();
            var items = db.Query<PickList>("Select * from PickList").OrderBy(i => i.ItemCode);
            PickList = items.ToList();
        }
        void GetOperators()
        {
            // This will load all operators from local database
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Operator>();
            var opers = db.Query<Operator>("Select * from Operator").OrderBy(o => o.LastName);
            OperatorList = opers.ToList();
        }
        void AddItem()
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<PickList>();
            var picks = db.Query<PickList>("Select * from PickList where ItemCode = " + scanValue) ;
            if (picks.Count == 0 )
            {
                var maxPK = db.Table<PickList>().OrderByDescending(s => s.Id).FirstOrDefault();

                var newItem = new PickList();
                newItem.Id = (maxPK == null ? 1 : maxPK.Id + 1);
                newItem.ItemCode = scanValue;
                newItem.Qty = 1;
                db.Insert(newItem);
            }
            else
            {
                var edititem = picks.FirstOrDefault();
                edititem.Qty += 1;
                db.Update(edititem);
                OnPropertyChanged("PickList");
            }
            GetItems();
        }
        void DeleteItem(string scancode)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<PickList>();
            var pick = db.Query<PickList>("Select * from PickList where ItemCode = " + scancode);
            if (pick.Count >= 0)
            {
                db.Delete<PickList>(pick);
            }
        }
        void ClearItems()
        {
            var db = new SQLiteConnection(dbPath);
            db.DeleteAll<PickList>();
            ScanType = "";
            ScanValue = "";
        }
        void ClockIn()
        {
                var db = DependencyService.Get<IDatabaseConnection>().DbConnection();
                db.CreateTable<Scan>();
                var maxPK = db.Table<Scan>().OrderByDescending(s => s.Id).FirstOrDefault();
                var newScan = new Scan();
                newScan.Id = (maxPK == null ? 1 : maxPK.Id + 1);
                newScan.ScanTime = DateTime.Now;
                newScan.OperatorId = Int32.Parse(UserSettings.OperatorId);

                newScan.ActionItemId = Int32.Parse("27");
                newScan.StationId = Int32.Parse("35");
                //   newScan.StationId = StationPicked.Id;
                //   newScan.ActionItemId = ActionPicked.Id;
                newScan.Latitude = CurrentPosition.Latitude;
                newScan.Longitude = CurrentPosition.Longitude;
                newScan.Item = "--IN--";
                db.Insert(newScan);
                db.Close();
                UserSettings.ShiftStart = DateTime.Now.ToString();
                OnPropertyChanged("DisplayMessage");


        }
        void ClockOut()
        {
            var db = DependencyService.Get<IDatabaseConnection>().DbConnection();
            db.CreateTable<Scan>();
            var maxPK = db.Table<Scan>().OrderByDescending(s => s.Id).FirstOrDefault();
            var newScan = new Scan();
            newScan.Id = (maxPK == null ? 1 : maxPK.Id + 1);
            newScan.ScanTime = DateTime.Now;
            newScan.OperatorId = Int32.Parse(UserSettings.OperatorId);
            newScan.ActionItemId = Int32.Parse("27");
            newScan.StationId = Int32.Parse("35");
            //   newScan.StationId = StationPicked.Id;
            //   newScan.ActionItemId = ActionPicked.Id;
            newScan.Latitude = CurrentPosition.Latitude;
            newScan.Longitude = CurrentPosition.Longitude;
            newScan.Item = "--OUT--";

            db.Insert(newScan);
            db.Close();
            UserSettings.ShiftEnd = DateTime.Now.ToString();
            OnPropertyChanged("DisplayMessage");

        }


        void CheckoutPicklist()
        {
            var db = new SQLiteConnection(dbPath);
            // Load Picklist Items
            var picklist = db.Table<PickList>().ToList();
            db.CreateTable<Scan>();
            // For Each Item / Create a Scan Record based on Action provided
            foreach (var pick in picklist)
            {
                var newscan = new Scan();
                var maxPK = db.Table<Scan>().OrderByDescending(s => s.Id).FirstOrDefault();
                var newScan = new Scan();
                newScan.Id = (maxPK == null ? 1 : maxPK.Id + 1);
                newScan.ScanTime = DateTime.Now;
                newScan.StationId = pick.StationId;
                newScan.Item = pick.ItemCode;
                newScan.Action = "Done";
                newScan.ActionItemId = Int32.Parse("1");
                newScan.OperatorId = pick.OperatorId;
                newScan.Processed = false;
                newScan.IsSaved = false;
                db.Insert(newScan);
            }


            // Notify to User that Scans have been Saved
            DisplayMessage = "Items have been Posted";
            // Clear All Items from List
            ClearItems();
            GetItems();

        }
        async Task GetBackgroundFiles()
        {
            if (IsOnline)
            {
                HttpClient client = new HttpClient();
                var db = DependencyService.Get<IDatabaseConnection>().DbConnection();
       //         var db = new SQLiteConnection(dbPath);
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

    }
}
