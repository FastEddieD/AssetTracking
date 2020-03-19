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
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace AssetTracking.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {


        public ItemViewModel()
        {
            GetItemHistoryCommand = new Command(async () => await GetScans(scanType,scanValue), () => !IsBusy);
            ShowMapCommand = new Command(async() => await ShowMap(), () => !(IsBusy));
        // Populate a couple of items in Items List for testing
        //            scansList.Add(new Scan { Id = 1, ScanTime = DateTime.Now, Item = "1", OperatorId = 1, ActionItemId = 1 });
        //            scansList.Add(new Scan { Id = 2, ScanTime = DateTime.Now, Item = "2", OperatorId = 1, ActionItemId = 1 });
        //            scansList.Add(new Scan { Id = 3, ScanTime = DateTime.Now, Item = "3", OperatorId = 1, ActionItemId = 1 });
        //            ScansList = scansList;
        //            ScanType = "";
        //            ScanValue = "";
    }
        public Command GetItemHistoryCommand { get; }
        public Command ShowMapCommand { get; }

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
                GetItemHistoryCommand.Execute(null);
                OnPropertyChanged(nameof(ScanValue));
            
            }
        }
        string itemPicked;
        public string ItemPicked
        {
            get
            {
             //   ScanType = "I";
             //   ScanValue = itemPicked;
                return itemPicked;
            }
            set
            {
                itemPicked = value;
                scanType = "I";
                scanValue = itemPicked;
                GetItemHistoryCommand.Execute(null);
                OnPropertyChanged(nameof(ScanValue));
                OnPropertyChanged(nameof(ItemPicked));
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
                itemPicked = scanValue;
                ScanValue = scanValue;
                ScanType = scanType;
                DisplayMessage = displayMessage;
                OnPropertyChanged(nameof(ScannedData));
             //   OnPropertyChanged(nameof(ScanType));
             //   OnPropertyChanged(nameof(ScanValue));
                OnPropertyChanged(nameof(DisplayMessage));
                OnPropertyChanged(nameof(ItemPicked));
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
                GetItemHistoryCommand.ChangeCanExecute();
            }
        }

        private bool isOnline = true;
        public bool IsOnline
        {
            get
            {
                return isOnline;
            }
            set
            {
                isOnline = value;
                OnPropertyChanged(nameof(IsOnline));
            }
        }
        private List<Scan> scansList = new List<Scan>();
        public List<Scan> ScansList
        {
            get
            {
                return scansList;
            }
            set
            {
                scansList = value;
                OnPropertyChanged(nameof(ScansList));
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


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        async Task GetScans(string type, string val)
        {
            //     IsBusy = true;
            isBusy = true;
            OnPropertyChanged(nameof(IsBusy));
                await Task.Delay(1000);
                HttpClient client = new HttpClient();
                if (type == "I")
                {
                    var result  = await client.GetStringAsync("https://assettrackingwebapi.azurewebsites.net/scan/getbyitem/" + val);
                    scansList = JsonConvert.DeserializeObject<List<Scan>>(result);
                    ScansList = scansList;
                }
                else
                {
                    var result = await client.GetStringAsync("https://assettrackingwebapi.azurewebsites.net/scan/getbystationid/" + val);
                    scansList = JsonConvert.DeserializeObject<List<Scan>>(result);
                    ScansList = scansList;
                }
            foreach (var s in ScansList)
            {
                if (s.Latitude != 0 && s.Longitude != 0)
                {
                    var loc = new Xamarin.Forms.Maps.Position(latitude: s.Latitude, longitude: s.Longitude);
                    _pinCollection.Add(new Pin { Position = loc, Type = PinType.Generic, Label = s.ToString() });
                }
            }
            OnPropertyChanged(nameof(PinCollection));

            IsBusy = false;
            
        }
        async Task ShowMap()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;
            var position = await locator.GetPositionAsync(timeout: TimeSpan.FromMilliseconds(10000));
            var currentPosition = position;



            await App.Current.MainPage.Navigation.PushAsync(new MapDisplayPage(currentPosition.Latitude, currentPosition.Longitude, _pinCollection), false);

        }

    }
}
