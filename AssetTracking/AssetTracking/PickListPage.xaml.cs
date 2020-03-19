using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketMobile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AssetTracking.ViewModels;
using AssetTracking.Services;

namespace AssetTracking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PickListPage : ContentPage
	{
		public PickListPage ()
		{
			InitializeComponent ();
            BindingContext = new DeviceViewModel();

            ScannerSupport.OnScanData += OnScannedData;
            ScannerSupport.OnArrival += OnDeviceArrived;
            ScannerSupport.OnRemoval += OnDeviceRemoved;

            var vm = (DeviceViewModel)BindingContext;
 
        }
        private void OnDeviceRemoved(string DeviceName)
        {
            var vm = (ScanViewModel)BindingContext;
            //   vm.IsConnected = false;
            vm.DisplayMessage = DeviceName + " Disconnected";
        }

        private void OnDeviceArrived(string DeviceName, string BatteryLevel, string BluetoothAddress)
        {
            var vm = (DeviceViewModel)BindingContext;
            vm.DisplayMessage = DeviceName = " " + BatteryLevel + "(" + BluetoothAddress + ")";
        }

        private void OnScannedData(string ScannedData, string Symbology)
        {
            var vm = (DeviceViewModel)BindingContext;
            vm.ScannedData = ScannedData;

        }
    }
}