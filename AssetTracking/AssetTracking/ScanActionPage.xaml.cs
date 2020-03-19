using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SocketMobile.Capture;
using AssetTracking.ViewModels;

using System.Threading;
using AssetTracking.Services;
using System.Reflection;
using System.IO;
using AssetTracking.Helpers;

namespace AssetTracking
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanActionPage : ContentPage
    {
        public ScanActionPage()
        {
            InitializeComponent();

            BindingContext = new ScanViewModel();

            ScannerSupport.OnScanData += OnScannedData;
            ScannerSupport.OnArrival += OnDeviceArrived;
            ScannerSupport.OnRemoval += OnDeviceRemoved;


            var vm = (ScanViewModel)BindingContext;
            var soundEnabled = vm.IsSoundEnabled;
            vm.PropertyChanged += Vm_PropertyChanged;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
             {
                 vm.UpdateElapsedTimeCommand.Execute(null);
                 return true;

             });
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                vm.UploadScansToServerCommand.Execute(null);
                vm.GetCurrentLocationCommand.Execute(null);
                return true;
            });
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var s = sender;
            var p = e;
            if (p.PropertyName == "IsSaved")
            {
                NotifyValidScan(this, null);
            }
            if (p.PropertyName == "TriggerErrorMessage")
            {
                NotifyInvalidScan(this, null);
            }

        }

        private void OnDeviceRemoved(string DeviceName)
        {
            var vm = (ScanViewModel)BindingContext;
            //   vm.IsConnected = false;
            vm.DisplayMessage = DeviceName + " Disconnected";
        }

        private void OnDeviceArrived(string DeviceName, string BatteryLevel, string BluetoothAddress)
        {
            var vm = (ScanViewModel)BindingContext;
            vm.DisplayMessage = DeviceName = " " + BatteryLevel + "(" + BluetoothAddress + ")";
        }

        private void OnScannedData(string ScannedData, string Symbology)
        {
            var vm = (ScanViewModel)BindingContext;
            vm.ScannedData = ScannedData;
       //    vm.ValidateScanCommand.Execute(null);
        }
           
        async void NotifyInvalidScan(object sender, EventArgs e)
        {
            if (UserSettings.SoundEnabled == "True")
            {
                var assembly = typeof(App).GetTypeInfo().Assembly;
                Stream audioStream = assembly.GetManifestResourceStream("AssetTracking.Assets." + "buzzer.wav");
                //    Stream audioStream = assembly.GetManifestResourceStream("AssetTracking." + "success.wav");
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load(audioStream);
                player.Play();
            }
            SaveResult.Text = "Invalid Scan";
            SaveResult.BackgroundColor = Color.Red;
            await SaveResult.FadeTo(1);
            await SaveResult.TranslateTo(100, 0, 500, Easing.BounceOut);
            await SaveResult.TranslateTo(0, 0);
            await SaveResult.FadeTo(0, 1000);
        }

        async void NotifyValidScan(object sender, EventArgs e)
        {
            if (UserSettings.SoundEnabled == "True")
            {
                var assembly = typeof(App).GetTypeInfo().Assembly;
                Stream audioStream = assembly.GetManifestResourceStream("AssetTracking.Assets."+"success.wav");
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load(audioStream);
                player.Play();
            }

            SaveResult.Text = "Saved";
            SaveResult.BackgroundColor = Color.Green;
            await SaveResult.FadeTo(1);
            await SaveResult.TranslateTo(100, 0, 500, Easing.BounceOut);
            await SaveResult.TranslateTo(0, 0);
            await SaveResult.FadeTo(0, 1000);
        }
        
    }
  }