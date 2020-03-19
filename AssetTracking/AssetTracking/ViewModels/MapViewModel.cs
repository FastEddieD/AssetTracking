using AssetTracking.Helpers;
using AssetTracking.Model;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AssetTracking.ViewModels
{
    public class MapViewModel : INotifyPropertyChanged
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "assetTracking.db3");
     //   public Command GetCurrentLocationCommand { get; }
     //   public Command SetPushPinsCommand { get; }
     //   public Command ShowMapCommand { get; }
     //   public Command SetMapPinsCommand { get; }

        public MapViewModel()
        {
        //    GetCurrentLocationCommand = new Command(async () => await GetCurrentLocation(), () => !IsBusy);
        //    SetMapPinsCommand = new Command(async () => await InitializePushPins(latitude, longitude, pins), () => !IsBusy);
        //    InitializePushPins();
        }
        void InitializePushPins()
        {

        //    var loc = new Xamarin.Forms.Maps.Position(CurrentPosition.Latitude, CurrentPosition.Longitude);
        //    _pinCollection.Add(new Pin { Position = loc, Type = PinType.Generic, Label = "You are Here!" });


        }

        //private Plugin.Geolocator.Abstractions.Position _currentPosition = new Plugin.Geolocator.Abstractions.Position(42.5742501, -83.1970502);
        //public Plugin.Geolocator.Abstractions.Position CurrentPosition
        //{
        //    get
        //    {
        //        return _currentPosition;
        //    }
        //    set
        //    {
        //        _currentPosition = value;
        //        OnPropertyChanged(nameof(CurrentPosition));
        //    }
        //}
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
        private Xamarin.Forms.Maps.Position mapCenter;
        public Xamarin.Forms.Maps.Position MapCenter
            {
            get
            {

                return mapCenter;
            }
            set
            {
                mapCenter = value;
                OnPropertyChanged(nameof(MapCenter));
}
            }

        private string displayMessage;
        public string DisplayMessage
        {
            get
            {
                return displayMessage;
                // + $"{actionPicked} {stationPicked} {itemPicked}";
            }
            set
            {
                displayMessage = value;
                OnPropertyChanged(nameof(DisplayMessage));
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //async Task GetCurrentLocation()
        //{
        //    if (UserSettings.UseGPS == "True")
        //    {
        //        try
        //        {
        //            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

        //            var locator = CrossGeolocator.Current;
        //            locator.DesiredAccuracy = 20;
        //            var position = await locator.GetPositionAsync(timeout: TimeSpan.FromMilliseconds(10000));
        //            CurrentPosition = position;
        //            var loc = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
        //            MapCenter = loc ; 
        //            PinCollection.Add(new Pin { Position = loc, Type = PinType.Generic, Label = "My Position" });

        //            DisplayMessage = $"You are at {position.Latitude} Latitude and {position.Longitude} Longitude";
        //        }
        //        catch (Exception ex)
        //        {
        //            DisplayMessage = ex.Message.ToString();
        //        }
        //    }
        //    else
        //    {
        //        DisplayMessage = "GPS is Disabled";
        //    }

        //}
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
            }
        }

        private List<Scan> scansList;
        public List<Scan> ScansList
        {
            get
            {
                return scansList;
            }
            set
            {
                scansList = value;
                OnPropertyChanged("ScansList");
            }
        }

        // Map Pins are set prior to calling Map and passed in as a parameter.
        //async Task GetScans()
        //{
        //    IsBusy = true;
        //    await Task.Delay(1000);
        //    // Retrieve current Scans
        //    HttpClient client = new HttpClient();
        //    var response = await client.GetStringAsync("https://assettrackingwebapi.azurewebsites.net/scan/getbyitem/123456");
        //    ScansList = JsonConvert.DeserializeObject<List<Scan>>(response);
        //    // For each scan in Items Collection / Update Pin Collection to be Displayed on Map
        //    foreach (var s in ScansList)
        //    {
        //        if (s.Latitude != 0 && s.Longitude != 0)
        //        {
        //            var loc = new Xamarin.Forms.Maps.Position(latitude: s.Latitude, longitude: s.Longitude);
        //            _pinCollection.Add(new Pin { Position = loc, Type = PinType.Generic, Label = s.ToString() });
        //        }
        //        OnPropertyChanged(nameof(PinCollection));
        //    }


        //    IsBusy = false;

        //}

     //   async Task ShowMap()
     //   {
     //       await GetCurrentLocation();
     //       await App.Current.MainPage.Navigation.PushAsync(new MapDisplayPage(CurrentPosition.Latitude, CurrentPosition.Longitude), false);
     //   }
    }
}