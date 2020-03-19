using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using SocketMobile.Capture;
using AssetTracking.Services;
using Xamarin.Forms.Maps;
using Xamarin;

namespace AssetTracking.Droid
{
    [Activity(Label = "AssetTracking", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public string CaptureServiceAction = "com.socketmobile.capture.START_SERVICE";
        public string CaptureServicePackage = "com.socketmobile.companion";
        public string CaptureServiceStartCmd = "com.socketmobile.capture.StartService";

        public CaptureHelper Capture = null;

        // Start the Capture service by constructing and broadcasting an intent
        private void StartCaptureService()
        {
            Intent intent = new Intent(CaptureServiceAction);
            intent.SetComponent(new ComponentName(CaptureServicePackage, CaptureServiceStartCmd));
            intent.AddFlags(ActivityFlags.ReceiverForeground);
            SendBroadcast(intent);
        }

        // This actually opens a Capture client session
        public async Task<long> OpenCapture()
        {
     //       long result = await Capture.OpenAsync("android:com.socketmobile.formscapture05",                            // the name of our package
     //                                             "bb57d8e1-f911-47ba-b510-693be162686a",                                // sample code Developer ID
     //                                             "MCwCFCewuhlm19T7vCE5fwMp4x/WMMRzAhRpheRY+Knz3ZYEFtsh1+byjtPjqg==");   // our App ID

            string appId = "android:com.ssitroy.assettrack";
            string developerId = "1da93068-6160-e911-a970-000d3a36399f";
            string appKey = "MCwCFE89fIm/qViN3t20DZo9neyXONusAhRIYR0qnoAsOAHv2MHpUaW5egKznw==";
            long result = await Capture.OpenAsync(appId, developerId, appKey); 

            return result;
        }

#if DEBUG
        // This lets us get Capture debug messages in the Xamarin Applicaiton Output window
        class DebugConsole : CaptureHelperDebug
        {
            public void PrintLine(string message)
            {
                DateTimeOffset dtNow = System.DateTimeOffset.Now;
                Console.WriteLine("[FORMS CAPTURE 05 DEBUG] " + dtNow.ToString("HH:mm:ss:fff") + ": " + message);
            }
        }
#endif

        // Start the CaptureHelper client using the required elements
        private async Task<long> StartCaptureClient()
        {
            int Retries = 0;
            long result;
            Capture = new CaptureHelper();

#if DEBUG
            Capture.DebugConsole = new DebugConsole();
#endif
            Capture.DoNotUseWebSocket = true;
            Capture.DeviceArrival += OnDeviceArrival;
            Capture.DeviceRemoval += OnDeviceRemoval;
            Capture.DecodedData += OnDecodedData;

            while ((result = await OpenCapture()) == SktErrors.ESKT_UNABLEOPENDEVICE)
            {
                Retries++;
                if (Retries == 60)
                {
                    break;
                }
                await Task.Delay(500);
            }

            // Report the result
            Android.Widget.Toast.MakeText(this, "StartCaptureClient result is: " + result + " Retries: " + Retries, Android.Widget.ToastLength.Short).Show();
            return result;
        }

        // Stop the CaptureHelper client
        private async void StopCaptureClient()
        {
            await Capture.CloseAsync();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FormsMaps.Init(this, savedInstanceState);
            StartCaptureService();
            StartCaptureClient();
            LoadApplication(new App());
        }
        public async void OnDeviceArrival(object sender, CaptureHelper.DeviceArgs arrivedDevice)
        {
            // first, get the BDADDR of the device so we can supply that to the GUI
            CaptureHelperDevice.BluetoothAddressResult resultBdAddr = await arrivedDevice.CaptureDevice.GetBluetoothAddressAsync("");
            // next get the battery level
            CaptureHelperDevice.BatteryLevelResult resultBattery = await arrivedDevice.CaptureDevice.GetBatteryLevelAsync();
            // now call the universal event
            ScannerSupport.OnDeviceArrival(arrivedDevice.CaptureDevice.GetDeviceInfo().Name, resultBattery.Percentage, resultBdAddr.BluetoothAddress);
        }

        public void OnDeviceRemoval(object sender, CaptureHelper.DeviceArgs removedDevice)
        {
            ScannerSupport.OnDeviceRemoval(removedDevice.CaptureDevice.DeviceTypeName);
        }

        public void OnDecodedData(object sender, CaptureHelper.DecodedDataArgs decodedData)
        {
            ScannerSupport.OnDecodedData(decodedData.DecodedData.DataToUTF8String, decodedData.DecodedData.SymbologyName);
        }
    }
}