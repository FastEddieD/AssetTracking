using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AssetTracking.ViewModels;
using AssetTracking.Services;

namespace AssetTracking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScannerTestPage : ContentPage
	{
		public ScannerTestPage ()
		{
			InitializeComponent ();
            BindingContext = new ScannerTestViewModel();

            ScannerSupport.OnScanData += OnScannedData;
            ScannerSupport.OnArrival += OnDeviceArrived;
            ScannerSupport.OnRemoval += OnDeviceRemoved;

            var vm = (ScannerTestViewModel)BindingContext;
        //    vm.ConnectCommand.Execute(null);
        }

        private void OnDeviceRemoved(string DeviceName)
        {
            var vm = (ScannerTestViewModel)BindingContext;
            vm.IsConnected = false;
            vm.DisplayMessage = DeviceName + " Disconnected";
        }

        private void OnDeviceArrived(string DeviceName, string BatteryLevel, string BluetoothAddress)
        {
            var vm = (ScannerTestViewModel)BindingContext;
            vm.IsConnected = true ;
            vm.DisplayMessage = DeviceName = " " + BatteryLevel + "(" + BluetoothAddress + ")";
        }

        private void OnScannedData(string ScannedData, string Symbology)
        {
            var vm = (ScannerTestViewModel)BindingContext;
            vm.ScanValue = ScannedData;
        //    vm.ScanList.Add(ScannedData);
        }
    }
}