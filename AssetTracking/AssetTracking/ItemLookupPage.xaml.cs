using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AssetTracking.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using SQLite;
using SocketMobile.Capture;
using AssetTracking.ViewModels;
using AssetTracking.Services;

namespace AssetTracking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemLookupPage : ContentPage
    {
        public ItemLookupPage()
        {
            InitializeComponent();

            BindingContext = new ItemViewModel();

            ScannerSupport.OnScanData += OnScannedData;
            ScannerSupport.OnArrival += OnDeviceArrived;
            ScannerSupport.OnRemoval += OnDeviceRemoved;

            var vm = (ItemViewModel)BindingContext;
        }

        private void OnDeviceRemoved(string DeviceName)
        {
            var vm = (ItemViewModel)BindingContext;
            //     vm.IsConnected = false;
            vm.DisplayMessage = DeviceName + " Disconnected";
        }

        private void OnDeviceArrived(string DeviceName, string BatteryLevel, string BluetoothAddress)
        {
            var vm = (ItemViewModel)BindingContext;
            vm.DisplayMessage = DeviceName = " " + BatteryLevel + "(" + BluetoothAddress + ")";
        }

        private void OnScannedData(string ScannedData, string Symbology)
        {
            try
            {
                var vm = (ItemViewModel)BindingContext;
                vm.ScannedData = ScannedData;
            //    vm.GetItemHistoryCommand.Execute(null);
            }
            catch (Exception)
            {
            //    vm.DisplayMessage = Exception.Message;
            }
           
        }



    }
}
