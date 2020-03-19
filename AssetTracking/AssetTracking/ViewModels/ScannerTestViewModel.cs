using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using SocketMobile.Capture;
using Xamarin.Forms;



namespace AssetTracking.ViewModels
{
    public class ScannerTestViewModel : INotifyPropertyChanged
    {
        //   CaptureHelper capture;
        public Command ConnectCommand { get; }

        public ScannerTestViewModel()
        {

            //     SaveScanCommand = new Command(async () => await SaveScan(), () => !IsBusy);
            //     ClearScanCommand = new Command(ClearScan);
            //     UpdateHistoryCommand = new Command(async () => await GetScans(), () => !IsBusy);
            //     ConnectCommand = new Command(async () => await Connect(), () => !IsBusy);

            //          capture = new CaptureHelper();
            //          capture.DecodedData += Capture_DecodedData;
            //          capture.DeviceArrival += Capture_DeviceArrival;
            //          capture.DeviceRemoval += Capture_DeviceRemoval;
            //          capture.Errors += Capture_Errors;

        }

        //private void Capture_Errors(object sender, CaptureHelper.ErrorEventArgs e)
        //{
        //    string error = string.Format("Error:{0} {1}", e.Result, e.Message);
        //    DisplayMessage = error;

        //}

        async Task Connect()
        {
            //  string appId = "com.ssitroy.assetttracker";
            //  string appKey = "MC0CFFDBk5AS7qiix+K6GrZf5Qmb/+jRAhUA/x1umYMPfwTxy0TN25Xrs8MXEv0=";

            //  string appId = "com.ssitroy.assettracking";
            //  string developerId = "1da93068-6160-e911-a970-000d3a36399f";
            //string appKey = "MC0CFBoaxKP90CQrmZ0mm6/ZYwlpjUW3AhUAldVh1cVyRXIVt0yM22bcPKvyifU=";

            //   string appId = "android:com.ssitroy.assettrack";
            //   string developerId = "1da93068-6160-e911-a970-000d3a36399f";
            //   string appKey = "MCwCFE89fIm/qViN3t20DZo9neyXONusAhRIYR0qnoAsOAHv2MHpUaW5egKznw==";

            IsBusy = true;
            DisplayMessage = "Turn on Scanner and wait for it to Connect...";
            //   capture.DoNotUseWebSocket = true;
            await Task.Delay(1000);
            //long result;
            //try
            //{
            //    result = await capture.OpenAsync(appId, developerId, appKey);

            //    if (SktErrors.SKTSUCCESS(result))
            //    {
            //        CaptureHelper.VersionResult version = await capture.GetCaptureVersionAsync();

            //        DisplayMessage = "Connected - Version " + version;
            //        IsConnected = true ;
            //    }
            //    else
            //    {
            //        DisplayMessage = "Unable to Connect to Scanner - Make sure device is paired";
            //        IsConnected = false;
            //    }


            //}
            //catch (Exception ex)
            //{

            //    var error = ex.Message.ToString();
            //    DisplayMessage = error;
            //    IsConnected = false;

            //}

            IsBusy = false;
        }
        private bool isConnected;
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        private string displayMessage;
        public string DisplayMessage
        {
            get
            {
                return displayMessage;
            }
            set
            {
                displayMessage = value;
                OnPropertyChanged(nameof(DisplayMessage));
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
                //        SaveScanCommand.ChangeCanExecute();
            }
        }



        string scannerName = "no scanner";
        public string ScannerName
        {
            get
            {
                return scannerName;
            }
            set
            {
                scannerName = value;
                OnPropertyChanged(nameof(ScannerName));
            }
        }
        private List<string> scanList = new List<string> { "a", "b", "c" };
        public List<string> ScanList
        {
            get
            {
                return scanList;
            }
            set
            {
                scanList = value;
                OnPropertyChanged(nameof(ScanList));
            }

        }
        private string scanValue;
        public string ScanValue
        {
            get
            {
                return scanValue;
            }
            set
            {
                scanValue = value;
                scanList.Add(value);
                OnPropertyChanged(nameof(ScanValue));
                OnPropertyChanged(nameof(ScanList));
            }
        }

        //private void Capture_DeviceRemoval(object sender, CaptureHelper.DeviceArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void Capture_DeviceArrival(object sender, CaptureHelper.DeviceArgs e)
        //{
        //    scannerList.Add(e.CaptureDevice.DeviceTypeName);
        //}

        //private void Capture_DecodedData(object sender, CaptureHelper.DecodedDataArgs e)
        //{
        //    ScanValue = e.DecodedData.DataToUTF8String;
        //}
        
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
