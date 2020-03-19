using AssetTracking.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using SQLite;
using SocketMobile.Capture;

namespace AssetTracking.ViewModels
{
    class StationViewModel : INotifyPropertyChanged
    {
        public StationViewModel() 
        {
            UpdateHistoryCommand = new Command(async () => await GetScans(), () => !IsBusy);
        }

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "assetTracking.db3");

        public Command UpdateHistoryCommand { get; }

        string stationPicked = "";

        public string StationPicked
        {
            get
            {
                return stationPicked;
            }
            set
            {
                stationPicked = value;
                OnPropertyChanged(nameof(StationPicked));
                OnPropertyChanged(nameof(DisplayMessage));
            }
        }
        public List<string> StationList
        {
            get
            {
                var stationList = new List<string>();
                stationList.Add("Machine A");
                stationList.Add("Machine B");
                stationList.Add("Machine C");
                stationList.Add("Line 1");
                stationList.Add("Line 2");
                stationList.Add("Receiving Dock");
                stationList.Add("Shipping Dock");
                stationList.Add("Inspection");
                stationList.Add("Repair");
                stationList.Add("Assembly");
                return stationList;
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
                
        private string displayMessage;
        public string DisplayMessage
        {
            get
            {
                return $"{stationPicked}";
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
                if (scannedData.StartsWith("1"))
                {
          //          actionPicked = scannedData.Substring(2, length - 2);
          //          OnPropertyChanged(nameof(ActionPicked));
                }
                else if (scannedData.StartsWith("7"))
                {
                    stationPicked = scannedData.Substring(2, length - 2);
                    OnPropertyChanged(nameof(StationPicked));
                }
                else
                {
           //         itemPicked = ScannedData;
           //         OnPropertyChanged(nameof(ItemPicked));
                }
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
                UpdateHistoryCommand.ChangeCanExecute();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        async Task GetScans()
        {
            IsBusy = true;
            await Task.Delay(1000);
            var db = new SQLiteAsyncConnection(dbPath);
            ScansList = await db.Table<Scan>().Where(s => s.Station == stationPicked).OrderBy(s => s.Station).ToListAsync();
            IsBusy = false;
        }
        async Task GetStations()
        {
            IsBusy = true;
            await Task.Delay(1000);




            IsBusy = false;
        }





    }
}
