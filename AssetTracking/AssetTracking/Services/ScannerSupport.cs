using System;
using AssetTracking.ViewModels;
namespace AssetTracking.Services
{
    public class ScannerSupport
    {
        // Delegates to be used by the activities

        public delegate void OnArrivalDelegate(string DeviceName, string BatteryLevel, string BluetoothAddress);
        public delegate void OnRemovalDelegate(string DeviceName);
        public delegate void OnDecodedDataDelegate(string ScannedData, string Symbology);

        public static void OnArrivalNop(string DeviceName, string BatteryLevel, string BluetoothAddress)
        {

        }

        public static void OnRemovalNop(string DeviceName)
        {
        }

        public static void OnDataNop(string ScannedData, string Symboloby)
        {

        }

        public static OnArrivalDelegate OnArrival = new OnArrivalDelegate(OnArrivalNop);
        public static OnRemovalDelegate OnRemoval = new OnRemovalDelegate(OnRemovalNop);
        public static OnDecodedDataDelegate OnScanData = new OnDecodedDataDelegate(OnDataNop);

        public ScannerSupport()
        {
        }

        public static void OnDeviceArrival(string DeviceName, string BatteryLevel, string BluetoothAddress)
        {
            // Call the delegate
            OnArrival(DeviceName, BatteryLevel, BluetoothAddress);
        }

        public static void OnDeviceRemoval(string DeviceName)
        {
            // Call the delegate
            OnRemoval(DeviceName);
        }

        public static void OnDecodedData(string ScannedData, string Symbology)
        {
            // Call the delegate
            OnScanData(ScannedData, Symbology);
            
        }
    }
}
